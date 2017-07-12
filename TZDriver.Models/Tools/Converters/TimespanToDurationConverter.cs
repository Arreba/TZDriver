using System;
using Windows.UI.Xaml.Data;

namespace TZDriver.Models.Tools.Converters
{
    public class TimespanToDurationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return String.Format("{0:00:00}");

            TimeSpan durationValue = (TimeSpan)value;
            //return $"{durationValue:hh\\:mm}";
            return String.Format("{0:%h}:{0:mm} min", durationValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
