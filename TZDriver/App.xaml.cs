using GalaSoft.MvvmLight.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Threading.Tasks;
using TZDriver.DataStores;
using TZDriver.DataStores.IDataStores;
using TZDriver.Models.DataModels;
using TZDriver.Models.Services.IServices;
using TZDriver.Models.Tools.Helpers;
using TZDriver.Models.Tools.Utilities;
using TZDriver.Styles;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Data;

namespace TZDriver
{
    [Bindable]
    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();
            RequestedTheme = Windows.UI.Xaml.ApplicationTheme.Light;

            using (var db = new DataContext())
            {
                db.Database.Migrate();
            }
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            DispatcherHelper.Initialize();
            Theme.ApplyToContainer();
            CheckApplication();
            await NavigationService.NavigateAsync(typeof(Views.ActiveTripView));
        }

        private void CheckApplication()
        {
            ServiceLocator.Current.GetInstance<ITripDataManager>().CleanUpTrips();
            ServiceLocator.Current.GetInstance<IBackgroundService>().CheckApplicationtriggerStatus();
        }

        public override Task OnSuspendingAsync(object s, SuspendingEventArgs e, bool prelaunchActivated)
        {
            return base.OnSuspendingAsync(s, e, prelaunchActivated);
        }

        public override void OnResuming(object s, object e, AppExecutionState previousExecutionState)
        {
            base.OnResuming(s, e, previousExecutionState);
        }
    }
}
