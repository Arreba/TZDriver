using Windows.Foundation.Metadata;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace TZDriver.Styles
{
    public class Theme
    {
        // Call this in App OnLaunched.
        // Requires reference to Windows Mobile Extensions for the UWP.
        public static void ApplyToContainer()
        {
            // PC customization
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            {
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                if (titleBar != null)
                {
                    titleBar.ButtonBackgroundColor = (Color)Application.Current.Resources["AtransBrownDarkColor2"];
                    titleBar.ButtonForegroundColor = (Color)Application.Current.Resources["AtransBrownDarkColor2"];
                    titleBar.BackgroundColor = (Color)Application.Current.Resources["AtransBrownDarkColor2"];
                    titleBar.ForegroundColor = (Color)Application.Current.Resources["AtransBrownDarkColor2"];
                }
            }

            // Mobile customization
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = StatusBar.GetForCurrentView();
                statusBar.BackgroundOpacity = 1;
                statusBar.BackgroundColor = (Color)App.Current.Resources["AtransBrownDarkColor2"];
                statusBar.ForegroundColor = (Color)App.Current.Resources["AtransBrownDarkColor2"];
                DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait | DisplayOrientations.PortraitFlipped;
            }
        }
    }
}
