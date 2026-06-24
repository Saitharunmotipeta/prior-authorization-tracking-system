using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Payer.API.Services;
using PriorAuthorization.Payer.API.Services.Interfaces;
using PriorAuthorization.Shared.Data;
using PriorAuthorization.Shared.Middleware;
using PriorAuthorization.Shared.Validations;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Logging

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File(
        path: "Logs/application-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        shared: true,
        outputTemplate:
            "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

#endregion

#region Services

// ✅ ✅ ✅ ADD CORS HERE (BEFORE BUILD)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins("http://localhost:5173") // ✅ frontend URL
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Controllers
builder.Services.AddControllers();

// Model Validation
builder.Services.AddModelValidationConfiguration();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection
builder.Services.AddScoped<IPayerService, PayerService>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

// ✅ BUILD APPLICATION
var app = builder.Build();

#region Middleware

// ✅ ✅ ✅ USE CORS HERE (AFTER BUILD, BEFORE CONTROLLERS)
app.UseCors("AllowFrontend");

// Global Exception Handling
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

#endregion

#region Application Startup

try
{
    Log.Information("Payer API started successfully");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Payer API failed to start");
}
finally
{
    Log.CloseAndFlush();
}

#endregion