namespace PriorAuthorization.Specialist.API.DTOs;

public class CreateAuthorizationRequestDto
{
    public int EncounterId { get; set; }

    public int PayerId { get; set; }

    public byte Priority { get; set; }

    public List<AddAuthorizationServiceDto> Services { get; set; }
        = new();
}