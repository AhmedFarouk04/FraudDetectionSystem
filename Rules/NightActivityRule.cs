using FraudDetectionSystem.Entities;

namespace FraudDetectionSystem.Rules
{
    public class NightActivityRule : FraudRule
    {
        private readonly int _startHour;
        private readonly int _endHour;

        public NightActivityRule(int startHour = 0, int endHour = 6) : base("Night Activity Rule")
        {
            Name = "Night Activity Rule";
            _startHour = startHour;
            _endHour = endHour;
        }

        public override bool IsSuspicious(Transaction tx)
        {
            var hour = tx.Time.Hour;
            return hour >= _startHour && hour <= _endHour;
        }
    }
}
