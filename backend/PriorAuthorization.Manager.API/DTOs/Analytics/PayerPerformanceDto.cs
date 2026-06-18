namespace PriorAuthorization.Manager.API.DTOs.Analytics;

public class PayerPerformanceDto
{
    public string PayerName { get; set; } = string.Empty;

    public int TotalRequests { get; set; }

    public int ApprovedRequests { get; set; }

    public int DeniedRequests { get; set; }

    public int PendingRequests { get; set; }

    public decimal ApprovalRate { get; set; }
}