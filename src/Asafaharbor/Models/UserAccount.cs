using System;

namespace Asafaharbor.Web.Models
{
    public class UserAccount
    {
        public string Id { get; set; }
        public Guid UserId { get; set; }
        public string LoginType { get; set; }
        public string Username { get; set; }
        public string FriendlyName { get; set; }
        public string EMailAddress { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Website { get; set; }
        public Guid ConfirmKey { get; set; }
        public bool Confirmed { get; set; }
    }
}