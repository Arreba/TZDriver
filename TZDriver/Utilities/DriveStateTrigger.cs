using System;
using TZDriver.Models.Tools.Utilities;
using Windows.UI.Xaml;

namespace TZDriver.Utilities
{
    public class DriveStateTrigger : StateTriggerBase
    {
        public DriveStateTrigger()
        {

        }

        public string DriveState
        {
            get { return (string)GetValue(DriveStateProperty); }
            set { SetValue(DriveStateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DriveState.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DriveStateProperty =
            DependencyProperty.Register("DriveState", typeof(string), typeof(DriveStateTrigger), new PropertyMetadata(null, PropertyChangedCallback));

        public DriveStateStatus DriveStatus
        {
            get { return (DriveStateStatus)GetValue(DriveStatusProperty); }
            set { SetValue(DriveStatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DriveStatus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DriveStatusProperty =
            DependencyProperty.Register("DriveStatus", typeof(DriveStateStatus), typeof(DriveStateTrigger), new PropertyMetadata(DriveStateStatus.PickupState));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DriveStateTrigger driveTrigger = d as DriveStateTrigger;

            if (driveTrigger == null)
            {
                return;
            }

            switch (driveTrigger.DriveStatus)
            {
                case DriveStateStatus.FreeState:

                    driveTrigger.SetActive(driveTrigger.DriveState.ToString().Equals(DriveStateStatus.FreeState.ToString()));

                    break;
                case DriveStateStatus.PickupState:

                    driveTrigger.SetActive(driveTrigger.DriveState.ToString().Equals(DriveStateStatus.PickupState.ToString()));

                    break;
                case DriveStateStatus.TripCompleted:

                    driveTrigger.SetActive(driveTrigger.DriveState.ToString().Equals(DriveStateStatus.TripCompleted.ToString()));

                    break;
                case DriveStateStatus.TripStarted:

                    driveTrigger.SetActive(driveTrigger.DriveState.ToString().Equals(DriveStateStatus.TripStarted.ToString()));

                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
