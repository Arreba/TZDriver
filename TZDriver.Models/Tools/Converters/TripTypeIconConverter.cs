using System;
using TZDriver.Models.Tools.Helpers;
using TZDriver.Models.Tools.Utilities;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;

namespace TZDriver.Models.Tools.Converters
{
    public class TripTypeIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var path = new Glyphs().UnicodeString;
            if (value == null)
                return path;

            var tripType = EnumHelper.Parse<ServiceType>(value.ToString(), true);

            switch(tripType)
            {
                case ServiceType.Hours:
                    path = "\uE81C";
                    break;

                case ServiceType.FullDay:
                    path = "\uED39";
                    break;

                case ServiceType.Airport:
                    path = "\uE709";
                    break;

                case ServiceType.SchoolRun:
                    path = "\uE806";
                    break;

                case ServiceType.Travel:
                    path = "\uE787";
                    break;
            }

            return path;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
