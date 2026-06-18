using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Manager.API.DTOs.Dashboard;
using PriorAuthorization.Manager.API.Services.Interfaces;
using PriorAuthorization.Shared.Exceptions;
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
            if (filter.FacilityId.Value <= 0)
            {
                throw new ValidationException(
                    "FacilityId must be greater than zero.");
            }

            var facilityExists =
                await _context.Facilities
                    .AnyAsync(x =>
                        x.FacilityId ==
                        filter.FacilityId.Value);

            if (!facilityExists)
            {
                throw new NotFoundException(
                    $"Facility {filter.FacilityId} not found.");
            }

            encounterQuery =
                encounterQuery.Where(x =>
                    x.FacilityId ==
                    filter.FacilityId.Value);
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

    var approvedRevenue =
        await authorizationQuery
            .Where(x =>
                x.Status == (byte)RequestStatus.Approved)
            .SumAsync(x =>
                x.ApprovedAmount ?? 0);

    var reminderQuery =
        _context.Reminders
            .Where(x =>
                encounterIds.Contains(
                    x.Auth.EncounterId));

    var totalReminders =
        await reminderQuery.CountAsync();

    var successfulReminders =
    await reminderQuery.CountAsync(x =>
        x.Status == (byte)ReminderStatus.Completed &&
        x.Auth.Status ==
            (byte)RequestStatus.Approved);

        var approvalRate =
        totalAuthorizationRequests == 0
            ? 0
            : Math.Round(
                (decimal)approvedRequests /
                totalAuthorizationRequests * 100,
                2);

        var denialRate =
            totalAuthorizationRequests == 0
                ? 0
                : Math.Round(
                    (decimal)deniedRequests /
                    totalAuthorizationRequests * 100,
                    2);

        var reminderSuccessRate =
            totalReminders == 0
                ? 0
                : Math.Round(
                    (decimal)successfulReminders /
                    totalReminders * 100,
                    2);

        return new DashboardResponseDto
        {
            TotalEncounters = totalEncounters,

            TotalAuthorizationRequests =
            totalAuthorizationRequests,

            ApprovedRequests =
            approvedRequests,

            DeniedRequests =
            deniedRequests,

            PendingRequests =
            pendingRequests,

            ExpiredRequests =
            expiredRequests,

            ApprovalRate =
            approvalRate,

            DenialRate =
            denialRate,

            ApprovedRevenue =
            approvedRevenue,

            TotalReminders =
            totalReminders,

            SuccessfulReminders =
            successfulReminders,

            ReminderSuccessRate =
            reminderSuccessRate
        };
    }
}