using PriorAuthorization.Shared.Entities;
using PriorAuthorization.Shared.Enums;
using PriorAuthorization.Specialist.API.DTOs;

namespace PriorAuthorization.Specialist.API.Services.Interfaces;

public interface IAuthorizationService
{
    Task<int> CreateAuthorizationRequestAsync(
     CreateAuthorizationRequestDto dto);

    Task AddServiceAsync(
        int authId,
        AddAuthorizationServiceDto dto);

    Task RemoveServiceAsync(
        int authId,
        int serviceId);

    Task<List<AuthorizationServiceResponseDto>>
    GetServicesAsync(int authId);

    Task SubmitAuthorizationRequestAsync(
        int authId);
    Task ResubmitAuthorizationAsync(
    int authId,
    ResubmitAuthorizationDto dto);

    Task<List<AuthorizationTimelineDto>>
    GetTimelineAsync(int authId);

    Task<List<AuthorizationTatResponse>>
        GetTatPriorityQueueAsync(int facilityId);
    Task<List<AuthorizationListItemDto>>
    GetAuthorizationsAsync(
        RequestStatus? status);

    Task<List<AuthorizationListItemDto>>
        GetAwaitingReviewAuthorizationsAsync();

}