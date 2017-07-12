using System;
using TZDriver.Models.Tools.Utilities;

namespace TZDriver.Models.DataModels
{
    public class FuelData
    {
        public int FuelDataId { get; set; }

        public string OfficeRepName { get; set; }

        public string VehicleNumber { get; set; }

        public DateTime RefillDateTime { get; set; }

        public double RefillVolume { get; set; }

        public double RefillLatitude { get; set; }

        public double RefillLongitude { get; set; }

        public string RefillStation { get; set; }

        public string RefillCost { get; set; }

        public string VehicleOdometer { get; set; }

        public VehicleAssetType VehicleAssetType { get; set; }

        public byte[] VehicleBeforePicture { get; set; }

        public byte[] VehicleAfterPicture { get; set; }
    }
}
