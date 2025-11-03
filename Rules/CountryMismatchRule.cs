using FraudDetectionSystem.Entities;

namespace FraudDetectionSystem.Rules
{
    public class CountryMismatchRule : FraudRule
    {
        public CountryMismatchRule()
            : base("Country Mismatch Rule") { }

        public override bool IsSuspicious(Transaction tx)
        {
            if (tx == null || tx.User == null) return false;
            return tx.Country.Trim().ToLower() != tx.User.Country.Trim().ToLower();
        }

        public override void Explain()
        {
            Console.WriteLine("Rule: Country Mismatch → Flags if transaction country differs from user’s home country.");
        }
    }
}
