namespace PriorAuthorization.Specialist.API.DTOs
{
    public class AuthorizationTimelineDto
    {
        public string Action { get; set; } = string.Empty;

        public string? Remarks { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
