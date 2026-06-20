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

    public AuthorizationRequestService(
        ApplicationDbContext context)
    {
        _context = context;
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

        if (dto.Services == null ||
            !dto.Services.Any())
        {
            throw new BadRequestException(
                "At least one authorization service is required.");
        }

        decimal totalAmount = 0;

        var servicesToCreate =
            new List<AuthorizationService>();

        foreach (var service in dto.Services)
        {
            var cpt =
                await _context.Cptcodes
                    .FirstOrDefaultAsync(c =>
                        c.CptCode1 == service.CptCode);

            if (cpt == null)
            {
                throw new NotFoundException(
                    $"CPT Code '{service.CptCode}' not found.");
            }

            var icdExists =
                await _context.Icdcodes
                    .AnyAsync(i =>
                        i.IcdCode1 == service.IcdCode);

            if (!icdExists)
            {
                throw new NotFoundException(
                    $"ICD Code '{service.IcdCode}' not found.");
            }

            totalAmount += cpt.EstimatedCost;

            servicesToCreate.Add(
                new AuthorizationService
                {
                    CptCode = service.CptCode,
                    IcdCode = service.IcdCode,
                    EstimatedCost = cpt.EstimatedCost,
                    Notes = service.Notes,
                    CreatedAt = DateTime.UtcNow
                });
        }

        using var transaction =
            await _context.Database
                .BeginTransactionAsync();

        try
        {
            var authorizationRequest =
                new AuthorizationRequest
                {
                    EncounterId = dto.EncounterId,

                    PayerId = dto.PayerId,

                    Priority = dto.Priority,

                    Status =
                        (byte)RequestStatus.Draft,

                    EstimatedTotalAmount =
                        totalAmount,

                    ApprovedAmount = null,

                    SubmittedAt = null,

                    ReviewedAt = null,

                    ExpirationDate = null,

                    CreatedAt = DateTime.UtcNow,

                    UpdatedAt = DateTime.UtcNow
                };

            _context.AuthorizationRequests
                .Add(authorizationRequest);

            await _context.SaveChangesAsync();

            foreach (var authorizationService
                in servicesToCreate)
            {
                authorizationService.AuthId =
                    authorizationRequest.AuthId;

                _context.AuthorizationServices
                    .Add(authorizationService);
            }

            var auditHistory =
                new AuditHistory
                {
                    EncounterId =
                        dto.EncounterId,

                    AuthId =
                        authorizationRequest.AuthId,

                    EntityId =
                        $"Authorization-{authorizationRequest.AuthId}",

                    ActionType =
                        (byte)AuditActionType.Created,

                    PerformedByRole =
                        (byte)UserRole.Specialist,

                    Remarks =
                        $"Authorization request created. Estimated Amount: {totalAmount}",

                    CreatedAt =
                        DateTime.UtcNow
                };

            _context.AuditHistories
                .Add(auditHistory);

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
}