using PriorAuthorization.Specialist.API.DTOs;

namespace PriorAuthorization.Specialist.API.Services.Interfaces;

public interface IAuthorizationService
{
    Task<int> CreateAuthorizationRequestAsync(
        CreateAuthorizationRequestDto dto);
}