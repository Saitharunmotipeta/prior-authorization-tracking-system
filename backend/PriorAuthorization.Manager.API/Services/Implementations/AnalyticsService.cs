using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Manager.API.DTOs.Analytics;
using PriorAuthorization.Manager.API.Services.Interfaces;
using PriorAuthorization.Shared.Data;
using PriorAuthorization.Shared.Enums;

namespace PriorAuthorization.Manager.API.Services.Implementations;

public class AnalyticsService : IAnalyticsService
{
    private readonly ApplicationDbContext _context;

    public AnalyticsService(
        ApplicationDbContext context)
    {
        _context = context;
    }

public async Task<List<PayerPerformanceDto>>
    GetPayerPerformanceAsync(int? facilityId)
{
    var encounterQuery =
        _context.Encounters.AsQueryable();

    if (facilityId.HasValue)
    {
        encounterQuery =
            encounterQuery.Where(x =>
                x.FacilityId ==
                facilityId.Value);
    }

    var encounterIds =
        await encounterQuery
            .Select(x => x.EncounterId)
            .ToListAsync();

    var requests =
        await _context.AuthorizationRequests
            .Include(x => x.Payer)
            .Where(x =>
                encounterIds.Contains(
                    x.EncounterId))
            .ToListAsync();

    var result =
        requests
        .GroupBy(x => x.Payer.PayerName)
        .Select(g =>
        {
            var total =
                g.Count();

            var approved =
                g.Count(x =>
                    x.Status ==
                    (byte)RequestStatus.Approved);

            var denied =
                g.Count(x =>
                    x.Status ==
                    (byte)RequestStatus.Denied);

            var pending =
                g.Count(x =>
                    x.Status ==
                    (byte)RequestStatus.UnderReview ||
                    x.Status ==
                    (byte)RequestStatus.AdditionalInfoRequired ||
                    x.Status ==
                    (byte)RequestStatus.ReSubmitted);

            return new PayerPerformanceDto
            {
                PayerName = g.Key,

                TotalRequests = total,

                ApprovedRequests = approved,

                DeniedRequests = denied,

                PendingRequests = pending,

                ApprovalRate =
                    total == 0
                        ? 0
                        : Math.Round(
                            (decimal)approved /
                            total * 100,
                            2)
            };
        })
        .OrderByDescending(x =>
            x.ApprovalRate)
        .ToList();

    return result;
    }

    public async Task<List<SlowPayerDto>>
    GetSlowestPayersAsync(int? facilityId)
    {
        var encounterQuery =
            _context.Encounters.AsQueryable();

        if (facilityId.HasValue)
        {
            encounterQuery =
                encounterQuery.Where(x =>
                    x.FacilityId ==
                    facilityId.Value);
        }

        var encounterIds =
            await encounterQuery
                .Select(x => x.EncounterId)
                .ToListAsync();

        var requests =
            await _context.AuthorizationRequests
                .Include(x => x.Payer)
                .Where(x =>
                    encounterIds.Contains(
                        x.EncounterId))
                .Where(x =>
                    x.SubmittedAt.HasValue &&
                    x.ReviewedAt.HasValue)
                .ToListAsync();

        var result =
            requests
            .GroupBy(x => x.Payer.PayerName)
            .Select(g =>
            {
                var reviewedRequests =
                    g.Count();

                var averageDays =
                    g.Average(x =>
                        (x.ReviewedAt!.Value -
                         x.SubmittedAt!.Value)
                        .TotalDays);

                return new SlowPayerDto
                {
                    PayerName = g.Key,

                    TotalReviewedRequests =
                        reviewedRequests,

                    AverageResponseDays =
                        Math.Round(
                            (decimal)averageDays,
                            2)
                };
            })
            .OrderByDescending(x =>
                x.AverageResponseDays)
            .ToList();

        return result;
    }

