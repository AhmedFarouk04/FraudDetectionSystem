using System;
using System.Device.Location;
using System.Linq;
using FraudDetectionSystem.Entities;

namespace FraudDetectionSystem.Rules
{
    public class GeoLocationRule : FraudRule
    {
        private readonly double _maxTravelSpeedKmH;
        private readonly double _timeWindowMinutes;

        public GeoLocationRule(double maxSpeed = 1000, double timeWindowMinutes = 60)
            : base("GeoLocation Rule")
        {
            _maxTravelSpeedKmH = maxSpeed;
            _timeWindowMinutes = timeWindowMinutes;
        }

        public override bool IsSuspicious(Transaction tx)
        {
            if (tx?.User?.Transactions == null || tx.User.Transactions.Count == 0)
                return false;

            var lastTx = tx.User.Transactions
                .OrderByDescending(t => t.Time)
                .FirstOrDefault();

            if (lastTx == null || lastTx == tx)
                return false;

            var coords = GetCountryCoordinates(tx.Country);
            var lastCoords = GetCountryCoordinates(lastTx.Country);

            if (coords == null || lastCoords == null)
                return false;

            var distanceKm = GetDistanceKm(coords, lastCoords);
            var timeDiffH = (tx.Time - lastTx.Time).TotalHours;
            if (timeDiffH == 0) return false;

            var speed = distanceKm / timeDiffH;
            return speed > _maxTravelSpeedKmH;
        }

        private GeoCoordinate? GetCountryCoordinates(string country)
        {
            return country.ToLower() switch
            {
                "egypt" => new GeoCoordinate(26.8, 30.8),
                "germany" => new GeoCoordinate(51.1, 10.4),
                "usa" => new GeoCoordinate(38.0, -97.0),
                "uae" => new GeoCoordinate(24.4, 54.4),
                _ => null
            };
        }

        private double GetDistanceKm(GeoCoordinate a, GeoCoordinate b)
        {
            return a.GetDistanceTo(b) / 1000.0;
        }
    }
}
