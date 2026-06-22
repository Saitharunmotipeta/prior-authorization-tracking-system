using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Manager.API.Services.Implementations;
using PriorAuthorization.Manager.API.Services.Interfaces;
using PriorAuthorization.Shared.Data;
using PriorAuthorization.Shared.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger =
    new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.File(
            path: "Logs/application-.txt",
            rollingInterval:
                RollingInterval.Day,
            retainedFileCountLimit: 30,
            shared: true)
        .CreateLogger();

//builder.Host.UseSerilog();

// Controllers

builder.Services.AddControllers();

// Swagger / OpenAPI

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

// Database

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Services

builder.Services.AddScoped<
    IDashboardService,
    DashboardService>();

builder.Services.AddScoped<
    IAnalyticsService,
    AnalyticsService>();

var app = builder.Build();

// Configure HTTP pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<GlobalExceptionMiddleware>();


app.MapControllers();

try
{
    Log.Information(
        "Application Started");

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