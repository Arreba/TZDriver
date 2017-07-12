using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using TZDriver.Controls;
using TZDriver.Models.DataModels;
using TZDriver.Models.Tools.Extensions;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace TZDriver.ViewModels
{
    public class ClosedTripViewModel : BaseViewModel
    {
        public static ClosedTripViewModel Current;

        public ClosedTripViewModel()
        {
            Current = this;
            _completedTripCollection = new ObservableCollection<TripData>();
        }

        private ObservableCollection<TripData> _completedTripCollection;
        public ObservableCollection<TripData> CompletedTripCollection
        {
            get { return _completedTripCollection; }
            set
            {
                _completedTripCollection = value;
                RaisePropertyChanged(nameof(CompletedTripCollection));
            }
        }

        private TripData _selectedCompletedTripData;
        public TripData SelectedCompletedTripData
        {
            get { return _selectedCompletedTripData; }
            set
            {
                _selectedCompletedTripData = value;
                RaisePropertyChanged(nameof(SelectedCompletedTripData));
            }
        }

        private ICommand _selectedCompletedTripCommand;
        public ICommand SelectedCompletedTripCommand
            => _selectedCompletedTripCommand ?? (_selectedCompletedTripCommand = new DelegateCommand<ItemClickEventArgs>( args =>
            {
                SelectedCompletedTripData = args.ClickedItem as TripData;
                TripMapControl.SetBusy(true);
            }));

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (suspensionState.Any())
            {
            }
            CompletedTripCollection.Clear();
            await LoadOpenTaskListView();
            await Task.CompletedTask;
        }

        private async Task LoadOpenTaskListView()
        {
            var progressDialog = UserDialogs.Instance.Loading("Loading Completed Trips...", maskType: MaskType.Clear);

            var closedTrips = tripDataManager.GetClosedTrips();

            CompletedTripCollection.AddRange(closedTrips);

            progressDialog?.Dispose();

            if (CompletedTripCollection.Count == 0)
            {
                UserDialogs.Instance.Alert(
                    "Looks like you haven't driven any client recently" + "\n" +
                    "Keep an Open Mind", "Be Ready!");
            }
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending)
        {
            if (suspending)
            {
            }

            await Task.CompletedTask;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            await Task.CompletedTask;
        }

        DelegateCommand _GotoSettingsCommand;
        public DelegateCommand GotoSettingsCommand
          => _GotoSettingsCommand ?? (_GotoSettingsCommand = new DelegateCommand(() =>
          {
          }));
    }
}
