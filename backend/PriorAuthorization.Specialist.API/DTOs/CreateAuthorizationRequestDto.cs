namespace PriorAuthorization.Specialist.API.DTOs;

public class CreateAuthorizationRequestDto
{
    public int EncounterId { get; set; }

    public int PayerId { get; set; }

    public byte Priority { get; set; }

    public List<CreateAuthorizationServiceDto> Services { get; set; }
        = new();
}