using Microsoft.AspNetCore.Mvc;
using PriorAuthorization.Shared.Common;
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
}