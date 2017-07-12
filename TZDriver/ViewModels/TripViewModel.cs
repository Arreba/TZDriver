using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using TZDriver.Controls;
using TZDriver.Extensions;
using TZDriver.Models.DataModels;
using TZDriver.Models.Tools.Helpers;
using TZDriver.Models.Tools.Utilities;
using TZDriver.Services.IServices;
using TZDriver.Utilities;
using Windows.ApplicationModel.Calls;
using Windows.Devices.Geolocation;
using Windows.System.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace TZDriver.ViewModels
{
    public class TripViewModel : BaseViewModel
    {
        private IntervalTimer smartTimer;
        private TimeOut timeOut;
        private DisplayRequest dispRequest;
        private DispatcherTimer upTimer;
        public static TripViewModel Current;
        IGeoLocationService geoLocationService;

        private bool _isReachflyoutOpen;
        public bool IsReachflyoutOpen
        {
            get { return this._isReachflyoutOpen; }
            set { this.Set(ref _isReachflyoutOpen, value); }
        }

        private TripData _selectedTripData;
        public TripData SelectedTripData
        {
            get { return _selectedTripData; }
            set { Set(ref _selectedTripData, value); }
        }

        private TripDataRepository _currentTripDataRepo;
        public TripDataRepository CurrentTripDataRepo
        {
            get { return _currentTripDataRepo; }
            set { Set(ref _currentTripDataRepo, value); }
        }

        public TripViewModel(IGeoLocationService _geoLocationService)
        {
            Current = this;
            this.geoLocationService = _geoLocationService;
            this._currentTripDataRepo = new TripDataRepository();
            if (dispRequest == null)
            {
                dispRequest = new DisplayRequest();
                dispRequest.RequestActive();
            }

            smartTimer = new IntervalTimer(1);
            upTimer = new DispatcherTimer();
            upTimer.Interval = TimeSpan.FromSeconds(1);
            upTimer.Tick += new EventHandler<object>(UpTimerTick);
        }

        private DelegateCommand _StartCommand;
        public DelegateCommand StartCommand
           => _StartCommand ?? (_StartCommand = new DelegateCommand(ExecuteStartCommand, StartCommandCanExecute));
        bool StartCommandCanExecute() => CurrentTripDataRepo.IsTripStarted != true;

        public async void ExecuteStartCommand()
        {
            BeginDialog dialog = new BeginDialog();
            await dialog.ShowAsync();

            if (dialog.DiagResult == ContentDialogResult.Primary)
            {
                CurrentTripDataRepo.DriveValue = EnumHelper.DeParse(DriveStateStatus.TripStarted);
                smartTimer.StopTimer();
                upTimer.Start();

                if (SelectedTripData.IsTripStarted != true)
                {
                    SelectedTripData.TripStartTime = CurrentTripDataRepo.StartTime = DateTime.Now;
                    SelectedTripData.IsTripStarted = CurrentTripDataRepo.IsTripStarted = true;
                    tripDataManager.SaveTrip(SelectedTripData);
                }

                StartCommand.RaiseCanExecuteChanged();
                StopCommand.RaiseCanExecuteChanged();
            }
        }

        private DelegateCommand _StopCommand;
        public DelegateCommand StopCommand
           => _StopCommand ?? (_StopCommand = new DelegateCommand(ExecuteStopCommand, StopCommandCanExecute));
        bool StopCommandCanExecute() => CurrentTripDataRepo.IsTripStarted == true;

        public async void ExecuteStopCommand()
        {
            EndDialog dialog = new EndDialog();
            await dialog.ShowAsync();

            if (dialog.DiagResult == ContentDialogResult.Primary)
            {
                CurrentTripDataRepo.DriveValue = EnumHelper.DeParse(DriveStateStatus.TripCompleted);

                if (SelectedTripData.IsTripComplete != true)
                {
                    CurrentTripDataRepo.EndTime = DateTime.Now;
                    upTimer.Stop();
                    TripSummary.SetBusy(true);

                    SelectedTripData.TripEndTime = CurrentTripDataRepo.EndTime;
                    SelectedTripData.IsTripComplete = true;
                    SelectedTripData.Pay = CurrentTripDataRepo.SelectedPayStatus = dialog.SelectedPayStatus;
                    SelectedTripData.Mode = CurrentTripDataRepo.SelectedPayMode = dialog.SelectedPayMode;
                    tripDataManager.SaveTrip(SelectedTripData);
                }

                geoLocationService.PositionChanged -= GeoLocationService_PositionChanged;
                geoLocationService.StopListening();
                StartCommand.RaiseCanExecuteChanged();
                StopCommand.RaiseCanExecuteChanged();
                GotoHomeTaskCommand.RaiseCanExecuteChanged();
            }
        }

        void UpTimerTick(object sender, object e)
        {
            var duration = DateTime.Now.Subtract(CurrentTripDataRepo.StartTime);
            CurrentTripDataRepo.CurrentDuration = Math.Ceiling(duration.TotalMinutes);
            CurrentTripDataRepo.TripFare = Math.Ceiling(duration.TotalMinutes) * 25;
            CurrentTripDataRepo.TripHours = duration.Hours;
            CurrentTripDataRepo.TripMinutes = duration.Minutes;
            CurrentTripDataRepo.TripSeconds = duration.Seconds;
        }

        private void GeoLocationService_PositionChanged(object sender, Geoposition geoposition)
        {
            CurrentTripDataRepo.CurrentDistance += CurrentTripDataRepo.CurrentUserPosition.CalculatePositionDistance(geoposition);
            CurrentTripDataRepo.CurrentUserPosition = geoposition;
        }

        private DelegateCommand _ReachCommand;
        public DelegateCommand ReachCommand
           => _ReachCommand ?? (_ReachCommand = new DelegateCommand(ExecuteReachCommand));
        public void ExecuteReachCommand()
        {
            IsReachflyoutOpen = true;
            timeOut = new TimeOut(20, CloseFlyout);
        }

        DelegateCommand _ReachOfficeMTNCommand;
        public DelegateCommand ReachOfficeMTNCommand
           => _ReachOfficeMTNCommand ?? (_ReachOfficeMTNCommand = new DelegateCommand(() =>
           {
               PhoneCallManager.ShowPhoneCallUI(phoneNumber: Constants.MtnOfficePhone, displayName: Constants.OfficeDisplayName.ToUpper());
               timeOut.Cancel();
               IsReachflyoutOpen = false;
           }));

        DelegateCommand _ReachOfficeEtisaCommand;
        public DelegateCommand ReachOfficeEtisaCommand
           => _ReachOfficeEtisaCommand ?? (_ReachOfficeEtisaCommand = new DelegateCommand(() =>
           {
               PhoneCallManager.ShowPhoneCallUI(phoneNumber: Constants.EtisalatOfficePhone, displayName: Constants.OfficeDisplayName.ToUpper());
               timeOut.Cancel();
               IsReachflyoutOpen = false;
           }));

        DelegateCommand _ReachPickupClientCommand;
        public DelegateCommand ReachPickupClientCommand
           => _ReachPickupClientCommand ?? (_ReachPickupClientCommand = new DelegateCommand(() =>
           {
               PhoneCallManager.ShowPhoneCallUI(phoneNumber: SelectedTripData.CustomerPhone1.ToString(), displayName: SelectedTripData.CustomerName.ToUpper());
               timeOut.Cancel();
               IsReachflyoutOpen = false;
           }));

        DelegateCommand _GotoHomeTaskCommand;
        public DelegateCommand GotoHomeTaskCommand
           => _GotoHomeTaskCommand ?? (_GotoHomeTaskCommand = new DelegateCommand(() =>
           {
               TripSummary.SetBusy(false);
               NavigationService.Navigate(typeof(Views.ActiveTripView), new DrillInNavigationTransitionInfo());
           }, () => IsBusy == false));

        private async void CloseFlyout()
        {
            await DispatcherHelper.RunAsync(() =>
            {
                IsReachflyoutOpen = false;
            });
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (suspensionState.Any())
            {
                // use cache value(s)
                if (suspensionState.ContainsKey(nameof(SelectedTripData)))
                    SelectedTripData = suspensionState[nameof(SelectedTripData)] as TripData;
                // clear any cache
                suspensionState.Clear();
            }
            else
            {
                // use navigation parameter
                if (parameter.GetType().Equals(typeof(TripData)))
                    SelectedTripData = parameter as TripData;
            }

            ApplicationView.GetForCurrentView().TryEnterFullScreenMode();

            if (SelectedTripData.IsTripStarted == true)
            {
                CurrentTripDataRepo.DriveValue = EnumHelper.DeParse(DriveStateStatus.TripStarted);
                CurrentTripDataRepo.StartTime = SelectedTripData.TripStartTime;
                CurrentTripDataRepo.CurrentUser = SelectedTripData.CustomerName;
                smartTimer.StopTimer();
                upTimer.Start();
                CurrentTripDataRepo.IsTripStarted = true;
                StartCommand.RaiseCanExecuteChanged();
                StopCommand.RaiseCanExecuteChanged();
            }
            else
            {
                CurrentTripDataRepo.CurrentUser = SelectedTripData.CustomerName;
                CurrentTripDataRepo.CurrentPickupMunites = Math.Round((SelectedTripData.TripStartTime - DateTime.Now).TotalMinutes) <= 0 ?
                    0.0 : Math.Round((SelectedTripData.TripStartTime - DateTime.Now).TotalMinutes);

                smartTimer.StartTimer(() =>
                {
                    if (CurrentTripDataRepo.CurrentPickupMunites != 0)
                    {
                        CurrentTripDataRepo.CurrentPickupMunites--;
                    }
                    else smartTimer.StopTimer();
                });
            }

            geoLocationService.DesiredAccuracyInMeters = 10;
            geoLocationService.MovementThreshold = 5;
            var initializationSuccessful = await geoLocationService.InitializeAsync();
            if (initializationSuccessful)
            {
                await geoLocationService.StartListeningAsync();
                CurrentTripDataRepo.CurrentUserPosition = geoLocationService.CurrentPosition;
                if (geoLocationService.IsGeolocationAvailable)
                {
                    CurrentTripDataRepo.IsPositonAvailable = true;
                }
            }

            if (geoLocationService.IsListening)
                geoLocationService.PositionChanged += GeoLocationService_PositionChanged;

            NavigationService.FrameFacade.BackRequested += FrameFacade_BackRequested;
            await Task.CompletedTask;
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending)
        {
            if (suspending)
            {
                // persist into cache
            }

            ApplicationView.GetForCurrentView().ExitFullScreenMode();
            await base.OnNavigatedFromAsync(suspensionState, suspending);

            await Task.CompletedTask;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            NavigationService.FrameFacade.BackRequested -= FrameFacade_BackRequested;
            args.Cancel = false;
            await Task.CompletedTask;
        }

        public void FrameFacade_BackRequested(object sender, HandledEventArgs e)
        {
            e.Handled = true;
            if (!CurrentTripDataRepo.DriveValue.Equals(DriveStateStatus.TripStarted.ToString()))
            {
                if (this.NavigationService.FrameFacade.CanGoBack)
                {
                    this.NavigationService.FrameFacade.GoBack();
                }
            }
        }
    }
}
