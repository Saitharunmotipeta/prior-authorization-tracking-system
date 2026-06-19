using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Shared.Data;
using PriorAuthorization.Shared.Exceptions;
using PriorAuthorization.Specialist.API.DTOs;
using PriorAuthorization.Specialist.API.Services.Interfaces;

namespace PriorAuthorizationSpecialist.API.Services.Implementations;

public class DepartmentService : IDepartmentService
{
    private readonly ApplicationDbContext _context;

    public DepartmentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DepartmentDto>>
        GetDepartmentsByFacilityAsync(int facilityId)
    {
        if (facilityId <= 0)
        {
            throw new ValidationException(
                "Valid Facility Id is required.");
        }

        bool facilityExists = await _context.Facilities
            .AnyAsync(f =>
                f.FacilityId == facilityId &&
                f.IsActive);

        if (!facilityExists)
        {
            throw new NotFoundException(
                "Facility not found.");
        }

        return await _context.Departments
            .Where(d =>
                d.FacilityId == facilityId &&
                d.IsActive)
            .OrderBy(d => d.DepartmentId)
            .Select(d => new DepartmentDto
            {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.DepartmentName
            })
            .ToListAsync();
    }
}