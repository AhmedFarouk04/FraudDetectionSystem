using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FraudDetectionSystem.Entities
{
    public class User
    {
        [JsonInclude]
        public int Id { get; private set; }

        [JsonInclude]
        public string Name { get; private set; }

        [JsonInclude]
        public string Country { get; private set; }

        [JsonInclude]
        public List<Transaction> Transactions { get; private set; }

        public User()
        {
            Name = string.Empty;
            Country = string.Empty;
            Transactions = new List<Transaction>();
        }

        public User(int id, string name, string country)
        {
            if (id <= 0)
                throw new ArgumentException("User ID must be positive.", nameof(id));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "User name cannot be empty.");

            if (string.IsNullOrWhiteSpace(country))
                throw new ArgumentNullException(nameof(country), "Country cannot be empty.");

            Id = id;
            Name = name.Trim();
            Country = country.Trim();
            Transactions = new List<Transaction>();
        }

        public void AddTransaction(Transaction tx)
        {
            if (tx == null)
                throw new ArgumentNullException(nameof(tx), "Transaction cannot be null.");

            Transactions.Add(tx);
        }

        public override string ToString()
        {
            return $"{Id} - {Name} ({Country}) | Transactions: {Transactions.Count}";
        }
    }
}
