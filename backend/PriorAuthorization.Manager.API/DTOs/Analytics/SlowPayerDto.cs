namespace PriorAuthorization.Manager.API.DTOs.Analytics;

public class SlowPayerDto
{
    public string PayerName { get; set; } = string.Empty;

    public int TotalReviewedRequests { get; set; }

    public decimal AverageResponseDays { get; set; }
}