namespace PriorAuthorization.Specialist.API.DTOs;

public class UpdateEncounterDto
{
    public byte? VerificationStatus { get; set; }

    public bool? IdentificationVerified { get; set; }

    public bool? PrescriptionVerified { get; set; }

    public bool? ScanVerified { get; set; }

    public bool? DoctorNotesVerified { get; set; }

    public bool? InsuranceCardVerified { get; set; }

    public string? Remarks { get; set; }
}