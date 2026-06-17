using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Shared.Data;
using PriorAuthorization.Specialist.API.DTOs;
using PriorAuthorization.Specialist.API.Services.Interfaces;

using System;

namespace Specialist.API.Services.Implementations;

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