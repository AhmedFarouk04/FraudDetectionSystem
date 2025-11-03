using FraudDetectionSystem.Entities;
using FraudDetectionSystem.Interfaces;

namespace FraudDetectionSystem.Services
{
    
    public class TransactionService : BaseRepository<Transaction>
    {
        public TransactionService(ILogger logger) : base(logger)
        {
        }

        
        public IEnumerable<Transaction> GetByUser(User user)
        {
            if (user == null)
                yield break;

            foreach (var tx in _items)
            {
                if (tx.User.Id == user.Id)
                    yield return tx;
            }
        }

        
        public IEnumerable<Transaction> FilterByCountry(string country)
        {
            if (string.IsNullOrWhiteSpace(country))
                yield break;

            foreach (var tx in _items)
            {
                if (tx.Country.Equals(country, StringComparison.OrdinalIgnoreCase))
                    yield return tx;
            }
        }
    }
}
