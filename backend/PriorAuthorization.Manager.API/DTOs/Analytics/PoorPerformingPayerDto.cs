namespace PriorAuthorization.Manager.API.DTOs.Analytics;

public class PoorPerformingPayerDto
{
    public int Rank { get; set; }

    public string PayerName { get; set; } = string.Empty;

    public int TotalRequests { get; set; }

    public int DeniedRequests { get; set; }

    public decimal DenialRate { get; set; }
}