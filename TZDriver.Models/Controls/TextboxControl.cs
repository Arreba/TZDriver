using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TZDriver.Models.Controls
{
    public sealed class TextboxControl : TextBox
    {
        private const string StateCollapse = "Collapse";
        private const string StateExpand = "Expand";
        private bool hasFocus;

        public TextboxControl()
        {
            DefaultStyleKey = typeof(TextboxControl);
            VisualStateManager.GoToState(this, StateCollapse, false);
            GotFocus += TextboxControl_GotFocus;
            LostFocus += TextboxControl_LostFocus;
            Loaded += TextboxControl_Loaded;
            TextChanged += TextboxControl_TextChanged;
        }

        private void TextboxControl_TextChanged(object sender, TextChangedEventArgs e)
        {
            // In case viewmodels clear the string
            if (string.IsNullOrWhiteSpace(Text) && hasFocus == false)
            {
                VisualStateManager.GoToState(this, StateCollapse, true);
            }
        }

        private void TextboxControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Text))
            {
                VisualStateManager.GoToState(this, StateExpand, true);
            }
        }

        private void TextboxControl_LostFocus(object sender, RoutedEventArgs e)
        {
            hasFocus = false;

            if (string.IsNullOrWhiteSpace(Text) == true)
            {
                VisualStateManager.GoToState(this, StateCollapse, true);
            }
        }

        private void TextboxControl_GotFocus(object sender, RoutedEventArgs e)
        {
            hasFocus = true;

            if (string.IsNullOrWhiteSpace(Text) == true)
            {
                VisualStateManager.GoToState(this, StateExpand, true);
            }
        }

        public static readonly DependencyProperty BackgroundOverlayProperty = DependencyProperty.Register(
        "BackgroundOverlay", typeof(SolidColorBrush), typeof(TextboxControl),
        new PropertyMetadata(new SolidColorBrush(Windows.UI.Color.FromArgb(255, 20, 20, 90))));

        public SolidColorBrush BackgroundOverlay
        {
            get { return (SolidColorBrush)GetValue(BackgroundOverlayProperty); }
            set { SetValue(BackgroundOverlayProperty, value); }
        }

        public static readonly DependencyProperty ForegroundOverlayProperty = DependencyProperty.Register(
        "ForegroundOverlay", typeof(SolidColorBrush), typeof(TextboxControl),
        new PropertyMetadata(new SolidColorBrush(Windows.UI.Color.FromArgb(255, 20, 20, 90))));

        public SolidColorBrush ForegroundOverlay
        {
            get { return (SolidColorBrush)GetValue(ForegroundOverlayProperty); }
            set { SetValue(ForegroundOverlayProperty, value); }
        }
    }
}
