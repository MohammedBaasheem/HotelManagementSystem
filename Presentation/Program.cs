using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

builder.Services.AddDbContext<Context>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
    .LogTo(log => Debug.WriteLine(log), LogLevel.Information)
    .EnableSensitiveDataLogging(true);
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
