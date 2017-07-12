using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace TZDriver.Models.Tools.Converters
{
    /// <summary>
    /// Convert null value to visibilty
    /// </summary>
    public class NullToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Convert from source to UI, if parameter is "inv" it will use equivalence for null = Visible, default is null = collapsed
        /// </summary>
        /// <param name="value">The nullable value</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language) => value == null ? (parameter as string == "inv" ? Visibility.Visible : Visibility.Collapsed) : (parameter as string == "inv" ? Visibility.Collapsed : Visibility.Visible);

        /// <summary>
        /// Convert from UI to source
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
