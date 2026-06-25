using Bogus;

namespace PriorAuthorization.DataSeeder.Generators
{
    public class Patient
    {
        public Guid PatientId { get; set; }
        public string MrnNumber { get; set; }
        public string FullName { get; set; }
        public DateTime Dob { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }

    public static class PatientGenerator
    {
        public static List<Patient> Generate(int count)
        {
            var faker = new Faker<Patient>()
                .RuleFor(x => x.PatientId, f => Guid.NewGuid())
                .RuleFor(x => x.MrnNumber, f => "MRN" + Guid.NewGuid().ToString("N").Substring(0, 8))

                .RuleFor(x => x.FullName, f => f.Name.FullName())
                .RuleFor(x => x.Dob, f => f.Date.Past(60))
                .RuleFor(x => x.Age, (f, p) => DateTime.Now.Year - p.Dob.Year)
                .RuleFor(x => x.Gender, f => f.PickRandom("Male", "Female"))
                .RuleFor(x => x.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(x => x.IdentificationType, f => f.PickRandom("Aadhar", "PAN", "Passport"))
                 .RuleFor(x => x.PhoneNumber, f => f.Random.ReplaceNumbers("##########"))
                .RuleFor(x => x.CreatedAt, DateTime.Now)
                .RuleFor(x => x.IsActive, true);

            return faker.Generate(count);
        }
    }
}