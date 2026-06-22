using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Payer.API.DTOs;
using PriorAuthorization.Payer.API.Services.Interfaces;
using PriorAuthorization.Shared.Data;
using PriorAuthorization.Shared.Entities;
using PriorAuthorization.Shared.Enums;
using PriorAuthorization.Shared.Exceptions;
using PriorAuthorization.Shared.Utilities;
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


    public async Task<List<FacilityDto>> GetFacilities()
    {
        var stopwatch =
            StopwatchUtility.Start();

        _logger.LogInformation(
            "GetFacilities started");

        var result =
            await _context.Facilities
                .AsNoTracking()
                .Select(f => new FacilityDto
                {
                    FacilityId =
                        f.FacilityId,

                    FacilityName =
                        f.FacilityName,

                    PendingCount =
                        _context.AuthorizationRequests
                            .Count(a =>
                                a.Encounter.FacilityId ==
                                f.FacilityId &&
                                (
                                    a.Status ==
                                    (byte)RequestStatus.Submitted ||

                                    a.Status ==
                                    (byte)RequestStatus.UnderReview ||

                                    a.Status ==
                                    (byte)RequestStatus.AdditionalInfoRequired
                                ))
                })
                .ToListAsync();

        if (!result.Any())
        {
            _logger.LogWarning(
                "No facilities found");

            throw new NotFoundException(
                "No facilities found.");
        }

        var elapsedMs =
            StopwatchUtility.Stop(
                stopwatch);

        _logger.LogInformation(
            "GetFacilities completed in {ElapsedMs} ms. Facilities Returned: {Count}",
            elapsedMs,
            result.Count);

        return result;
    }


    public async Task<List<RequestLists>> GetRequestsByFacility(
       int facilityId)
    {
        var stopwatch =
            StopwatchUtility.Start();

        _logger.LogInformation(
            "GetRequestsByFacility started. FacilityId: {FacilityId}",
            facilityId);

        if (facilityId <= 0)
        {
            _logger.LogWarning(
                "Invalid FacilityId received: {FacilityId}",
                facilityId);

            throw new ValidationException(
                "FacilityId must be greater than zero.");
        }

        var facilityExists =
            await _context.Facilities
                .AsNoTracking()
                .AnyAsync(x =>
                    x.FacilityId ==
                    facilityId);

        if (!facilityExists)
        {
            _logger.LogWarning(
                "Facility not found. FacilityId: {FacilityId}",
                facilityId);

            throw new NotFoundException(
                $"Facility {facilityId} not found.");
        }

        var result =
            await _context.AuthorizationRequests
                .AsNoTracking()
                .Include(a => a.Encounter)
                    .ThenInclude(e => e.Patient)
                .Include(a => a.Encounter.Facility)
                .Where(a =>
                    a.Encounter.FacilityId ==
                    facilityId &&
                    (
                        a.Status ==
                        (byte)RequestStatus.Submitted ||

                        a.Status ==
                        (byte)RequestStatus.UnderReview ||

                        a.Status ==
                        (byte)RequestStatus.AdditionalInfoRequired
                    ))
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

                    ConditionOrder =
                        a.Encounter.ConditionType
                })
                .OrderByDescending(x =>
                    x.ConditionOrder)
                .ThenByDescending(x =>
                    x.Data.SubmittedAt)
                .Select(x =>
                    x.Data)
                .ToListAsync();

        if (!result.Any())
        {
            _logger.LogWarning(
                "No pending requests found for FacilityId: {FacilityId}",
                facilityId);

            throw new NotFoundException(
                $"No pending requests found for facility {facilityId}.");
        }

        var elapsedMs =
            StopwatchUtility.Stop(
                stopwatch);

        _logger.LogInformation(
            "GetRequestsByFacility completed in {ElapsedMs} ms. Requests Returned: {Count}",
            elapsedMs,
            result.Count);

        return result;
    }

    public async Task<RequestsDetails> GetAuthorizationDetails(
        int authId)
    {
        var stopwatch =
            StopwatchUtility.Start();

        _logger.LogInformation(
            "GetAuthorizationDetails started. AuthId: {AuthId}",
            authId);

        if (authId <= 0)
        {
            _logger.LogWarning(
                "Invalid AuthId received: {AuthId}",
                authId);

            throw new ValidationException(
                "AuthId must be greater than zero.");
        }

        var auth =
            await _context.AuthorizationRequests
                .AsNoTracking()
                .Include(a => a.Encounter)
                    .ThenInclude(e => e.Patient)
                .Include(a => a.Encounter.Facility)
                .Include(a => a.Encounter.Department)
                .Include(a => a.AuthorizationServices)
                .FirstOrDefaultAsync(a =>
                    a.AuthId == authId);

        if (auth == null)
        {
            _logger.LogWarning(
                "Authorization not found. AuthId: {AuthId}",
                authId);

            throw new NotFoundException(
                $"Authorization request {authId} not found.");
        }

        var result =
            new RequestsDetails
            {
                AuthId = auth.AuthId,

                EncounterId =
                    auth.EncounterId,

                PatientName =
                    auth.Encounter.Patient.FullName,

                Dob =
                    auth.Encounter.Patient.Dob,

                Gender =
                    auth.Encounter.Patient.Gender,

                PhoneNumber =
                    auth.Encounter.Patient.PhoneNumber,

                FacilityName =
                    auth.Encounter.Facility.FacilityName,

                DepartmentName =
                    auth.Encounter.Department.DepartmentName,

                ConditionType =
                    auth.Encounter.ConditionType.ToString(),

                Priority =
                    auth.Priority.ToString(),

                Status =
                    auth.Status.ToString(),

                EstimatedAmount =
                    auth.EstimatedTotalAmount,

                ApprovedAmount =
                    auth.ApprovedAmount,

                SubmittedAt =
                    auth.SubmittedAt,

                ReviewedAt =
                    auth.ReviewedAt,

                Documents =
                    new DocumentVerificationDto
                    {
                        IdentificationVerified =
                            auth.Encounter.IdentificationVerified,

                        PrescriptionVerified =
                            auth.Encounter.PrescriptionVerified,

                        ScanVerified =
                            auth.Encounter.ScanVerified,

                        DoctorNotesVerified =
                            auth.Encounter.DoctorNotesVerified,

                        InsuranceCardVerified =
                            auth.Encounter.InsuranceCardVerified
                    },

                Services =
                    auth.AuthorizationServices
                        .Select(s =>
                            new ServiceDto
                            {
                                CptCode =
                                    s.CptCode,

                                IcdCode =
                                    s.IcdCode,

                                EstimatedCost =
                                    s.EstimatedCost,

                                Notes =
                                    s.Notes
                            })
                        .ToList()
            };

        var elapsedMs =
            StopwatchUtility.Stop(
                stopwatch);

        _logger.LogInformation(
            "GetAuthorizationDetails completed in {ElapsedMs} ms. AuthId: {AuthId}",
            elapsedMs,
            authId);
        return result;

        }

    public async Task<bool> ReviewAuthorization(
        int authId,
        ReviewRequest dto)
    {
        var stopwatch =
            StopwatchUtility.Start();

        _logger.LogInformation(
            "ReviewAuthorization started. AuthId: {AuthId}, Action: {Action}",
            authId,
            dto?.Action);

        if (authId <= 0)
        {
            _logger.LogWarning(
                "Invalid AuthId received: {AuthId}",
                authId);

            throw new ValidationException(
                "AuthId must be greater than zero.");
        }

        if (dto == null)
        {
            _logger.LogWarning(
                "Review request payload is null. AuthId: {AuthId}",
                authId);

            throw new ValidationException(
                "Review request is required.");
        }

        var auth =
            await _context.AuthorizationRequests
                .FirstOrDefaultAsync(x =>
                    x.AuthId == authId);

        if (auth == null)
        {
            _logger.LogWarning(
                "Authorization not found. AuthId: {AuthId}",
                authId);

            throw new NotFoundException(
                $"Authorization request {authId} not found.");
        }

        var oldStatus =
            auth.Status;

        var oldApprovedAmount =
            auth.ApprovedAmount;

        switch (dto.Action)
        {
            case ReviewActionType.Approve:

                if (!dto.ApprovedAmount.HasValue)
                {
                    throw new ValidationException(
                        "Approved amount is required for approval.");
                }

                if (dto.ApprovedAmount <= 0)
                {
                    throw new ValidationException(
                        "Approved amount must be greater than zero.");
                }

                if (dto.ApprovedAmount >
                    auth.EstimatedTotalAmount)
                {
                    throw new ValidationException(
                        "Approved amount cannot be greater than estimated amount.");
                }

                auth.Status =
                    (byte)RequestStatus.Approved;

                auth.ApprovedAmount =
                    dto.ApprovedAmount;

                auth.ReviewedAt =
                    DateTime.UtcNow;

                break;

            case ReviewActionType.Deny:

                auth.Status =
                    (byte)RequestStatus.Denied;

                auth.ReviewedAt =
                    DateTime.UtcNow;

                break;

            case ReviewActionType.RequestMoreInfo:

                auth.Status =
                    (byte)RequestStatus.AdditionalInfoRequired;

                auth.ReviewedAt =
                    DateTime.UtcNow;

                break;

            default:

                throw new ValidationException(
                    "Invalid review action.");
        }

        auth.UpdatedAt =
            DateTime.UtcNow;

        var audit =
            new AuditHistory
            {
                EncounterId =
                    auth.EncounterId,

                AuthId =
                    auth.AuthId,

                ActionType =
                    dto.Action switch
                    {
                        ReviewActionType.Approve =>
                            (byte)AuditActionType.Approved,

                        ReviewActionType.Deny =>
                            (byte)AuditActionType.Denied,

                        ReviewActionType.RequestMoreInfo =>
                            (byte)AuditActionType.RequestedMoreInfo,

                        _ =>
                            (byte)AuditActionType.Updated
                    },

                OldValue =
                    $"Status: {(RequestStatus)oldStatus}, ApprovedAmount: {oldApprovedAmount}",

                NewValue =
                    $"Status: {(RequestStatus)auth.Status}, ApprovedAmount: {auth.ApprovedAmount}",

                PerformedByRole =
                    (byte)UserRole.Payer,

                Remarks =
                    dto.Remarks,

                CreatedAt =
                    DateTime.UtcNow
            };

        _context.AuditHistories.Add(
            audit);

        await _context.SaveChangesAsync();

        var elapsedMs =
            StopwatchUtility.Stop(
                stopwatch);

        _logger.LogInformation(
            "ReviewAuthorization completed in {ElapsedMs} ms. AuthId: {AuthId}, Action: {Action}",
            elapsedMs,
            authId,
            dto.Action);
        return true;
    }

    public async Task<List<RequestLists>> GetEmergencyRequests()
    {
        var stopwatch =
            StopwatchUtility.Start();

        _logger.LogInformation(
            "GetEmergencyRequests started");

        var result =
            await _context.AuthorizationRequests
                .AsNoTracking()
                .Include(a => a.Encounter)
                    .ThenInclude(e => e.Patient)
                .Include(a => a.Encounter.Facility)
                .Where(a =>
                    a.Encounter.ConditionType ==
                    (byte)ConditionType.Emergency &&
                    (
                        a.Status ==
                        (byte)RequestStatus.Submitted ||

                        a.Status ==
                        (byte)RequestStatus.UnderReview ||

                        a.Status ==
                        (byte)RequestStatus.AdditionalInfoRequired
                    ))
                .Select(a => new RequestLists
                {
                    AuthId =
                        a.AuthId,

                    EncounterId =
                        a.EncounterId,

                    PatientName =
                        a.Encounter.Patient.FullName,

                    FacilityName =
                        a.Encounter.Facility.FacilityName,

                    ConditionType =
                        a.Encounter.ConditionType.ToString(),

                    Priority =
                        a.Priority.ToString(),

                    Status =
                        a.Status.ToString(),

                    EstimatedAmount =
                        a.EstimatedTotalAmount,

                    SubmittedAt =
                        a.SubmittedAt
                })
                .OrderByDescending(x =>
                    x.SubmittedAt)
                .ToListAsync();

        if (!result.Any())
        {
            _logger.LogWarning(
                "No emergency authorization requests found");

            throw new NotFoundException(
                "No emergency authorization requests found.");
        }

        var elapsedMs =
            StopwatchUtility.Stop(
                stopwatch);

        _logger.LogInformation(
            "GetEmergencyRequests completed in {ElapsedMs} ms. Requests Returned: {Count}",
            elapsedMs,
            result.Count);

        return result;
    }

    public async Task<ReminderListResponseDto> GetReminders()
    {
        var stopwatch =
            StopwatchUtility.Start();

        _logger.LogInformation(
            "GetReminders started");

        var reminders =
            await _context.Reminders
                .AsNoTracking()
                .OrderBy(r =>
                    r.Status)
                .ThenByDescending(r =>
                    r.ScheduledAt)
                .ToListAsync();

        if (!reminders.Any())
        {
            _logger.LogWarning(
                "No reminders found");

            throw new NotFoundException(
                "No reminders found.");
        }

        var pendingCount =
            reminders.Count(r =>
                r.Status ==
                (byte)ReminderStatus.Requested ||

                r.Status ==
                (byte)ReminderStatus.Scheduled);

        var data =
            reminders
                .Select(r =>
                    new ReminderDto
                    {
                        ReminderId =
                            r.ReminderId,

                        AuthId =
                            r.AuthId,

                        Category =
                            ((ReminderCategory)r.Category)
                            .ToString(),

                        Status =
                            ((ReminderStatus)r.Status)
                            .ToString(),

                        ScheduledAt =
                            r.ScheduledAt,

                        CompletedAt =
                            r.CompletedAt,

                        Remarks =
                            r.Remarks,

                        UpdatedAt =
                            r.UpdatedAt
                    })
                .ToList();

        var result =
            new ReminderListResponseDto
            {
                PendingCount =
                    pendingCount,

                Data =
                    data
            };

        var elapsedMs =
            StopwatchUtility.Stop(
                stopwatch);

        _logger.LogInformation(
            "GetReminders completed in {ElapsedMs} ms. Total Reminders: {TotalCount}, Pending Reminders: {PendingCount}",
            elapsedMs,
            data.Count,
            pendingCount);

        return result;
    }

}