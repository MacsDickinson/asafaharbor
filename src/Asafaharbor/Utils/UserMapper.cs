using System;
using System.Linq;
using System.Web.Helpers;
using ASafaWeb.Library.DomainModels;
using Asafaharbor.Web.ViewModels.Account;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Security;
using Raven.Client;

namespace Asafaharbor.Web.Utils
{
    public class UserMapper : IUserMapper
    {
        private readonly IDocumentSession _documentSession;

        public UserMapper(IDocumentSession documentSession)
        {
            _documentSession = documentSession;

        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            var userRecord = _documentSession.Query<UserAccount>().FirstOrDefault(x => x.UserId == identifier);

            return userRecord == null ? null : new UserIdentity { UserName = userRecord.Username, FriendlyName = userRecord.FriendlyName, UserId = userRecord.Id, ImageUrl = userRecord.ImageUrl };
        }

        public Guid? ValidateUser(string email, string password)
        {
            var userRecord = _documentSession.Query<UserAccount>().FirstOrDefault(x => x.EMailAddress == email);
            if (userRecord == null) return null;

            if (Crypto.VerifyHashedPassword(userRecord.Password, password + userRecord.Salt))
                return userRecord.UserId;
            return null;
        }

        public UserAccount ValidateRegisterNewUser(RegisterModel newUser)
        {
            var salt = Crypto.GenerateSalt();
            var userRecord = new UserAccount
                {
                UserId = Guid.NewGuid(),
                LoginType = "ASafaHarbor",
                EMailAddress = newUser.Email,
                FriendlyName = newUser.Name,
                Username = newUser.UserName,
                Salt = salt,
                Password = Crypto.HashPassword(newUser.Password + salt),
                ConfirmKey = Guid.NewGuid(),
                Confirmed = false
            };

            var existingUser = _documentSession.Query<UserAccount>().FirstOrDefault(x => x.EMailAddress == userRecord.EMailAddress && x.LoginType == "ASafaHarbor");
            if (existingUser != null)
                return null;

            _documentSession.Store(userRecord);
            _documentSession.SaveChanges();

            return userRecord;
        }
    }
}