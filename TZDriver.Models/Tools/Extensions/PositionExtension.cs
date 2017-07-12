using System;
using System.Collections.Generic;
using System.Linq;
using TZDriver.Models.Tools.Utilities;

namespace TZDriver.Models.Tools.Extensions
{
    public static class PositionExtensions
    {
        private const double earthRadiusKM = 6367.0;

        //public static GeoLocationData GetCentrePoint(this IEnumerable<GeoLocationData> coordinates)
        //{
        //    if (coordinates == null || !coordinates.Any())
        //    {
        //        return GeoLocationData.Unknown;
        //    }

        //    double xSum = 0;
        //    double ySum = 0;
        //    double zSum = 0;
        //    int total = 0;

        //    foreach (var i in coordinates.Where(x => !x.IsUnknown))
        //    {
        //        double lat = i.Latitude * Math.PI / 180;
        //        double lon = i.Longitude * Math.PI / 180;
        //        double x = Math.Cos(lat) * Math.Cos(lon);
        //        double y = Math.Cos(lat) * Math.Sin(lon);
        //        double z = Math.Sin(lat);
        //        xSum += x;
        //        ySum += y;
        //        zSum += z;
        //        total++;
        //    }

        //    xSum = xSum / total;
        //    ySum = ySum / total;
        //    zSum = zSum / total;

        //    double Lon = Math.Atan2(ySum, xSum);
        //    double Hyp = Math.Sqrt(xSum * xSum + ySum * ySum);
        //    double Lat = Math.Atan2(zSum, Hyp);

        //    return new GeoLocationData(Lat * 180 / Math.PI, Lon * 180 / Math.PI);
        //}

        /// <summary>
        ///     Can be converted to Gets the closet Driver from th client Pickup Location if the Geolocationdata include an IsOccupied property.
        /// </summary>
        /// <param name="coordinates">The Driver coordinates.</param>
        /// <param name="centerCoordinate">The client coordinate.</param>
        /// <param name="">use Google map to return each driver drive distance to the client </param>
        /// <param name="IsOccupied">The status of the vehicle.</param>
        /// <returns>System.Double.</returns>
        //public static GeoLocationData GetFarthestCoordinate(this IEnumerable<GeoLocationData> coordinates, GeoLocationData centerCoordinate)
        //{
        //    GeoLocationData farthestCoordinate = null;

        //    double farthestDistance = 0;
        //    foreach (var coordinate in coordinates)
        //    {
        //        var distance = CalculateDistance(centerCoordinate.Latitude, centerCoordinate.Longitude, centerCoordinate.Latitude, centerCoordinate.Longitude);
        //        if (distance > farthestDistance)
        //        {
        //            farthestCoordinate = coordinate;
        //            farthestDistance = distance;
        //        }
        //    }

        //    return farthestCoordinate;
        //}

        /// <summary>
        ///     Gets the longest radius from center.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <param name="centerCoordinate">The center coordinate.</param>
        /// <param name="zoomLevel">The zoom level.</param>
        /// <returns>System.Double.</returns>
        //public static double GetLongestRadiusFromCenter(this IEnumerable<GeoLocationData> coordinates, GeoLocationData centerCoordinate, double zoomLevel)
        //{
        //    var farthestCoordinate = coordinates.GetFarthestCoordinate(centerCoordinate);
        //    if (farthestCoordinate == null)
        //    {
        //        return 0;
        //    }

        //    var distanceInMeters = CalculateDistance(farthestCoordinate.Latitude, farthestCoordinate.Longitude, farthestCoordinate.Latitude, farthestCoordinate.Longitude);
        //    return MetersToPixels(distanceInMeters, centerCoordinate.Latitude, zoomLevel);
        //}

        public static double MetersToPixels(double meters, double latitude, double zoomLevel)
        {
            // The ground resolution (in meters per pixel) varies depending on the level of detail
            // and the latitude at which it’s measured. It can be calculated as follows:
            double metersPerPixels = meters / ((Math.Cos(latitude * Math.PI / 180) * 2 * Math.PI * 6378137) / (256 * Math.Pow(2, zoomLevel)));
            return metersPerPixels;
        }

        //TODO GATH: This is the official way to translate meters into pixels (BUT IT DOESNT WORK)
        ////public static double MetersToPixels(double meters, double latitude, double zoomLevel)
        ////{
        ////    var pixels = meters / (156543.04 * Math.Cos(latitude) / Math.Pow(2, zoomLevel));
        ////    return Math.Abs(pixels);
        ////}

        /// <summary>
        ///     Gets the accuracy radius in pixels.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="zoomLevel">The zoom level.</param>
        //public static double GetAccuracyRadius(this GeoLocationData coordinate, double zoomLevel)
        //{
        //    var radius = MetersToPixels(coordinate.Accuracy, coordinate.Latitude, zoomLevel);
        //    return radius;
        //}

        private const double LatitudeOffset = -0.000025;
        private const double LongitudeOffset = -0.00005;

        /// <summary>
        ///     Add some meters of offset to the given Position.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The new Position.</returns>
        //public static GeoLocationData WithRandomOffset(this GeoLocationData coordinate)
        //{
        //    return new GeoLocationData(coordinate.Latitude + LatitudeOffset, coordinate.Longitude + LongitudeOffset);
        //}

        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            //double theta = lon1 - lon2;
            //double distance = Math.Sin(DegreeToRadian(lat1)) * Math.Sin(DegreeToRadian(lat2)) + Math.Cos(DegreeToRadian(lat1)) * Math.Cos(DegreeToRadian(lat2)) * Math.Cos(DegreeToRadian(theta));
            //distance = Math.Acos(distance);
            //distance = RadianToDegree(distance);
            //distance = distance * 60 * 1.1515 * 1.609344;
            //return distance;

            var dLat = DegreeToRadian(lat2 - lat1);
            var dLong = DegreeToRadian(lon2 - lon1);
            var sinLat = Math.Sin(0.5 * dLat);
            var sinLong = Math.Sin(0.5 * dLong);
            var cosLat1 = Math.Cos(DegreeToRadian(lat1));
            var cosLat2 = Math.Cos(DegreeToRadian(lat2));
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
