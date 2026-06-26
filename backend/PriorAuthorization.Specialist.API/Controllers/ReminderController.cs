using Microsoft.AspNetCore.Mvc;
using PriorAuthorization.Shared.Common;
using PriorAuthorization.Specialist.API.Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class ReminderController : ControllerBase
{
    private readonly IReminderService _reminderService;

    public ReminderController(
        IReminderService reminderService)
    {
        _reminderService = reminderService;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> Generate()
    {
        await _reminderService
            .GenerateTatRemindersAsync();

        return Ok(
            ApiResponse<string>
                .SuccessResponse(
                    string.Empty,
                    "Reminders generated successfully."));
    }
}