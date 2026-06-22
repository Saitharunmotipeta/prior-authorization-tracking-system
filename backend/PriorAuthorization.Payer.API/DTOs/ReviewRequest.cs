using System.ComponentModel.DataAnnotations;
using PriorAuthorization.Shared.Enums;

namespace PriorAuthorization.Payer.API.DTOs
{
    public class ReviewRequest
    {
        [Required(ErrorMessage = "Action is required")]
        public ReviewActionType Action { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Approved amount must be greater than or equal to 0")]
        public decimal? ApprovedAmount { get; set; }

        [MaxLength(500, ErrorMessage = "Remarks cannot exceed 500 characters")]
        public string? Remarks { get; set; }
    }
}