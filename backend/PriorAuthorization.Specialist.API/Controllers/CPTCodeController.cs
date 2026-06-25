using Microsoft.AspNetCore.Mvc;
using PriorAuthorization.Shared.Common;
using PriorAuthorization.Specialist.API.DTOs;
using PriorAuthorization.Specialist.API.Services.Interfaces;

namespace PriorAuthorization.Specialist.API.Controllers;

[ApiController]
[Route("api/cptcodes")]
public class CPTCodeController : ControllerBase
{
    private readonly ICPTCodeService _cptCodeService;

    public CPTCodeController(
        ICPTCodeService cptCodeService)
    {
        _cptCodeService = cptCodeService;
    }

    [HttpGet]
    public async Task<IActionResult>
    GetCPTCodes()
    {
        var codes =
            await _cptCodeService
                .GetCPTCodesAsync();

        return Ok(
            ApiResponse<List<CPTCodeDto>>
                .SuccessResponse(
                    codes,
                    "CPT codes retrieved successfully."));
    }
}