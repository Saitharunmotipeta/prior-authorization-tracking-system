namespace PriorAuthorization.Specialist.API.DTOs;

public class UpdateReminderStatusRequest
{
    public byte Status { get; set; }

    public string? Remarks { get; set; }
}