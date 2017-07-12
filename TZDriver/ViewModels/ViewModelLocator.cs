using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using TZDriver.DataStores;
using TZDriver.DataStores.IDataStores;
using TZDriver.Models.Services;
using TZDriver.Models.Services.IServices;
using TZDriver.Services;
using TZDriver.Services.IServices;

namespace TZDriver.ViewModels
{
    public class ViewModelLocator
    {
        internal static void Register<T>(bool createImmediately = false) where T : class
        {
            SimpleIoc.Default.Register<T>(createImmediately);
        }

        internal static T Get<T>() where T : class
        {
            return SimpleIoc.Default.GetInstance<T>();
        }

        public static void ResetViewModel<T>(bool createImmediately = false) where T : class
        {
            if (SimpleIoc.Default.IsRegistered<T>())
            {
                SimpleIoc.Default.Unregister<T>();
            }

            SimpleIoc.Default.Register<T>(createImmediately);
        }

        public static void ResetViewModel<T>(T instance) where T : class
        {
            if (SimpleIoc.Default.IsRegistered<T>())
            {
                SimpleIoc.Default.Unregister<T>(instance);
            }

            SimpleIoc.Default.Register<T>();
        }

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (BaseViewModel.IsInDesignModeStatic)
            {
                //SimpleIoc.Default.Register<ITripStore, TripMockService>();
            }
            else
            {
                //SimpleIoc.Default.Register<ITripStore, TripStore>();
            }

            SimpleIoc.Default.Register<ISmsService, SmsService>();
            SimpleIoc.Default.Register<ILocalStoreService, LocalStoreService>();
            SimpleIoc.Default.Register<IGeoLocationService, GeoLocationService>();
            SimpleIoc.Default.Register<ITripDataManager, TripDataManager>();
            SimpleIoc.Default.Register<IBackgroundService, BackgroundService>();

            //Register<LoginViewModel>();
            Register<ActiveTripViewModel>();
            Register<TripViewModel>();
            Register<ClosedTripViewModel>();
            //Register<FuelViewModel>();
            Register<SettingsViewModel>();
        }

        //public LoginViewModel LoginPage => Get<LoginViewModel>();
        public ActiveTripViewModel ActivePage => Get<ActiveTripViewModel>();
        public TripViewModel TripPage => Get<TripViewModel>();
        public ClosedTripViewModel ClosedPage => Get<ClosedTripViewModel>();
        //public FuelViewModel FuelPage => Get<FuelViewModel>();
        public SettingsViewModel SettingsPage => Get<SettingsViewModel>();
    }
}
