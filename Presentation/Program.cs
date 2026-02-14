using Application.Helpers;
using Application.Interface;
using Application.IRepositories;
using Application.Services;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Domain.Models;
using FluentValidation;
using Infrastructure.Data;
using Infrastructure.Helpers;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Presentation.MiddleWares;
using System.Diagnostics;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

 //--------------------
 //Autofac
 //--------------------
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterModule<ApplicationModule>();
    container.RegisterModule<AutoMapperModule>();

    container.RegisterType<GlobalErrorHandlerMiddleware>().InstancePerDependency();
    container.RegisterType<TransactionMiddleware>().InstancePerDependency();
});

// --------------------
// Database connection selection
// --------------------
var authMode = builder.Configuration.GetValue<string>(
    "Database:AuthMode",
    "Windows"
);

string connectionString = authMode switch
{
    "Windows" => builder.Configuration.GetConnectionString("WindowsAuth")!,
    "Sql" => builder.Configuration.GetConnectionString("SqlAuth")!,
    _ => throw new InvalidOperationException(
        $"Invalid Database:AuthMode '{authMode}'"
    )
};


// --------------------
// DbContext (MUST be before Build)
// --------------------
builder.Services.AddDbContext<Context>(options =>
{
    options.UseSqlServer(
            connectionString,
            b => b.MigrationsAssembly("Infrastructure")
        )
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
        .LogTo(log => Debug.WriteLine(log), LogLevel.Information)
        .EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
});

// --------------------
// Other services
// --------------------
builder.Services.AddControllers();
builder.Services.AddOpenApi();


//////////////////////////////
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<GlobalErrorHandlerMiddleware>();
builder.Services.AddScoped<TransactionMiddleware>();


//////////////////////////////////////////
var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()!;
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
        };
    });

var app = builder.Build();

app.UseMiddleware<GlobalErrorHandlerMiddleware>();
app.UseMiddleware<TransactionMiddleware>();

// --------------------
// Pipeline
// --------------------
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.Services.SeedAsync();

app.Run();
