using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Payer.API.DTOs;
using PriorAuthorization.Payer.API.Services.Interfaces;
using PriorAuthorization.Shared.Data;
using PriorAuthorization.Shared.Entities;
using PriorAuthorization.Shared.Enums;
using PriorAuthorization.Shared.Exceptions;
using PriorAuthorization.Shared.Middleware;
using PriorAuthorization.Shared.Validations;
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

    private static readonly byte[] PendingStatuses =
{
    (byte)RequestStatus.Submitted,
    (byte)RequestStatus.UnderReview,
    (byte)RequestStatus.AdditionalInfoRequired
};


    public async Task<List<FacilityDto>> GetFacilities()
    {
        _logger.LogInformation("Fetching facilities with pending request counts");

        try
        {
            
           


            var result = await _context.Facilities
     .AsNoTracking()
     .Select(f => new FacilityDto
     {
         FacilityId = f.FacilityId,
         FacilityName = f.FacilityName,
         PendingCount = _context.AuthorizationRequests
             .Where(a =>
                 a.Encounter.FacilityId == f.FacilityId &&
                 PendingStatuses.Contains(a.Status))
             .Count()
     })
     .ToListAsync();

            _logger.LogInformation("Fetched {Count} facilities", result.Count);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching facilities");

            throw new InternalServerException("Failed to fetch facilities");

        }
    }


    public async Task<List<RequestLists>> GetRequestsByFacility(int facilityId)
    {
        _logger.LogInformation("Fetching authorization requests for FacilityId: {FacilityId}", facilityId);

        try
        {
            // ✅ Validate input
            if (facilityId <= 0)
                throw new BadRequestException("Invalid facility id");

            // ✅ Optional: Check if facility exists (important edge case)
            var facilityExists = await _context.Facilities
                .AnyAsync(f => f.FacilityId == facilityId);

            if (!facilityExists)
                throw new NotFoundException("Facility not found");

            var result = await _context.AuthorizationRequests
                .AsNoTracking()

                // ✅ Filtering FIRST (important for performance)
                .Where(a =>
                    a.Encounter.FacilityId == facilityId &&
                    (
                       PendingStatuses.Contains(a.Status)
                    )
                )

                // ✅ Projection (EF will auto join only required data)
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

                // ✅ Sorting (priority-based queue)
                .OrderByDescending(x => x.ConditionOrder)        // Emergency → Urgent → Normal
                .ThenByDescending(x => x.Data.SubmittedAt)       // latest first

                .Select(x => x.Data)
                .ToListAsync();

            _logger.LogInformation(
                "Fetched {Count} requests for FacilityId: {FacilityId}",
                result.Count,
                facilityId);

            return result;
        }
        catch (AppException)
        {
            throw; // ✅ Already handled properly
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error occurred while fetching requests for FacilityId: {FacilityId}",
                facilityId);

            throw new InternalServerException("Failed to fetch authorization requests");
        }
    }

    public async Task<RequestsDetails> GetAuthorizationDetails(int authId)
    {
        _logger.LogInformation("Fetching authorization details for AuthId: {AuthId}", authId);

        try
        {
            // ✅ Validate input
            if (authId <= 0)
                throw new BadRequestException("Invalid authorization id");

            var auth = await _context.AuthorizationRequests
                .AsNoTracking()

                // ✅ Project directly (NO Include needed)
                .Where(a => a.AuthId == authId)
                .Select(a => new RequestsDetails
                {
                    AuthId = a.AuthId,
                    EncounterId = a.EncounterId,

                    // ✅ Patient Info
                    PatientName = a.Encounter.Patient.FullName,
                    Dob = a.Encounter.Patient.Dob,
                    Gender = a.Encounter.Patient.Gender,
                    PhoneNumber = a.Encounter.Patient.PhoneNumber,

                    // ✅ Facility & Department
                    FacilityName = a.Encounter.Facility.FacilityName,
                    DepartmentName = a.Encounter.Department.DepartmentName,

                    // ✅ Condition & Priority
                    ConditionType = a.Encounter.ConditionType.ToString(),
                    Priority = a.Priority.ToString(),

                    // ✅ Authorization Info
                    Status = a.Status.ToString(),
                    EstimatedAmount = a.EstimatedTotalAmount,
                    ApprovedAmount = a.ApprovedAmount,
                    SubmittedAt = a.SubmittedAt,
                    ReviewedAt = a.ReviewedAt,

                    // ✅ Documents
                    Documents = new DocumentVerificationDto
                    {
                        IdentificationVerified = a.Encounter.IdentificationVerified,
                        PrescriptionVerified = a.Encounter.PrescriptionVerified,
                        ScanVerified = a.Encounter.ScanVerified,
                        DoctorNotesVerified = a.Encounter.DoctorNotesVerified,
                        InsuranceCardVerified = a.Encounter.InsuranceCardVerified
                    },

                    // ✅ Services
                    Services = a.AuthorizationServices
                        .Select(s => new ServiceDto
                        {
                            CptCode = s.CptCode,
                            IcdCode = s.IcdCode,
                            EstimatedCost = s.EstimatedCost,
                            Notes = s.Notes
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            // ✅ Not found case
            if (auth == null)
            {
                _logger.LogWarning("Authorization not found for AuthId: {AuthId}", authId);
                throw new NotFoundException("Authorization request not found");
            }

            _logger.LogInformation("Successfully fetched authorization details for AuthId: {AuthId}", authId);

            return auth;
        }
        catch (AppException)
        {
            throw; // already handled properly
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error occurred while fetching authorization details for AuthId: {AuthId}",
                authId);

            throw new InternalServerException("Failed to fetch authorization details");
        }
    }

    public async Task<bool> ReviewAuthorization(int authId, ReviewRequest dto)
    {
        _logger.LogInformation("Review action {Action} started for AuthId: {AuthId}", dto.Action, authId);

        try
        {
            // ✅ Validate input
            if (authId <= 0)
                throw new BadRequestException("Invalid authorization id");

            if (dto == null)
                throw new BadRequestException("Request data is required");

            var auth = await _context.AuthorizationRequests
                .FirstOrDefaultAsync(x => x.AuthId == authId);

            // ✅ Not found
            if (auth == null)
            {
                _logger.LogWarning("Authorization not found for AuthId: {AuthId}", authId);
                throw new NotFoundException("Authorization request not found");
            }

            // ✅ Prevent re-processing (VERY important)
            if (auth.Status == (byte)RequestStatus.Approved ||
                auth.Status == (byte)RequestStatus.Denied)
            {
                throw new BadRequestException("Authorization already finalized. Cannot modify further.");
            }

            var oldStatus = auth.Status;
            var oldApprovedAmount = auth.ApprovedAmount;

            switch (dto.Action)
            {
                case ReviewActionType.Approve:

                    // ✅ Validation
                    if (!dto.ApprovedAmount.HasValue)
                        throw new BadRequestException("Approved amount is required");

                    if (dto.ApprovedAmount <= 0)
                        throw new BadRequestException("Approved amount must be greater than 0");

                    if (dto.ApprovedAmount > auth.EstimatedTotalAmount)
                        throw new BadRequestException("Approved amount cannot exceed estimated amount");

                    auth.Status = (byte)RequestStatus.Approved;
                    auth.ApprovedAmount = dto.ApprovedAmount;
                    auth.ReviewedAt = DateTime.UtcNow;
                    break;

                case ReviewActionType.Deny:

                    if (string.IsNullOrWhiteSpace(dto.Remarks))
                        throw new BadRequestException("Remarks are required when denying a request");

                    auth.Status = (byte)RequestStatus.Denied;
                    auth.ReviewedAt = DateTime.UtcNow;
                    break;

                case ReviewActionType.RequestMoreInfo:

                    if (string.IsNullOrWhiteSpace(dto.Remarks))
                        throw new BadRequestException("Remarks are required when requesting more information");

                    auth.Status = (byte)RequestStatus.AdditionalInfoRequired;
                    auth.ReviewedAt = DateTime.UtcNow;
                    break;

                default:
                    throw new BadRequestException("Invalid review action");
            }

            // ✅ Always update UpdatedAt
            auth.UpdatedAt = DateTime.UtcNow;

            // ✅ Audit log
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

                OldValue = $"Status: {(RequestStatus)oldStatus}, ApprovedAmount: {oldApprovedAmount}",

                NewValue = $"Status: {(RequestStatus)auth.Status}, ApprovedAmount: {auth.ApprovedAmount}",

                PerformedByRole = (byte)UserRole.Payer,

                Remarks = dto.Remarks,

                CreatedAt = DateTime.UtcNow
            };
            
            _context.AuditHistories.Add(audit);

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Review action {Action} completed successfully for AuthId: {AuthId}",
                dto.Action, authId);

            return true;
        }
        catch (AppException)
        {
            throw; // ✅ handled by middleware
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error occurred while reviewing authorization for AuthId: {AuthId}",
                authId);

            throw new InternalServerException("Failed to process authorization review");
        }
    }

    public async Task<List<RequestLists>> GetEmergencyRequests()
    {
        _logger.LogInformation("Fetching emergency pending requests");

        try
        {
            var result = await _context.AuthorizationRequests
                .AsNoTracking()

                // ✅ Filter FIRST (important for performance)
                .Where(a =>
                    a.Encounter.ConditionType == (byte)ConditionType.Emergency &&
                    (
                        PendingStatuses.Contains(a.Status)
                    )
                )

                // ✅ Projection (NO Include needed)
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

                // ✅ Sorting (latest first)
                .OrderByDescending(x => x.SubmittedAt)

                .ToListAsync();

            _logger.LogInformation("Fetched {Count} emergency requests", result.Count);

            return result;
        }
        catch (AppException)
        {
            throw; // ✅ already handled
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching emergency requests");

            throw new InternalServerException("Failed to fetch emergency requests");
        }
    }

    public async Task<ReminderListResponseDto> GetReminders()
    {
        _logger.LogInformation("Fetching reminders with pending count");

        try
        {
            // ✅ Fetch only required fields (no full entity load)
            var reminders = await _context.Reminders
                .AsNoTracking()

                // ✅ Sorting: active first (Requested + Scheduled), then latest
                .OrderBy(r =>
                    r.Status == (byte)ReminderStatus.Completed ||
                    r.Status == (byte)ReminderStatus.Cancelled
                )
                .ThenByDescending(r => r.ScheduledAt)

                // ✅ Projection
                .Select(r => new ReminderDto
                {
                    ReminderId = r.ReminderId,
                    AuthId = r.AuthId,
                    Category = ((ReminderCategory)r.Category).ToString(),
                    Status = ((ReminderStatus)r.Status).ToString(),
                    ScheduledAt = r.ScheduledAt,
                    CompletedAt = r.CompletedAt,
                    Remarks = r.Remarks,
                    UpdatedAt = r.UpdatedAt
                })
                .ToListAsync();

            // ✅ Pending (Active) Count: Requested + Scheduled
            var pendingCount = await _context.Reminders
                .Where(r =>
                    r.Status == (byte)ReminderStatus.Requested ||
                    r.Status == (byte)ReminderStatus.Scheduled
                )
                .CountAsync();

            _logger.LogInformation("Fetched {Count} reminders, PendingCount: {PendingCount}",
                reminders.Count, pendingCount);

            return new ReminderListResponseDto
            {
                PendingCount = pendingCount,
                Data = reminders
            };
        }
        catch (AppException)
        {
            throw; // ✅ already handled
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching reminders");

            throw new InternalServerException("Failed to fetch reminders");
        }
    }

}


