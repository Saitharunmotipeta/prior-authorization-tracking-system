namespace PriorAuthorization.Payer.API.DTOs
{
    public class ReminderListResponseDto
    {
        public int PendingCount { get; set; }

        public List<ReminderDto> Data { get; set; } = new();
    }
}
