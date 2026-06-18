namespace PriorAuthorization.Payer.API.DTOs
{
    public class RequestsDetails
    {
        public int AuthId { get; set; }

        public int EncounterId { get; set; }

        // ✅ Patient Info
        public string? PatientName { get; set; }
        public DateOnly? Dob { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }

        // ✅ Facility & Department
        public string FacilityName { get; set; }
        public string DepartmentName { get; set; }

        // ✅ Condition & Priority
        public string ConditionType { get; set; }
        public string Priority { get; set; }

        // ✅ Authorization Status
        public string Status { get; set; }
        public decimal EstimatedAmount { get; set; }
        public decimal? ApprovedAmount { get; set; }

        public DateTime? SubmittedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }

        // ✅ Document Verification
        public DocumentVerificationDto Documents { get; set; }

        // ✅ Services
       public List<ServiceDto> Services { get; set; }
    }
}
