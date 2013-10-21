using ASafaWeb.Library.DomainModels.Enums;

namespace ASafaWeb.Library.DomainModels
{
    public class NotificationModel
    {
        public string Message { get; set; }
        public NotificationType Type { get; set; }
    }
}