using System.ComponentModel.DataAnnotations;
using Asafaharbor.Web.Models;

namespace Asafaharbor.Web.ViewModels.Account
{
    public class EditAccountModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string FriendlyName { get; set; }
        [Required, EmailAddress]
        public string EMailAddress { get; set; }
        [Url]
        public string Website { get; set; }
        public string ASafaApiUsername { get; set; }
        public string ASafaApiKey { get; set; }
        [Url]
        public string ImageUrl { get; set; }

        public EditAccountModel()
        {
            
        }

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