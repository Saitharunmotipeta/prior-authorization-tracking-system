using Microsoft.AspNetCore.Mvc;
using PriorAuthorization.Manager.API.Services.Interfaces;

namespace PriorAuthorization.Manager.API.Controllers;

[ApiController]
[Route("api/analytics")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsController(
        IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    [HttpGet("payer-performance")]
    public async Task<IActionResult>
        GetPayerPerformance(
            [FromQuery] int? facilityId)
    {
        var result =
            await _analyticsService
                .GetPayerPerformanceAsync(
                    facilityId);

        return Ok(result);
    }

    [HttpGet("slowest-payers")]
    public async Task<IActionResult>
    GetSlowestPayers(
        [FromQuery] int? facilityId)
    {
        var result =
            await _analyticsService
                .GetSlowestPayersAsync(
                    facilityId);

        return Ok(result);
    }

    [HttpGet("revenue-at-risk")]
    public async Task<IActionResult>
    GetRevenueAtRisk(
        [FromQuery] int? facilityId)
    {
        var result =
            await _analyticsService
                .GetRevenueAtRiskAsync(
                    facilityId);

        return Ok(result);
    }

    [HttpGet("facility-comparison")]
    public async Task<IActionResult>
    GetFacilityComparison()
    {
        var result =
            await _analyticsService
                .GetFacilityComparisonAsync();

        return Ok(result);
    }

    [HttpGet("top-performing-payers")]
    public async Task<IActionResult>
    GetTopPerformingPayers(
        [FromQuery] int? facilityId)
    {
        var result =
            await _analyticsService
                .GetTopPerformingPayersAsync(
                    facilityId);

        return Ok(result);
    }

    [HttpGet("poor-performing-payers")]
    public async Task<IActionResult>
    GetPoorPerformingPayers(
        [FromQuery] int? facilityId)
    {
        var result =
            await _analyticsService
                .GetPoorPerformingPayersAsync(
                    facilityId);

        return Ok(result);
    }

    [HttpGet("delay-trends")]
    public async Task<IActionResult>
    GetDelayTrends(
        [FromQuery] int? facilityId)
    {
        var result =
            await _analyticsService
                .GetDelayTrendsAsync(
                    facilityId);

        return Ok(result);
    }
}