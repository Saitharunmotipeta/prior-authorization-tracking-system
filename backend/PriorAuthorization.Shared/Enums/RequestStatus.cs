namespace PriorAuthorization.Shared.Enums;

public enum RequestStatus : byte
{
    Draft = 1,
    VerificationFailed = 2,
    ReadyForSubmission = 3,

    Submitted = 4,
    UnderReview = 5,
    AdditionalInfoRequired = 6,
    ReSubmitted = 8,

    Approved = 7,
    Denied = 9,
    Expired = 10
}