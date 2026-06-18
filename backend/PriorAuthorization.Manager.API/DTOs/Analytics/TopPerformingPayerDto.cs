namespace PriorAuthorization.Manager.API.DTOs.Analytics;

public class TopPerformingPayerDto
{
    public int Rank { get; set; }

    public string PayerName { get; set; } = string.Empty;

    public int TotalRequests { get; set; }

    public int ApprovedRequests { get; set; }

    public decimal ApprovalRate { get; set; }
}