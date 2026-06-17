using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Manager.API.DTOs.Dashboard;
using PriorAuthorization.Manager.API.Services.Interfaces;
using PriorAuthorization.Shared.Data;
using PriorAuthorization.Shared.Enums;

namespace PriorAuthorization.Manager.API.Services.Implementations;

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _context;

    public DashboardService(
        ApplicationDbContext context)
    {
        _context = context;
    }

public async Task<DashboardResponseDto> GetDashboardAsync(
    DashboardFilterDto filter)
{
    var encounterQuery = _context.Encounters.AsQueryable();

    if (filter.FacilityId.HasValue)
    {
        encounterQuery = encounterQuery.Where(x =>
            x.FacilityId == filter.FacilityId.Value);
    }

    var totalEncounters =
        await encounterQuery.CountAsync();

    var encounterIds =
        await encounterQuery
            .Select(x => x.EncounterId)
            .ToListAsync();

    var authorizationQuery =
        _context.AuthorizationRequests
            .Where(x =>
                encounterIds.Contains(x.EncounterId));

    var totalAuthorizationRequests =
        await authorizationQuery.CountAsync();

    var approvedRequests =
        await authorizationQuery.CountAsync(x =>
            x.Status == (byte)RequestStatus.Approved);

    var deniedRequests =
        await authorizationQuery.CountAsync(x =>
            x.Status == (byte)RequestStatus.Denied);

    var pendingRequests =
        await authorizationQuery.CountAsync(x =>
            x.Status == (byte)RequestStatus.UnderReview ||
            x.Status == (byte)RequestStatus.AdditionalInfoRequired ||
            x.Status == (byte)RequestStatus.ReSubmitted);

    var expiredRequests =
        await authorizationQuery.CountAsync(x =>
            x.Status == (byte)RequestStatus.Expired);

    decimal approvalRate = 0;
    decimal denialRate = 0;

    if (totalAuthorizationRequests > 0)
    {
        approvalRate = Math.Round(
            (decimal)approvedRequests /
            totalAuthorizationRequests * 100,
            2);

        denialRate = Math.Round(
            (decimal)deniedRequests /
            totalAuthorizationRequests * 100,
            2);
    }

    return new DashboardResponseDto
    {
        TotalEncounters = totalEncounters,

        TotalAuthorizationRequests =
            totalAuthorizationRequests,

        ApprovedRequests = approvedRequests,

        DeniedRequests = deniedRequests,

        PendingRequests = pendingRequests,

        ExpiredRequests = expiredRequests,

        ApprovalRate = approvalRate,

        DenialRate = denialRate
    };
}
}