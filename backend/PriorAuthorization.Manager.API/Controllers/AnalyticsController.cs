using Microsoft.AspNetCore.Mvc;
using PriorAuthorization.Manager.API.Services.Interfaces;
using PriorAuthorization.Shared.Common;
using PriorAuthorization.Manager.API.DTOs.Analytics;

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

        return Ok(
           ApiResponse<List<PayerPerformanceDto>>
            .SuccessResponse(
                result,
                "Payer performance retrieved successfully"));
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

        return Ok(
            ApiResponse<List<SlowPayerDto>>
                .SuccessResponse(
                    result,
                    "Slowest payers retrieved successfully"));
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

          return Ok(
            ApiResponse<RevenueAtRiskDto>
                .SuccessResponse(
                    result,
                    "Revenue at risk retrieved successfully"));
    }

    [HttpGet("facility-comparison")]
    public async Task<IActionResult>
    GetFacilityComparison()
    {
        var result =
            await _analyticsService
                .GetFacilityComparisonAsync();

        return Ok(
            ApiResponse<List<FacilityComparisonDto>>
                .SuccessResponse(
                    result,
                    "Facility comparison retrieved successfully"));
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

        return Ok(
            ApiResponse<List<TopPerformingPayerDto>>
                .SuccessResponse(
                    result,
                    "Top performing payers retrieved successfully"));
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

        return Ok(
            ApiResponse<List<PoorPerformingPayerDto>>
                .SuccessResponse(
                    result,
                    "Poor performing payers retrieved successfully"));
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

        return Ok(
            ApiResponse<List<DelayTrendDto>>
                .SuccessResponse(
                    result,
                    "Delay trends retrieved successfully"));
    }
}