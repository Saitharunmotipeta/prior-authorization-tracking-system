using Bogus;

namespace PriorAuthorization.DataSeeder.Generators
{
    public class Reminder
    {
        public int AuthId { get; set; }
        public int PayerId { get; set; }

        public int Category { get; set; }
        public int Status { get; set; }

        public DateTime? ScheduledAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public string? Remarks { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public static class ReminderGenerator
    {
        public static List<Reminder> Generate(
            List<(int AuthId, int Status)> authData)
        {
            var faker = new Faker();
            var reminders = new List<Reminder>();

            foreach (var item in authData)
            {
                var authId = item.AuthId;
                var status = item.Status;

                // ✅ Only for specific statuses
                if (status == 5 || status == 6)
                {
                    var reminder = new Reminder
                    {
                        AuthId = authId,
                        PayerId = 1,

                        Category = faker.PickRandom(1, 2, 3, 4),
                        Status = faker.PickRandom(1, 2), // Pending / Completed

                        ScheduledAt = DateTime.Now.AddDays(faker.Random.Int(1, 5)),
                        CompletedAt = null,

                        Remarks = faker.Lorem.Sentence(),
                        UpdatedAt = DateTime.Now
                    };

                    reminders.Add(reminder);
                }
            }

            return reminders;
        }
    }
}