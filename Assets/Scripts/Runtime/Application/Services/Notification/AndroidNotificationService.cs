#if UNITY_ANDROID
using System;
using Unity.Notifications.Android;

namespace Runtime.Services.Notification
{
    public class AndroidNotificationService : NotificationService
    {
        private const string AndroidChannelId = "default_channel";

        public override void Initialize()
        {
            var androidChannel = new AndroidNotificationChannel()
            {
                Id = AndroidChannelId,
                Name = "Default Channel",
                Importance = Importance.Default,
                Description = "Generic notifications"
            };
            AndroidNotificationCenter.RegisterNotificationChannel(androidChannel);
        }

        public override void SendNotification(string title, string body, string smallIconId, string largeIconId, DateTime deliveryTime)
        {
            var androidNotification = new AndroidNotification()
            {
                Title = title,
                Text = body,
                SmallIcon = smallIconId,
                LargeIcon = largeIconId,
                FireTime = deliveryTime
            };
            AndroidNotificationCenter.SendNotification(androidNotification, AndroidChannelId);
        }

        public override void CancelAllNotifications()
        {
            AndroidNotificationCenter.CancelAllNotifications();
        }
    }
}
#endif