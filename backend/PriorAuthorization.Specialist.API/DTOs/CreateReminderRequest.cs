namespace PriorAuthorization.Specialist.API.DTOs
{
    public class CreateReminderRequest
    {
        public int AuthId { get; set; }

        public byte Category { get; set; }

        public string? Remarks { get; set; }
    }
}
