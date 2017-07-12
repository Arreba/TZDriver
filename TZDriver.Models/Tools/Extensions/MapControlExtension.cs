using System.Collections.Generic;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Maps;

namespace TZDriver.Models.Tools.Extensions
{
    public static class MapControlExtensions
    {
        public static void ClearMap<T>(this MapControl map, MapElement element = null) where T : MapElement
        {
            if (element == null)
            {
                List<MapElement> mapElements = map.MapElements.Where(item => item is T).ToList();
                foreach (var item in mapElements)
                    map.MapElements.Remove(item);
            }
            else
                map.MapElements.Remove(element);
        }

        public static GeoboundingBox GetViewArea(this MapControl map)
        {
            Geopoint p1, p2;
            map.GetLocationFromOffset(new Point(0, 0), out p1);
            map.GetLocationFromOffset(new Point(map.ActualWidth, map.ActualHeight), out p2);
            return new GeoboundingBox(p1.Position, p2.Position);
        }

        public static void SetViewArea(this MapControl map, Geopoint p1, Geopoint p2)
        {
            var b = GeoboundingBox.TryCompute(new[] { p1.Position, p2.Position });
            map.TrySetViewBoundsAsync(b, new Thickness(1.0), MapAnimationKind.Bow);
        }
    }
}
