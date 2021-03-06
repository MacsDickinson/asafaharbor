﻿using System.Collections.Generic;
using Nancy.Security;

namespace ASafaWeb.Library.DomainModels
{
    public class UserIdentity : IUserIdentity
    {
        public IEnumerable<string> Claims { get; set; }
        public string UserName { get; set; }
        public string FriendlyName { get; set; }
        public string UserId { get; set; }
        public string ImageUrl { get; set; }
    }
}