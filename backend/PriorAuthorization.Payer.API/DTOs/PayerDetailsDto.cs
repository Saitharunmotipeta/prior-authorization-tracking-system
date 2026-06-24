namespace PriorAuthorization.Payer.API.DTOs
{
    public class PayerDetailsDto
    {
        public int PayerId { get; set; }

        public string PayerName { get; set; } = string.Empty;
    }
}
