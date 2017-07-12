using System;
using Windows.UI.Xaml.Data;

namespace TZDriver.Models.Tools.Converters
{
    public class NameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string[] splitName = Array.Empty<string>();

            if (value != null)
            {
                splitName = ((string)value).Split(' ');
                return splitName[0].ToUpper();
            }
            return splitName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
