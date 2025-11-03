using System;
using System.Text.Json.Serialization;

namespace FraudDetectionSystem.Entities
{
    public class Transaction
    {
        [JsonInclude]
        public int Id { get; private set; }

        [JsonIgnore]
        public User User { get; set; } = null!;

        [JsonInclude]
        public decimal Amount { get; private set; }

        [JsonInclude]
        public string Country { get; private set; } = string.Empty;

        [JsonInclude]
        public DateTime Time { get; private set; }

        [JsonInclude]
        public double RiskScore { get; set; }

        public Transaction() { }

        public Transaction(int id, User user, decimal amount, string country, DateTime time)
        {
            if (id <= 0)
                throw new ArgumentException("Transaction ID must be positive.", nameof(id));

            if (user == null)
                throw new ArgumentNullException(nameof(user), "User cannot be null.");

            if (amount <= 0)
                throw new ArgumentException("Transaction amount must be greater than zero.", nameof(amount));

            if (string.IsNullOrWhiteSpace(country))
                throw new ArgumentNullException(nameof(country), "Country cannot be empty.");

            Id = id;
            User = user;
            Amount = amount;
            Country = country.Trim();
            Time = time;
            RiskScore = 0;
        }

        public override string ToString()
        {
            return $"Tx#{Id} | {User?.Name ?? "Unknown"} | {Amount:C} | {Country} | {Time:g}";
        }
    }
}
