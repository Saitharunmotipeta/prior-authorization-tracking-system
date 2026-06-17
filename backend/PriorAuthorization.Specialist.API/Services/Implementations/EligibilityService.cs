using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Shared.Data;
using PriorAuthorization.Shared.Entities;
using PriorAuthorization.Specialist.API.DTOs;
using PriorAuthorization.Specialist.API.Services.Interfaces;

namespace Specialist.API.Services.Implementations;

public class EligibilityService : IEligibilityService
{
    private readonly ApplicationDbContext _context;

    public EligibilityService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EligibilityResponseDto> VerifyEligibilityAsync(Guid patientId)
    {
        var policy = await _context.Policies
            .FirstOrDefaultAsync(p => p.PatientId == patientId);

        if (policy == null)
        {
            return new EligibilityResponseDto
            {
                IsEligible = false,
                Message = "Patient not found."
            };
        }

        if (policy == null)
        {
            return new EligibilityResponseDto
            {
                IsEligible = false,
                Message = "No insurance policy found."
            };
        }

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        bool eligible = today <= policy.PolicyExpiryDate;

        return new EligibilityResponseDto
        {
            IsEligible = eligible,
            PolicyId = policy.PolicyId,
            PolicyExpiryDate = policy.PolicyExpiryDate,
            Message = eligible
                ? "Insurance coverage is active."
                : "Insurance coverage has expired."
        };
    }
}