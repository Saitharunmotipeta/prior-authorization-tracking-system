using Microsoft.AspNetCore.Mvc;
using PriorAuthorization.Shared.Common;
using PriorAuthorization.Specialist.API.Services.Interfaces;
using PriorAuthorizationSpecialist.API.DTOs;

namespace PriorAuthorization.Specialist.API.Controllers;

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

        return Ok(
            ApiResponse<IEnumerable<FacilityDto>>
                .SuccessResponse(
                    facilities,
                    "Facilities retrieved successfully."));
    }
}