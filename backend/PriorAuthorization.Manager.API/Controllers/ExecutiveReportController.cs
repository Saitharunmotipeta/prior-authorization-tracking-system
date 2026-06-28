using Microsoft.AspNetCore.Mvc;
using PriorAuthorization.Manager.API.DTOs;
using PriorAuthorization.Manager.API.Services.Interfaces;
using PriorAuthorization.Shared.Common;

namespace PriorAuthorization.Manager.API.Controllers;

[ApiController]
[Route("api/reports")]
public class ExecutiveReportController : ControllerBase
{
    private readonly IExecutiveReportService _executiveReportService;
    private readonly ILogger<ExecutiveReportController> _logger;

    public ExecutiveReportController(
        IExecutiveReportService executiveReportService,
        ILogger<ExecutiveReportController> logger)
    {
        _executiveReportService = executiveReportService;
        _logger = logger;
    }

    [HttpPost("executive")]
    [ProducesResponseType(typeof(ApiResponse<ExecutiveReportResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<ExecutiveReportResponse>>> GenerateExecutiveReport()
    {
        _logger.LogInformation(
            "GenerateExecutiveReport endpoint invoked.");

        ApiResponse<ExecutiveReportResponse> response =
            await _executiveReportService
                .GenerateExecutiveReportAsync();

        _logger.LogInformation(
            "GenerateExecutiveReport completed successfully.");

        return Ok(response);
    }
}