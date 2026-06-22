using System.ComponentModel.DataAnnotations;

namespace PriorAuthorization.Payer.API.DTOs
{
    public class FacilityDto
    {

        public int FacilityId { get; set; }

        [Required(ErrorMessage = "Facility name is required")]
        [MaxLength(200, ErrorMessage = "Facility name cannot exceed 200 characters")]
        public string FacilityName { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Pending count cannot be negative")]
        public int PendingCount { get; set; }

    }

}
