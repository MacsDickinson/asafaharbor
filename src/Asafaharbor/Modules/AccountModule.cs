using Asafaharbor.Web.ViewModels;
using Raven.Client;

namespace Asafaharbor.Web.Modules
{
    public class AccountModule : BaseModule
    {
        public AccountModule(IDocumentSession documentSession)
            : base("/account")
        {
            Get["/logon"] = parameters =>
            {
                Page.Title = "Login";

                var loginModel = new LoginModel();
                Model.LoginModel = loginModel;

                return View["LogOn", Model];
            };
        }
    }
}