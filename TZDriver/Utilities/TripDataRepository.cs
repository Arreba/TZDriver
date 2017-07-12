using GalaSoft.MvvmLight;
using System;
using System.Runtime.Serialization;
using TZDriver.Models.Tools.Helpers;
using TZDriver.Models.Tools.Utilities;
using Windows.Devices.Geolocation;
using Windows.Devices.Sensors;

namespace TZDriver.Utilities
{
    public class TripDataRepository : ObservableObject
    {
        private string _currentUser;
        public string CurrentUser
        {
            get { return this._currentUser; }
            set { this.Set<string>(ref this._currentUser, value); }
        }

        private double _currentUserRating = 3.8;
        public double CurrentUserRating
        {
            get { return this._currentUserRating; }
            set { this.Set<double>(ref this._currentUserRating, value); }
        }

        private double _currentPickupMunites;
        public double CurrentPickupMunites
        {
            get { return this._currentPickupMunites; }
            set { this.Set<double>(ref this._currentPickupMunites, value); }
        }

        private Geoposition _currentUserPosition = null;
        public Geoposition CurrentUserPosition
        {
            get { return this._currentUserPosition; }
            set
            {
                this.Set(ref this._currentUserPosition, value);
                this.Geopoint = value.Coordinate.Point;
                this.Accuracy = value.Coordinate.Accuracy;
                this.Heading = Convert.ToDouble(double.IsNaN(Convert.ToDouble(value.Coordinate.Heading)) ? 0 : value.Coordinate.Heading);
                this.Speed = Convert.ToDouble(double.IsNaN(Convert.ToDouble(value.Coordinate.Speed)) ? 0 : value.Coordinate.Speed);
                //this.Zoom = 18D;
                //this.Pitch = 70;
            }
        }

        /// <summary>
        /// Gets a Geopoint representation of the current location for use with the map service APIs.
        /// </summary>
        private Geopoint _geoPoint;
        public Geopoint Geopoint
        {
            get { return this._geoPoint; }
            set
            {
                this.Set<Geopoint>(ref this._geoPoint, value);
            }
        }

        /// <summary>
        /// Gets the geographic position of the location.
        /// </summary>
        public BasicGeoposition Position => this.CurrentUserPosition.Coordinate.Point.Position;

        /// <summary>
        /// Gets or sets a value that indicates whether the location data is available
        /// </summary>
        private bool _isPositonAvailable = false;
        public bool IsPositonAvailable
        {
            get { return this._isPositonAvailable; }
            set { this.Set<bool>(ref this._isPositonAvailable, value); }
        }

        private double _latitude;
        public double Latitude
        {
            get { return this._latitude; }
            set { this.Set<double>( ref this._latitude, value); }
        }

        private double _longitude;
        public double Longitude
        {
            get { return this._longitude; }
            set { this.Set<double>( ref this._longitude, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates when the travel info was last updated.
        /// </summary>
        private DateTimeOffset _timestamp;
        public DateTimeOffset Timestamp
        {
            get { return this._timestamp; }
            set
            {
                this.Set<DateTimeOffset>(ref this._timestamp, value);
            }
        }

        private double _speed;
        public double Speed
        {
            get { return this._speed; }
            set { this.Set<double>(ref this._speed, value); }
        }

        private double _heading;
        public double Heading
        {
            get { return this._heading; }
            set { this.Set<double>(ref this._heading, value); }
        }

        private double _accuracy;
        public double Accuracy
        {
            get { return this._accuracy; }
            set { this.Set<double>(ref this._accuracy, value); }
        }

        private double _currentDistance;
        public double CurrentDistance
        {
            get { return this._currentDistance; }
            set { this.Set<double>(ref this._currentDistance, value); }
        }

        private double _currentDuration;
        public double CurrentDuration
        {
            get { return this._currentDuration; }
            set { this.Set<double>(ref this._currentDuration, value); }
        }

        public string TripDuration => EndTime != DateTime.MinValue ? String.Format("{0:%h}:{0:mm} min", (EndTime.Subtract(StartTime))) : "0.00";

        private string _driveValue = EnumHelper.DeParse(DriveStateStatus.PickupState);
        public string DriveValue
        {
            get { return this._driveValue; }
            set
            {
                if (this._driveValue != value)
                {
                    this.Set(ref this._driveValue, value);
                }
            }
        }

        private double _tripHours = 0.0;
        public double TripHours
        {
            get { return this._tripHours; }
            set { this.Set(ref this._tripHours, value); }
        }

        private double _tripMinutes = 0.0;
        public double TripMinutes
        {
            get { return this._tripMinutes; }
            set { this.Set(ref this._tripMinutes, value); }
        }

        private double _tripSeconds = 0.0;
        public double TripSeconds
        {
            get { return this._tripSeconds; }
            set { this.Set(ref this._tripSeconds, value); }
        }

        private DateTime _startTime;
        public DateTime StartTime
        {
            get { return this._startTime; }
            set { this.Set(ref this._startTime, value); }
        }

        private DateTime _endTime;
        public DateTime EndTime
        {
            get { return this._endTime; }
            set { this.Set(ref this._endTime, value); }
        }

        private double _tripFare = 1500.0;
        public double TripFare
        {
            get { return this._tripFare; }
            set
            {
                if (value > this._tripFare)
                    this.Set(ref this._tripFare, value);
            }
        }

        private TripPayMode _selectedPayMode;
        public TripPayMode SelectedPayMode
        {
            get { return this._selectedPayMode; }
            set { this.Set(ref this._selectedPayMode, value); }
        }

        private TripPayStatus _selectedPayStatus;
        public TripPayStatus SelectedPayStatus
        {
            get { return this._selectedPayStatus; }
            set { this.Set(ref this._selectedPayStatus, value); }
        }

        private bool _isTripStarted = false;
        public bool IsTripStarted
        {
            get { return this._isTripStarted; }
            set { this._isTripStarted = value; }
        }

        private ActivityType _currentActivity;
        public ActivityType CurrentActivity
        {
            get { return this._currentActivity; }
            set { this.Set<ActivityType>(ref this._currentActivity, value); }
        }

        public TripDataRepository()
        {
            _geoPoint = new Geopoint(defaultPosition);
            _zoom = 18D;
            _pitch = 70;
        }

        //default location.
        private readonly BasicGeoposition defaultPosition = new BasicGeoposition()
        {
            Latitude = 4.609425,
            Longitude = 7.3417
        };

        private double _zoom;
        public double Zoom
        {
            get { return _zoom; }
            set { Set(ref _zoom, value); }
        }

        private double _pitch;
        public double Pitch
        {
            get { return _pitch; }
            set { Set(ref _pitch, value); }
        }

        [IgnoreDataMember]
        public bool IsUnknown
        {
            get
            {
                if (double.IsNaN(this.Longitude) || double.IsNaN(this.Latitude))
                {
                    return true;
                }

                return false;
            }
        }
    }
}
