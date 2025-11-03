using System;
using FraudDetectionSystem.Entities;
using FraudDetectionSystem.Services;

namespace FraudDetectionSystem.Core
{
    public static class TransactionSimulator
    {
        public static User CreateSampleUser(TransactionService service)
        {
            var user = new User(id: 1, name: "Ahmed", country: "Egypt");

            var transactions = new[]
            {
                new Transaction(1, user, 300m, "Egypt", DateTime.Now.AddMinutes(-30)),
                new Transaction(2, user, 500m, "Egypt", DateTime.Now.AddMinutes(-5)),
                new Transaction(3, user, 12000m, "Germany", DateTime.Now)
            };

            foreach (var tx in transactions)
            {
                user.Transactions.Add(tx);
                service.Add(tx);
            }

            return user;
        }
    }
}
