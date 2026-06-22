namespace PriorAuthorization.Specialist.API.DTOs;
public class AddAuthorizationServiceDto
{
    public string CptCode { get; set; } = string.Empty;

    public string IcdCode { get; set; } = string.Empty;

    public string? Notes { get; set; }
}