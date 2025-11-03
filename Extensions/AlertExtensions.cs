using System.Collections.Generic;
using System.Linq;
using FraudDetectionSystem.Entities;

namespace FraudDetectionSystem.Extensions
{
   
    public static class AlertExtensions
    {
        
        public static IEnumerable<Alert> GetHighSeverity(this IEnumerable<Alert> alerts)
        {
            return alerts.Where(a => a.Severity.Equals("High", System.StringComparison.OrdinalIgnoreCase));
        }

       
        public static int CountBySeverity(this IEnumerable<Alert> alerts, string severity)
        {
            return alerts.Count(a => a.Severity.Equals(severity, System.StringComparison.OrdinalIgnoreCase));
        }

        
        public static IEnumerable<Alert> SortBySeverity(this IEnumerable<Alert> alerts)
        {
            var order = new List<string> { "High", "Medium", "Low" };
            return alerts.OrderBy(a => order.IndexOf(a.Severity));
        }
    }
}
