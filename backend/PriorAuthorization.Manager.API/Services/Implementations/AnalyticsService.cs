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
}