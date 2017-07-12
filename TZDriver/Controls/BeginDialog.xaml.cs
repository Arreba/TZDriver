using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TZDriver.Controls
{
    public sealed partial class BeginDialog : ContentDialog
    {
        private bool _isLoaded;
        public ContentDialogResult DiagResult { get; set; }

        public BeginDialog()
        {
            this.InitializeComponent();
            this.Loaded += BeginDialogLoaded;
            this.Unloaded += (s, e) => _isLoaded = false;
            myCountdownControl.CountdownComplete += MyCountdownControl_CountdownComplete;
        }

        private async void BeginDialogLoaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = true;

            while (_isLoaded)
            {
                myCountdownControl.Visibility = Visibility.Visible;
                await myCountdownControl.StartCountdownAsync(10);
            }
        }

        private void MyCountdownControl_CountdownComplete(object sender, RoutedEventArgs e)
        {
            myCountdownControl.Visibility = Visibility.Collapsed;
            DiagResult = ContentDialogResult.Secondary;
            this.Hide();
        }

        private void BeginButton_Click(object sender, RoutedEventArgs e)
        {
            DiagResult = ContentDialogResult.Primary;
            this.Hide();
        }
    }
}
