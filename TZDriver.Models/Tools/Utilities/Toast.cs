using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace TZDriver.Models.Tools.Utilities
{
    public class Toast
    {
        /// <summary>
        /// Helper method to pop a toast
        /// </summary>
        public static void DoToast( string eventName, int fenceCount)
        {
            // pop a toast for each geofence event
            ToastNotifier ToastNotifier = ToastNotificationManager.CreateToastNotifier();

            // Create a two line toast and add audio reminder

            // Here the xml that will be passed to the
            // ToastNotification for the toast is retrieved
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);

            // Set both lines of text
            XmlNodeList toastNodeList = toastXml.GetElementsByTagName("text");
            toastNodeList.Item(0).AppendChild(toastXml.CreateTextNode("TZDrive Background Geofence Sample"));

            if (1 == fenceCount)
            {
                toastNodeList.Item(1).AppendChild(toastXml.CreateTextNode(eventName));
            }
            else
            {
                string secondLine = "There are " + fenceCount + " Geofence events";
                toastNodeList.Item(1).AppendChild(toastXml.CreateTextNode(secondLine));
            }

            // now create a xml node for the audio source
            IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            XmlElement audio = toastXml.CreateElement("audio");
            audio.SetAttribute("src", "ms-winsoundevent:Notification.SMS");

            ToastNotification toast = new ToastNotification(toastXml);
            ToastNotifier.Show(toast);
        }

        // Create a two line toast and add audio reminder
        public static void ShowToast(string message)
        {
            ToastNotifier notifier = ToastNotificationManager.CreateToastNotifier();
            // Here the xml that will be passed to the
            // ToastNotification for the toast is retrieved
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);

            // Set both lines of text
            XmlNodeList toastNodeList = toastXml.GetElementsByTagName("text");
            toastNodeList.Item(0).AppendChild(toastXml.CreateTextNode("UWindows27 BackgroundTask Notification"));

            toastNodeList.Item(1).AppendChild(toastXml.CreateTextNode(message));

            // now create a xml node for the audio source
            IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            XmlElement audio = toastXml.CreateElement("audio");
            audio.SetAttribute("src", "ms-winsoundevent:Notification.SMS");

            ToastNotification notification = new ToastNotification(toastXml);
            notifier.Show(notification);
        }
    }
}
