using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Services.NavigationService;
using TZDriver.DataStores.IDataStores;
using TZDriver.Models.Services.IServices;
using Windows.UI.Xaml.Navigation;

namespace TZDriver.ViewModels
{
    public class BaseViewModel : ViewModelBase, INavigable
    {
        static ITripDataManager _tripDataManager;
        public static ITripDataManager tripDataManager =>
            _tripDataManager ??
            (_tripDataManager = ServiceLocator.Current.GetInstance<ITripDataManager>());

        static ILocalStoreService _localStoreService;
        public static ILocalStoreService localStoreService =>
            _localStoreService ??
            (_localStoreService = ServiceLocator.Current.GetInstance<ILocalStoreService>());

        private bool _isBusy = false;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { Set<bool>(() => IsBusy, ref _isBusy, value); }
        }

        private bool _canLoadMore = false;
        public bool CanLoadMore
        {
            get { return _canLoadMore; }
            set { Set<bool>(() => CanLoadMore, ref _canLoadMore, value); }
        }

        private bool _isRecording = false;
        public bool IsRecording
        {
            get { return _isRecording; }
            private set { Set<bool>(() => IsRecording, ref _isRecording, value); }
        }

        public virtual Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            return Task.CompletedTask;
        }

        [JsonIgnore]
        public INavigationService NavigationService { get; set; }

        [JsonIgnore]
        public IDispatcherWrapper Dispatcher { get; set; }

        [JsonIgnore]
        public IStateItems SessionState { get; set; }
    }
}
