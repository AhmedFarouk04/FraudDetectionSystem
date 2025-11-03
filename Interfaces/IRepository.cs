using System.Collections.Generic;

namespace FraudDetectionSystem.Interfaces
{
    
    public interface IRepository<T>
    {
        void Add(T item);
        IEnumerable<T> GetAll();
        void Remove(T item);
        void Clear();
    }
}
