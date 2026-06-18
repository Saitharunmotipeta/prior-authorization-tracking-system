namespace PriorAuthorization.Specialist.API.DTOs;

public class PatientLookupDto
{
    public string MrnNumber { get; set; } = string.Empty;

    public string PatientName { get; set; } = string.Empty;

    public byte Age { get; set; }
}