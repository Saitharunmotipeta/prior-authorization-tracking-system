using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Payer.API.DTOs;
using PriorAuthorization.Payer.API.Services.Interfaces;
using PriorAuthorization.Shared.Data;
using PriorAuthorization.Shared.Entities;
using PriorAuthorization.Shared.Enums;
using System;

public class PayerService : IPayerService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PayerService> _logger;

    public PayerService(ApplicationDbContext context, ILogger<PayerService> logger)
    {
        _context = context;
        _logger = logger;
    }


    public async Task<List<FacilityDto>> GetFacilities()
    {
        _logger.LogInformation("Fetching facilities with pending request counts");

        var result = await _context.Facilities
            .AsNoTracking()
            .Select(f => new FacilityDto
            {
                FacilityId = f.FacilityId,
                FacilityName = f.FacilityName,

                // ✅ Count pending requests
                PendingCount = _context.AuthorizationRequests
                    .Count(a =>
                        a.Encounter.FacilityId == f.FacilityId &&

                        (
                            a.Status == (byte)RequestStatus.Submitted ||
                            a.Status == (byte)RequestStatus.UnderReview ||
                            a.Status == (byte)RequestStatus.AdditionalInfoRequired
                        )
                    )
            })
            .ToListAsync();

        _logger.LogInformation("Fetched {Count} facilities", result.Count);

        return result;
    }



    public async Task<List<RequestLists>> GetRequestsByFacility(int facilityId)
    {
        var result = await _context.AuthorizationRequests
            .AsNoTracking()
            .Include(a => a.Encounter)
                .ThenInclude(e => e.Patient)
            .Include(a => a.Encounter.Facility)
            .Where(a =>
                a.Encounter.FacilityId == facilityId &&

                // ✅ Only pending
                (
                    a.Status == (byte)RequestStatus.Submitted ||
                    a.Status == (byte)RequestStatus.UnderReview ||
                    a.Status == (byte)RequestStatus.AdditionalInfoRequired
                )
            )

            // ✅ Sorting Logic
            .Select(a => new
            {
                Data = new RequestLists
                {
                    AuthId = a.AuthId,
                    EncounterId = a.EncounterId,
                    PatientName = a.Encounter.Patient.FullName,
                    FacilityName = a.Encounter.Facility.FacilityName,
                    ConditionType = a.Encounter.ConditionType.ToString(),
                    Priority = a.Priority.ToString(),
                    Status = a.Status.ToString(),
                    EstimatedAmount = a.EstimatedTotalAmount,
                    SubmittedAt = a.SubmittedAt
                },
                ConditionOrder = a.Encounter.ConditionType
            })

            
            .OrderByDescending(x => x.ConditionOrder)

           
            .ThenByDescending(x => x.Data.SubmittedAt)

            .Select(x => x.Data)
            .ToListAsync();

        return result;
    }


    public async Task<RequestsDetails> GetAuthorizationDetails(int authId)
    {
        _logger.LogInformation("Fetching authorization details for AuthId: {AuthId}", authId);

        var auth = await _context.AuthorizationRequests
            .AsNoTracking()
            .Include(a => a.Encounter)
                .ThenInclude(e => e.Patient)
            .Include(a => a.Encounter.Facility)
            .Include(a => a.Encounter.Department)
            .Include(a => a.AuthorizationServices)
            .FirstOrDefaultAsync(a => a.AuthId == authId);

        if (auth == null)
        {
            _logger.LogWarning("Authorization not found for AuthId: {AuthId}", authId);
            return null;
        }

        var result = new RequestsDetails
        {
            AuthId = auth.AuthId,
            EncounterId = auth.EncounterId,

            // ✅ Patient Info
            PatientName = auth.Encounter.Patient.FullName,

            Dob = auth.Encounter.Patient.Dob,
            Gender = auth.Encounter.Patient.Gender,
            PhoneNumber = auth.Encounter.Patient.PhoneNumber,

            // ✅ Facility & Department
            FacilityName = auth.Encounter.Facility.FacilityName,
            DepartmentName = auth.Encounter.Department.DepartmentName,

            // ✅ Condition & Priority
            ConditionType = auth.Encounter.ConditionType.ToString(),
            Priority = auth.Priority.ToString(),

            // ✅ Authorization Info
            Status = auth.Status.ToString(),
            EstimatedAmount = auth.EstimatedTotalAmount,
            ApprovedAmount = auth.ApprovedAmount,
            SubmittedAt = auth.SubmittedAt,
            ReviewedAt = auth.ReviewedAt,

            // ✅ Document Verification
            Documents = new DocumentVerificationDto
            {
                IdentificationVerified = auth.Encounter.IdentificationVerified,
                PrescriptionVerified = auth.Encounter.PrescriptionVerified,
                ScanVerified = auth.Encounter.ScanVerified,
                DoctorNotesVerified = auth.Encounter.DoctorNotesVerified,
                InsuranceCardVerified = auth.Encounter.InsuranceCardVerified
            },

            // ✅ Services (CPT / ICD)
            Services = auth.AuthorizationServices
                .Select(s => new ServiceDto
                {
                    CptCode = s.CptCode,
                    IcdCode = s.IcdCode,
                    EstimatedCost = s.EstimatedCost,
                    Notes = s.Notes
                })
                .ToList()
        };

        _logger.LogInformation("Successfully fetched authorization details for AuthId: {AuthId}", authId);

        return result;
    }

    public async Task<bool> ReviewAuthorization(int authId, ReviewRequest dto)
    {
        var auth = await _context.AuthorizationRequests
            .FirstOrDefaultAsync(x => x.AuthId == authId);

        if (auth == null)
        {
            _logger.LogWarning("Authorization not found for AuthId: {AuthId}", authId);
            return false;
        }

        // ✅ Capture OLD values for audit
        var oldStatus = auth.Status;
        var oldApprovedAmount = auth.ApprovedAmount;

        // ✅ Handle Actions
        switch (dto.Action)
        {
            case ReviewActionType.Approve:

                // ✅ VALIDATION
                if (!dto.ApprovedAmount.HasValue)
                    throw new Exception("Approved amount is required for approval");

                if (dto.ApprovedAmount > auth.EstimatedTotalAmount)
                    throw new Exception("Approved amount cannot be greater than estimated amount");

                auth.Status = (byte)RequestStatus.Approved;
                auth.ApprovedAmount = dto.ApprovedAmount;
                auth.ReviewedAt = DateTime.UtcNow;
                break;


            case ReviewActionType.Deny:
                auth.Status = (byte)RequestStatus.Denied;
                auth.ReviewedAt = DateTime.UtcNow;
                break;


            case ReviewActionType.RequestMoreInfo:
                auth.Status = (byte)RequestStatus.AdditionalInfoRequired;
                auth.ReviewedAt = DateTime.UtcNow;
                break;


            default:
                throw new Exception("Invalid review action");
        }

        // ✅ Always update UpdatedAt
        auth.UpdatedAt = DateTime.UtcNow;

        // ✅ CREATE AUDIT ENTRY (VERY IMPORTANT)
        var audit = new AuditHistory
        {
            EncounterId = auth.EncounterId,
            AuthId = auth.AuthId,

            ActionType = dto.Action switch
            {
                ReviewActionType.Approve => (byte)AuditActionType.Approved,
                ReviewActionType.Deny => (byte)AuditActionType.Denied,
                ReviewActionType.RequestMoreInfo => (byte)AuditActionType.RequestedMoreInfo,
                _ => (byte)AuditActionType.Updated
            },

            // ✅ OLD values
            OldValue = $"Status: {(RequestStatus)oldStatus}, ApprovedAmount: {oldApprovedAmount}",

            // ✅ NEW values
            NewValue = $"Status: {(RequestStatus)auth.Status}, ApprovedAmount: {auth.ApprovedAmount}",

            PerformedByRole = (byte)UserRole.Payer,

            Remarks = dto.Remarks,

            CreatedAt = DateTime.UtcNow
        };

        _context.AuditHistories.Add(audit);

        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Review action {Action} completed for AuthId: {AuthId}",
            dto.Action, authId);

        return true;
    }

    public async Task<List<RequestLists>> GetEmergencyRequests()
    {
        _logger.LogInformation("Fetching emergency pending requests");

        var result = await _context.AuthorizationRequests
            .AsNoTracking()
            .Include(a => a.Encounter)
                .ThenInclude(e => e.Patient)
            .Include(a => a.Encounter.Facility)

            // ✅ Filter: Emergency + Pending
            .Where(a =>
    a.Encounter.ConditionType == (byte)ConditionType.Emergency &&

    (
        a.Status == (byte)RequestStatus.Submitted ||
        a.Status == (byte)RequestStatus.UnderReview ||
        a.Status == (byte)RequestStatus.AdditionalInfoRequired
    )
)
            // ✅ Projection
            .Select(a => new RequestLists
            {
                AuthId = a.AuthId,
                EncounterId = a.EncounterId,
                PatientName = a.Encounter.Patient.FullName,
                FacilityName = a.Encounter.Facility.FacilityName,
                ConditionType = a.Encounter.ConditionType.ToString(),
                Priority = a.Priority.ToString(),
                Status = a.Status.ToString(),
                EstimatedAmount = a.EstimatedTotalAmount,
                SubmittedAt = a.SubmittedAt
            })

            // ✅ Sort latest first
            .OrderByDescending(x => x.SubmittedAt)

            .ToListAsync();

        _logger.LogInformation("Fetched {Count} emergency requests", result.Count);

        return result;
    }



    public async Task<ReminderListResponseDto> GetReminders()
    {
        _logger.LogInformation("Fetching reminders with pending count");

        var reminders = await _context.Reminders
            .AsNoTracking()
            .OrderBy(r => r.Status)                 // Pending first
            .ThenByDescending(r => r.ScheduledAt)
            .ToListAsync();

        // ✅ Count only pending
        var pendingCount = reminders.Count(r =>
    r.Status == (byte)ReminderStatus.Requested ||
    r.Status == (byte)ReminderStatus.Scheduled
);

        // ✅ Map data
        var data = reminders.Select(r => new ReminderDto
        {
            ReminderId = r.ReminderId,
            AuthId = r.AuthId,

            Category = ((ReminderCategory)r.Category).ToString(),
            Status = ((ReminderStatus)r.Status).ToString(),

            ScheduledAt = r.ScheduledAt,
            CompletedAt = r.CompletedAt,

            Remarks = r.Remarks,
            UpdatedAt = r.UpdatedAt
        }).ToList();

        return new ReminderListResponseDto
        {
            PendingCount = pendingCount,
            Data = data
        };
    }

}