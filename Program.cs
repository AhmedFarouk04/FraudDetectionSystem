using FraudDetectionSystem.Core;
using FraudDetectionSystem.Entities;
using FraudDetectionSystem.Extensions;
using FraudDetectionSystem.Interfaces;
using FraudDetectionSystem.Services;
using System.Collections.Generic;
using System.Linq;

namespace FraudDetectionSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var logger = new ConsoleLogger();
            var (analyzer, alertService, transactionService) = AppInitializer.Initialize(logger);

            var storage = new DataStorageService("data.json");
            var users = storage.LoadData() ?? new List<User>();

            foreach (var user in users)
            {
                foreach (var tx in user.Transactions)
                {
                    tx.User = user;
                }
            }

            var riskService = new RiskScoreService();
            foreach (var user in users)
            {
                foreach (var tx in user.Transactions)
                {
                    var triggeredRules = analyzer.GetActiveRules()
                        .Where(r => r.IsSuspicious(tx))
                        .ToList();

                    tx.RiskScore = riskService.CalculateScore(tx, triggeredRules);
                }
            }

            SimulationMenu.Start(users, analyzer, transactionService, alertService);

            storage.SaveData(users);
            logger.LogInfo("Simulation ended. Goodbye!");
        }
    }
}
