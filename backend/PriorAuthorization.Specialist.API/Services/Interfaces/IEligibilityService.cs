using PriorAuthorization.Specialist.API.DTOs;

namespace PriorAuthorization.Specialist.API.Services.Interfaces
{
    public interface IEligibilityService
    {
        Task<EligibilityResponseDto> VerifyEligibilityAsync(Guid patientId);
    }
}
