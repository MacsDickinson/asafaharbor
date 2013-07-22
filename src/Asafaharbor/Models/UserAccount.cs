using System;
using System.Security.Cryptography;
using System.Text;

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
        public string ImageUrl
        {
            get
            {
                MD5 md5 = MD5.Create();
                byte[] inputBytes = Encoding.ASCII.GetBytes(EMailAddress);
                byte[] hash = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }
                return string.Format("http://www.gravatar.com/avatar/{0}", sb.ToString().ToLower());
            }
        }
    }
}