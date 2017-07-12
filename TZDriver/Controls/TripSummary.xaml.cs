using GalaSoft.MvvmLight.Threading;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Controls;
using TZDriver.Models.DataModels;
using TZDriver.Models.Services.IServices;
using TZDriver.Models.Tools.Utilities;
using TZDriver.ViewModels;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;

namespace TZDriver.Controls
{
    public sealed partial class TripSummary : UserControl
    {
        TripViewModel tripView = TripViewModel.Current;
        ILocalStoreService localStoreService;

        public TripSummary()
        {
            this.InitializeComponent();
            this.DataContext = tripView;
            this.localStoreService = ServiceLocator.Current.GetInstance<ILocalStoreService>();
            this.Loaded += TripSummary_Loaded;
        }

        private async void TripSummary_Loaded(object sender, RoutedEventArgs e)
        {
            sumTripMap.MapServiceToken = Constants.BingMapsAPIKey;
            Uri appUri = new Uri(@"ms-appx:///Styles/MapStyle.json");
            var file = await StorageFile.GetFileFromApplicationUriAsync(appUri);
            string jsonText = await FileIO.ReadTextAsync(file);
            sumTripMap.StyleSheet = MapStyleSheet.ParseFromJson(jsonText);
            var tripLocations = await localStoreService.LoadLocalData<List<Location>>(Constants.LocationFileIdentifier);

            if (tripLocations != null)
            {
                var positions = (from locations in tripLocations
                                 where locations.TimeStamp >= tripView.CurrentTripDataRepo.StartTime && locations.TimeStamp <= tripView.CurrentTripDataRepo.EndTime
                                 select new BasicGeoposition { Latitude = locations.Latitude, Longitude = locations.Longitude }).ToList();

                await DrawPath(positions);
            }
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
                var modal = Window.Current.Content as ModalDialog;
                var view = modal.ModalContent as TripSummary;
                if (view == null)
                    modal.ModalContent = view = new TripSummary();
                modal.IsModal = view.IsBusy = busy;
            });
        }

        private async Task DrawPath(IList<BasicGeoposition> TripLocations)
        {
            MapPolyline mapPolyLine = new MapPolyline();
            mapPolyLine.Path = new Geopath(TripLocations);
            mapPolyLine.ZIndex = 1;
            mapPolyLine.Visible = true;
            mapPolyLine.StrokeColor = new Color { A = 255, R = 94, G = 185, B = 255 };
            mapPolyLine.StrokeThickness = 5;
            // Starting off with the first point as center
            if (TripLocations.Count > 0)
                sumTripMap.Center = new Geopoint(TripLocations.First());

            sumTripMap.MapElements.Add(mapPolyLine);

            // Draw Start Icon
            MapIcon mapStartIcon = new MapIcon
            {
                Location = new Geopoint(TripLocations.First()),
                NormalizedAnchorPoint = new Point(0.5, 1.0),
                Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Icons/Start05.png")),
                ZIndex = 1,
            };

            sumTripMap.MapElements.Add(mapStartIcon);

            //Draw End Icon
            MapIcon mapEndIcon = new MapIcon
            {
                Location = new Geopoint(TripLocations.Last()),
                NormalizedAnchorPoint = new Point(0.5, 1.0),
                Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Icons/End05.png")),
                ZIndex = 1,
            };
            sumTripMap.MapElements.Add(mapEndIcon);

            var boundingBox = GeoboundingBox.TryCompute(TripLocations);
            // Delay to allow the Map to finish loading - on faster machines, animation fails
            await Task.Delay(500);
            await sumTripMap.TrySetViewBoundsAsync(boundingBox, new Thickness(0, 10, 0, 10), MapAnimationKind.Default);

            // Draw the Car
            //DrawCarOnMap(carLocations.First());
            await Task.CompletedTask;
        }

        private void DrawCarOnMap(BasicGeoposition basicGeoposition)
        {
            MapIcon carIcon = null;
            // Find if there is a MapIcon with title Car
            if (sumTripMap.MapElements != null)
            {
                var mapIcons = sumTripMap.MapElements.OfType<MapIcon>().ToList();
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
                sumTripMap.MapElements.Add(carIcon);
            }
            else
            {
                carIcon.Location = new Geopoint(basicGeoposition);
            }
            sumTripMap.Center = carIcon.Location;
        }
    }
}
