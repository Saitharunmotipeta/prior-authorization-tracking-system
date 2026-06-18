using Microsoft.AspNetCore.Mvc;
using PriorAuthorization.Specialist.API.Services.Interfaces;

namespace PriorAuthorization.Specialist.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientLookupController : ControllerBase
{
    private readonly IPatientLookupService _patientLookupService;

    public PatientLookupController(IPatientLookupService patientLookupService)
    {
        _patientLookupService = patientLookupService;
    }

    [HttpGet("{mrnNumber}")]
    public async Task<IActionResult> GetByMrn(string mrnNumber)
    {
        var patient = await _patientLookupService.GetByMrnAsync(mrnNumber);

        if (patient == null)
        {
            return NotFound(new
            {
                Message = $"Patient not found for MRN {mrnNumber}"
            });
        }

        return Ok(patient);
    }
}