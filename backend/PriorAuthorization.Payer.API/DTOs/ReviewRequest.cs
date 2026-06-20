using PriorAuthorization.Shared.Enums;

namespace PriorAuthorization.Payer.API.DTOs
{
    public class ReviewRequest
    {


        public ReviewActionType Action { get; set; }

        // Approve, Deny, RequestMoreInfo

        public decimal? ApprovedAmount { get; set; }

        public string? Remarks { get; set; }

    }
}
