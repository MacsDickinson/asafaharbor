using Asafaharbor.Web.Models.Enums;

namespace Asafaharbor.Web.Models
{
    public class NotificationModel
    {
        public string Message { get; set; }
        public NotificationType Type { get; set; }
    }
}