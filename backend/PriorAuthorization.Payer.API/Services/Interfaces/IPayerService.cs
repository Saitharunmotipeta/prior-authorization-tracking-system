using PriorAuthorization.Payer.API.DTOs;

namespace PriorAuthorization.Payer.API.Services.Interfaces
{
    public interface IPayerService
    {
        Task<List<RequestLists>> GetAuthorizationRequests(
            int payerId,
            RequestsFilter filter);
        Task<RequestsDetails> GetAuthorizationDetails(int authId);
        Task<bool> ReviewAuthorization(int authId, ReviewRequest dto);

    }
}