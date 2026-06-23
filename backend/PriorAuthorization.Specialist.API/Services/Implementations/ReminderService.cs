using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Shared.Data;
using PriorAuthorization.Shared.Entities;
using PriorAuthorization.Shared.Enums;
using PriorAuthorization.Specialist.API.DTOs;
using PriorAuthorization.Specialist.API.Services.Interfaces;

namespace PriorAuthorization.Specialist.API.Services;

public class ReminderService : IReminderService
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;

    public ReminderService(
        ApplicationDbContext context,
        IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    public async Task CreateReminderAsync(
        CreateReminderRequest request)
    {
        var authorizationRequest =
            await _context.AuthorizationRequests
                .FirstOrDefaultAsync(x =>
                    x.AuthId == request.AuthId);

        if (authorizationRequest == null)
        {
            throw new Exception(
                "Authorization request not found.");
        }

        var isClosedAuth =
            authorizationRequest.Status ==
                (byte)RequestStatus.Approved ||
            authorizationRequest.Status ==
                (byte)RequestStatus.Denied ||
            authorizationRequest.Status ==
                (byte)RequestStatus.Expired;

        if (isClosedAuth)
        {
            var activeReminders =
                await _context.Reminders
                    .Where(x =>
                        x.AuthId == request.AuthId &&
                        x.Status != (byte)ReminderStatus.Completed &&
                        x.Status != (byte)ReminderStatus.Cancelled)
                    .ToListAsync();

            if (!activeReminders.Any())
            {
                return;
            }

            foreach (var activeReminder in activeReminders)
            {
                activeReminder.Status =
                    (byte)ReminderStatus.Completed;

                activeReminder.CompletedAt =
                    DateTime.UtcNow;

                activeReminder.UpdatedAt =
                    DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return;
        }

        var existingReminder =
            await _context.Reminders
                .FirstOrDefaultAsync(x =>
                    x.AuthId == request.AuthId &&
                    x.Category == request.Category &&
                    x.Status != (byte)ReminderStatus.Completed &&
                    x.Status != (byte)ReminderStatus.Cancelled);

        if (existingReminder != null)
        {
            throw new Exception(
                "An active reminder already exists for this category.");
        }

        var reminder = new Reminder
        {
            AuthId = authorizationRequest.AuthId,
            PayerId = authorizationRequest.PayerId,
            Category = request.Category,
            Status = (byte)ReminderStatus.Requested,
            Remarks = request.Remarks,
            UpdatedAt = DateTime.UtcNow
        };

        await _context.Reminders.AddAsync(reminder);

        await _context.SaveChangesAsync();

        var payer =
            await _context.Payers
                .FirstOrDefaultAsync(x =>
                    x.PayerId ==
                    authorizationRequest.PayerId);

        if (payer != null &&
            !string.IsNullOrWhiteSpace(
                payer.PayerEmail))
        {
            await _emailService.SendAsync(
                payer.PayerEmail,
                $"Reminder - Authorization Request #{authorizationRequest.AuthId}",
                $"""
            Reminder Type:
            {((ReminderCategory)request.Category)}

            Authorization Request:
            {authorizationRequest.AuthId}

            Remarks:
            {request.Remarks}

            Please review the authorization request and provide an update.
            """);
        }
    }
    public async Task<List<ReminderDto>> GetRemindersByAuthIdAsync(
        int authId)
    {
        var reminders =
            await _context.Reminders
                .Where(x => x.AuthId == authId)
                .OrderByDescending(x => x.UpdatedAt)
                .Select(x => new ReminderDto
                {
                    ReminderId = x.ReminderId,

                    Category =
                        ((ReminderCategory)x.Category)
                        .ToString(),

                    Status =
                        ((ReminderStatus)x.Status)
                        .ToString(),

                    Remarks = x.Remarks,

                    UpdatedAt = x.UpdatedAt
                })
                .ToListAsync();

        return reminders;
    }

    public async Task UpdateReminderStatusAsync(
        int reminderId,
        UpdateReminderStatusRequest request)
    {
        var reminder =
            await _context.Reminders
                .FirstOrDefaultAsync(x =>
                    x.ReminderId == reminderId);

        if (reminder == null)
        {
            throw new Exception(
                $"Reminder {reminderId} not found.");
        }

        if (!Enum.IsDefined(
            typeof(ReminderStatus),
            request.Status))
        {
            throw new Exception(
                "Invalid reminder status.");
        }

        reminder.Status = request.Status;

        reminder.Remarks = request.Remarks;

        reminder.UpdatedAt = DateTime.UtcNow;

        if (request.Status ==
            (byte)ReminderStatus.Scheduled)
        {
            reminder.ScheduledAt =
                DateTime.UtcNow;
        }

        if (request.Status ==
            (byte)ReminderStatus.Completed)
        {
            reminder.CompletedAt =
                DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
    }

    public async Task<List<ReminderResponseDto>> GetRemindersAsync(
     int? facilityId,
     int? payerId,
     byte? status)
    {
        var query =
            from r in _context.Reminders
            join ar in _context.AuthorizationRequests
                on r.AuthId equals ar.AuthId
            join e in _context.Encounters
                on ar.EncounterId equals e.EncounterId
            select new
            {
                Reminder = r,
                FacilityId = e.FacilityId
            };

        if (facilityId.HasValue)
        {
            query = query.Where(x =>
                x.FacilityId == facilityId.Value);
        }

        if (payerId.HasValue)
        {
            query = query.Where(x =>
                x.Reminder.PayerId == payerId.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(x =>
                x.Reminder.Status == status.Value);
        }

        return await query
            .Select(x => new ReminderResponseDto
            {
                ReminderId = x.Reminder.ReminderId,
                AuthId = x.Reminder.AuthId,
                PayerId = x.Reminder.PayerId,
                Category = x.Reminder.Category,
                Status = x.Reminder.Status,
                ScheduledAt = x.Reminder.ScheduledAt,
                CompletedAt = x.Reminder.CompletedAt,
                Remarks = x.Reminder.Remarks
            })
            .ToListAsync();
    }
}