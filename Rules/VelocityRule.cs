using System;
using System.Linq;
using FraudDetectionSystem.Entities;

namespace FraudDetectionSystem.Rules
{
    public class VelocityRule : FraudRule
    {
        private readonly int _maxTransactions;
        private readonly int _timeWindowSeconds;

        public VelocityRule(int maxTransactions = 5, int timeWindowSeconds = 60)
            : base("Velocity Rule")
        {
            _maxTransactions = maxTransactions;
            _timeWindowSeconds = timeWindowSeconds;
        }

        public override bool IsSuspicious(Transaction tx)
        {
            if (tx == null || tx.User == null || tx.User.Transactions == null)
                return false;

            var recentCount = tx.User.Transactions
                .Count(t => (tx.Time - t.Time).TotalSeconds <= _timeWindowSeconds);

            return recentCount > _maxTransactions;
        }

        public override void Explain()
        {
            Console.WriteLine($"Rule: Velocity → Flags if more than {_maxTransactions} txs in {_timeWindowSeconds}s");
        }
    }
}
