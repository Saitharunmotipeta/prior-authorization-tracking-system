using Bogus;

namespace PriorAuthorization.DataSeeder.Generators
{
    public class Encounter
    {
        public Guid PatientId { get; set; }
        public int FacilityId { get; set; }
        public int DepartmentId { get; set; }

        public int ConditionType { get; set; }
        public int VerificationStatus { get; set; }
        public int? RequestStatus { get; set; }

        public bool IdentificationVerified { get; set; }
        public bool PrescriptionVerified { get; set; }
        public bool ScanVerified { get; set; }
        public bool DoctorNotesVerified { get; set; }
        public bool InsuranceCardVerified { get; set; }

        public string Remarks { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
    }

    public static class EncounterGenerator
    {
        public static List<Encounter> Generate(
            List<Guid> patientIds,
            List<int> facilityIds,
            List<int> departmentIds)
        {
            var faker = new Faker();

            var encounters = new List<Encounter>();

            foreach (var patientId in patientIds)
            {
                var encounter = new Encounter
                {
                    PatientId = patientId,
                    FacilityId = faker.PickRandom(facilityIds),
                    DepartmentId = faker.PickRandom(departmentIds),

                    ConditionType = faker.PickRandom(1, 2, 3), // Elective, Urgent, Emergency
                    VerificationStatus = 1,
                    RequestStatus = faker.PickRandom(1, 3),

                    IdentificationVerified = true,
                    PrescriptionVerified = true,
                    ScanVerified = true,
                    DoctorNotesVerified = true,
                    InsuranceCardVerified = true,

                    Remarks = "Auto generated encounter",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsActive = true
                };

                encounters.Add(encounter);
            }

            return encounters;
        }
    }
}