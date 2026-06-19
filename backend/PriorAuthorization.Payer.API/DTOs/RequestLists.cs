namespace PriorAuthorization.Payer.API.DTOs
{
    public class RequestLists
    {
       
            public int AuthId { get; set; }

            public int EncounterId { get; set; }

            public string PatientName { get; set; }

            public string FacilityName { get; set; }

            public string ConditionType { get; set; }

            public string Priority { get; set; }

            public string Status { get; set; }

            public decimal EstimatedAmount { get; set; }

            public DateTime? SubmittedAt { get; set; }
        
    }
}
