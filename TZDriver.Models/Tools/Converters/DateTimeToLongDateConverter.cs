using System;
using TZDriver.Models.Tools.Helpers;
using Windows.UI.Xaml.Data;

namespace TZDriver.Models.Tools.Converters
{
    public class DateTimeToLongDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var dateTimeToParse = (DateTime)value;

            return string.Format(new DateTimeFormatHelper(), "{0}", dateTimeToParse);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
