using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Payer.API.Services;
using PriorAuthorization.Payer.API.Services.Interfaces;
using PriorAuthorization.Shared.Data;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add Controllers
builder.Services.AddControllers();


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
// ✅ Run application
app.Run();