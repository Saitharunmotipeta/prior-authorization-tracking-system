using Microsoft.AspNetCore.Mvc;
using PriorAuthorization.Shared.Common;
using PriorAuthorization.Specialist.API.DTOs;
using PriorAuthorization.Specialist.API.Services.Interfaces;

namespace PriorAuthorization.Specialist.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EncounterController : ControllerBase
{
    private readonly IEncounterService _encounterService;

    public EncounterController(IEncounterService encounterService)
    {
        _encounterService = encounterService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateEncounter(
        CreateEncounterDto dto)
    {
        var encounterId =
            await _encounterService.CreateEncounterAsync(dto);

        return Ok(
            ApiResponse<int>.SuccessResponse(
                encounterId,
                "Encounter created successfully."));
    }

    [HttpPatch("{encounterId}")]
    public async Task<IActionResult> UpdateEncounter(
        int encounterId,
        UpdateEncounterDto dto)
    {
        await _encounterService
            .UpdateEncounterAsync(
                encounterId,
                dto);

        return Ok(
            ApiResponse<string>.SuccessResponse(
                null,
                "Encounter updated successfully."));
    }

    [HttpPatch("{encounterId}/verify")]
    public async Task<IActionResult> VerifyEncounter(
        int encounterId)
    {
        await _encounterService
            .VerifyEncounterAsync(encounterId);

        return Ok(
            ApiResponse<string>.SuccessResponse(
                null,
                "Encounter verified successfully."));
    }
}