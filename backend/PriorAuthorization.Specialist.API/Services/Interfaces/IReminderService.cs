using PriorAuthorization.Specialist.API.DTOs;

namespace PriorAuthorization.Specialist.API.Services.Interfaces;

public interface IReminderService
{
    Task CreateReminderAsync(CreateReminderRequest request);

    Task<List<ReminderDto>> GetRemindersByAuthIdAsync(
        int authId);
    Task UpdateReminderStatusAsync(
    int reminderId,
    UpdateReminderStatusRequest request);

    Task<List<ReminderResponseDto>> GetRemindersAsync(
    int? facilityId,
    int? payerId,
    byte? status);
}