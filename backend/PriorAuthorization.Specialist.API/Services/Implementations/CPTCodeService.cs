using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Shared.Data;
using PriorAuthorization.Shared.Entities;
using PriorAuthorization.Shared.Enums;
using PriorAuthorization.Specialist.API.DTOs;
using PriorAuthorization.Specialist.API.Services.Interfaces;

namespace PriorAuthorization.Specialist.API.Services;
public class CPTCodeService : ICPTCodeService
{
    private readonly ApplicationDbContext _context;

    public CPTCodeService(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CPTCodeDto>> GetCPTCodesAsync()
    {
        return await _context.Cptcodes
            .Select(c => new CPTCodeDto
            {
                CPTCode = c.CptCode1,
                CPTDescription = c.CptDescription
            })
            .ToListAsync();
    }
}

