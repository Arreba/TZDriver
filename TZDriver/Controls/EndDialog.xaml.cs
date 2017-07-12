using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using TZDriver.Models.Tools.Helpers;
using TZDriver.Models.Tools.Utilities;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TZDriver.Controls
{
    public sealed partial class EndDialog : ContentDialog, INotifyPropertyChanged
    {
        private bool _isLoaded;
        public ContentDialogResult DiagResult { get; set; }

        public EndDialog()
        {
            this.InitializeComponent();
            this.Loaded += EndDialogLoaded;
            PayStatusList = EnumHelper.EnumToList<TripPayStatus>();
            PayModeList = EnumHelper.EnumToList<TripPayMode>();
            SelectedPayStatus = PayStatusList.FirstOrDefault();
            SelectedPayMode = PayModeList.FirstOrDefault();
            DefaultIndex = 0;
            this.Unloaded += (s, e) => _isLoaded = false;
            myCountdownControl.CountdownComplete += MyCountdownControl_CountdownComplete;
        }

        private async void EndDialogLoaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = true;

            while (_isLoaded)
            {
                myCountdownControl.Visibility = Visibility.Visible;
                await myCountdownControl.StartCountdownAsync(20);
            }
        }

        private TripPayMode _selectedPayMode;
        public TripPayMode SelectedPayMode
        {
            get { return _selectedPayMode; }
            set
            {
                this.SetProperty(ref this._selectedPayMode, value);
            }
        }

        private List<TripPayMode> _payModeList = new List<TripPayMode>();
        public List<TripPayMode> PayModeList
        {
            get { return _payModeList; }
            set
            {
                this.SetProperty(ref this._payModeList, value);
            }
        }

        private TripPayStatus _selectedPayStatus;
        public TripPayStatus SelectedPayStatus
        {
            get { return _selectedPayStatus; }
            set
            {
                this.SetProperty(ref this._selectedPayStatus, value);
            }
        }

        private List<TripPayStatus> _payStatusList = new List<TripPayStatus>();
        public List<TripPayStatus> PayStatusList
        {
            get { return _payStatusList; }
            set
            {
                this.SetProperty(ref this._payStatusList, value);
            }
        }

        private int _defaultIndex;
        public int DefaultIndex
        {
            get { return _defaultIndex; }
            set
            {
                _defaultIndex = value;
            }
        }

        private void MyCountdownControl_CountdownComplete(object sender, RoutedEventArgs e)
        {
            myCountdownControl.Visibility = Visibility.Collapsed;
            DiagResult = ContentDialogResult.Secondary;
            this.Hide();
        }

        private void EndButton_Click(object sender, RoutedEventArgs e)
        {
            DiagResult = ContentDialogResult.Primary;
            this.Hide();
        }

        private void cancelOperation_Click(object sender, RoutedEventArgs e)
        {
            DiagResult = ContentDialogResult.Secondary;
            this.Hide();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, value)) return false;
            storage = value;
            this.OnPropertyChaned(propertyName);
            return true;
        }

        private void OnPropertyChaned(string propertyName)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
