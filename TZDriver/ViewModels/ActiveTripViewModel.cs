using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows.Input;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using TZDriver.Controls;
using TZDriver.Models.DataModels;
using TZDriver.Models.Tools.Extensions;
using TZDriver.Views;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace TZDriver.ViewModels
{
    public class ActiveTripViewModel : BaseViewModel
    {
        Compositor _compositor;

        public ActiveTripViewModel()
        {
            _activeJobDataCollection = new ObservableCollection<TripData>();
        }



        private ObservableCollection<TripData> _activeJobDataCollection;
        public ObservableCollection<TripData> ActiveJobDataCollection
        {
            get { return _activeJobDataCollection; }
            set
            {
                _activeJobDataCollection = value;
                RaisePropertyChanged(nameof(ActiveJobDataCollection));
            }
        }

        private TripData _selectedJobData;
        public TripData SelectedJobData
        {
            get { return _selectedJobData; }
            set
            {
                _selectedJobData = value;
                RaisePropertyChanged(nameof(SelectedJobData));
            }
        }

        DelegateCommand _GotoDoneCommand;
        public DelegateCommand GotoDoneCommand
           => _GotoDoneCommand ?? (_GotoDoneCommand = new DelegateCommand(GotoDoneCommandExecute, GotoDoneCommandCanExecute));
        bool GotoDoneCommandCanExecute() => true;
        public void GotoDoneCommandExecute() =>
            NavigationService.Navigate(typeof(Views.ClosedTripView), new DrillInNavigationTransitionInfo());

        DelegateCommand _AddJobsCommand;
        public DelegateCommand AddJobsCommand
           => _AddJobsCommand ?? (_AddJobsCommand = new DelegateCommand(AddClientCommandExecute));

        private async void AddClientCommandExecute()
        {
            AddJobDialog dialog = new AddJobDialog();
            await dialog.ShowAsync();

            if (dialog.DiagResult == ContentDialogResult.Primary && dialog.NewTrip != null)
            {
                var progressDialog = UserDialogs.Instance.Loading("Adding New Trip...", maskType: MaskType.Clear);

                tripDataManager.SaveTrip(dialog.NewTrip);
                ActiveJobDataCollection.Clear();
                var activeTrips = tripDataManager.GetActiveTrips();
                ActiveJobDataCollection.AddRange(activeTrips);

                progressDialog?.Dispose();
            }
        }

        private ICommand _selectedActiveJobCommand;
        public ICommand SelectedActiveJobCommand
            => _selectedActiveJobCommand ?? (_selectedActiveJobCommand = new DelegateCommand<ItemClickEventArgs>(args =>
            {
                SelectedJobData = args.ClickedItem as TripData;
                NavigationService.Navigate(typeof(Views.TripView), SelectedJobData, new DrillInNavigationTransitionInfo());
            }));

        private ICommand _activeJobsListviewContainerContentChanging;
        public ICommand ActiveJobsListviewContainerContentChanging
            => _activeJobsListviewContainerContentChanging ?? (_activeJobsListviewContainerContentChanging = new DelegateCommand<ContainerContentChangingEventArgs>(args =>
            {
                var presenter = VisualTreeHelper.GetChild(args.ItemContainer, 0) as ListViewItemPresenter;
                var contentHost = VisualTreeHelper.GetChild(presenter, 0) as Grid;
                var shadowHost = VisualTreeHelper.GetChild(contentHost, 1) as Canvas;
                var content = VisualTreeHelper.GetChild(contentHost, 2) as Grid;

                var contentVisual = ElementCompositionPreview.GetElementVisual(content);
                _compositor = contentVisual.Compositor;

                // create a red sprite visual
                var listViewItemVisual = _compositor.CreateSpriteVisual();
                //listViewItemVisual.Brush = _compositor.CreateColorBrush(Colors.Red);
                listViewItemVisual.Size = contentVisual.Size;
                listViewItemVisual.CenterPoint = contentVisual.CenterPoint;

                // create a blue drop shadow
                var listViewItemShadow = _compositor.CreateDropShadow();
                listViewItemShadow.BlurRadius = 10;
                listViewItemShadow.Offset = new Vector3(0, 0, 0);
                listViewItemShadow.Color = (Color)App.Current.Resources["AtransBrownDarkColor2"];
                listViewItemVisual.Shadow = listViewItemShadow;

                ElementCompositionPreview.SetElementChildVisual(shadowHost, listViewItemVisual);

                var bindSizeAnimation = _compositor.CreateExpressionAnimation("hostVisual.Size");
                bindSizeAnimation.SetReferenceParameter("hostVisual", contentVisual);

                listViewItemVisual.StartAnimation("Size", bindSizeAnimation);
            }));

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

                ActiveJobDataCollection.Clear();
                var activeTrips = tripDataManager.GetActiveTrips();
                ActiveJobDataCollection.AddRange(activeTrips);

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
