
using System.ComponentModel.DataAnnotations;
namespace PriorAuthorization.Payer.API.DTOs
{

   

    public class ReminderDto
    {
        public int ReminderId { get; set; }

        public int AuthId { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Status { get; set; }

        public DateTime? ScheduledAt { get; set; }

        public DateTime? CompletedAt { get; set; }

        [MaxLength(500)]
        public string? Remarks { get; set; }

        public DateTime UpdatedAt { get; set; }
    }


}
