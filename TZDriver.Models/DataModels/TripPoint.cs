using System;

namespace TZDriver.Models.DataModels
{
    public class TripPoint
    {
        //To recognise the Trip
        public int TripPointId { get; set; }

        public double TripLatitude { get; set; }

        public double TripLongitude { get; set; }

        public double TripHeading { get; set; }

        public double TripSpeed { get; set; }

        public DateTime RecordedTimeStamp { get; set; }

        #region Relationships

        //To recognise the Trip
        public int TripDataId { get; set; }
        public TripData TripData{ get; set; }

        #endregion
    }
}
