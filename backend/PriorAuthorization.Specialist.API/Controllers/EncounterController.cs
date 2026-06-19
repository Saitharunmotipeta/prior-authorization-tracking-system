using Microsoft.AspNetCore.Mvc;
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

        return Ok(new
        {
            EncounterId = encounterId,
            Message = "Encounter created successfully."
        });
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

        return Ok(new
        {
            Message = "Encounter updated successfully."
        });
    }
}