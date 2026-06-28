using Bogus;
using Bogus.Extensions;
using PriorAuthorization.Shared.Entities;

namespace PriorAuthorization.DataSeeder.Generators
{
    public class AuthorizationRequest
    {
        public int EncounterId { get; set; }
        public int PayerId { get; set; }

        public int Priority { get; set; }
        public int Status { get; set; }

        public decimal EstimatedTotalAmount { get; set; }
        public decimal? ApprovedAmount { get; set; }

        public DateTime? SubmittedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public static class AuthorizationRequestGenerator
    {
        public static List<AuthorizationRequest> Generate(List<int> encounterIds, List<int> payerIds)

        {
            var faker = new Faker();

            var statusPool = new[]
            {
                4,4,4,4,  // Submitted ✅
                8,8,8,    // ReSubmitted ✅
                5,5,
                6,
                7,
                9
            };

            var list = new List<AuthorizationRequest>();

            foreach (var encounterId in encounterIds)
            {
                var status = faker.PickRandom(statusPool);
                var priority = faker.PickRandom(1, 2, 3);

                var estimated = faker.Random.Decimal(5000, 100000);


                decimal? approved = status == 7
     ? faker.Random.Decimal(3000, estimated)
     : null;


                var request = new AuthorizationRequest
                {
                    EncounterId = encounterId,
                     PayerId = faker.PickRandom(payerIds),

                    Priority = priority,
                    Status = status,

                    EstimatedTotalAmount = estimated,
                    ApprovedAmount = approved,

                    SubmittedAt = DateTime.Now.AddDays(-faker.Random.Int(1, 10)),
                    ReviewedAt = (status == 7 || status == 9)
                        ? DateTime.Now
                        : null,

                    ExpirationDate = DateTime.Now.AddDays(faker.Random.Int(5, 15)),

                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                list.Add(request);
            }

            return list;
        }
    }
}
