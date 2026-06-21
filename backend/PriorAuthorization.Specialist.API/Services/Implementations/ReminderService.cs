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

    public ReminderService(
       ApplicationDbContext context)
    {
        _context = context;
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
}