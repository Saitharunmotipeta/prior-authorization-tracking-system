using System.Text.Json;
using PriorAuthorization.Manager.API.DTOs.Analytics;
using PriorAuthorization.Manager.API.DTOs.Dashboard;

namespace PriorAuthorization.Manager.API.Utilities;

public static class PromptBuilder
{
    public static string BuildExecutiveReportPrompt(
        DashboardResponseDto dashboard,
        List<FacilityComparisonDto> facilities,
        List<PayerPerformanceDto> payers)
    {
        var metrics = new
        {
            Organization = dashboard,
            Facilities = facilities,
            Payers = payers
        };

        string metricsJson = JsonSerializer.Serialize(
            metrics,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });

        string promptTemplate = File.ReadAllText(
             Path.Combine(
        Directory.GetCurrentDirectory(),
        "Prompts",
        "ExecutiveReportPrompt.txt"));

        return promptTemplate.Replace(
            "{{METRICS}}",
            metricsJson);
    }
}