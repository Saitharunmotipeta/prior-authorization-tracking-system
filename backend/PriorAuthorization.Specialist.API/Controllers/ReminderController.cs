using Microsoft.AspNetCore.Mvc;
using PriorAuthorization.Specialist.API.DTOs;
using PriorAuthorization.Specialist.API.Services.Interfaces;

namespace PriorAuthorization.Specialist.API.Controllers;

[ApiController]
[Route("api/reminders")]
public class ReminderController : ControllerBase
{
    private readonly IReminderService _reminderService;

    public ReminderController(
        IReminderService reminderService)
    {
        _reminderService = reminderService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateReminder(
        [FromBody] CreateReminderRequest request)
    {
        await _reminderService
            .CreateReminderAsync(request);

        return Ok(
            "Reminder created successfully.");
    }

    [HttpGet("auth/{authId}")]
    public async Task<IActionResult> GetRemindersByAuthId(
    int authId)
    {
        var reminders =
            await _reminderService
                .GetRemindersByAuthIdAsync(authId);

        return Ok(reminders);
    }

    [HttpPatch("{reminderId}/status")]
    public async Task<IActionResult> UpdateReminderStatus(
        int reminderId,
        UpdateReminderStatusRequest request)
    {
        await _reminderService
            .UpdateReminderStatusAsync(
                reminderId,
                request);

        return Ok(
            "Reminder status updated successfully.");
    }
    [HttpGet]
    public async Task<IActionResult> GetReminders(
        [FromQuery] int? facilityId,
        [FromQuery] int? payerId,
        [FromQuery] byte? status)
    {
        var reminders = await _reminderService
            .GetRemindersAsync(
                facilityId,
                payerId,
                status);

        return Ok(reminders);
    }
}