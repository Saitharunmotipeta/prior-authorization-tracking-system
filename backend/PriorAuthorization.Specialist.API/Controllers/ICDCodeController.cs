using Microsoft.AspNetCore.Mvc;
using PriorAuthorization.Specialist.API.DTOs;
using PriorAuthorization.Specialist.API.Services.Interfaces;

namespace PriorAuthorization.Specialist.API.Controllers;

[ApiController]
[Route("api/icd-codes")]
public class ICDCodeController : ControllerBase
{
    private readonly IICDCodeService _icdCodeService;

    public ICDCodeController(
        IICDCodeService icdCodeService)
    {
        _icdCodeService = icdCodeService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ICDCodeDto>>>
        GetICDCodes()
    {
        return Ok(
            await _icdCodeService.GetICDCodesAsync()
        );
    }
}