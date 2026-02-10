using Application.Helpers;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Presentation.MiddleWares;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Autofac
// --------------------
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterModule<ApplicationModule>();
    container.RegisterModule<AutoMapperModule>();
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
app.UseAuthorization();
app.MapControllers();
app.Run();
