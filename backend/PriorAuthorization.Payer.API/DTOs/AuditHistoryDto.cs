namespace PriorAuthorization.Payer.API.DTOs
{
    public class AuditHistoryDto
    {
        public int AuditId { get; set; }

        public int? AuthId { get; set; }

        public string ActionType { get; set; }

        public string? Remarks { get; set; }

        public string? OldValue { get; set; }

        public string? NewValue { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
