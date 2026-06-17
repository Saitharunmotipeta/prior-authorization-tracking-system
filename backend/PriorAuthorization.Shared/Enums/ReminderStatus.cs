namespace PriorAuthorization.Shared.Enums;

public enum ReminderStatus : byte
{
    Requested = 1,
    Scheduled = 2,
    Completed = 3,
    Cancelled = 4
}