namespace PriorAuthorization.Payer.API.DTOs
{
   
        public class ReminderDto
        {
            public int ReminderId { get; set; }

            public int AuthId { get; set; }

            public string Category { get; set; }

            public string Status { get; set; }

            public DateTime? ScheduledAt { get; set; }

            public DateTime? CompletedAt { get; set; }

            public string? Remarks { get; set; }

            public DateTime UpdatedAt { get; set; }
        }
    
}
