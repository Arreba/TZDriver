using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TZDriver.Models.DataModels;
using TZDriver.Models.Services.IServices;
using TZDriver.Models.Tools.Utilities;
using Windows.ApplicationModel.Background;
using Windows.Storage;

namespace TZDriver.Models.Services
{
    public class BackgroundService : IBackgroundService
    {
        bool IsApplicationtriggerRunning = false;
        // A pointer to the ApplicationTrigger so we can signal it later
        ApplicationTrigger applicationtrigger = null;
        LocalStoreService localStore;

        public BackgroundService()
        {
            // Create a new trigger
            applicationtrigger = new ApplicationTrigger();
            localStore = new LocalStoreService();
        }

        public async Task<bool> CheckApplicationtriggerStatus()
        {
            var locations = await localStore.LoadLocalData<List<Location>>(Constants.LocationFileIdentifier);
            if (locations != null && locations.Count > 1)
            {
                var diff = DateTimeOffset.Now.Subtract(locations.OrderBy(t => t.TimeStamp).FirstOrDefault().TimeStamp);
                if (diff.TotalMinutes > 30)
                {
                    // Reset the completion status
                    var settings = ApplicationData.Current.LocalSettings;
                    settings.Values.Remove(Constants.ApplicationTriggerBackgroundTaskName);

                    //Signal the ApplicationTrigger
                    var result = await applicationtrigger.RequestAsync();
                    if (result.Equals(ApplicationTriggerResult.Allowed))
                    {
                        Toast.ShowToast("Your location is now being Monitored and " + result.ToString());
                        IsApplicationtriggerRunning = true;
                    }
                    else
                    {
                        Toast.ShowToast("We are having difficult Monitoring your location because it was " + result.ToString());
                        IsApplicationtriggerRunning = false;
                    }
                }
                else
                {
                    Toast.ShowToast("Your location monitoring is Running");
                    IsApplicationtriggerRunning = true;
                }
            }
            else
            {
                Toast.ShowToast("There are no stored locations yet");
            }

            return IsApplicationtriggerRunning;
        }
    }
}
