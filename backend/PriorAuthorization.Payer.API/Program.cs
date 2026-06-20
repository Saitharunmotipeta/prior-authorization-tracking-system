<<<<<<< HEAD
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

builder.Host.UseSerilog();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
=======
﻿using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Payer.API.Services;
using PriorAuthorization.Payer.API.Services.Interfaces;
using PriorAuthorization.Shared.Data;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add Controllers
builder.Services.AddControllers();
>>>>>>> 92c03a0398f22830403aa2db600852c1864b35eb


// ✅ Add DbContext (from Shared project)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// ✅ Register Services
builder.Services.AddScoped<IPayerService, PayerService>();


// ✅ Swagger Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// ✅ Logging (already built-in, no extra config needed)
builder.Logging.ClearProviders();
builder.Logging.AddConsole();


// ✅ Build app
var app = builder.Build();


// ✅ Enable Swagger (ONLY in Development)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// ✅ Middleware Pipeline
app.UseHttpsRedirection();

app.UseAuthorization();

<<<<<<< HEAD
app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");
    app.Run();
=======
app.MapControllers();

>>>>>>> 92c03a0398f22830403aa2db600852c1864b35eb

// ✅ Run application
app.Run();