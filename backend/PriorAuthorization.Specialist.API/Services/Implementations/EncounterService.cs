using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Shared.Data;
using PriorAuthorization.Shared.Entities;
using PriorAuthorization.Shared.Enums;
using PriorAuthorization.Shared.Exceptions;
using PriorAuthorization.Specialist.API.DTOs;
using PriorAuthorization.Specialist.API.Services.Interfaces;
using System.Text.Json;

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

    public async Task UpdateEncounterAsync(
    int encounterId,
    UpdateEncounterDto dto)
    {
        var encounter = await _context.Encounters
            .FirstOrDefaultAsync(e => e.EncounterId == encounterId);

        if (encounter == null)
        {
            throw new Exception("Encounter not found.");
        }

        var oldValues = new Dictionary<string, object?>();
        var newValues = new Dictionary<string, object?>();

        if (dto.VerificationStatus.HasValue)
        {
            oldValues["verificationStatus"] = encounter.VerificationStatus;

            encounter.VerificationStatus =
                dto.VerificationStatus.Value;

            newValues["verificationStatus"] =
                encounter.VerificationStatus;
        }

        if (dto.IdentificationVerified.HasValue)
        {
            oldValues["identificationVerified"] =
                encounter.IdentificationVerified;

            encounter.IdentificationVerified =
                dto.IdentificationVerified.Value;

            newValues["identificationVerified"] =
                encounter.IdentificationVerified;
        }

        if (dto.PrescriptionVerified.HasValue)
        {
            oldValues["prescriptionVerified"] =
                encounter.PrescriptionVerified;

            encounter.PrescriptionVerified =
                dto.PrescriptionVerified.Value;

            newValues["prescriptionVerified"] =
                encounter.PrescriptionVerified;
        }

        if (dto.ScanVerified.HasValue)
        {
            oldValues["scanVerified"] =
                encounter.ScanVerified;

            encounter.ScanVerified =
                dto.ScanVerified.Value;

            newValues["scanVerified"] =
                encounter.ScanVerified;
        }

        if (dto.DoctorNotesVerified.HasValue)
        {
            oldValues["doctorNotesVerified"] =
                encounter.DoctorNotesVerified;

            encounter.DoctorNotesVerified =
                dto.DoctorNotesVerified.Value;

            newValues["doctorNotesVerified"] =
                encounter.DoctorNotesVerified;
        }

        if (dto.InsuranceCardVerified.HasValue)
        {
            oldValues["insuranceCardVerified"] =
                encounter.InsuranceCardVerified;

            encounter.InsuranceCardVerified =
                dto.InsuranceCardVerified.Value;

            newValues["insuranceCardVerified"] =
                encounter.InsuranceCardVerified;
        }

        if (dto.Remarks != null)
        {
            oldValues["remarks"] = encounter.Remarks;

            encounter.Remarks = dto.Remarks;

            newValues["remarks"] = encounter.Remarks;
        }

        if (oldValues.Count == 0)
        {
            throw new Exception("No fields supplied for update.");
        }

        encounter.UpdatedAt = DateTime.UtcNow;

        var audit = new AuditHistory
        {
            EncounterId = encounter.EncounterId,

            EntityId = $"Encounter-{encounter.EncounterId}",

            ActionType = (byte)AuditActionType.Updated,

            OldValue = JsonSerializer.Serialize(oldValues),

            NewValue = JsonSerializer.Serialize(newValues),

            PerformedByRole = (byte)UserRole.Specialist,

            Remarks = "Encounter updated",

            CreatedAt = DateTime.UtcNow
        };

        _context.AuditHistories.Add(audit);

        await _context.SaveChangesAsync();
    }
    

public async Task VerifyEncounterAsync(
    int encounterId)
{
    var encounter =
        await _context.Encounters
            .FirstOrDefaultAsync(e =>
                e.EncounterId == encounterId);

    if (encounter == null)
    {
        throw new NotFoundException(
            $"Encounter {encounterId} not found.");
    }

    if (!encounter.IdentificationVerified)
    {
        throw new ConflictException(
            "Identification document is not verified.");
    }

    if (!encounter.PrescriptionVerified)
    {
        throw new ConflictException(
            "Prescription document is not verified.");
    }

    if (!encounter.ScanVerified)
    {
        throw new ConflictException(
            "Scan document is not verified.");
    }

    if (!encounter.DoctorNotesVerified)
    {
        throw new ConflictException(
            "Doctor notes are not verified.");
    }

    if (!encounter.InsuranceCardVerified)
    {
        throw new ConflictException(
            "Insurance card is not verified.");
    }

    if (encounter.VerificationStatus ==
        (byte)VerificationStatus.Verified)
    {
        throw new ConflictException(
            "Encounter is already verified.");
    }

    var oldValues =
        new Dictionary<string, object?>
        {
            ["verificationStatus"] =
                Enum.GetName(
                    typeof(VerificationStatus),
                    encounter.VerificationStatus)
        };

    encounter.VerificationStatus =
        (byte)VerificationStatus.Verified;

    encounter.UpdatedAt =
        DateTime.UtcNow;

    var newValues =
        new Dictionary<string, object?>
        {
            ["verificationStatus"] =
                Enum.GetName(
                    typeof(VerificationStatus),
                    encounter.VerificationStatus)
        };

    var audit =
        new AuditHistory
        {
            EncounterId =
                encounter.EncounterId,

            EntityId =
                $"Encounter-{encounter.EncounterId}",

            ActionType =
                (byte)AuditActionType.Updated,

            OldValue =
                JsonSerializer.Serialize(oldValues),

            NewValue =
                JsonSerializer.Serialize(newValues),

            PerformedByRole =
                (byte)UserRole.Specialist,

            Remarks =
                "Encounter verified",

            CreatedAt =
                DateTime.UtcNow
        };

    _context.AuditHistories.Add(audit);

    await _context.SaveChangesAsync();
}

}