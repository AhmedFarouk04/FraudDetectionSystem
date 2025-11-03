using System.Linq;
using FraudDetectionSystem.Entities;

namespace FraudDetectionSystem.Rules
{
    public class BehaviorChangeRule : FraudRule
    {
        private readonly decimal _suspiciousMultiplier;

        public BehaviorChangeRule(decimal multiplier = 5) : base("Behavior Change Rule")
        {
            Name = "Behavior Change Rule";
            _suspiciousMultiplier = multiplier;
        }

        public override bool IsSuspicious(Transaction tx)
        {
            if (tx?.User?.Transactions == null || tx.User.Transactions.Count < 3)
                return false;

            var avg = tx.User.Transactions.Average(t => t.Amount);
            return tx.Amount > avg * _suspiciousMultiplier;
        }
    }
}
