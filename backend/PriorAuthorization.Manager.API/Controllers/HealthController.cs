using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Shared.Data;

namespace PriorAuthorization.Manager.API.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public HealthController(
        ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetHealth()
    {
        try
        {
            var databaseConnected =
                await _context.Database.CanConnectAsync();

            if (!databaseConnected)
            {
                return StatusCode(
                    StatusCodes.Status503ServiceUnavailable,
                    new
                    {
                        Status = "Unhealthy",
                        Application = "PriorAuthorization.Manager.API",
                        Database = "Disconnected",
                        Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                        Timestamp = DateTime.UtcNow
                    });
            }

            return Ok(new
            {
                Status = "Healthy",
                Application = "PriorAuthorization.Manager.API",
                Database = "Connected",
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                new
                {
                    Status = "Unhealthy",
                    Application = "PriorAuthorization.Manager.API",
                    Error = ex.Message,
                    Timestamp = DateTime.UtcNow
                });
        }
    }
}