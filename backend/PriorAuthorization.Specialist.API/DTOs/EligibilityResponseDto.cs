namespace PriorAuthorization.Specialist.API.DTOs
{
    public class EligibilityResponseDto
    {
        public bool IsEligible { get; set; }

        public int PolicyId { get; set; }

        public string Message { get; set; } = string.Empty;

        public DateOnly PolicyExpiryDate { get; set; }
    }
}
