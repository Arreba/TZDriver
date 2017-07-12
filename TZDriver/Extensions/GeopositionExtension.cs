using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace TZDriver.Extensions
{
    public static class GeopositionExtension
    {
        private const double earthRadiusKM = 6367.0;

        public static double CalculatePositionDistance(this Geoposition OldPostion, Geoposition NewPostion)
        {
            double oldLat = OldPostion.Coordinate.Point.Position.Latitude;
            double oldLon = OldPostion.Coordinate.Point.Position.Longitude;
            double newLat = NewPostion.Coordinate.Point.Position.Latitude;
            double newLon = NewPostion.Coordinate.Point.Position.Longitude;

            var dLat = DegreeToRadian(newLat - oldLat);
            var dLong = DegreeToRadian(newLon - oldLon);
            var sinLat = Math.Sin(0.5 * dLat);
            var sinLong = Math.Sin(0.5 * dLong);
            var cosLat1 = Math.Cos(DegreeToRadian(oldLat));
            var cosLat2 = Math.Cos(DegreeToRadian(newLat));
            var cordLength = Math.Pow(sinLat, 2) + cosLat1 * cosLat2 * Math.Pow(sinLong, 2);
            var centralAngle = 2 * Math.Atan2(Math.Sqrt(cordLength), Math.Sqrt(1 - cordLength));
            return earthRadiusKM * centralAngle;
        }

        private static double DegreeToRadian(double degree)
        {
            return (degree * Math.PI / 180.0);
        }

        private static double RadianToDegree(double radian)
        {
            return (radian / Math.PI * 180.0);
        }
    }
}