    public async Task<RevenueAtRiskDto>
    GetRevenueAtRiskAsync(int? facilityId)
    {
        var encounterQuery =
            _context.Encounters.AsQueryable();

        if (facilityId.HasValue)
        {
            encounterQuery =
                encounterQuery.Where(x =>
                    x.FacilityId ==
                    facilityId.Value);
        }

        var encounterIds =
            await encounterQuery
                .Select(x => x.EncounterId)
                .ToListAsync();

        var deniedRequests =
            await _context.AuthorizationRequests
                .Where(x =>
                    encounterIds.Contains(
                        x.EncounterId))
                .Where(x =>
                    x.Status ==
                    (byte)RequestStatus.Denied)
                .ToListAsync();

        return new RevenueAtRiskDto
        {
            DeniedRequests =
                deniedRequests.Count,

            RevenueAtRisk =
                deniedRequests.Sum(x =>
                    x.EstimatedTotalAmount)
        };
    }

    public async Task<List<FacilityComparisonDto>>
    GetFacilityComparisonAsync()
    {
        var facilities =
            await _context.Facilities
                .Include(x => x.Encounters)
                .ToListAsync();

        var result =
            new List<FacilityComparisonDto>();

        foreach (var facility in facilities)
        {
            var encounterIds =
                facility.Encounters
                    .Select(x => x.EncounterId)
                    .ToList();

            var requests =
                await _context.AuthorizationRequests
                    .Where(x =>
                        encounterIds.Contains(
                            x.EncounterId))
                    .ToListAsync();

            var totalRequests =
                requests.Count;

            var approvedRequests =
                requests.Count(x =>
                    x.Status ==
                    (byte)RequestStatus.Approved);

            var deniedRequests =
                requests.Count(x =>
                    x.Status ==
                    (byte)RequestStatus.Denied);

            decimal approvalRate = 0;

            if (totalRequests > 0)
            {
                approvalRate =
                    Math.Round(
                        (decimal)approvedRequests /
                        totalRequests * 100,
                        2);
            }

            var approvedRevenue =
                requests
                    .Where(x =>
                        x.Status ==
                        (byte)RequestStatus.Approved)
                    .Sum(x =>
                        x.ApprovedAmount ?? 0);

            var reviewedRequests =
                requests
                    .Where(x =>
                        x.SubmittedAt.HasValue &&
                        x.ReviewedAt.HasValue)
                    .ToList();

            decimal averageResponseDays = 0;

            if (reviewedRequests.Any())
            {
                averageResponseDays =
                    Math.Round(
                        (decimal)reviewedRequests
                            .Average(x =>
                                (x.ReviewedAt!.Value -
                                 x.SubmittedAt!.Value)
                                .TotalDays),
                        2);
            }

            result.Add(
                new FacilityComparisonDto
                {
                    FacilityName =
                        facility.FacilityName,

                    TotalRequests =
                        totalRequests,

                    ApprovedRequests =
                        approvedRequests,

                    DeniedRequests =
                        deniedRequests,

                    ApprovalRate =
                        approvalRate,

                    ApprovedRevenue =
                        approvedRevenue,

                    AverageResponseDays =
                        averageResponseDays
                });
        }

        var ranked =
            result
            .OrderByDescending(x =>
                x.ApprovalRate)
            .ThenByDescending(x =>
                x.ApprovedRevenue)
            .ToList();

        for (int i = 0; i < ranked.Count; i++)
        {
            ranked[i].Rank = i + 1;
        }

        return ranked;
    }

    public async Task<List<TopPerformingPayerDto>>
    GetTopPerformingPayersAsync(
        int? facilityId)
    {
        var encounterQuery =
            _context.Encounters.AsQueryable();

        if (facilityId.HasValue)
        {
            encounterQuery =
                encounterQuery.Where(x =>
                    x.FacilityId ==
                    facilityId.Value);
        }

        var encounterIds =
            await encounterQuery
                .Select(x => x.EncounterId)
                .ToListAsync();

        var requests =
            await _context.AuthorizationRequests
                .Include(x => x.Payer)
                .Where(x =>
                    encounterIds.Contains(
                        x.EncounterId))
                .ToListAsync();

        var result =
            requests
            .GroupBy(x => x.Payer.PayerName)
            .Select(g =>
            {
                var totalRequests =
                    g.Count();

                var approvedRequests =
                    g.Count(x =>
                        x.Status ==
                        (byte)RequestStatus.Approved);

                decimal approvalRate = 0;

                if (totalRequests > 0)
                {
                    approvalRate =
                        Math.Round(
                            (decimal)approvedRequests /
                            totalRequests * 100,
                            2);
                }

                return new TopPerformingPayerDto
                {
                    PayerName = g.Key,

                    TotalRequests =
                        totalRequests,

                    ApprovedRequests =
                        approvedRequests,

                    ApprovalRate =
                        approvalRate
                };
            })
            .OrderByDescending(x =>
                x.ApprovalRate)
            .ThenByDescending(x =>
                x.ApprovedRequests)
            .ToList();

        for (int i = 0; i < result.Count; i++)
        {
            result[i].Rank = i + 1;
        }

        return result;
    }

