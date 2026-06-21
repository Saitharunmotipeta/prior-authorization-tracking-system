using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Payer.API.Services;
using PriorAuthorization.Payer.API.Services.Interfaces;
using PriorAuthorization.Shared.Data;
using PriorAuthorization.Shared.Middleware;
using PriorAuthorization.Shared.Validations;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//
// ✅ Run App
//
app.Run();