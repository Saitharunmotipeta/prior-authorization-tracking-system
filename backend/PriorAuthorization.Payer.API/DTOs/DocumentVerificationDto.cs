namespace PriorAuthorization.Payer.API.DTOs
{
    public class DocumentVerificationDto
    {
       
            public bool IdentificationVerified { get; set; }

            public bool PrescriptionVerified { get; set; }

            public bool ScanVerified { get; set; }

            public bool DoctorNotesVerified { get; set; }

            public bool InsuranceCardVerified { get; set; }
        

    }

}
