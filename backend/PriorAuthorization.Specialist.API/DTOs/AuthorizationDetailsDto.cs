namespace PriorAuthorization.Specialist.API.DTOs
{
    public class AuthorizationDetailsDto
    {
        public int AuthId { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public string PayerName { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string Priority { get; set; } = string.Empty;

        public decimal EstimatedAmount { get; set; }

        public decimal? ApprovedAmount { get; set; }

        public DateTime? SubmittedAt { get; set; }

        public DateTime? ReviewedAt { get; set; }

        //public DateTime? ExpirationDate { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}