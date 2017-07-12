using System;
using Windows.Devices.Geolocation;

namespace TZDriver.Models.Tools.Events
{
    public class LocationChangeEventArgs : EventArgs
    {
        public Geoposition Position { get; private set; }
        public LocationChangeEventArgs(Geoposition pos)
        {
            Position = pos;
        }
    }
}
