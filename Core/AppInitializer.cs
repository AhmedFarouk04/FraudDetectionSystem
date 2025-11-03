using FraudDetectionSystem.Rules;
using FraudDetectionSystem.Services;
using FraudDetectionSystem.Interfaces;

namespace FraudDetectionSystem.Core
{
    public static class AppInitializer
    {
        public static (RiskAnalyzer, AlertService, TransactionService) Initialize(ILogger logger)
        {
            var analyzer = new RiskAnalyzer(logger);
            var alertService = new AlertService(logger);
            var transactionService = new TransactionService(logger);

            analyzer.OnFraudDetected += (tx, ruleName) =>
            {
                var alert = new Entities.Alert(
                    id: alertService.GetAll().Count() + 1,
                    tx: tx,
                    message: $"Suspicious transaction detected by {ruleName}",
                    severity: "High"
                );
                alertService.Add(alert);
            };

            analyzer.AddRule(new HighAmountRule());
            analyzer.AddRule(new CountryMismatchRule());
            analyzer.AddRule(new VelocityRule());

            analyzer.AddRule(new GeoLocationRule());
            analyzer.AddRule(new BehaviorChangeRule());
            analyzer.AddRule(new NightActivityRule());

            var composite = new CompositeRiskRule(0.5);
            composite.AddSubRule(new HighAmountRule());
            composite.AddSubRule(new CountryMismatchRule());
            composite.AddSubRule(new VelocityRule());
            analyzer.AddRule(composite);

            return (analyzer, alertService, transactionService);
        }
    }
}
