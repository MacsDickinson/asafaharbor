using System;
using System.Linq;
using System.Web;
using Asafaharbor.Web.Models;
using Asafaharbor.Web.Models.Enums;
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
                Page.Title = "Log in";

                var loginModel = new LoginModel();
                Model.LoginModel = loginModel;

                return View["LogIn", Model];
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
                    UserAccount userAccount = userMapper.ValidateRegisterNewUser(model);
                    //User already exists
                    if (userAccount == null)
                    {
                        Page.Title = "Register";
                        Model.RegisterModel = model;
                        Page.Errors.Add(new ErrorModel() { Name = "Email", ErrorMessage = "This email address has already been registered" });
                        return View["Register", Model];
                    }
                    string confirmationLink = string.Format("{0}/Account/Confirm/{1}/{2}",
                                                            HttpContext.Current.Request.Url.Authority, userAccount.UserId, userAccount.ConfirmKey);
                    Email activationEmail = Email.NewConfirmationEmail(model.Email, confirmationLink, model.Name);
                    activationEmail.Send();

                    documentSession.Store(activationEmail);
                    documentSession.SaveChanges();

                    Model.Page.Notifications.Add(new NotificationModel
                        {
                            Message = string.Format("Activation email sent to {0}", model.Email),
                            Type = NotificationType.Success
                        });

                    return View["LogIn", Model];
                };

            Get["/Confirm/{userid}/{key}"] = parameters =>
                {
                    bool linkValid = true;
                    Guid userId;
                    if (!Guid.TryParse(parameters.userid, out userId))
                        linkValid = false;
                    Guid key;
                    if (!Guid.TryParse(parameters.key, out key))
                        linkValid = false;
                    if (linkValid)
                    {
                        var userRecord = documentSession.Query<UserAccount>().FirstOrDefault(x => x.UserId == userId);
                        if (userRecord != null && userRecord.ConfirmKey == key)
                        {
                            userRecord.Confirmed = true;
                            documentSession.SaveChanges();
                            Model.Page.Notifications.Add(new NotificationModel
                            {
                                Message = "Account confirmed. Welcome to ASafaHarbor " + userRecord.FriendlyName,
                                Type = NotificationType.Success
                            });
                            DateTime? expiry = DateTime.Now.AddDays(7);
                            this.Login(userRecord.UserId, expiry);
                            return this.LoginAndRedirect(userRecord.UserId, expiry, "~/Home");
                        }
                    }
                    Model.Page.Notifications.Add(new NotificationModel
                    {
                        Message = "Confirmation link invalid",
                        Type = NotificationType.Error
                    });
                    return View["LogIn", Model];
                };
        }
    }
}