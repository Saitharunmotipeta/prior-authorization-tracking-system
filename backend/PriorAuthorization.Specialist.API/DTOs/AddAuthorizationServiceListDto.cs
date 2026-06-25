namespace PriorAuthorization.Specialist.API.DTOs;
public class AddAuthorizationServiceListDto
{
    public List<AddAuthorizationServiceDto> Services
    {
        get;
        set;
    } = new();
}
