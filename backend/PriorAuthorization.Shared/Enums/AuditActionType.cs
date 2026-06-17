namespace PriorAuthorization.Shared.Enums;

public enum AuditActionType : byte
{
    Created = 1,
    Updated = 2,

    Submitted = 3,
    Approved = 4,
    Denied = 5,

    RequestedMoreInfo = 6,
    ReSubmitted = 7,

    ReminderCreated = 8,
    ReminderCompleted = 9,

    PeerReviewRequested = 10,
    PeerReviewCompleted = 11
}