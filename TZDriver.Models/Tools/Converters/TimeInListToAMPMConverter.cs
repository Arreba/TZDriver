using System;
using Windows.UI.Xaml.Data;

namespace TZDriver.Models.Tools.Converters
{
    public class TimeInListToAMPMConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return String.Format("{0: tt}");

            DateTime dateTime = (DateTime)value;
            return string.Format("{0: tt}", dateTime).ToLower();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
