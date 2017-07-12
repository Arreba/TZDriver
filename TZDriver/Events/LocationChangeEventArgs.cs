using System;
using Windows.Devices.Geolocation;

namespace TZDriver.Events
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
