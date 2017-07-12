using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace TZDriver.Models.Tools.Converters
{
    public class TripStateColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return new SolidColorBrush(Colors.Transparent);
            bool TripState = (bool)value;
            var tripStateColor = TripState ?
                new SolidColorBrush((Color)Application.Current.Resources["AtransGreenDarkColor"]) :
                new SolidColorBrush((Color)Application.Current.Resources["AtransBrownLightColor5"]);
            return tripStateColor;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
