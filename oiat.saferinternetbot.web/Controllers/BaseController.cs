using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using oiat.saferinternetbot.web.Helpers;

namespace oiat.saferinternetbot.web.Controllers
{
    public abstract class BaseController : Controller
    {
        private const string NotificationKey = "Notifications";

        protected void PushError(string title, string text)
        {
            PushNotification(NotificationType.Error, title, text);
        }

        protected void PushWarning(string title, string text)
        {
            PushNotification(NotificationType.Warning, title, text);
        }

        protected void PushSuccess(string title, string text)
        {
            PushNotification(NotificationType.Success, title, text);
        }

        private void PushNotification(NotificationType type, string title, string text)
        {
            var notifications = new List<Notification>();
            if (TempData[NotificationKey] != null)
            {
                notifications = (List<Notification>)TempData[NotificationKey];
            }
            notifications.Add(new Notification { Type = type, Title = title, Text = text });
            TempData[NotificationKey] = notifications;
        }
    }
}