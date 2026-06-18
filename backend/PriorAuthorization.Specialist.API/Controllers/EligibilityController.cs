using Microsoft.AspNetCore.Mvc;
using PriorAuthorization.Shared.Common;
using PriorAuthorization.Specialist.API.DTOs;
using PriorAuthorization.Specialist.API.Services.Interfaces;
namespace PriorAuthorization.Specialist.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EligibilityController : ControllerBase
{
    private readonly IEligibilityService _eligibilityService;

    public EligibilityController(IEligibilityService eligibilityService)
    {
        _eligibilityService = eligibilityService;
    }

    [HttpGet("eligibility/{patientId}")]
    public async Task<IActionResult>VerifyEligibility(Guid patientId)
    {
        var result =
            await _eligibilityService
                .VerifyEligibilityAsync(patientId);

        return Ok(
            ApiResponse<EligibilityResponseDto>
                .SuccessResponse(
                    result,
                    "Eligibility verification completed."));
    }
}