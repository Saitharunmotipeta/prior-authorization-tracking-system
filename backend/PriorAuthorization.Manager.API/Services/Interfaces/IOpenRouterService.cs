using PriorAuthorization.Manager.API.DTOs;

namespace PriorAuthorization.Manager.API.Services.Interfaces;

public interface IOpenRouterService
{
    Task<string> GenerateExecutiveSummaryAsync(
        string prompt);
}