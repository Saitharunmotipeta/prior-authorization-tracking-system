using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Shared.Data;
using PriorAuthorization.Specialist.API.Services.Interfaces;
using Specialist.API.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Services

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

// Database

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Basic Health Checks

builder.Services.AddScoped<IFacilityService,FacilityService>();

builder.Services.AddScoped<IDepartmentService,DepartmentService>();
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

app.Run();