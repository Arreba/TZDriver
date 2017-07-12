using System;
using Windows.Devices.Geolocation.Geofencing;

namespace TZDriver.Models.DataModels
{
    public class Fence
    {
        public GeofenceState FenceState { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public double Accuracy { get; set; }
    }
}
