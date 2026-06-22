using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Shared.Data;
using PriorAuthorization.Shared.Middleware;
using PriorAuthorization.Shared.Validations;
using PriorAuthorization.Specialist.API.Services;
using PriorAuthorization.Specialist.API.Services.Implementations;
using PriorAuthorization.Specialist.API.Services.Interfaces;
using PriorAuthorizationSpecialist.API.Services.Implementations;
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

// Services

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

// Database

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection

builder.Services.AddScoped<IEligibilityService, EligibilityService>();
builder.Services.AddScoped<IFacilityService, FacilityService>();

builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IPatientLookupService, PatientLookupService>();
builder.Services.AddScoped<IEncounterService, EncounterService>();
builder.Services.AddScoped<IAuthorizationService,AuthorizationRequestService>();
builder.Services.AddScoped<IReminderService, ReminderService>();

builder.Services.AddModelValidationConfiguration();


// Basic Health Checks

builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllers();

// Health Check Endpoint

app.MapHealthChecks("/health");

// Database Connectivity Check

app.MapGet("/health/database", async (ApplicationDbContext context) =>
{
    try
    {
        var canConnect = await context.Database.CanConnectAsync();

        return Results.Ok(new
        {
            Status = canConnect ? "Healthy" : "Unhealthy",
            Database = "PriorAuthorizationDB",
            Connected = canConnect,
            Timestamp = DateTime.UtcNow
        });
    }
    catch (Exception ex)
    {
        return Results.Problem(
            title: "Database Connection Failed",
            detail: ex.Message);
    }
});

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