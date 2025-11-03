using System;
using FraudDetectionSystem.Entities;
using FraudDetectionSystem.Interfaces;

namespace FraudDetectionSystem.Services
{

    public class AlertService : BaseRepository<Alert>
    {
        public AlertService(ILogger logger) : base(logger)
        {
        }

  
        public void DisplayAll()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n Active Alerts:");

            if (_items.Count == 0)
            {
                Console.WriteLine("No active alerts.");
            }
            else
            {
                foreach (var alert in _items)
                    Console.WriteLine(alert.ToString());
            }

            Console.ResetColor();
        }

  
        public void DisplayBySeverity(string severity)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n Alerts with severity: {severity}");

            foreach (var alert in _items)
            {
                if (alert.Severity.Equals(severity, StringComparison.OrdinalIgnoreCase))
                    Console.WriteLine(alert.ToString());
            }

            Console.ResetColor();
        }

        public void ClearLowSeverity()
        {
            _items.RemoveAll(a => a.Severity.Equals("Low", StringComparison.OrdinalIgnoreCase));
            _logger.LogWarning("All low-severity alerts cleared.");
        }
    }
}
