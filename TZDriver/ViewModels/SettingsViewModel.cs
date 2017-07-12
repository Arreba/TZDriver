using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using TZDriver.Models.DataModels;
using TZDriver.Models.Services.IServices;
using TZDriver.Models.Tools.Helpers;
using TZDriver.Models.Tools.Utilities;
using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation;
using Windows.Devices.Geolocation.Geofencing;
using Windows.Globalization;
using Windows.Globalization.DateTimeFormatting;
using Windows.Storage;
using Windows.UI.Xaml.Navigation;

namespace TZDriver.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private Geolocator geolocator;
        private Calendar calendar;
        private StorageFolder localFolder;
        private DateTimeFormatter formatterLongTime;
        Fence fence;
        Location location;
        IBackgroundService backgroundService;
        private IBackgroundTaskRegistration _geofenceBackgroundTask = null;
        private IBackgroundTaskRegistration _applicationTriggerBackgroundTask = null;

        // A pointer to the ApplicationTrigger so we can signal it later
        ApplicationTrigger applicationtrigger = null;
        LocationTrigger locationTrigger = null;

        private ObservableCollection<Fence> _fences;
        public ObservableCollection<Fence> Fences
        {
            get { return _fences; }
            set { _fences = value; }
        }

        private ObservableCollection<Location> _locations;
        public ObservableCollection<Location> Locations
        {
            get { return _locations; }
            set { _locations = value; }
        }

        public SettingsViewModel(IBackgroundService _backgroundService)
        {
            backgroundService = _backgroundService;

            // Create a new trigger
            applicationtrigger = new ApplicationTrigger();
            localFolder = ApplicationData.Current.LocalFolder;

            _fences = new ObservableCollection<Fence>();
            _locations = new ObservableCollection<Location>();
            formatterLongTime = new DateTimeFormatter("{hour.integer}:{minute.integer(2)}:{second.integer(2)}", new[] { "en-US" }, "US", Windows.Globalization.CalendarIdentifiers.Gregorian, Windows.Globalization.ClockIdentifiers.TwentyFourHour);
            calendar = new Calendar();
        }

        private async void InitializeMainViewModel()
        {
            await InitializeForegroundGeolocation();
            await InitializeBackgroundGeolocation();
            await InitializeForegroundGeoFence();
            await InitializeBackgroundGeoFence();
        }

        public async Task InitializeForegroundGeolocation()
        {
            // Request permission to access location
            var accessStatus = await Geolocator.RequestAccessAsync();

            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:
                    //Initaliases the GPS with custom parameters
                    geolocator = new Geolocator { DesiredAccuracyInMeters = 5, MovementThreshold = 2 };
                    Geoposition pos = await geolocator.GetGeopositionAsync();
                    break;

                case GeolocationAccessStatus.Denied:
                case GeolocationAccessStatus.Unspecified:
                    Toast.ShowToast("Unspecificed error or Location access was denied!");
                    break;
            }
        }

        private DelegateCommand _startLocationMonitoringCommand;
        public DelegateCommand StartLocationMonitoringCommand
           => _startLocationMonitoringCommand ?? (_startLocationMonitoringCommand = new DelegateCommand(ExecuteStartLocationCommand, StartLocationCommandCanExecute));
        bool StartLocationCommandCanExecute() => CurrentTripDataRepo.IsTripStarted != true;

        public async void ExecuteStartLocationCommand()
        {

        }

        public async Task InitializeBackgroundGeolocation()
        {
            // Loop through all background tasks to see if SampleGeofenceBackgroundTask is already registered
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == Constants.ApplicationTriggerBackgroundTaskName)
                    _applicationTriggerBackgroundTask = task.Value;
            }

            if (_applicationTriggerBackgroundTask == null)
            {
                // Create a new trigger
                //applicationtrigger = new ApplicationTrigger();

                SystemCondition condition = new SystemCondition(SystemConditionType.UserNotPresent | SystemConditionType.InternetAvailable);

                _applicationTriggerBackgroundTask = await BackgroundTaskHelper.RegisterBackgroundTask(
                Constants.ApplicationTriggerBackgroundTaskEntryPoint,
                Constants.ApplicationTriggerBackgroundTaskName,
                applicationtrigger,
                null);
                var result = await applicationtrigger.RequestAsync();
                if (result.Equals(ApplicationTriggerResult.Allowed))
                    Toast.ShowToast("Location Monitoring Started");
            }

            await LoadExitingLocations();
        }

        /// <summary>
        /// Populate events list box with entries from Background Location Events.
        /// </summary>
        private async Task LoadExitingLocations()
        {
            if (Locations != null)
            {
                Locations.Clear();
                Locations = await localStoreService.LoadLocalData<ObservableCollection<Location>>(Constants.LocationFileIdentifier);
            }
        }

        private DelegateCommand _startGeofenceMonitoringCommand;
        public DelegateCommand StartGeofenceMonitoringCommand
           => _startGeofenceMonitoringCommand ?? (_startGeofenceMonitoringCommand = new DelegateCommand(ExecuteStartGeofenceCommand, StartGeofenceCommandCanExecute));
        bool StartGeofenceCommandCanExecute() => CurrentTripDataRepo.IsTripStarted != true;

        public async void ExecuteStartGeofenceCommand()
        {

        }

        public async Task InitializeForegroundGeoFence()
        {
            if (!IsGeofenceRegistered())
            {
                GeofenceMonitor.Current.Geofences.Clear();
                Geofence TZGeofence = GenerateGeofence();
                GeofenceMonitor.Current.Geofences.Add(TZGeofence);
            }

            LoadExistingFences();
        }

        public bool IsGeofenceRegistered()
        {
            bool IsRegistered = false;
            foreach (var geofenceItem in GeofenceMonitor.Current.Geofences)
            {
                if (geofenceItem.Id == Constants.GeofenceId)
                {
                    IsRegistered = true;

                    break;
                }
            }
            return IsRegistered;
        }

        private Geofence GenerateGeofence()
        {
            BasicGeoposition position = new BasicGeoposition()
            {
                Altitude = 0.0,
                Latitude = 4.796973,
                Longitude = 7.032254
            };

            double radius = 100.0;

            var dwellTime = TimeSpan.FromMinutes(2);

            // the geofence is a circular region
            Geocircle geocircle = new Geocircle(position, radius);

            bool singleUse = false;

            // want to listen for enter geofence, exit geofence and remove geofence events
            // you can select a subset of these event states
            MonitoredGeofenceStates monitoredStates = MonitoredGeofenceStates.Entered | MonitoredGeofenceStates.Exited | MonitoredGeofenceStates.Removed;

            return new Geofence(Constants.GeofenceId, geocircle, monitoredStates, singleUse, dwellTime);
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (suspensionState.Any())
            {
                // use cache value(s)
                // clear any cache
                suspensionState.Clear();
            }
            else
            {
                var progressDialog = UserDialogs.Instance.Loading("Loading Active Jobs...", maskType: MaskType.Clear);



                progressDialog?.Dispose();
            }

            if (mode == NavigationMode.New)
            {
                var navigationView = NavigationService.Frame.BackStack.FirstOrDefault(b => b.SourcePageType.Name == nameof(TripView));
                if (navigationView != null)
                    NavigationService.Frame.BackStack.Remove(navigationView);
            }

            await Task.CompletedTask;
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending)
        {
            if (suspending)
            {
                // persist into cache
            }
            await base.OnNavigatedFromAsync(suspensionState, suspending);

            await Task.CompletedTask;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            await Task.CompletedTask;
        }
    }
}
