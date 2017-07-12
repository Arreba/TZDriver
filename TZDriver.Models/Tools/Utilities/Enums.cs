using System.ComponentModel.DataAnnotations;

namespace TZDriver.Models.Tools.Utilities
{
    public enum TripPayMode
    {
        Cash = 0,
        POS = 1,
        Transfer = 2
    }

    public enum TripPayStatus
    {
        Paid = 0,
        Awaiting = 1,
        NotPaid = 2
    }

    public enum ServiceType
    {
        [Display(Name = "Hourly Service")]
        Hours = 0,
        [Display(Name = "Airport Service")]
        Airport = 1,
        [Display(Name = "School Run Service")]
        SchoolRun = 2,
        [Display(Name = "Travel Service")]
        Travel = 3,
        [Display(Name = "Full Day Service")]
        FullDay = 4
    }

    public enum ServiceUser
    {
        Individual = 0,
        Partner = 1,
        Corporate = 2
    }

    public enum VehicleAssetType
    {
        Sedan = 0,
        SUV = 1,
        Bus = 2,
        Van = 3,
        Truck = 4
    }

    public enum DriveStateStatus
    {
        [Display(Name = "Pickup State")]
        PickupState = 0,
        [Display(Name = "Trip Started")]
        TripStarted = 1,
        [Display(Name = "Trip Completed")]
        TripCompleted = 2,
        [Display(Name = "Free State")]
        FreeState = 3
    }

    public enum RotateAxis
    {
        Y = 1,
        X = 2,
    }

    public enum Direction
    {
        [Display(Name = "Front To Back")]
        FrontToBack = 0,
        [Display(Name = "Back To Front")]
        BackToFront = 1
    }
}
