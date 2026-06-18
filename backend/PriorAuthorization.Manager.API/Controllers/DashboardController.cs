using Microsoft.AspNetCore.Mvc;
using PriorAuthorization.Manager.API.DTOs.Dashboard;
using PriorAuthorization.Manager.API.Services.Interfaces;

namespace PriorAuthorization.Manager.API.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(
        IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet]
    public async Task<IActionResult> GetDashboard(
        [FromQuery] int? facilityId)
    {
        var filter = new DashboardFilterDto
        {
            FacilityId = facilityId
        };

        var result =
            await _dashboardService.GetDashboardAsync(
                filter);

        return Ok(result);
    }
}