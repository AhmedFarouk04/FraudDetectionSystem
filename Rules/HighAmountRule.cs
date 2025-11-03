using FraudDetectionSystem.Entities;

namespace FraudDetectionSystem.Rules
{
    public class HighAmountRule : FraudRule
    {
        private readonly decimal _limit;

        public HighAmountRule(decimal limit = 10000)
            : base("High Amount Rule")
        {
            _limit = limit;
        }

        public override bool IsSuspicious(Transaction tx)
        {
            if (tx == null) return false;
            return tx.Amount > _limit;
        }

        public override void Explain()
        {
            Console.WriteLine($" Rule: {Name} → Flags any transaction over {_limit:C}");
        }
    }
}
