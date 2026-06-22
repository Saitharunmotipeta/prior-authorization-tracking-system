namespace PriorAuthorization.Specialist.API.DTOs
{
    public class ReminderDto
    {
        public int ReminderId { get; set; }

        public string Category { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string? Remarks { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
