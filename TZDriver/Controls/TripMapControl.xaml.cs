using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Template10.Controls;
using TZDriver.Models.Tools.Utilities;
using TZDriver.ViewModels;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;

namespace TZDriver.Controls
{
    public sealed partial class TripMapControl : UserControl, INotifyPropertyChanged
    {
        private IList<MapPolyline> lines;
        private IList<BasicGeoposition> RideLocations;
        public ContentDialogResult DiagResult { get; set; }
        ModalDialog modal;
        ClosedTripViewModel closedTripView = ClosedTripViewModel.Current;

        public TripMapControl()
        {
            this.InitializeComponent();
            this.DataContext = closedTripView;
            modal = Window.Current.Content as ModalDialog;
            lines = new List<MapPolyline>();
            RideLocations = new List<BasicGeoposition>();
            this.Loaded += TripMapControl_Loaded;
            this.Unloaded += TripMapControl_Unloaded; ;
        }

        private TripPayMode _selectedPayMode;
        public TripPayMode SelectedPayMode
        {
            get { return _selectedPayMode; }
            set
            {
                this.SetProperty(ref this._selectedPayMode, value);
            }
        }

        private TripPayStatus _selectedPayStatus;
        public TripPayStatus SelectedPayStatus
        {
            get { return _selectedPayStatus; }
            set
            {
                this.SetProperty(ref this._selectedPayStatus, value);
            }
        }

        private double _tripDistance = 0.0;
        public double TripDistance
        {
            get { return _tripDistance; }
            set { SetProperty(ref _tripDistance, value); }
        }

        private async void TripMapControl_Loaded(object sender, RoutedEventArgs e)
        {
            ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
            CompletedTripMap.MapServiceToken = Constants.BingMapsAPIKey;
            Uri appUri = new Uri(@"ms-appx:///Styles/MapStyle.json");
            var file = await StorageFile.GetFileFromApplicationUriAsync(appUri);
            string jsonText = await FileIO.ReadTextAsync(file);
            CompletedTripMap.StyleSheet = MapStyleSheet.ParseFromJson(jsonText);

            await LoadOpenTaskListView();

            if (RideLocations.Count != 0)
                await DrawPath(RideLocations);
        }

        private void TripMapControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ApplicationView.GetForCurrentView().ExitFullScreenMode();
        }

        private async Task LoadOpenTaskListView()
        {
            var TripDetails = closedTripView.SelectedCompletedTripData;
            SelectedPayMode = TripDetails.Mode;
            SelectedPayStatus = TripDetails.Pay;
            var ridePoints = TripDetails.Points;

            foreach (var point in ridePoints)
            {
                RideLocations.Add(new BasicGeoposition { Latitude = point.TripLatitude, Longitude = point.TripLongitude });
            }
        }

        private async Task DrawPath(IList<BasicGeoposition> locations)
        {
            CompletedTripMap.MapServiceToken = Constants.BingMapsAPIKey;
            MapPolyline mapPolyLine = new MapPolyline();
            mapPolyLine.Path = new Geopath(locations);
            mapPolyLine.ZIndex = 1;
            mapPolyLine.Visible = true;
            mapPolyLine.StrokeColor = new Color { A = 255, R = 94, G = 185, B = 255 };
            mapPolyLine.StrokeThickness = 5;
            lines.Add(mapPolyLine);
            // Starting off with the first point as center
            if (locations.Count > 0)
                CompletedTripMap.Center = new Geopoint(locations.First());

            foreach (var polyline in lines)
            {
                CompletedTripMap.MapElements.Add(polyline);
            }
            //sumTripMap.MapElements.Add(mapPolyLine);

            // Draw Start Icon
            MapIcon mapStartIcon = new MapIcon
            {
                Location = new Geopoint(locations.First()),
                NormalizedAnchorPoint = new Point(0.5, 1.0),
                Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Icons/Start05.png")),
                ZIndex = 1,
            };

            CompletedTripMap.MapElements.Add(mapStartIcon);

            //Draw End Icon
            MapIcon mapEndIcon = new MapIcon
            {
                Location = new Geopoint(locations.Last()),
                NormalizedAnchorPoint = new Point(0.5, 1.0),
                Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Icons/End05.png")),
                ZIndex = 1,
            };
            CompletedTripMap.MapElements.Add(mapEndIcon);

            var boundingBox = GeoboundingBox.TryCompute(locations);
            // Delay to allow the Map to finish loading - on faster machines, animation fails
            await Task.Delay(500);
            await CompletedTripMap.TrySetViewBoundsAsync(boundingBox, new Thickness(0, 10, 0, 10), MapAnimationKind.Default);

            // Draw the Car
            //DrawCarOnMap(carLocations.First());
            await Task.CompletedTask;
        }

        private void DrawCarOnMap(BasicGeoposition basicGeoposition)
        {
            MapIcon carIcon = null;
            // Find if there is a MapIcon with title Car
            if (CompletedTripMap.MapElements != null)
            {
                var mapIcons = CompletedTripMap.MapElements.OfType<MapIcon>().ToList();
                foreach (var item in mapIcons)
                {
                    if (item.Title == "Car")
                        carIcon = item;
                }
            }

            if (carIcon == null)
            {
                // Car Icon not found creating it at the position and adding to maps
                carIcon = new MapIcon
                {
                    Location = new Geopoint(basicGeoposition),
                    NormalizedAnchorPoint = new Point(0.5, 0.5),
                    Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Icons/car05.png")),
                    ZIndex = 2,
                    Title = "Car"
                };
                CompletedTripMap.MapElements.Add(carIcon);
            }
            else
            {
                carIcon.Location = new Geopoint(basicGeoposition);
            }
            CompletedTripMap.Center = carIcon.Location;
        }

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register(nameof(IsBusy), typeof(bool), typeof(TripSummary), new PropertyMetadata(false));

        // hide and show busy dialog
        public static async void SetBusy(bool busy)
        {
            await DispatcherHelper.RunAsync(() =>
            {
                TripMapControl mapView = new TripMapControl();
                var view = mapView.modal.ModalContent as TripMapControl;
                if (view == null)
                    mapView.modal.ModalContent = view = new TripMapControl();
                mapView.modal.IsModal = view.IsBusy = busy;
            });
        }



        public event PropertyChangedEventHandler PropertyChanged;

        private bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, value)) return false;
            storage = value;
            this.OnPropertyChaned(propertyName);
            return true;
        }

        private void OnPropertyChaned(string propertyName)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.modal.IsModal = IsBusy = false;
        }
    }
}
