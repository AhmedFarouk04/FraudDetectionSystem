using System;
using System.Collections.Generic;
using FraudDetectionSystem.Interfaces;

namespace FraudDetectionSystem.Services
{
    
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly List<T> _items = new();
        protected readonly ILogger _logger;

        public BaseRepository(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public virtual void Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _items.Add(item);
            _logger.LogInfo($"{typeof(T).Name} added successfully.");
        }

        public virtual IEnumerable<T> GetAll() => _items;

        public virtual void Remove(T item)
        {
            if (item == null)
                return;

            _items.Remove(item);
            _logger.LogWarning($"{typeof(T).Name} removed.");
        }

        public virtual void Clear()
        {
            _items.Clear();
            _logger.LogWarning($"All {typeof(T).Name}s cleared.");
        }
    }
}
