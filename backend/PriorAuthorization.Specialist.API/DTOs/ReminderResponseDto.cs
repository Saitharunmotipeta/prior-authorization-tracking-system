namespace PriorAuthorization.Specialist.API.DTOs
{
    public class ReminderResponseDto
    {
        public int ReminderId { get; set; }

        public int AuthId { get; set; }

        public int PayerId { get; set; }

        public byte Category { get; set; }

        public byte Status { get; set; }

        public DateTime? ScheduledAt { get; set; }

        public DateTime? CompletedAt { get; set; }

        public string? Remarks { get; set; }
    }
}
