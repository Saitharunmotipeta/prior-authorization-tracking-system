namespace PriorAuthorization.Manager.API.DTOs.Dashboard;

public class DashboardResponseDto
{
    public int TotalEncounters { get; set; }

    public int TotalAuthorizationRequests { get; set; }

    public int ApprovedRequests { get; set; }

    public int DeniedRequests { get; set; }

    public int PendingRequests { get; set; }

    public int ExpiredRequests { get; set; }

    public decimal ApprovalRate { get; set; }

    public decimal DenialRate { get; set; }

    public decimal ApprovedRevenue { get; set; }

    public int TotalReminders { get; set; }

    public int SuccessfulReminders { get; set; }

    public decimal ReminderSuccessRate { get; set; }


}