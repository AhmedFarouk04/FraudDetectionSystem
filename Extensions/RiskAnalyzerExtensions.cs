using System.Collections.Generic;
using System.Reflection;
using FraudDetectionSystem.Rules;
using FraudDetectionSystem.Services;

namespace FraudDetectionSystem.Extensions
{
    public static class RiskAnalyzerExtensions
    {
        
        public static List<FraudRule> GetActiveRules(this RiskAnalyzer analyzer)
        {
            var field = typeof(RiskAnalyzer).GetField("_rules", BindingFlags.NonPublic | BindingFlags.Instance);

            return field?.GetValue(analyzer) as List<FraudRule> ?? new List<FraudRule>();
        }
    }
}
