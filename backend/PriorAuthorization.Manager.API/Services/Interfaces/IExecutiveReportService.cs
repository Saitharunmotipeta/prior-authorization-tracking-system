using PriorAuthorization.Manager.API.DTOs;
using PriorAuthorization.Shared.Common;

namespace PriorAuthorization.Manager.API.Services.Interfaces;

public interface IExecutiveReportService
{
    Task<ApiResponse<ExecutiveReportResponse>> GenerateExecutiveReportAsync();
}