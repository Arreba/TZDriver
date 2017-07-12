using System;
using System.Threading.Tasks;
using TZDriver.Events;
using Windows.Devices.Geolocation;

namespace TZDriver.Services.IServices
{
    public interface IGeoLocationService
    {
        /// <summary>
        /// Desired accuracy in meteres.
        /// The better accuracy (=the lower values), the longer it takes until the GPS sensor returns a position.
        /// </summary>
        uint DesiredAccuracyInMeters { get; set; }

        /// <summary>
        /// Movement in meteres.
        /// The distance of movement that is required for the Geolocator to raise a PositionChanged event.
        /// </summary>
        double MovementThreshold { get; set; }

        /// <summary>
        /// Check whether location is currently monitored.
        /// </summary>
        bool IsListening { get; }

        /// <summary>
        /// Indicates if the geolocation service is available on this device.
        /// </summary>
        bool IsGeolocationAvailable { get; }

        /// <summary>
        /// Raised when the current position is updated.
        /// </summary>
        event EventHandler<Geoposition> PositionChanged;

        /// <summary>
        /// Gets the last known recorded position.
        /// </summary>
        Geoposition CurrentPosition { get; set; }

        /// <summary>
        /// Initializes the location service with a default accuracy (100 meters) and movement threshold.
        /// </summary>
        /// <returns>True if the initialization was successful and the service can be used.</returns>
        Task<bool> InitializeAsync();

        /// <summary>
        /// Initializes the location service with the specified accuracy and default movement threshold.
        /// </summary>
        /// <param name="desiredAccuracyInMeters">The desired accuracy at which the service provides location updates.</param>
        /// <returns>True if the initialization was successful and the service can be used.</returns>
        Task<bool> InitializeAsync(uint desiredAccuracyInMeters);

        /// <summary>
        /// Initializes the location service with the specified accuracy and movement threshold.
        /// </summary>
        /// <param name="desiredAccuracyInMeters">The desired accuracy at which the service provides location updates.</param>
        /// <param name="movementThreshold">The distance of movement, in meters, that is required for the service to raise the PositionChanged event.</param>
        /// <returns>True if the initialization was successful and the service can be used.</returns>
        Task<bool> InitializeAsync(uint desiredAccuracyInMeters, double movementThreshold);

        /// <summary>
        /// gets location for the service.
        /// </summary>
        /// <returns>Geoposition.</returns>
        Task<Geoposition> GetPositionAsync();

        /// <summary>
        /// Starts the service listening for location updates.
        /// </summary>
        /// <returns>An object that is used to manage the asynchronous operation.</returns>
        Task StartListeningAsync();

        /// <summary>
        /// Stops listening for GPS position updates.
        /// </summary>
        void StopListening();
    }
}
