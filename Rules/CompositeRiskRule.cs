using System.Collections.Generic;
using System.Linq;
using FraudDetectionSystem.Entities;

namespace FraudDetectionSystem.Rules
{
    public class CompositeRiskRule : FraudRule
    {
        private readonly List<FraudRule> _rules = new();
        private readonly double _threshold;

        public CompositeRiskRule(double threshold = 0.6) : base("Composite Risk Rule")
        {
            Name = "Composite Risk Rule";
            _threshold = threshold;
        }

        public void AddSubRule(FraudRule rule)
        {
            if (rule != null)
                _rules.Add(rule);
        }

        public override bool IsSuspicious(Transaction tx)
        {
            if (!_rules.Any()) return false;

            int total = _rules.Count;
            int triggered = _rules.Count(r => r.IsSuspicious(tx));
            double riskScore = (double)triggered / total;

            return riskScore >= _threshold;
        }
    }
}
