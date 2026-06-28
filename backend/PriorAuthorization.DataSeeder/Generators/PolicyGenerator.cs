using Bogus;
using PriorAuthorization.Shared.Entities;

namespace PriorAuthorization.DataSeeder.Generators
{
    public class Policy
    {
        public Guid PatientId { get; set; }
        public int PayerId { get; set; }

        public DateTime PolicyStartDate { get; set; }
        public DateTime PolicyExpiryDate { get; set; }

        public bool IsActive { get; set; }
    }

    public static class PolicyGenerator
    {
        public static List<Policy> Generate(List<Guid> patientIds, List<int> payerIds)

        {
            var faker = new Faker();
            var policies = new List<Policy>();

            foreach (var patientId in patientIds)
            {
                var startDate = faker.Date.Past(2);
                var expiryDate = startDate.AddYears(4);

                var policy = new Policy
                {
                    PatientId = patientId,
                    PayerId = faker.PickRandom(payerIds),

                    PolicyStartDate = startDate,
                    PolicyExpiryDate = expiryDate,

                    IsActive = expiryDate > DateTime.Now
                };

                policies.Add(policy);
            }

            return policies;
        }
    }
}