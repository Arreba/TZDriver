using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TZDriver.Controls
{
    public sealed partial class Navigation : UserControl
    {
        public Navigation()
        {
            this.InitializeComponent();
        }

        public double LocationAccuraccy
        {
            get { return (double)GetValue(LocationAccuraccyProperty); }
            set { SetValue(LocationAccuraccyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LocationAccuraccy.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LocationAccuraccyProperty =
            DependencyProperty.Register("LocationAccuraccy", typeof(double), typeof(Navigation), new PropertyMetadata(90, new PropertyChangedCallback(OnLocationAccuraccyChanged)));

        private static void OnLocationAccuraccyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Navigation nav = d as Navigation;
            var accuracyValue = nav.LocationAccuraccy * 10;
            var gridSize = accuracyValue < 150 ? (accuracyValue > 85 ? accuracyValue : 85) : 150;
            nav.accuracyGrid.MinHeight = gridSize;
            nav.accuracyGrid.MinWidth = gridSize;
            nav.accuracyGrid.CornerRadius = new CornerRadius((gridSize / 2));
        }
    }
}
