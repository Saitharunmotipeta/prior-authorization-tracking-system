namespace PriorAuthorization.Manager.API.DTOs;

public class ExecutiveReportResponse
{
    public DateTime GeneratedAt { get; set; }

    public string Report { get; set; } = string.Empty;
}