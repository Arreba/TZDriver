namespace TZDriver.Models.Tools.Utilities
{
    public static class Constants
    {
        public static readonly string BingMapsAPIKey = "AuIlDdgPqrzYoI6YkoWZ~5QjQi7DmSz-1bwuI2K2oOg~Auk3a3lSiLLXYdh2NqsWFPvcvR-i3vjtflfkcocbXjXKrsRMJUUY2S1Tj2D9HUt5";

        public static readonly string GeofenceId = "TZGeofenceId";
        public static readonly string GeofenceBackgroundTaskName = "TZGeofenceMonitor";
        public static readonly string GeofenceBackgroundTaskEntryPoint = "TZDriver.Monitor.MonitorFence";

        public static readonly string ApplicationTriggerBackgroundTaskName = "TZLocationMonitor";
        public static readonly string ApplicationTriggerBackgroundTaskEntryPoint = "TZDriver.Monitor.MonitorLocation";

        public static readonly string LocalFolderIdentifier = "TZFolder";
        public static readonly string LocationFileIdentifier = "TZLocation.json";
        public static readonly string FenceFileIdentifier = "TZFence.json";


        public static readonly string OfficeDisplayName = "CLIENT RELATIONSHIP OFFICER";
        public static readonly string MtnOfficePhone = "08160000205";
        public static readonly string EtisalatOfficePhone = "08099923030";
    }
}
