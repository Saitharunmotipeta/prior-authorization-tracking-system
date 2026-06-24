using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Shared.Data;
using PriorAuthorization.Specialist.API.DTOs;
using PriorAuthorization.Specialist.API.Services.Interfaces;

namespace PriorAuthorization.Specialist.API.Services;

public class ICDCodeService : IICDCodeService
{
    private readonly ApplicationDbContext _context;

    public ICDCodeService(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ICDCodeDto>> GetICDCodesAsync()
    {
        return await _context.Icdcodes
            .Select(i => new ICDCodeDto
            {
                ICDCode = i.IcdCode1,
                ICDDescription = i.IcdDescription
            })
            .ToListAsync();
    }
}