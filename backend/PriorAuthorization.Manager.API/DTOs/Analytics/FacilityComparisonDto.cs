namespace PriorAuthorization.Manager.API.DTOs.Analytics;

public class FacilityComparisonDto
{
    public int Rank { get; set; }

    public string FacilityName { get; set; } = string.Empty;

    public int TotalRequests { get; set; }

    public int ApprovedRequests { get; set; }

    public int DeniedRequests { get; set; }

    public decimal ApprovalRate { get; set; }

    public decimal ApprovedRevenue { get; set; }

    public decimal AverageResponseDays { get; set; }
}