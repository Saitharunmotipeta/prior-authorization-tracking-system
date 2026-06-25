using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Shared.Data;
using PriorAuthorization.Shared.Entities;
using PriorAuthorization.Shared.Enums;
using PriorAuthorization.Shared.Exceptions;
using PriorAuthorization.Specialist.API.DTOs;
using PriorAuthorization.Specialist.API.Services.Interfaces;

namespace PriorAuthorization.Specialist.API.Services.Implementations;

public class AuthorizationRequestService : IAuthorizationService
{
    private readonly ApplicationDbContext _context;
    private readonly IReminderService _reminderService;

    public AuthorizationRequestService(
        ApplicationDbContext context, IReminderService reminderService)
    {
        _context = context;
        _reminderService = reminderService;
    }
    public async Task<int> CreateAuthorizationRequestAsync(
    CreateAuthorizationRequestDto dto)
    {
        var encounter =
            await _context.Encounters
                .FirstOrDefaultAsync(e =>
                    e.EncounterId == dto.EncounterId &&
                    e.IsActive);

        if (encounter == null)
        {
            throw new NotFoundException(
                $"Encounter {dto.EncounterId} not found.");
        }

        if (encounter.VerificationStatus !=
            (byte)VerificationStatus.Verified)
        {
            throw new ConflictException(
                "Encounter must be verified before creating an authorization request.");
        }

        var authorizationExists =
            await _context.AuthorizationRequests
                .AnyAsync(a =>
                    a.EncounterId == dto.EncounterId);

        if (authorizationExists)
        {
            throw new ConflictException(
                "Authorization request already exists for this encounter.");
        }

        var payerExists =
            await _context.Payers
                .AnyAsync(p =>
                    p.PayerId == dto.PayerId);

        if (!payerExists)
        {
            throw new NotFoundException(
                $"Payer {dto.PayerId} not found.");
        }

        var expirationDate =
            encounter.ConditionType switch
            {
                (byte)ConditionType.Emergency =>
                    DateOnly.FromDateTime(
                        DateTime.UtcNow.AddDays(1)),

                (byte)ConditionType.Urgent =>
                    DateOnly.FromDateTime(
                        DateTime.UtcNow.AddDays(7)),

                _ =>
                    DateOnly.FromDateTime(
                        DateTime.UtcNow.AddDays(15))
            };

        using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            var authorizationRequest =
                new AuthorizationRequest
                {
                    EncounterId = dto.EncounterId,
                    PayerId = dto.PayerId,
                    Priority = dto.Priority,

                    Status = (byte)RequestStatus.Draft,

                    EstimatedTotalAmount = 0,

                    ApprovedAmount = null,

                    SubmittedAt = null,

                    ReviewedAt = null,

                    ExpirationDate = expirationDate,

                    CreatedAt = DateTime.UtcNow,

                    UpdatedAt = DateTime.UtcNow
                };

            _context.AuthorizationRequests
                .Add(authorizationRequest);

            await _context.SaveChangesAsync();

            _context.AuditHistories.Add(
                new AuditHistory
                {
                    EncounterId = dto.EncounterId,

                    AuthId = authorizationRequest.AuthId,

                    EntityId =
                        $"Authorization-{authorizationRequest.AuthId}",

                    ActionType =
                        (byte)AuditActionType.Created,

                    PerformedByRole =
                        (byte)UserRole.Specialist,

                    Remarks =
                        "Authorization request created in Draft status.",

                    CreatedAt =
                        DateTime.UtcNow
                });

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return authorizationRequest.AuthId;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    public async Task AddServicesAsync(
    int authId,
        AddAuthorizationServiceListDto dto)
    {
        var authorization =
            await _context.AuthorizationRequests
                .FirstOrDefaultAsync(a =>
                    a.AuthId == authId);

        if (authorization == null)
        {
            throw new NotFoundException(
                $"Authorization Request {authId} not found.");
        }

        if (authorization.Status !=
            (byte)RequestStatus.Draft)
        {
            throw new ConflictException(
                "Services can only be modified in Draft status.");
        }

        var cpt =
            await _context.Cptcodes
                .FirstOrDefaultAsync(c =>
                    c.CptCode1 == dto.CptCode);

        if (cpt == null)
        {
            throw new NotFoundException(
                $"CPT Code '{dto.CptCode}' not found.");
        }

        var icdExists =
            await _context.Icdcodes
                .AnyAsync(i =>
                    i.IcdCode1 == dto.IcdCode);

        if (!icdExists)
        {
            throw new NotFoundException(
                $"ICD Code '{dto.IcdCode}' not found.");
        }

        var service =
            new AuthorizationService
            {
                AuthId = authId,

                CptCode = dto.CptCode,

                IcdCode = dto.IcdCode,

                EstimatedCost = cpt.EstimatedCost,

                Notes = dto.Notes,

                CreatedAt = DateTime.UtcNow
            };

        _context.AuthorizationServices.Add(service);

        _context.AuditHistories.Add(
            new AuditHistory
            {
                AuthId = authId,

                EncounterId = authorization.EncounterId,

                EntityId = $"Service-{dto.CptCode}",

                ActionType =
                    (byte)AuditActionType.Created,

                PerformedByRole =
                    (byte)UserRole.Specialist,

                Remarks =
                    $"Added service CPT {dto.CptCode}",

                CreatedAt =
                    DateTime.UtcNow
            });

        await _context.SaveChangesAsync();
    }
    public async Task<List<AuthorizationServiceResponseDto>>
    GetServicesAsync(int authId)
    {
        var authorizationExists =
            await _context.AuthorizationRequests
                .AnyAsync(a => a.AuthId == authId);

        if (!authorizationExists)
        {
            throw new NotFoundException(
                $"Authorization Request {authId} not found.");
        }

        return await _context.AuthorizationServices
            .Where(s => s.AuthId == authId)
            .Select(s => new AuthorizationServiceResponseDto
            {
                ServiceId = s.ServiceId,
                CptCode = s.CptCode,
                IcdCode = s.IcdCode,
                EstimatedCost = s.EstimatedCost,
                Notes = s.Notes
            })
            .ToListAsync();
    }
    public async Task RemoveServiceAsync(
    int authId,
    int serviceId)
    {
        var authorization =
            await _context.AuthorizationRequests
                .FirstOrDefaultAsync(a =>
                    a.AuthId == authId);

        if (authorization == null)
        {
            throw new NotFoundException(
                $"Authorization Request {authId} not found.");
        }

        if (authorization.Status !=
            (byte)RequestStatus.Draft)
        {
            throw new ConflictException(
                "Services can only be modified in Draft status.");
        }

        var service =
            await _context.AuthorizationServices
                .FirstOrDefaultAsync(s =>
                    s.ServiceId == serviceId &&
                    s.AuthId == authId);

        if (service == null)
        {
            throw new NotFoundException(
                $"Service {serviceId} not found.");
        }

        _context.AuthorizationServices.Remove(service);

        await _context.SaveChangesAsync();
    }
    public async Task SubmitAuthorizationRequestAsync(
    int authId)
    {
        var authorization =
            await _context.AuthorizationRequests
                .FirstOrDefaultAsync(a =>
                    a.AuthId == authId);

        if (authorization == null)
        {
            throw new NotFoundException(
                $"Authorization Request {authId} not found.");
        }

        if (authorization.Status !=
            (byte)RequestStatus.Draft)
        {
            throw new ConflictException(
                "Only draft authorization requests can be submitted.");
        }

        var services =
            await _context.AuthorizationServices
                .Where(s => s.AuthId == authId)
                .ToListAsync();

        if (!services.Any())
        {
            throw new BadRequestException(
                "At least one service must be attached before submission.");
        }

        var totalAmount =
            services.Sum(s => s.EstimatedCost);

        authorization.EstimatedTotalAmount =
            totalAmount;

        authorization.Status =
            (byte)RequestStatus.Submitted;

        authorization.SubmittedAt =
            DateTime.UtcNow;

        authorization.UpdatedAt =
            DateTime.UtcNow;

        _context.AuditHistories.Add(
            new AuditHistory
            {
                AuthId = authorization.AuthId,

                EncounterId =
                    authorization.EncounterId,

                EntityId =
                    $"Authorization-{authorization.AuthId}",

                ActionType =
                    (byte)AuditActionType.Updated,

                PerformedByRole =
                    (byte)UserRole.Specialist,

                Remarks =
                    $"Authorization submitted. Total Amount: {totalAmount}",

                CreatedAt =
                    DateTime.UtcNow
            });

        await _context.SaveChangesAsync();
    }
    public async Task ResubmitAuthorizationAsync(
    int authId,
    ResubmitAuthorizationDto dto)
    {
        var authorization =
            await _context.AuthorizationRequests
                .FirstOrDefaultAsync(a =>
                    a.AuthId == authId);

        if (authorization == null)
        {
            throw new NotFoundException(
                $"Authorization Request {authId} not found.");
        }

        if (authorization.Status !=
            (byte)RequestStatus.AdditionalInfoRequired)
        {
            throw new ConflictException(
                "Only requests awaiting additional information can be resubmitted.");
        }

        authorization.Status =
            (byte)RequestStatus.ReSubmitted;
        authorization.SubmittedAt =
            DateTime.UtcNow;

        authorization.UpdatedAt =
            DateTime.UtcNow;

        _context.AuditHistories.Add(
            new AuditHistory
            {
                AuthId =
                    authorization.AuthId,

                EncounterId =
                    authorization.EncounterId,

                EntityId =
                    $"Authorization-{authorization.AuthId}",

                ActionType =
                    (byte)AuditActionType.Updated,

                PerformedByRole =
                    (byte)UserRole.Specialist,

                Remarks =
                    $"Authorization resubmitted. {dto.Remarks}",

                CreatedAt =
                    DateTime.UtcNow
            });

        await _context.SaveChangesAsync();
    }
    public async Task<List<AuthorizationTimelineDto>>
    GetTimelineAsync(int authId)
    {
        var authorizationExists =
            await _context.AuthorizationRequests
                .AnyAsync(a =>
                    a.AuthId == authId);

        if (!authorizationExists)
        {
            throw new NotFoundException(
                $"Authorization Request {authId} not found.");
        }

        return await _context.AuditHistories
            .Where(a =>
                a.AuthId == authId)
            .OrderBy(a =>
                a.CreatedAt)
            .Select(a =>
                new AuthorizationTimelineDto
                {
                    Action =
                        ((AuditActionType)a.ActionType)
                            .ToString(),

                    Remarks =
                        a.Remarks,

                    CreatedAt =
                        a.CreatedAt
                })
            .ToListAsync();
    }
    public async Task<List<AuthorizationTatResponse>>
    GetTatPriorityQueueAsync(
        int facilityId)
    {
        var today =
            DateTime.UtcNow.Date;

        var authRequests =
            await (
                from auth in _context.AuthorizationRequests

                join payer in _context.Payers
                    on auth.PayerId equals payer.PayerId

                join encounter in _context.Encounters
                    on auth.EncounterId equals encounter.EncounterId

                where encounter.FacilityId ==
                      facilityId

                where auth.Status ==
                        (byte)RequestStatus.Submitted
                    || auth.Status ==
                        (byte)RequestStatus.ReSubmitted

                select new
                {
                    auth.AuthId,

                    payer.PayerName,

                    auth.Priority,

                    auth.Status,

                    SubmittedAt =
                        auth.SubmittedAt!.Value,

                    TatDays =
                        auth.Priority ==
                        (byte)Priority.Normal
                            ? payer.NormalTatDays
                            : payer.UrgentTatDays
                }
            )
            .ToListAsync();

        var result =
            authRequests
            .Select(x =>
            {
                var expectedReviewDate =
                    x.SubmittedAt.Date
                        .AddDays(x.TatDays);

                var daysLeft =
                    (expectedReviewDate - today).Days;

                string tatStatus;

                if (daysLeft < 0)
                {
                    tatStatus =
                        "Review SLA Exceeded";
                }
                else if (daysLeft == 0)
                {
                    tatStatus =
                        "Review Deadline Reached";
                }
                else if (daysLeft <= 3)
                {
                    tatStatus =
                        "Review Deadline Approaching";
                }
                else
                {
                    tatStatus =
                        "Within Review Timeline";
                }

                return new AuthorizationTatResponse
                {
                    AuthId =
                        x.AuthId,

                    PayerName =
                        x.PayerName,

                    Priority =
                        x.Priority,

                    SubmittedAt =
                        x.SubmittedAt,

                    TatDays =
                        x.TatDays,

                    ExpectedReviewDate =
                        expectedReviewDate,

                    DaysLeft =
                        daysLeft,

                    TatStatus =
                        tatStatus
                };
            })
            .OrderBy(x => x.DaysLeft)
            .ToList();

        return result;
    }
    public async Task<List<AuthorizationListItemDto>>
    GetAuthorizationsAsync(
        RequestStatus? status)
    {
        var query =
            _context.AuthorizationRequests
                .AsNoTracking()
                .Include(a => a.Payer)
                .Include(a => a.Encounter)
                    .ThenInclude(e => e.Patient)
                .AsQueryable();

        if (status.HasValue)
        {
            query =
                query.Where(a =>
                    a.Status ==
                    (byte)status.Value);
        }

        return await query
            .OrderByDescending(a =>
                a.CreatedAt)
            .Select(a =>
                new AuthorizationListItemDto
                {
                    AuthId =
                        a.AuthId,

                    PatientName =
                        a.Encounter.Patient.FullName,

                    PayerName =
                        a.Payer.PayerName,

                    Status =
                        ((RequestStatus)a.Status)
                            .ToString(),

                    Priority =
                        ((Priority)a.Priority)
                            .ToString(),

                    EstimatedAmount =
                        a.EstimatedTotalAmount,

                    CreatedAt =
                        a.CreatedAt,

                    SubmittedAt =
                        a.SubmittedAt
                })
            .ToListAsync();
    }
    public async Task<List<AuthorizationListItemDto>>
GetAwaitingReviewAuthorizationsAsync()
    {
        await _reminderService
            .GenerateTatRemindersAsync();

        return await _context.AuthorizationRequests
            .AsNoTracking()
            .Include(a => a.Payer)
            .Include(a => a.Encounter)
                .ThenInclude(e => e.Patient)
            .Where(a =>
                a.Status ==
                    (byte)RequestStatus.Submitted
                ||
                a.Status ==
                    (byte)RequestStatus.ReSubmitted)
            .OrderByDescending(a =>
                a.SubmittedAt)
            .Select(a =>
                new AuthorizationListItemDto
                {
                    AuthId =
                        a.AuthId,

                    PatientName =
                        a.Encounter.Patient.FullName,

                    PayerName =
                        a.Payer.PayerName,

                    Status =
                        ((RequestStatus)a.Status)
                            .ToString(),

                    Priority =
                        ((Priority)a.Priority)
                            .ToString(),

                    EstimatedAmount =
                        a.EstimatedTotalAmount,

                    CreatedAt =
                        a.CreatedAt,

                    SubmittedAt =
                        a.SubmittedAt
                })
            .ToListAsync();
    }
}