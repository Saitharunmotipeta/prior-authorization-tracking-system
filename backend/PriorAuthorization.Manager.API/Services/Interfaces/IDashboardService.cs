using PriorAuthorization.Manager.API.DTOs.Dashboard;

namespace PriorAuthorization.Manager.API.Services.Interfaces;

public interface IDashboardService
{
    Task<DashboardResponseDto> GetDashboardAsync(
        DashboardFilterDto filter);
}