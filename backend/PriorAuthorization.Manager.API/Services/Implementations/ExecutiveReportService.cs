using PriorAuthorization.Manager.API.DTOs;
using PriorAuthorization.Manager.API.DTOs.Analytics;
using PriorAuthorization.Manager.API.DTOs.Dashboard;
using PriorAuthorization.Manager.API.Services.Interfaces;
using PriorAuthorization.Manager.API.Utilities;
using PriorAuthorization.Shared.Common;

namespace PriorAuthorization.Manager.API.Services.Implementations;

public class ExecutiveReportService : IExecutiveReportService
{
    private readonly IDashboardService _dashboardService;

    private readonly IAnalyticsService _analyticsService;

    private readonly IOpenRouterService _openRouterService;

    private readonly ILogger<ExecutiveReportService> _logger;

    public ExecutiveReportService(
        IDashboardService dashboardService,
        IAnalyticsService analyticsService,
        IOpenRouterService openRouterService,
        ILogger<ExecutiveReportService> logger)
    {
        _dashboardService = dashboardService;
        _analyticsService = analyticsService;
        _openRouterService = openRouterService;
        _logger = logger;
    }

    public async Task<ApiResponse<ExecutiveReportResponse>>
        GenerateExecutiveReportAsync()
    {
        _logger.LogInformation(
            "Executive report generation started.");

        DashboardResponseDto dashboard =
            await _dashboardService.GetDashboardAsync(
                new DashboardFilterDto());

        List<FacilityComparisonDto> facilities =
            await _analyticsService.GetFacilityComparisonAsync();

        List<PayerPerformanceDto> payers =
            await _analyticsService.GetPayerPerformanceAsync(
                null);

        string prompt =
            PromptBuilder.BuildExecutiveReportPrompt(
                dashboard,
                facilities,
                payers);

        _logger.LogInformation(
            "Prompt generated successfully.");

        string report =
            await _openRouterService
                .GenerateExecutiveSummaryAsync(
                    prompt);

        _logger.LogInformation(
            "AI report received successfully.");

        var response =
            new ExecutiveReportResponse
            {
                GeneratedAt = DateTime.UtcNow,
                Report = report,
            };

        _logger.LogInformation(
            "Executive report generated successfully.");

        return ApiResponse<ExecutiveReportResponse>.SuccessResponse(
            response,
            "Executive report generated successfully.");
    }
}