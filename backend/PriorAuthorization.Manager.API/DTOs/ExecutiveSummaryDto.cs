namespace PriorAuthorization.Manager.API.DTOs;

public class ExecutiveSummaryDto
{
    public string ExecutiveSummary { get; set; } = string.Empty;

    public string OrganizationHealth { get; set; } = string.Empty;

    public List<string> KeyHighlights { get; set; } = [];

    public List<string> FacilityInsights { get; set; } = [];

    public List<string> PayerInsights { get; set; } = [];

    public List<string> RevenueInsights { get; set; } = [];

    public List<string> OperationalRisks { get; set; } = [];

    public List<string> Recommendations { get; set; } = [];

    public string ExecutiveConclusion { get; set; } = string.Empty;
}