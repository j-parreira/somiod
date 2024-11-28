using somiod.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace somiod.Handlers
{
    public class NotificationHandler
    {
        internal static void AddNotificationToDatabase(string application, string container, Notification notification)
        {
            throw new NotImplementedException();
        }

        internal static void DeleteNotificationFromDatabase(string application, string container, string notification)
        {
            throw new NotImplementedException();
        }

        internal static Notification FindNotificationInDatabase(string application, string container, string notification)
        {
            throw new NotImplementedException();
        }

        internal static List<Notification> FindNotificationsByApplication(string application)
        {
            throw new NotImplementedException();
        }

        internal static List<Notification> FindNotificationsByContainer(string application, string container)
        {
            throw new NotImplementedException();
        }

        internal static bool NotificationExists(string application, string container, string notification)
        {
            throw new NotImplementedException();
        }
    }
}