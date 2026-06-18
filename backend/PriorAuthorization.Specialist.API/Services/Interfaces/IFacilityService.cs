using PriorAuthorizationSpecialist.API.DTOs;

namespace PriorAuthorization.Specialist.API.Services.Interfaces
{
    public interface IFacilityService
    {
        Task<IEnumerable<FacilityDto>> GetFacilitiesAsync();
    }
}
