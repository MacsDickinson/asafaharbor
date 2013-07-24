using Asafaharbor.Web.Models;

namespace Asafaharbor.Web.ViewModels.Account
{
    public class EditAccountModel
    {
        public string Username { get; set; }
        public string FriendlyName { get; set; }
        public string EMailAddress { get; set; }
        public string Password { get; set; }
        public string Website { get; set; }
        public string ASafaApiUsername { get; set; }
        public string ASafaApiKey { get; set; }
        public string ImageUrl { get; set; }

        public EditAccountModel(UserAccount user)
        {
            Username = user.Username;
            FriendlyName = user.FriendlyName;
            EMailAddress = user.EMailAddress;
            Website = user.Website;
            ASafaApiUsername = user.ASafaApiUsername;
            ASafaApiKey = user.ASafaApiKey;
            ImageUrl = user.ImageUrl;
        }
    }
}