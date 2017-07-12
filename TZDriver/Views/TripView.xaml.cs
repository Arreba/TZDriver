using GalaSoft.MvvmLight.Messaging;
using System;
using TZDriver.Models.Tools.Utilities;
using TZDriver.ViewModels;
using Windows.Devices.Geolocation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;

namespace TZDriver.Views
{
    public sealed partial class TripView : Page
    {
        public TripViewModel vm
        {
            get
            {
                return (TripViewModel)DataContext;
            }
        }

        public TripView()
        {
            this.InitializeComponent();
            this.InitializeMapStyle();
            this.Unloaded += TripView_Unloaded;
        }

        private async void InitializeMapStyle()
        {
            drivewMap.MapServiceToken = Constants.BingMapsAPIKey;
            drivewMap.MapProjection = MapProjection.Globe;
            drivewMap.Style = MapStyle.Road;

            Uri appUri = new Uri(@"ms-appx:///Styles/MapStyle.json");
            var file = await StorageFile.GetFileFromApplicationUriAsync(appUri);
            string jsonText = await FileIO.ReadTextAsync(file);
            drivewMap.StyleSheet = MapStyleSheet.ParseFromJson(jsonText);
            Messenger.Default.Register<Geoposition>(this, Constants.BingMapsAPIKey, SetMapView);
        }

        private async void SetMapView(Geoposition geoposition)
        {
            await drivewMap.TrySetViewAsync(geoposition.Coordinate.Point, 18D, geoposition.Coordinate.Heading, 70, MapAnimationKind.Bow);
        }

        private void TripView_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister<Geoposition>(this);
            ViewModelLocator.ResetViewModel<TripViewModel>();
        }
    }
}
