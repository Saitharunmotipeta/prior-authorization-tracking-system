using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Payer.API.Services;
using PriorAuthorization.Payer.API.Services.Interfaces;
using PriorAuthorization.Shared.Data;
<<<<<<< HEAD
using PriorAuthorization.Shared.Middleware;
using PriorAuthorization.Shared.Validations;
=======
>>>>>>> 60783b2069519d2a6c40a7375a78b782e8db311f
using Serilog;

var builder = WebApplication.CreateBuilder(args);

<<<<<<< HEAD
//
// ✅ Configure Serilog
//
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File(
        path: "Logs/application-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        shared: true)
    .CreateLogger();

builder.Host.UseSerilog();

//
// ✅ Add Services
//
builder.Services.AddControllers();

// ✅ Model Validation
builder.Services.AddModelValidationConfiguration();

// ✅ DbContext
=======
#region Logging

Log.Logger =
    new LoggerConfiguration()
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

#region Database

>>>>>>> 60783b2069519d2a6c40a7375a78b782e8db311f
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

<<<<<<< HEAD
// ✅ Dependency Injection
builder.Services.AddScoped<IPayerService, PayerService>();

// ✅ Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//
// ✅ Build App
//
var app = builder.Build();

//
// ✅ Middleware Pipeline
//

// ✅ Global Exception Middleware (VERY IMPORTANT → must be first)
app.UseMiddleware<GlobalExceptionMiddleware>();
=======
#endregion

#region Dependency Injection

builder.Services.AddScoped<IPayerService, PayerService>();

#endregion

#region Controllers

builder.Services.AddControllers();

#endregion

#region Swagger

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

var app = builder.Build();

#region Middleware
>>>>>>> 60783b2069519d2a6c40a7375a78b782e8db311f

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

<<<<<<< HEAD
app.UseAuthorization();

app.MapControllers();

//
// ✅ Run App
//
app.Run();
=======
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
    Log.Fatal(
        ex,
        "Payer API failed to start");
}
finally
{
    Log.CloseAndFlush();
}

#endregion
>>>>>>> 60783b2069519d2a6c40a7375a78b782e8db311f
