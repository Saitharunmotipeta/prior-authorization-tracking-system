using PriorAuthorization.Specialist.API.DTOs;

namespace PriorAuthorization.Specialist.API.Services.Interfaces;

public interface IReminderService
{
    Task GenerateTatRemindersAsync();
}