using System;
using FraudDetectionSystem.Entities;

namespace FraudDetectionSystem.Rules
{
    
    public abstract class FraudRule
    {
        public string Name { get; protected set; }

        protected FraudRule(string name)
        {
            Name = name ?? "Unnamed Rule";
        }

        
        public abstract bool IsSuspicious(Transaction tx);

        
        public virtual void Explain()
        {
            Console.WriteLine($"Rule active: {Name}");
        }
    }
}
