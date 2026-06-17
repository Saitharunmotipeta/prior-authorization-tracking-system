using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Manager.API.Services.Implementations;
using PriorAuthorization.Manager.API.Services.Interfaces;
using PriorAuthorization.Shared.Data;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

// Configure HTTP pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();