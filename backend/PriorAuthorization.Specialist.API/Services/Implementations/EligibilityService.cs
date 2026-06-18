using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Shared.Data;
using PriorAuthorization.Shared.Entities;
using PriorAuthorization.Shared.Exceptions;
using PriorAuthorization.Specialist.API.DTOs;
using PriorAuthorization.Specialist.API.Services.Interfaces;

namespace PriorAuthorization.Specialist.API.Services.Implementations;

public class EligibilityService : IEligibilityService
{
    private readonly ApplicationDbContext _context;

    public EligibilityService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EligibilityResponseDto>
     VerifyEligibilityAsync(Guid patientId)
    {
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.PatientId == patientId);

        if (patient == null)
        {
            throw new NotFoundException(
                "Patient not found.");
        }

        var policy = await _context.Policies
            .FirstOrDefaultAsync(p => p.PatientId == patientId);

        if (policy == null)
        {
            throw new NotFoundException(
                "No insurance policy found.");
        }

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        bool eligible =
            policy.IsActive &&
            today <= policy.PolicyExpiryDate;

        return new EligibilityResponseDto
        {
            IsEligible = eligible,
            PolicyId = policy.PolicyId,
            PolicyExpiryDate = policy.PolicyExpiryDate,
            Message = eligible
                ? "Insurance coverage is active."
                : "Insurance coverage is inactive or expired."
        };
    }
}