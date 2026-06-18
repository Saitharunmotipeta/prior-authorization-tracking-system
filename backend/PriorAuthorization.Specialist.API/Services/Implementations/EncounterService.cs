using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Shared.Data;
using PriorAuthorization.Shared.Entities;
using PriorAuthorization.Shared.Enums;
using PriorAuthorization.Specialist.API.DTOs;
using PriorAuthorization.Specialist.API.Services.Interfaces;

namespace PriorAuthorization.Specialist.API.Services.Implementations;

public class EncounterService : IEncounterService
{
    private readonly ApplicationDbContext _context;

    public EncounterService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateEncounterAsync(CreateEncounterDto dto)
    {
        var patientExists = await _context.Patients
            .AnyAsync(p => p.PatientId == dto.PatientId);

        if (!patientExists)
            throw new Exception("Patient not found.");

        var facilityExists = await _context.Facilities
            .AnyAsync(f => f.FacilityId == dto.FacilityId);

        if (!facilityExists)
            throw new Exception("Facility not found.");

        var departmentExists = await _context.Departments
            .AnyAsync(d =>
                d.DepartmentId == dto.DepartmentId &&
                d.FacilityId == dto.FacilityId);

        if (!departmentExists)
            throw new Exception("Department does not belong to selected facility.");

        var encounter = new Encounter
        {
            PatientId = dto.PatientId,
            FacilityId = dto.FacilityId,
            DepartmentId = dto.DepartmentId,
            ConditionType = dto.ConditionType,

            VerificationStatus = (byte)VerificationStatus.Pending,
            RequestStatus = (byte)RequestStatus.Draft,

            IdentificationVerified = false,
            PrescriptionVerified = false,
            ScanVerified = false,
            DoctorNotesVerified = false,
            InsuranceCardVerified = false,

            Remarks = null,

            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,

            IsActive = true
        };

        _context.Encounters.Add(encounter);

        await _context.SaveChangesAsync();

        var audit = new AuditHistory
        {
            EncounterId = encounter.EncounterId,

            EntityId = encounter.EncounterId.ToString(),

            ActionType = (byte)AuditActionType.Created,

            PerformedByRole = (byte)UserRole.Specialist,

            Remarks = "Encounter created",

            CreatedAt = DateTime.UtcNow
        };

        _context.AuditHistories.Add(audit);

        await _context.SaveChangesAsync();

        return encounter.EncounterId;
    }
}