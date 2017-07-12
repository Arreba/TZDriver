namespace TZDriver.Models.Services.IServices
{
    public interface ISmsService
    {
        bool IsSendSuccessful { get; set; }
        void TripPickupSms(string tripClientName, string driverName, string estimatedPickupTime);
        void TripStartSms(string tripClientName, string clientDuration, string clientDistance);
        void TripCompleteSms(string tripClientName, string clientFare, string clientDuration, string clientDistance);
        void SchoolRunSms(string tripClientName, string tripStartTime, string tripStartLocation, string tripEndtime, string tripEndLocation, string tripDate);
    }
}
