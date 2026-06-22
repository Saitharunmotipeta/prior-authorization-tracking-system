using PriorAuthorization.Specialist.API.DTOs;

namespace PriorAuthorization.Specialist.API.Services.Interfaces;

public interface IEncounterService
{
    Task<int> CreateEncounterAsync(CreateEncounterDto dto);
    Task UpdateEncounterAsync(
        int encounterId,
        UpdateEncounterDto dto);
    Task VerifyEncounterAsync(int encounterId);
}