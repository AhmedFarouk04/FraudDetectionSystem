using System;
using System.Collections.Generic;
using FraudDetectionSystem.Entities;
using FraudDetectionSystem.Rules;

namespace FraudDetectionSystem.Services
{
    public class RiskScoreService
    {
        private readonly Dictionary<string, double> _weights = new()
        {
            { "High Amount Rule", 0.4 },
            { "Country Mismatch Rule", 0.3 },
            { "Velocity Rule", 0.2 },
            { "Geo Location Rule", 0.15 },
            { "Behavior Change Rule", 0.25 },
            { "Night Activity Rule", 0.1 }
        };

       
        public double CalculateScore(Transaction tx, List<FraudRule> triggeredRules)
        {
            if (tx == null) return 0;
            if (triggeredRules == null || triggeredRules.Count == 0)
                return 0;

            double score = 0;
            foreach (var rule in triggeredRules)
            {
                if (_weights.TryGetValue(rule.Name, out double w))
                    score += w;
                else
                    score += 0.1; 
            }

            score = Math.Min(score * 100, 100);
            return Math.Round(score, 1);
        }


        public string InterpretScore(double score)
        {
            if (score < 30)
                return "Low Risk";
            if (score < 70)
                return "Medium Risk";
            return "High Risk";
        }
    }
}
