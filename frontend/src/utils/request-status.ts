import {
  RequestStatus
}
from "../enums/request-status.enum";

export const getStatusLabel =
  (
    status:
      RequestStatus
  ): string => {
    switch (status) {
      case RequestStatus.Draft:
        return "Draft";

      case RequestStatus.VerificationFailed:
        return "Verification Failed";

      case RequestStatus.ReadyForSubmission:
        return "Ready For Submission";

      case RequestStatus.Submitted:
        return "Submitted";

      case RequestStatus.UnderReview:
        return "Under Review";

      case RequestStatus.AdditionalInfoRequired:
        return "Additional Info Required";

      case RequestStatus.ReSubmitted:
        return "Re-Submitted";

      case RequestStatus.Approved:
        return "Approved";

      case RequestStatus.Denied:
        return "Denied";

      case RequestStatus.Expired:
        return "Expired";

      default:
        return "Unknown";
    }
  };