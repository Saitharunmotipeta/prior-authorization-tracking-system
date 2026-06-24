using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Shared.Data;
using PriorAuthorization.Shared.Entities;
using PriorAuthorization.Shared.Enums;
using PriorAuthorization.Specialist.API.Services.Interfaces;

namespace PriorAuthorization.Specialist.API.Services.Implementations;

public class ReminderService : IReminderService
{
    private readonly ApplicationDbContext _context;

    public ReminderService(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task GenerateTatRemindersAsync()
    {
        var today =
            DateTime.UtcNow.Date;

        var requests =
            await _context.AuthorizationRequests
                .Include(a => a.Payer)
                .Where(a =>
                    a.Status ==
                        (byte)RequestStatus.Submitted ||

                    a.Status ==
                        (byte)RequestStatus.ReSubmitted)
                .ToListAsync();

        foreach (var auth in requests)
        {
            var tatDays =
                auth.Priority ==
                (byte)Priority.Normal
                    ? auth.Payer.NormalTatDays
                    : auth.Payer.UrgentTatDays;

            var expectedReviewDate =
                auth.SubmittedAt!.Value
                    .Date
                    .AddDays(tatDays);

            if (today ==
                expectedReviewDate.AddDays(-3))
            {
                await CreateReminderIfNotExists(
                    auth,
                    "Authorization #{auth.AuthId} review deadline approaching.");
            }

            else if (today ==
                     expectedReviewDate)
            {
                await CreateReminderIfNotExists(
                    auth,
                    "Authorization #{auth.AuthId} review deadline reached.");
            }

            else if (today >
                     expectedReviewDate)
            {
                await CreateReminderIfNotExists(
                    auth,
                    "Authorization #{auth.AuthId} review SLA exceeded.");
            }
        }

        await _context.SaveChangesAsync();
    }
    private async Task CreateReminderIfNotExists(
    AuthorizationRequest auth,
    string message)
    {
        var exists =
            await _context.Reminders
                .AnyAsync(r =>
                    r.AuthId == auth.AuthId &&
                    r.Remarks == message &&
                    r.Status ==
                        (byte)ReminderStatus.Pending);

        if (exists)
        {
            return;
        }

        var reminder =
            new Reminder
            {
                AuthId =
                    auth.AuthId,

                PayerId =
                    auth.PayerId,

                Category =
                    (byte)ReminderCategory.FollowUp,

                Status =
                    (byte)ReminderStatus.Pending,

                ScheduledAt =
                    DateTime.UtcNow,

                UpdatedAt =
                    DateTime.UtcNow,

                Remarks =
                    message
            };

        _context.Reminders.Add(reminder);
        _context.AuditHistories.Add(
    new AuditHistory
    {
        AuthId =
            auth.AuthId,

        EncounterId =
            auth.EncounterId,

        ActionType =
            (byte)AuditActionType.ReminderCreated,

        PerformedByRole =
            (byte)UserRole.System,

        Remarks =
            message,

        CreatedAt =
            DateTime.UtcNow
    });
    }
}