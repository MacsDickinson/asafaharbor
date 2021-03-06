﻿using System;
using System.Security.Cryptography;
using System.Text;

namespace ASafaWeb.Library.DomainModels
{
    public class UserAccount
    {
        private string _imageUrl;

        public string Id { get; set; }
        public Guid UserId { get; set; }
        public string LoginType { get; set; }
        public string Username { get; set; }
        public string FriendlyName { get; set; }
        public string EMailAddress { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Website { get; set; }
        public string ASafaApiUsername { get; set; }
        public string ASafaApiKey { get; set; }
        public Guid ConfirmKey { get; set; }
        public bool Confirmed { get; set; }
        public string ImageUrl
        {
            get
            {
                if (_imageUrl == null)
                {
                    MD5 md5 = MD5.Create();
                    byte[] inputBytes = Encoding.ASCII.GetBytes(EMailAddress);
                    byte[] hash = md5.ComputeHash(inputBytes);

                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hash.Length; i++)
                    {
                        sb.Append(hash[i].ToString("X2"));
                    }
                    _imageUrl = string.Format("http://www.gravatar.com/avatar/{0}", sb.ToString().ToLower());
                }
                return _imageUrl;
            }
            set { _imageUrl = value; }
        }
    }
}