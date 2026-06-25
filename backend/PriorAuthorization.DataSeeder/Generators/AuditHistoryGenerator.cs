using Bogus;

namespace PriorAuthorization.DataSeeder.Generators
{
    public class AuditHistory
    {
        public int EncounterId { get; set; }
        public int AuthId { get; set; }
        public string? EntityId { get; set; }

        public int ActionType { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }

        public int PerformedByRole { get; set; }
        public string? Remarks { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public static class AuditHistoryGenerator
    {
        public static List<AuditHistory> Generate(
            List<(int AuthId, int EncounterId, int Status)> authData)
        {
            var faker = new Faker();
            var audits = new List<AuditHistory>();

            foreach (var item in authData)
            {
                int authId = item.AuthId;
                int encounterId = item.EncounterId;
                int status = item.Status;

                // ✅ ALWAYS: Created
                audits.Add(CreateAudit(encounterId, authId, 1, "Created"));

                // ✅ Submitted
                audits.Add(CreateAudit(encounterId, authId, 3, "Submitted"));

                // ✅ UnderReview (optional)
                audits.Add(CreateAudit(encounterId, authId, 2, "UnderReview"));

                // ✅ Conditional workflow
                if (status == 6) // AdditionalInfoRequired
                {
                    audits.Add(CreateAudit(encounterId, authId, 6, "Requested More Info"));
                }

                if (status == 8) // ReSubmitted
                {
                    audits.Add(CreateAudit(encounterId, authId, 3, "ReSubmitted"));
                }

                if (status == 7) // Approved
                {
                    audits.Add(CreateAudit(encounterId, authId, 4, "Approved"));
                }

                if (status == 9) // Denied
                {
                    audits.Add(CreateAudit(encounterId, authId, 5, "Denied"));
                }
            }

            return audits;
        }

        private static AuditHistory CreateAudit(
            int encounterId,
            int authId,
            int actionType,
            string remark)
        {
            return new AuditHistory
            {
                EncounterId = encounterId,
                AuthId = authId,
                EntityId = "AUTH_" + authId,
                ActionType = actionType,
                OldValue = null,
                NewValue = remark,
                PerformedByRole = 2, // Payer
                Remarks = remark,
                CreatedAt = DateTime.Now
            };
        }
    }
}
