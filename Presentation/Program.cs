using Application.Helpers;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Helpers;
using Infrastructure.Seeds;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Presentation.Helpers;
using Presentation.MiddleWares;
using System.Diagnostics;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

 //--------------------
 //Autofac
 //--------------------
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterModule<ApplicationModule>();
    container.RegisterModule(new Application.Helpers.AutoMapperModule(new[]
    {
        typeof(Program).Assembly
    }));    
    container.RegisterModule<PresentationModule>();
    container.RegisterModule<InfrastructureModule>();
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


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

app.UseMiddleware<GlobalErrorHandlerMiddleware>();
app.UseMiddleware<TransactionMiddleware>();
app.UseMiddleware<RequestTimeoutMiddleware>();
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

// await app.Services.SeedAsync();

// Create a scope to resolve the DbContext
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<Context>();
        
        // Apply migrations automatically (Optional but helpful)
        await context.Database.MigrateAsync();
        
        // Run your Seed
        await ContextSeed.SeedAsync(context);
        var mapper = services.GetRequiredService<AutoMapper.IMapper>();
        Application.Helpers.MapperHelper.Mapper = mapper;
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during database seeding.");
    }
}

// ... rest of your middleware (UseRouting, UseAuthentication, etc.)
app.Run();


