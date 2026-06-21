using System.ComponentModel.DataAnnotations;

namespace PriorAuthorization.Payer.API.DTOs
{
   

    public class RequestLists
    {
        public int AuthId { get; set; }

        public int EncounterId { get; set; }

        [Required]
        public string PatientName { get; set; }

        [Required]
        public string FacilityName { get; set; }

        [Required]
        public string ConditionType { get; set; }

        [Required]
        public string Priority { get; set; }

        [Required]
        public string Status { get; set; }

        [Range(0, double.MaxValue)]
        public decimal EstimatedAmount { get; set; }

        public DateTime? SubmittedAt { get; set; }
    }
}
