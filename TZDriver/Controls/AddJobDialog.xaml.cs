using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using TZDriver.Models.DataModels;
using TZDriver.Models.Tools.Helpers;
using TZDriver.Models.Tools.Utilities;
using Windows.ApplicationModel.Contacts;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace TZDriver.Controls
{
    public sealed partial class AddJobDialog : ContentDialog, INotifyPropertyChanged
    {
        public ContentDialogResult DiagResult { get; set; }
        Contact contact;

        private TripData _newtrip = null;
        public TripData NewTrip
        {
            get { return _newtrip; }
            set
            {
                this.SetProperty(ref this._newtrip, value);
            }
        }

        private List<ServiceType> _jobTypeList = new List<ServiceType>();
        public List<ServiceType> JobTypeList
        {
            get { return _jobTypeList; }
            set
            {
                this.SetProperty(ref this._jobTypeList, value);
            }
        }

        private ServiceType _selectedJobType;
        public ServiceType SelectedJobType
        {
            get { return _selectedJobType; }
            set
            {
                this.SetProperty(ref this._selectedJobType, value);
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

        private int _defaultIndex;
        public int DefaultIndex
        {
            get { return _defaultIndex; }
            set
            {
                _defaultIndex = value;
            }
        }

        public AddJobDialog()
        {
            this.InitializeComponent();
            JobTypeList = EnumHelper.EnumToList<ServiceType>();
            PayModeList = EnumHelper.EnumToList<TripPayMode>();
            SelectedJobType = JobTypeList.FirstOrDefault();
            SelectedPayMode = PayModeList.FirstOrDefault();
            DefaultIndex = 0;
        }

        private async void selectContact_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var contactPicker = new ContactPicker();
            contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.PhoneNumber);

            contact = await contactPicker.PickContactAsync();

            if (contact != null)
            {
                ClientName.Text = contact.DisplayName;
                PhoneNumber.Text = contact.Phones[0].Number;
            }
        }

        private void saveClient_Click(object sender, RoutedEventArgs e)
        {
            NewTrip = new TripData
            {
                CustomerName = ClientName.Text,
                CustomerPhone1 = PhoneNumber.Text,
                TripStartTime = Convert.ToDateTime(PickTime.Time.ToString()),
                IsTripStarted = false,
                IsTripComplete = false,
                IsPaymentComplete = false,
                TripType = SelectedJobType,
                Mode = SelectedPayMode
            };

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
