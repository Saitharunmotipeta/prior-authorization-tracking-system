using Microsoft.AspNetCore.Mvc;
using PriorAuthorization.Specialist.API.Services.Interfaces;
namespace Specialist.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EligibilityController : ControllerBase
{
    private readonly IEligibilityService _eligibilityService;

    public EligibilityController(IEligibilityService eligibilityService)
    {
        _eligibilityService = eligibilityService;
    }

    [HttpGet("{patientId:guid}")]
    public async Task<IActionResult> VerifyEligibility(Guid patientId)
    {
        var result = await _eligibilityService.VerifyEligibilityAsync(patientId);
        return Ok(result);
    }
}