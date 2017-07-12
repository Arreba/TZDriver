using System;
using Windows.UI.Xaml.Data;

namespace TZDriver.Models.Tools.Converters
{
    public class TimeInListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return String.Format("{0:00:00}");

            DateTime dateTime = (DateTime)value;
            return string.Format("{0:hh:mm}", dateTime);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
