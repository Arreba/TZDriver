using System;
using System.Collections.Generic;
using TZDriver.Models.Services.IServices;
using Windows.Web.Http;

namespace TZDriver.Models.Services
{
    public class SmsService : ISmsService
    {
        private HttpClient httpClient;
        private HttpResponseMessage responseMessage;
        Dictionary<string, string> SmsCollection;

        public bool IsSendSuccessful { get; set; }

        public SmsService()
        {
            SmsCollection = new Dictionary<string, string>();
        }

        #region SEND SMS
        public void TripPickupSms(string tripClientName, string driverName, string estimatedPickupTime)
        {
            var message = "Thank you " + tripClientName + " for choosing TZTAXI." + Environment.NewLine
                + "Your personal driver " + driverName + " will arrive at" + Environment.NewLine
                + "  Arrival Time: " + estimatedPickupTime + Environment.NewLine
                + "To Cancel or Reschedule, Please Call 08160000204 " + Environment.NewLine
                + "Please do enjoy the ride";
            SendSMS(message);
        }

        public void TripStartSms(string tripClientName, string clientDuration, string clientDistance)
        {
            var message = "Thank you " + tripClientName + " for using TZTAXI." + Environment.NewLine
                + "  TRIP DURATION:     " + clientDuration + Environment.NewLine
                + "  DRIVEN DISTANCE: " + clientDistance + Environment.NewLine
                + "For any Complains or Enguiries, Please Call 08160000204 " + Environment.NewLine
                + "Our goal is to make our service better than the last";
            SendSMS(message);
        }

        public void TripCompleteSms(string tripClientName, string clientFare, string clientDuration, string clientDistance)
        {
            var message = "Thank you " + tripClientName + " for using TZTAXI." + Environment.NewLine
                + "  TRIP FARE:              " + clientFare + " Naira" + Environment.NewLine
                + "  TRIP DURATION:     " + clientDuration + Environment.NewLine
                + "  DRIVEN DISTANCE: " + clientDistance + Environment.NewLine
                + "For any Complains or Enguiries, Please Call 08160000204 " + Environment.NewLine
                + "Our goal is to make our service better than the last";
            SendSMS(message);
        }

        public void SchoolRunSms(string tripClientName, string tripStartTime, string tripStartLocation, string tripEndtime, string tripEndLocation, string tripDate)
        {
            var message = "Thank you " + tripClientName + " for using TZTAXI." + Environment.NewLine
                + "Your Child has been picked at" + Environment.NewLine
                + "  TIME:    " + tripStartTime + Environment.NewLine
                + "  ADDRESS: " + tripStartLocation + Environment.NewLine
                + "and has been safely dropped at" + Environment.NewLine
                + "  TIME:    " + tripEndtime + Environment.NewLine
                + "  ADDRESS: " + tripEndLocation + Environment.NewLine
                + "You can always do a personal confirmation" + Environment.NewLine
                + "For any Complains or Enguiries, Please Call 08160000204 " + Environment.NewLine
                + "Our goal is to make our service better than the last"
                + tripDate + Environment.NewLine;
            SendSMS(message);
        }
        #endregion

        private async void SendSMS(string smsMessage)
        {
            SmsCollection.Add("api_key", "824f8643");
            SmsCollection.Add("api_secret", "62158cf7");
            SmsCollection.Add("from", "TZTAXI");
            SmsCollection.Add("text", smsMessage);
            var baseAddress = new Uri("https://rest.nexmo.com/sms/json?");
            try
            {
                httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                HttpFormUrlEncodedContent httpcontent = new HttpFormUrlEncodedContent(SmsCollection);
                httpcontent.Headers.ContentType = new Windows.Web.Http.Headers.HttpMediaTypeHeaderValue("application/x-www-form-urlencoded");
                responseMessage = await httpClient.PostAsync(baseAddress, httpcontent);
                if (responseMessage.IsSuccessStatusCode)
                {
                    IsSendSuccessful = true;
                }
            }
            catch (Exception)
            {
                IsSendSuccessful = false;
            }
        }
    }
}
