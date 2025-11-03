using System;

namespace FraudDetectionSystem.Entities
{
    public class Alert
    {
        public int Id { get; }
        public Transaction Transaction { get; }
        public string Message { get; }
        public string Severity { get; }
        public DateTime Timestamp { get; }   

        public Alert(int id, Transaction tx, string message, string severity)
        {
            Id = id;
            Transaction = tx;
            Message = message;
            Severity = severity;
            Timestamp = DateTime.Now; 
        }
    }
}
