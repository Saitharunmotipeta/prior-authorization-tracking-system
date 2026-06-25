using Microsoft.AspNetCore.Mvc;
using PriorAuthorization.Shared.Common;
using PriorAuthorization.Shared.Entities;
using PriorAuthorization.Shared.Enums;
using PriorAuthorization.Specialist.API.DTOs;
using PriorAuthorization.Specialist.API.Services.Interfaces;

namespace PriorAuthorization.Specialist.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorizationController : ControllerBase
{
    private readonly IAuthorizationService _authorizationService;

    public AuthorizationController(
        IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAuthorization(
        CreateAuthorizationRequestDto dto)
    {
        var authId =
            await _authorizationService
                .CreateAuthorizationRequestAsync(dto);

        return Ok(
            ApiResponse<int>.SuccessResponse(
                authId,
                "Authorization request created successfully."));
    }

    [HttpPost("{authId}/services")]
    public async Task<IActionResult> AddServices(
        int authId,
        AddAuthorizationServiceListDto dto)
    {
        var total =
            await _authorizationService
                .AddServicesAsync(
                    authId,
                    dto);

        return Ok(
            ApiResponse<decimal>
                .SuccessResponse(
                    total,
                    "Services added successfully."));
    }

    [HttpDelete("{authId}/services/{serviceId}")]
        public async Task<IActionResult> RemoveService(
            int authId,
            int serviceId)
        {
            await _authorizationService
                .RemoveServiceAsync(
                    authId,
                    serviceId);

            return Ok(
                ApiResponse<string>.SuccessResponse(
                    string.Empty,
                    "Service removed successfully."));
        }

    [HttpGet("{authId}/services")]
    public async Task<IActionResult> GetServices(
        int authId)
    {
        var services =
            await _authorizationService
                .GetServicesAsync(authId);

        return Ok(
     ApiResponse<List<AuthorizationServiceResponseDto>>
         .SuccessResponse(
             services,
             "Services retrieved successfully."));
    }

    [HttpPatch("{authId}/submit")]
    public async Task<IActionResult> SubmitAuthorizationRequest(
        int authId)
    {
        await _authorizationService
            .SubmitAuthorizationRequestAsync(authId);

        return Ok(
            ApiResponse<string>.SuccessResponse(
                string.Empty,
                "Authorization request submitted successfully."));
    }
    [HttpPatch("{authId}/resubmit")]
    public async Task<IActionResult> ResubmitAuthorization(
    int authId,
    ResubmitAuthorizationDto dto)
    {
        await _authorizationService
            .ResubmitAuthorizationAsync(
                authId,
                dto);

        return Ok(
            ApiResponse<string>.SuccessResponse(
                string.Empty,
                "Authorization request resubmitted successfully."));
    }
    [HttpGet("{authId}/timeline")]
    public async Task<IActionResult> GetTimeline(
    int authId)
    {
        var timeline =
            await _authorizationService
                .GetTimelineAsync(authId);

        return Ok(
            ApiResponse<List<AuthorizationTimelineDto>>
                .SuccessResponse(
                    timeline,
                    "Timeline retrieved successfully."));
    }
    [HttpGet("tat-priority")]
    public async Task<IActionResult>
        GetTatPriorityQueue(
            [FromQuery] int facilityId)
    {
        var result =
            await _authorizationService
                .GetTatPriorityQueueAsync(
                    facilityId);

        return Ok(result);
    }
    [HttpGet]
    public async Task<IActionResult>
    GetAuthorizations(
        [FromQuery]
        RequestStatus? status)
    {
        var result =
            await _authorizationService
                .GetAuthorizationsAsync(
                    status);

        return Ok(
            ApiResponse<
                List<AuthorizationListItemDto>>
                .SuccessResponse(
                    result,
                    "Authorizations retrieved successfully."));
    }
    [HttpGet("awaiting-review")]
    public async Task<IActionResult>
    GetAwaitingReview()
    {
        var result =
            await _authorizationService
                .GetAwaitingReviewAuthorizationsAsync();

        return Ok(
            ApiResponse<
                List<AuthorizationListItemDto>>
                .SuccessResponse(
                    result,
                    "Awaiting review authorizations retrieved successfully."));
    }
}