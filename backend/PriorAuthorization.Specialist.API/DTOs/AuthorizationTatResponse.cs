namespace PriorAuthorization.Specialist.API.DTOs
{
    public class AuthorizationTatResponse
    {
        public int AuthId { get; set; }

        public string PayerName { get; set; } = string.Empty;

        public byte Priority { get; set; }

        public DateTime SubmittedAt { get; set; }

        public int TatDays { get; set; }

        public DateTime ExpectedReviewDate { get; set; }

        public int DaysLeft { get; set; }

        public string TatStatus { get; set; } = string.Empty;
    }
}
