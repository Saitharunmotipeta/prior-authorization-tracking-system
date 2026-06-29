namespace PriorAuthorization.Specialist.API.DTOs
{
    public class SpecialistReminderDto
    {
        public int ReminderId { get; set; }

        public int AuthId { get; set; }

        public string PayerName { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; }
    }
}