    public async Task<List<PoorPerformingPayerDto>>
    GetPoorPerformingPayersAsync(
        int? facilityId)
    {
        var encounterQuery =
            _context.Encounters.AsQueryable();

        if (facilityId.HasValue)
        {
            encounterQuery =
                encounterQuery.Where(x =>
                    x.FacilityId ==
                    facilityId.Value);
        }

        var encounterIds =
            await encounterQuery
                .Select(x => x.EncounterId)
                .ToListAsync();

        var requests =
            await _context.AuthorizationRequests
                .Include(x => x.Payer)
                .Where(x =>
                    encounterIds.Contains(
                        x.EncounterId))
                .ToListAsync();

        var result =
            requests
            .GroupBy(x => x.Payer.PayerName)
            .Select(g =>
            {
                var totalRequests =
                    g.Count();

                var deniedRequests =
                    g.Count(x =>
                        x.Status ==
                        (byte)RequestStatus.Denied);

                decimal denialRate = 0;

                if (totalRequests > 0)
                {
                    denialRate =
                        Math.Round(
                            (decimal)deniedRequests /
                            totalRequests * 100,
                            2);
                }

                return new PoorPerformingPayerDto
                {
                    PayerName = g.Key,

                    TotalRequests =
                        totalRequests,

                    DeniedRequests =
                        deniedRequests,

                    DenialRate =
                        denialRate
                };
            })
            .OrderByDescending(x =>
                x.DenialRate)
            .ThenByDescending(x =>
                x.DeniedRequests)
            .ToList();

        for (int i = 0; i < result.Count; i++)
        {
            result[i].Rank = i + 1;
        }

        return result;
    }

    public async Task<List<DelayTrendDto>>
    GetDelayTrendsAsync(
        int? facilityId)
    {
        var encounterQuery =
            _context.Encounters.AsQueryable();

        if (facilityId.HasValue)
        {
            encounterQuery =
                encounterQuery.Where(x =>
                    x.FacilityId ==
                    facilityId.Value);
        }

        var encounterIds =
            await encounterQuery
                .Select(x => x.EncounterId)
                .ToListAsync();

        var requests =
            await _context.AuthorizationRequests
                .Include(x => x.Payer)
                .Where(x =>
                    encounterIds.Contains(
                        x.EncounterId))
                .Where(x =>
                    x.SubmittedAt.HasValue &&
                    x.ReviewedAt.HasValue)
                .ToListAsync();

        var result =
            requests
            .GroupBy(x => x.Payer.PayerName)
            .Select(g =>
            {
                return new DelayTrendDto
                {
                    PayerName = g.Key,

                    ZeroToTwoDays =
                        g.Count(x =>
                            (x.ReviewedAt!.Value -
                             x.SubmittedAt!.Value)
                            .TotalDays <= 2),

                    ThreeToFiveDays =
                        g.Count(x =>
                        {
                            var days =
                                (x.ReviewedAt!.Value -
                                 x.SubmittedAt!.Value)
                                .TotalDays;

                            return days > 2 &&
                                   days <= 5;
                        }),

                    SixToTenDays =
                        g.Count(x =>
                        {
                            var days =
                                (x.ReviewedAt!.Value -
                                 x.SubmittedAt!.Value)
                                .TotalDays;

                            return days > 5 &&
                                   days <= 10;
                        }),

                    MoreThanTenDays =
                        g.Count(x =>
                            (x.ReviewedAt!.Value -
                             x.SubmittedAt!.Value)
                            .TotalDays > 10)
                };
            })
            .OrderBy(x => x.PayerName)
            .ToList();

        return result;
    }
}