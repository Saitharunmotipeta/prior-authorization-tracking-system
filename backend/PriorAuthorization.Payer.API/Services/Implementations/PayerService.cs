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

    public async Task<List<RequestLists>> GetAuthorizationRequests(
        int payerId,
        RequestsFilter filter)
    {
        _logger.LogInformation("Fetching authorization requests for payer {PayerId}", payerId);

       
        var query = _context.AuthorizationRequests
            .AsNoTracking()
            .Include(a => a.Encounter)
                .ThenInclude(e => e.Patient)
            .Include(a => a.Encounter.Facility)
            .Where(a => a.PayerId == payerId);

       

        if (filter?.ConditionType != null)
        {
            query = query.Where(a =>
                (int)a.Encounter.ConditionType == filter.ConditionType);
        }

        if (filter?.FacilityId != null)
        {
            query = query.Where(a =>
                a.Encounter.FacilityId == filter.FacilityId);
        }


       
        var result = await query
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
            .OrderByDescending(x => x.SubmittedAt) 
            .ToListAsync();

        _logger.LogInformation("Fetched {Count} records for payer {PayerId}",
            result.Count, payerId);

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

        // ✅ Handle actions
        switch (dto.Action)
        {
            case ReviewActionType.Approve:
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
                break;

            default:
                throw new Exception("Invalid review action");
        }
        // ✅ Audit record (VERY IMPORTANT ✅)
        if (!string.IsNullOrEmpty(dto.Remarks))
        {
            _context.AuditHistories.Add(new AuditHistory
            {
                EncounterId = auth.EncounterId,
                AuthId = auth.AuthId,
                ActionType = (byte)dto.Action,
                Remarks = dto.Remarks,
                CreatedAt = DateTime.UtcNow
            });
        }

        auth.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Review action {Action} completed for AuthId: {AuthId}",
            dto.Action, authId);

        return true;
    }
}