using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Threading.Tasks;
using TZDriver.Events;
using TZDriver.Models.Tools.Utilities;
using TZDriver.Services.IServices;
using Windows.ApplicationModel;
using Windows.Devices.Geolocation;
using Windows.System;
using Windows.UI.Popups;

namespace TZDriver.Services
{
    public class GeoLocationService : IGeoLocationService
    {
        private Geolocator geolocator;

        /// <summary>
        /// Gets the last known recorded position.
        /// </summary>
        public Geoposition CurrentPosition { get; set; }

        /// <summary>
        /// Desired accuracy in meteres.
        /// The better accuracy (=the lower values), the longer it takes until the GPS sensor returns a position.
        /// </summary>
        public uint DesiredAccuracyInMeters { get; set; }

        /// <summary>
        /// Movement in meteres.
        /// The distance of movement that is required for the Geolocator to raise a PositionChanged event.
        /// </summary>
        public double MovementThreshold { get; set; }

        /// <summary>
        /// Check whether location is currently monitored.
        /// </summary>
        public bool IsListening { get; set; }

        /// <summary>
        /// Raised when the current position is updated.
        /// </summary>
        public event EventHandler<Geoposition> PositionChanged;

        /// <summary>
        /// Initializes the location service with a default accuracy (100 meters) and movement threshold.
        /// </summary>
        /// <returns>True if the initialization was successful and the service can be used.</returns>
        public Task<bool> InitializeAsync()
        {
            return InitializeAsync(DesiredAccuracyInMeters);
        }

        /// <summary>
        /// Initializes the location service with the specified accuracy and default movement threshold.
        /// </summary>
        /// <param name="desiredAccuracyInMeters">The desired accuracy at which the service provides location updates.</param>
        /// <returns>True if the initialization was successful and the service can be used.</returns>
        public Task<bool> InitializeAsync(uint desiredAccuracyInMeters)
        {
            return InitializeAsync(desiredAccuracyInMeters, MovementThreshold);
        }

        /// <summary>
        /// Initializes the location service with the specified accuracy and movement threshold.
        /// </summary>
        /// <param name="desiredAccuracyInMeters">The desired accuracy at which the service provides location updates.</param>
        /// <param name="movementThreshold">The distance of movement, in meters, that is required for the service to raise the PositionChanged event.</param>
        /// <returns>True if the initialization was successful and the service can be used.</returns>
        public async Task<bool> InitializeAsync(uint desiredAccuracyInMeters, double movementThreshold)
        {
            if (geolocator != null)
            {
                geolocator.PositionChanged -= GeolocatorOnPositionChanged;
                geolocator = null;
            }

            GeolocationAccessStatus accessStatus = GeolocationAccessStatus.Unspecified;
            bool result;

            try
            {
                // Dispatch back to the main thread
                accessStatus = await Geolocator.RequestAccessAsync();

                switch (accessStatus)
                {
                    case GeolocationAccessStatus.Allowed:
                        geolocator = new Geolocator
                        {
                            DesiredAccuracyInMeters = desiredAccuracyInMeters,
                            MovementThreshold = movementThreshold
                        };
                        result = true;
                        break;

                    case GeolocationAccessStatus.Denied:
                        result = false;
                        break;
                    case GeolocationAccessStatus.Unspecified:
                    default:
                        await DispatcherHelper.RunAsync(async () =>
                        {
                            var dlg = new MessageDialog("Whoops! You haven't granted permission for " + Package.Current.DisplayName + " to access your location. Would you like to go to Settings and do that now?");
                            dlg.Commands.Add(new UICommand("Yes", async (command) =>
                            {
                                var uri = new Uri("ms-settings:privacy-location");
                                await Launcher.LaunchUriAsync(uri);
                            }));
                            dlg.Commands.Add(new UICommand("No"));
                            dlg.DefaultCommandIndex = 0;
                            dlg.CancelCommandIndex = 1;
                            var dlgResult = await dlg.ShowAsync();
                        });
                        result = false;
                        break;
                }
            }
            catch (UnauthorizedAccessException)
            {
                accessStatus = GeolocationAccessStatus.Denied;
                result = false;
            }

            return result;
        }

        public bool IsGeolocationAvailable
        {
            get
            {
                PositionStatus status = geolocator.LocationStatus;
                while (status == PositionStatus.Initializing)
                {
                    Task.Delay(10).Wait();
                    status = geolocator.LocationStatus;
                }

                return status != PositionStatus.Disabled && status != PositionStatus.NotAvailable;
            }
        }

        /// <summary>
        /// gets location for the service.
        /// </summary>
        /// <returns>Geoposition.</returns>
        public async Task<Geoposition> GetPositionAsync()
        {
            var location = await geolocator.GetGeopositionAsync(TimeSpan.FromTicks(0), TimeSpan.FromSeconds(60));

            return location;
        }

        /// <summary>
        /// Starts the service listening for location updates.
        /// </summary>
        /// <returns>An object that is used to manage the asynchronous operation.</returns>
        public async Task StartListeningAsync()
        {
            if (geolocator == null || this.IsListening)
                throw new InvalidOperationException(
                    "The StartListening method cannot be called before the InitializeAsync method is called and returns true.");

            geolocator.PositionChanged += GeolocatorOnPositionChanged;
            geolocator.StatusChanged += this.OnLocatorStatusChanged;

            CurrentPosition = await GetPositionAsync();
            Messenger.Default.Send<Geoposition>(CurrentPosition, Constants.BingMapsAPIKey);

            this.IsListening = true;
        }

        /// <summary>
        /// Stops the service listening for location updates.
        /// </summary>
        public void StopListening()
        {
            if (geolocator == null || !this.IsListening) return;

            this.IsListening = false;
            geolocator.PositionChanged -= GeolocatorOnPositionChanged;
            geolocator.StatusChanged -= this.OnLocatorStatusChanged;
        }

        private async void GeolocatorOnPositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            if (args == null) return;

            CurrentPosition = args.Position;
            await DispatcherHelper.RunAsync(() =>
            {
                PositionChanged?.Invoke(this, CurrentPosition);
            });
        }

        private void OnLocatorStatusChanged(Geolocator sender, StatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case PositionStatus.Disabled:
                    if (this.IsListening)
                    {
                        this.StopListening();
                    }

                    this.geolocator = null;

                    break;
                case PositionStatus.NoData:
                    break;
                default:
                    return;
            }
        }
    }
}
