using PriorAuthorization.Specialist.API.DTOs;

namespace PriorAuthorization.Specialist.API.Services.Interfaces;

public interface IPatientLookupService
{
    Task<PatientLookupDto?> GetByMrnAsync(string mrnNumber);
}