using PriorAuthorization.Manager.API.DTOs.Analytics;

namespace PriorAuthorization.Manager.API.Services.Interfaces;

public interface IAnalyticsService
{
    Task<List<PayerPerformanceDto>>
        GetPayerPerformanceAsync(int? facilityId);

    Task<List<SlowPayerDto>>
        GetSlowestPayersAsync(int? facilityId);

    Task<RevenueAtRiskDto>
        GetRevenueAtRiskAsync(int? facilityId);

    Task<List<FacilityComparisonDto>>
        GetFacilityComparisonAsync();

    Task<List<TopPerformingPayerDto>>
        GetTopPerformingPayersAsync(
        int? facilityId);

    Task<List<PoorPerformingPayerDto>>
        GetPoorPerformingPayersAsync(
        int? facilityId);

    Task<List<DelayTrendDto>>
        GetDelayTrendsAsync(
        int? facilityId);
}