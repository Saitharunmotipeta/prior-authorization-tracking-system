namespace PriorAuthorization.Payer.API.DTOs
{
    public class AuditHistoryDto
    {
        public int AuditId { get; set; }

        public int? AuthId { get; set; }

        public string ActionType { get; set; }

        public string? Remarks { get; set; }

        public DateTime CreatedAt { get; set; }

        public string PatientId { get; set; } 
    public string FacilityName { get; set; } 
}
}
