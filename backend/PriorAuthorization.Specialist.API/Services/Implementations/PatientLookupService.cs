using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Shared.Data;
using PriorAuthorization.Specialist.API.DTOs;
using PriorAuthorization.Specialist.API.Services.Interfaces;

namespace PriorAuthorization.Specialist.API.Services.Implementations;

public class PatientLookupService : IPatientLookupService
{
    private readonly ApplicationDbContext _context;

    public PatientLookupService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PatientLookupDto?> GetByMrnAsync(string mrnNumber)
    {
        return await _context.Patients
            .Where(p => p.MrnNumber == mrnNumber)
            .Select(p => new PatientLookupDto
            {
                MrnNumber = p.MrnNumber,
                PatientName = p.FullName, 
                Age = p.Age
            })
            .FirstOrDefaultAsync();
    }
}