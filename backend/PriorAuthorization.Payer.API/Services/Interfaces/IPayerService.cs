using PriorAuthorization.Payer.API.DTOs;

namespace PriorAuthorization.Payer.API.Services.Interfaces
{
    public interface IPayerService
    {
        Task<List<FacilityDto>> GetFacilities();
        Task<List<RequestLists>> GetRequestsByFacility(int facilityId);
        Task<RequestsDetails> GetAuthorizationDetails(int authId);
        Task<bool> ReviewAuthorization(int authId, ReviewRequest dto);
        Task<List<RequestLists>> GetEmergencyRequests();
        Task<ReminderListResponseDto> GetReminders();


    }
}