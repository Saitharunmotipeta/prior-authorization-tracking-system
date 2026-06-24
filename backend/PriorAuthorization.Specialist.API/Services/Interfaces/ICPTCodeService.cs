using PriorAuthorization.Specialist.API.DTOs;

namespace PriorAuthorization.Specialist.API.Services.Interfaces
{
public interface ICPTCodeService
{
        Task<List<CPTCodeDto>> GetCPTCodesAsync();
    }
}
