namespace PriorAuthorization.Payer.API.DTOs
{
    public class ServiceDto
    {

        public string CptCode { get; set; }

        public string IcdCode { get; set; }

        public decimal EstimatedCost { get; set; }

        public string Notes { get; set; }

    }
}
