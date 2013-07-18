using System;
using Asafaharbor.Web.Models;
using Asafaharbor.Web.Utils;
using Asafaharbor.Web.ViewModels;
using Nancy.Authentication.Forms;
using Nancy.ModelBinding;
using Nancy.Validation;
using Raven.Client;

namespace Asafaharbor.Web.Modules
{
    public class AccountModule : BaseModule
    {
        public AccountModule(IDocumentSession documentSession)
            : base("/account")
        {
            Get["/log-in"] = parameters =>
            {
                Page.Title = "Login";

                var loginModel = new LoginModel();
                Model.LoginModel = loginModel;

                return View["LogOn", Model];
            };

            Get["/register"] = parameters =>
            {
                Page.Title = "Register";

                var registerModel = new RegisterModel();
                Model.RegisterModel = registerModel;

                return View["Register", Model];
            };

            Post["/register"] = parameters =>
                {

                    var model = this.Bind<RegisterModel>();
                    var result = this.Validate(model);
                    if (!result.IsValid)
                    {
                        Page.Title = "Register";

                        foreach (var item in result.Errors)
                        {
                            foreach (var member in item.MemberNames)
                            {
                                Page.Errors.Add(new ErrorModel() {Name = member, ErrorMessage = item.GetMessage(member)});
                            }
                        }
                        Model.RegisterModel = model;

                        return View["Register", Model];
                    }
                    var userMapper = new UserMapper(documentSession);
                    var userGuid = userMapper.ValidateRegisterNewUser(model);
                    //User already exists
                    if (userGuid == null)
                    {
                        Page.Title = "Register";
                        Model.RegisterModel = model;
                        Page.Errors.Add(new ErrorModel() { Name = "Email", ErrorMessage = "This email address has already been registered" });
                        return View["Register", Model];
                    }

                    DateTime? expiry = DateTime.Now.AddDays(7);

                    return this.LoginAndRedirect(userGuid.Value, expiry);
                };
        }
    }
}