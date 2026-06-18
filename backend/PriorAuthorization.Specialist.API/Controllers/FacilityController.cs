using Microsoft.AspNetCore.Mvc;
using PriorAuthorization.Specialist.API.Services.Interfaces;

namespace Specialist.API.Controllers;

[ApiController]
[Route("api/facilities")]
public class FacilityController : ControllerBase
{
    private readonly IFacilityService _facilityService;

    public FacilityController(
        IFacilityService facilityService)
    {
        _facilityService = facilityService;
    }

    [HttpGet]
    public async Task<IActionResult> GetFacilities()
    {
        var facilities =
            await _facilityService.GetFacilitiesAsync();

        return Ok(facilities);
    }
}