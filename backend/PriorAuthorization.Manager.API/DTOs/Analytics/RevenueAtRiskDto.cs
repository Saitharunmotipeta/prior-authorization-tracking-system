namespace PriorAuthorization.Manager.API.DTOs.Analytics;

public class RevenueAtRiskDto
{
    public int DeniedRequests { get; set; }

    public decimal RevenueAtRisk { get; set; }
}