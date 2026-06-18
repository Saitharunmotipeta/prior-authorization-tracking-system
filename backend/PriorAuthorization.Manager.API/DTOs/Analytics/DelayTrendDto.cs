namespace PriorAuthorization.Manager.API.DTOs.Analytics;

public class DelayTrendDto
{
    public string PayerName { get; set; } = string.Empty;

    public int ZeroToTwoDays { get; set; }

    public int ThreeToFiveDays { get; set; }

    public int SixToTenDays { get; set; }

    public int MoreThanTenDays { get; set; }
}