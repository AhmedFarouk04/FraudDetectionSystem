using System;
using System.Collections.Generic;
using FraudDetectionSystem.Entities;
using FraudDetectionSystem.Rules;
using FraudDetectionSystem.Interfaces;

namespace FraudDetectionSystem.Services

{
    public delegate void FraudDetectedHandler(Transaction tx, string ruleName);
    public class RiskAnalyzer
    {
        private readonly List<FraudRule> _rules = new();
        private readonly ILogger _logger;
        public event FraudDetectedHandler? OnFraudDetected;
        public RiskAnalyzer(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void AddRule(FraudRule rule)
        {
            if (rule == null)
                throw new ArgumentNullException(nameof(rule));

            _rules.Add(rule);
            _logger.LogInfo($"Rule added: {rule.Name}");
        }

        public string Analyze(Transaction tx)
        {
            if (tx == null)
                throw new ArgumentNullException(nameof(tx));

            var triggered = new List<FraudRule>();

            foreach (var rule in _rules)
            {
                try
                {
                    if (rule.IsSuspicious(tx))
                        triggered.Add(rule);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error in rule {rule.Name}", ex);
                }
            }

            if (triggered.Count > 0)
            {
                OnFraudDetected?.Invoke(tx, triggered[0].Name);
                _logger.LogWarning($"Suspicious Transaction Detected ({triggered.Count} rules triggered)");
                return $"Suspicious Transaction! [{string.Join(", ", triggered.Select(r => r.Name))}]";
            }

            _logger.LogInfo($"Transaction #{tx.Id} is clean.");
            return "Transaction is clean.";
        }




        public void ListRules()
        {
            _logger.LogInfo("Active Fraud Rules:");
            foreach (var rule in _rules)
                _logger.LogInfo($"- {rule.Name}");
        }
    }
}
