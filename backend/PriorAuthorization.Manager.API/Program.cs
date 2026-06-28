using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Manager.API.Services.Implementations;
using PriorAuthorization.Manager.API.Services.Interfaces;
using PriorAuthorization.Shared.Data;
using PriorAuthorization.Shared.Middleware;
using PriorAuthorization.Manager.API.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Logging

Log.Logger =
    new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.File(
            path: "Logs/application-.txt",
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 30,
            shared: true)
        .CreateLogger();

builder.Host.UseSerilog();

#endregion

#region Controllers

builder.Services.AddControllers();

#endregion

#region Swagger

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

#endregion

#region Database

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

#endregion

#region Dependency Injection

builder.Services.AddScoped<
    IDashboardService,
    DashboardService>();

builder.Services.AddScoped<
    IAnalyticsService,
    AnalyticsService>();

builder.Services.AddHttpClient();

builder.Services.AddScoped<
    IOpenRouterService,
    OpenRouterService>();

builder.Services.AddScoped<
    IExecutiveReportService,
    ExecutiveReportService>();

#endregion

#region CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

#endregion

var app = builder.Build();

#region Middleware Pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseCors("FrontendPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionMiddleware>();

#endregion

#region Endpoints

app.MapControllers();

#endregion

try
{
    Log.Information("Application Started Successfully");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(
        ex,
        "Application Failed To Start");
}
finally
{
    Log.CloseAndFlush();
}