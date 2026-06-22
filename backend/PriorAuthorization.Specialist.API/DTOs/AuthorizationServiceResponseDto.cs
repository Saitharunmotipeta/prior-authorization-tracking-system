namespace PriorAuthorization.Specialist.API.DTOs
{
    public class AuthorizationServiceResponseDto
    {
        public int ServiceId { get; set; }

        public string CptCode { get; set; } = string.Empty;

        public string IcdCode { get; set; } = string.Empty;

        public decimal EstimatedCost { get; set; }

        public string? Notes { get; set; }
    }
}
