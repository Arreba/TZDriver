using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TZDriver.Models.DataModels;
using TZDriver.Models.Tools.Utilities;
using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation.Geofencing;
using Windows.Storage;

namespace TZDriver.Monitor
{
    public sealed class MonitorFence : IBackgroundTask
    {
        BackgroundTaskDeferral _deferral;
        private List<Fence> _fences = null;
        private Fence _fence = null;

        public MonitorFence()
        {
            _fences = new List<Fence>();
        }

        void IBackgroundTask.Run(IBackgroundTaskInstance taskInstance)
        {
            // Associate a cancellation handler with the background task.
            taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCanceled);

            // Get the deferral object from the taskInstance;
            _deferral = taskInstance.GetDeferral();

            try
            {
                GetGeofenceStateChangedReports();
            }
            catch (UnauthorizedAccessException)
            {
                Toast.ShowToast("Location permissions are disabled. Enable access through the settings.");
            }
            finally
            {
                _deferral.Complete();
            }
        }

        private void GetGeofenceStateChangedReports()
        {
            GeofenceMonitor monitor = GeofenceMonitor.Current;

            // Registered geofence events can be filtered out if the geofence event time is stale.
            var report = monitor.ReadReports().FirstOrDefault();

            _fence = new Fence()
            {
                Accuracy = report.Geoposition.Coordinate.Accuracy,
                FenceState = report.NewState,
                Latitude = report.Geoposition.Coordinate.Point.Position.Latitude,
                Longitude = report.Geoposition.Coordinate.Point.Position.Longitude,
                TimeStamp = report.Geoposition.Coordinate.Timestamp
            };
            _fences.Add(_fence);

            if ( 0 != _fences.Count)
            {
                SaveExistingEvents();
            }
        }

        private void SaveExistingEvents()
        {
            var fencejson = JsonConvert.SerializeObject(_fences);
            Task.Run(async () =>
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                var dataFolder = await localFolder.CreateFolderAsync("TZFolder", CreationCollisionOption.OpenIfExists);
                var userFile = await dataFolder.CreateFileAsync("TZFence.json", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(userFile, fencejson);
            });
        }

        private void ReadExistingEvents()
        {
            _fences.Clear();
            Task.Run(async () =>
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                var dataFolder = await localFolder.GetFolderAsync("TZFolder");
                var userFile = await dataFolder.GetFileAsync("TZFence.json");
                var fenceStream = await FileIO.ReadTextAsync(userFile);
                _fences = JsonConvert.DeserializeObject<List<Fence>>(fenceStream);
            });
        }

        // Handles background task cancellation.
        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            // Indicate that the background task is canceled.
            Toast.ShowToast("Geofence Background Task has been canceled because of " + reason.ToString());
        }
    }
}
