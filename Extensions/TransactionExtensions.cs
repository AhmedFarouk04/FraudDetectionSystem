using System;
using System.Collections.Generic;
using System.Linq;
using FraudDetectionSystem.Entities;

namespace FraudDetectionSystem.Extensions
{
  
    public static class TransactionExtensions
    {
       
        public static IEnumerable<Transaction> GetRecent(this IEnumerable<Transaction> transactions, int count)
        {
            return transactions
                .OrderByDescending(t => t.Time)
                .Take(count);
        }

       
        public static IEnumerable<Transaction> GetHighValue(this IEnumerable<Transaction> transactions, decimal threshold)
        {
            return transactions
                .Where(t => t.Amount > threshold);
        }

        
        public static IEnumerable<Transaction> GetCountryMismatched(this IEnumerable<Transaction> transactions)
        {
            return transactions
                .Where(t => !t.Country.Equals(t.User.Country, StringComparison.OrdinalIgnoreCase));
        }
    }
}
