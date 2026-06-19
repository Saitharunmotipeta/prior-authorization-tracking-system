using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Manager.API.DTOs.Analytics;
using PriorAuthorization.Manager.API.Services.Interfaces;
using PriorAuthorization.Shared.Data;
using PriorAuthorization.Shared.Entities;
using PriorAuthorization.Shared.Enums;
using PriorAuthorization.Shared.Exceptions;

namespace PriorAuthorization.Manager.API.Services.Implementations;

public class AnalyticsService : IAnalyticsService
{
    private readonly ApplicationDbContext _context;

    public AnalyticsService(
        ApplicationDbContext context)
    {
        _context = context;
    }

    private async Task<List<int>> GetEncounterIdsAsync(
    int? facilityId)
    {
        if (facilityId.HasValue)
        {
            if (facilityId.Value <= 0)
            {
                throw new ValidationException(
                    "FacilityId must be greater than zero.");
            }

            var facilityExists =
                await _context.Facilities
                    .AsNoTracking()
                    .AnyAsync(x =>
                        x.FacilityId ==
                        facilityId.Value);

            if (!facilityExists)
            {
                throw new NotFoundException(
                    $"Facility {facilityId} not found.");
            }
        }

        var query =
            _context.Encounters
                .AsNoTracking();

        if (facilityId.HasValue)
        {
            query =
                query.Where(x =>
                    x.FacilityId ==
                    facilityId.Value);
        }

        return await query
            .Select(x =>
                x.EncounterId)
            .ToListAsync();
    }

    private IQueryable<AuthorizationRequest>
    GetAuthorizationRequestsQuery(
        List<int> encounterIds)
    {
        return _context.AuthorizationRequests
            .AsNoTracking()
            .Include(x => x.Payer)
            .Where(x =>
                encounterIds.Contains(
                    x.EncounterId));
    }

    private IQueryable<AuthorizationRequest>
    GetReviewedAuthorizationRequestsQuery(
        List<int> encounterIds)
    {
        return GetAuthorizationRequestsQuery(
                encounterIds)
            .Where(x =>
                x.SubmittedAt.HasValue &&
                x.ReviewedAt.HasValue);
    }

    public async Task<List<PayerPerformanceDto>>
    GetPayerPerformanceAsync(
        int? facilityId)
    {
        var encounterIds =
            await GetEncounterIdsAsync(
                facilityId);

        var requests =
            await GetAuthorizationRequestsQuery(
                encounterIds)
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
    GetSlowestPayersAsync(
        int? facilityId)
    {
        var encounterIds =
            await GetEncounterIdsAsync(
                facilityId);

        var requests =
            await GetReviewedAuthorizationRequestsQuery(
                encounterIds)
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
    GetRevenueAtRiskAsync(
        int? facilityId)
    {
        var encounterIds =
            await GetEncounterIdsAsync(
                facilityId);

        var deniedRequests =
            await _context.AuthorizationRequests
                .AsNoTracking()
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
                .AsNoTracking()
                .Include(x => x.Encounters)
                .ToListAsync();

        var allRequests =
            await _context.AuthorizationRequests
                .AsNoTracking()
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
                allRequests
                    .Where(x =>
                        encounterIds.Contains(
                            x.EncounterId))
                    .ToList();

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

            var approvalRate =
                totalRequests == 0
                    ? 0
                    : Math.Round(
                        (decimal)approvedRequests /
                        totalRequests * 100,
                        2);

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

            var averageResponseDays =
                reviewedRequests.Any()
                    ? Math.Round(
                        (decimal)reviewedRequests
                            .Average(x =>
                                (x.ReviewedAt!.Value -
                                 x.SubmittedAt!.Value)
                                .TotalDays),
                        2)
                    : 0;

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
        var encounterIds =
            await GetEncounterIdsAsync(
                facilityId);

        var requests =
            await GetAuthorizationRequestsQuery(
                encounterIds)
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

                var approvalRate =
                    totalRequests == 0
                        ? 0
                        : Math.Round(
                            (decimal)approvedRequests /
                            totalRequests * 100,
                            2);

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

        var rankedResult =
    result
        .Select((item, index) =>
        {
            item.Rank = index + 1;
            return item;
        })
        .ToList();

        return rankedResult;
    }

    public async Task<List<PoorPerformingPayerDto>>
    GetPoorPerformingPayersAsync(
        int? facilityId)
    {
        var encounterIds =
            await GetEncounterIdsAsync(
                facilityId);

        var requests =
            await GetAuthorizationRequestsQuery(
                encounterIds)
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

                var denialRate =
                    totalRequests == 0
                        ? 0
                        : Math.Round(
                            (decimal)deniedRequests /
                            totalRequests * 100,
                            2);

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
        var encounterIds =
            await GetEncounterIdsAsync(
                facilityId);

        var requests =
            await GetReviewedAuthorizationRequestsQuery(
                encounterIds)
                .ToListAsync();

        var result =
            requests
            .GroupBy(x => x.Payer.PayerName)
            .Select(g =>
            {
                var delays =
                    g.Select(x =>
                        (x.ReviewedAt!.Value -
                         x.SubmittedAt!.Value)
                        .TotalDays)
                    .ToList();

                return new DelayTrendDto
                {
                    PayerName = g.Key,

                    ZeroToTwoDays =
                        delays.Count(x =>
                            x <= 2),

                    ThreeToFiveDays =
                        delays.Count(x =>
                            x > 2 &&
                            x <= 5),

                    SixToTenDays =
                        delays.Count(x =>
                            x > 5 &&
                            x <= 10),

                    MoreThanTenDays =
                        delays.Count(x =>
                            x > 10)
                };
            })
            .OrderBy(x =>
                x.PayerName)
            .ToList();

        return result;
    }
}