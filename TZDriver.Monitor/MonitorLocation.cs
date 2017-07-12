using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TZDriver.Models.DataModels;
using TZDriver.Models.Tools.Utilities;
using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation;
using Windows.Storage;
using Windows.System.Threading;

namespace TZDriver.Monitor
{
    public sealed class MonitorLocation : IBackgroundTask
    {
        BackgroundTaskCancellationReason _cancelReason = BackgroundTaskCancellationReason.Abort;
        volatile bool _cancelRequested = false;
        BackgroundTaskDeferral _deferral = null;
        ThreadPoolTimer _periodicTimer = null;
        private List<Location> _locations = null;
        private Location _location = null;
        private Geolocator geolocator;
        uint _progress = 0;
        IBackgroundTaskInstance _taskInstance = null;

        public MonitorLocation()
        {
            _locations = new List<Location>();
        }

        // The Run method is the entry point of a background task.
        void IBackgroundTask.Run(IBackgroundTaskInstance taskInstance)
        {
            // Associate a cancellation handler with the background task.
            taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCanceled);

            // Get the deferral object from the task instance, and take a reference to the taskInstance;
            _deferral = taskInstance.GetDeferral();
            _taskInstance = taskInstance;

            _periodicTimer = ThreadPoolTimer.CreatePeriodicTimer(new TimerElapsedHandler(PeriodicTimerCallback), TimeSpan.FromHours(1));

            //Initaliases the GPS with custom parameters
            geolocator = new Geolocator { DesiredAccuracyInMeters = 10, MovementThreshold = 5 };
            Task.Run(() =>
            {
                geolocator.PositionChanged += OnPositionChanged;
            });
        }

        private void PeriodicTimerCallback(ThreadPoolTimer timer)
        {
            if ((_cancelRequested == false))
            {
                DateTime now = DateTime.Now;
                DateTime date = DateTime.Today;
                TimeSpan time1 = new TimeSpan(3, 0, 0);
                TimeSpan time2 = new TimeSpan(15, 0, 0);
                if (date.Add(time1) == now || date.Add(time2) == now)
                {
                    _locations.RemoveRange(0, (_locations.Count) / 4);
                }
            }
            else
            {
                _periodicTimer.Cancel();
            }
        }

        private void OnPositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            if ((_cancelRequested == false))
            {
                _progress += 10;
                _taskInstance.Progress = _progress;

                var pos = args.Position;

                _location = new Location
                {
                    Accuracy = pos.Coordinate.Accuracy,
                    Latitude = pos.Coordinate.Point.Position.Latitude,
                    Longitude = pos.Coordinate.Point.Position.Longitude,
                    TimeStamp = pos.Coordinate.Timestamp,
                    Speed = pos.Coordinate.Speed
                };
                _locations.Add(_location);

                if (_locations.Count != 0)
                    SaveExistingEvents();
            }
            else
            {
                _deferral.Complete();
            }
        }

        private void SaveExistingEvents()
        {
            var json = JsonConvert.SerializeObject(_locations);
            Task.Run(async () =>
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                var dataFolder = await localFolder.CreateFolderAsync("TZFolder", CreationCollisionOption.OpenIfExists);
                var userFile = await dataFolder.CreateFileAsync("TZLocation.json", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(userFile, json);
            });
        }

        private void ReadExistingEvents()
        {
            _locations.Clear();
            Task.Run(async () =>
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                var dataFolder = await localFolder.GetFolderAsync("TZFolder");
                var userFile = await dataFolder.GetFileAsync("TZLocation.json");
                var locationStream = await FileIO.ReadTextAsync(userFile);
                _locations = JsonConvert.DeserializeObject<List<Location>>(locationStream);
            });
        }

        // Handles background task cancellation.
        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            // Indicate that the background task is canceled.
            _cancelRequested = true;
            _cancelReason = reason;
            Toast.ShowToast("Location Background Task has been canceled because of " + reason.ToString());
            geolocator.PositionChanged -= OnPositionChanged;
        }
    }
}
