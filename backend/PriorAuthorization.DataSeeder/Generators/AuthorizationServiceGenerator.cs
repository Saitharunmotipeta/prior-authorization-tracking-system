using Bogus;

namespace PriorAuthorization.DataSeeder.Generators
{
    public class AuthorizationService
    {
        public int AuthId { get; set; }
        public string? CptCode { get; set; }
        public string? IcdCode { get; set; }
        public decimal EstimatedCost { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public static class AuthorizationServiceGenerator
    {
        public static List<AuthorizationService> Generate(
            List<int> authIds,
            List<string> cptCodes,
            List<string> icdCodes)
        {
            var faker = new Faker();

            var list = new List<AuthorizationService>();

            foreach (var authId in authIds)
            {
                // ✅ 1–3 services per auth
                int count = faker.Random.Int(1, 3);

                for (int i = 0; i < count; i++)
                {
                    var service = new AuthorizationService
                    {
                        AuthId = authId,
                        CptCode = faker.PickRandom(cptCodes),
                        IcdCode = faker.PickRandom(icdCodes),
                        EstimatedCost = faker.Random.Decimal(2000, 80000),
                        Notes = faker.Lorem.Sentence(),
                        CreatedAt = DateTime.Now
                    };

                    list.Add(service);
                }
            }

            return list;
        }
    }
}