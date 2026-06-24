using PriorAuthorization.Specialist.API.DTOs;
namespace PriorAuthorization.Specialist.API.Services.Interfaces;
    public interface IICDCodeService
    {
        Task<List<ICDCodeDto>> GetICDCodesAsync();
    }

