using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Shared.Data;
using PriorAuthorization.Shared.Exceptions;
using PriorAuthorization.Specialist.API.Services.Interfaces;
using PriorAuthorizationSpecialist.API.DTOs;
using System;

namespace PriorAuthorization.Specialist.API.Services.Implementations;

public class FacilityService : IFacilityService
{
    private readonly ApplicationDbContext _context;

    public FacilityService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FacilityDto>> GetFacilitiesAsync()
    {
        var facilities = await _context.Facilities
        .Where(f => f.IsActive)
        .ToListAsync();

        if (!facilities.Any())
        {
            throw new NotFoundException(
                "No facilities found.");
        }

        return facilities.Select(f => new FacilityDto
        {
            FacilityId = f.FacilityId,
            FacilityName = f.FacilityName,
            FacilityLocation = f.FacilityLocation
        });
    }
}