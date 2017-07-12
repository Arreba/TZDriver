using System;
using System.Collections.Generic;
using TZDriver.Models.Tools.Utilities;

namespace TZDriver.Models.DataModels
{
    public class TripData
    {
        //the Base Id has taken careof the primary Key
        public int TripDataId { get; set; }

        public ServiceType TripType { get; set; }

        public ServiceUser TripUser { get; set; }

        //Pick up time and Date
        public DateTime TripStartTime { get; set; }

        public double? TripStartLatitude { get; set; }

        public double? TripStartLongitude { get; set; }

        public string TripStartAddress { get; set; }

        //what is the area like woji or rumudara
        public string TripStartArea { get; set; }

        public string TripEndAddress { get; set; }

        //what is the area like woji or rumudara
        public string TripEndArea { get; set; }

        public bool IsTripComplete { get; set; }

        public bool IsTripStarted { get; set; }

        public string VehicleNumber { get; set; }

        public DateTime TripEndTime { get; set; }

        public string TripEndFare => TripEndTime != DateTime.MinValue && Math.Ceiling(TripEndTime.Subtract(TripStartTime).TotalMinutes * 25) >= 1500 ? string.Format("₦ {0:##,#}", Math.Ceiling(TripEndTime.Subtract(TripStartTime).TotalMinutes)*25) : string.Format("₦ {0:##,#}", 1500);

        public string TripDuration => TripEndTime != DateTime.MinValue ? string.Format("{0}mins", Math.Ceiling((TripEndTime.Subtract(TripStartTime)).TotalMinutes)) : "0.00";

        #region Customer

        public string CustomerName { get; set; }

        public string CustomerPhone1 { get; set; }

        public string CustomerPhone2 { get; set; }

        #endregion

        #region TripPay

        public TripPayStatus Pay { get; set; }

        //Cash or Deposite with us or online or POS
        public TripPayMode Mode { get; set; }

        public string AmountPaid { get; set; }

        public bool IsPaymentComplete { get; set; }

        #endregion

        #region Relationships

        public List<TripPoint> Points { get; set; }

        #endregion

    }
}
