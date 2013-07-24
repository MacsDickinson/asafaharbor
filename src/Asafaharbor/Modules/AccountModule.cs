using System;
using System.Linq;
using System.Web;
using Asafaharbor.Web.Models;
using Asafaharbor.Web.Models.Enums;
using Asafaharbor.Web.Utils;
using Asafaharbor.Web.ViewModels.Account;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.ModelBinding;
using Nancy.Responses;
using Nancy.Security;
using Nancy.Validation;
using Raven.Client;

namespace Asafaharbor.Web.Modules
{
    public class AccountModule : BaseModule
    {
        public AccountModule(IDocumentSession documentSession)
            : base("/account")
        {
            Get["/"] = parameters =>
                {
                    this.RequiresAuthentication();

                    UserAccount user = documentSession.Load<UserAccount>(Page.CurrentUser);

                    if (user != null)
                    {
                        Page.Title = "Account";
                        Model.User = new EditAccountModel(user);
                        return View["Edit", Model];
                    }

                    Page.Notifications.Add(new NotificationModel
                        {
                            Message = "User not found. Please log in.",
                            Type = NotificationType.Error
                        });

                    Page.Title = "Log in";
                    Model.LoginModel = new LoginModel();
                    return View["LogIn", Model];
                };

            Post["/"] = parameters =>
                {
                    this.RequiresAuthentication();

                    var model = this.Bind<EditAccountModel>();
                    var result = this.Validate(model);

                    if (!result.IsValid)
                    {
                        AddPageErrors(result);

                        Page.Title = "Account";
                        Model.User = model;
                        return View["Edit", Model];
                    }

                    UserAccount user = documentSession.Load<UserAccount>(Page.CurrentUser);

                    if (user != null)
                    {
                        user.Username = model.Username;
                        user.FriendlyName = model.FriendlyName;
                        user.EMailAddress = model.EMailAddress;
                        user.Website = model.Website;
                        user.ASafaApiUsername = model.ASafaApiUsername;
                        user.ASafaApiKey = model.ASafaApiKey;
                        user.ImageUrl = model.ImageUrl;

                        documentSession.SaveChanges();
                    }

                    return new RedirectResponse("Projects", RedirectResponse.RedirectType.Temporary);
                };

            Get["/log-in"] = parameters =>
            {
                Page.Title = "Log in";

                var loginModel = new LoginModel();
                Model.LoginModel = loginModel;

                return View["LogIn", Model];
            };

            Post["/log-in"] = parameters =>
                {
                    var model = this.Bind<LoginModel>();
                    var result = this.Validate(model);
                    if (!result.IsValid)
                    {
                        AddPageErrors(result);

                        Page.Title = "Log in";
                        Model.LoginModel = model;
                        return View["LogIn", Model];
                    }

                    var userMapper = new UserMapper(documentSession);
                    Guid? userId = userMapper.ValidateUser(model.Email, model.Password);
                    if (!userId.HasValue)
                    {
                        Page.Notifications.Add(new NotificationModel
                            {
                                Message = "Login failed: Incorrect credentials supplied",
                                Type = NotificationType.Error
                            });
                        Model.LoginModel = model;
                        return View["LogIn", Model];
                    }
                    DateTime? expiry;
                    if (model.RememberMe)
                    {
                        expiry = DateTime.Now.AddDays(7);
                    }
                    else
                    {
                        expiry = null;
                    }
                    return this.LoginAndRedirect(userId.Value, expiry, "~/Projects");
                };

            Get["/log-out"] = parameters =>
                {
                    return this.LogoutAndRedirect("~/Account/Log-in");
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
                        AddPageErrors(result);

                        Page.Title = "Register";
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
                        Page.Errors.Add(new ErrorModel { Name = "Email", ErrorMessage = "This email address has already been registered" });
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
                    Model.LoginModel = new LoginModel();
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
                        if (userRecord != null && userRecord.ConfirmKey == key && userRecord.Confirmed == false)
                        {
                            userRecord.Confirmed = true;
                            documentSession.SaveChanges();
                            Model.Page.Notifications.Add(new NotificationModel
                            {
                                Message = "Account confirmed. Welcome to ASafaHarbor " + userRecord.FriendlyName,
                                Type = NotificationType.Success
                            });
                            DateTime? expiry = DateTime.Now.AddDays(7);
                            return this.LoginAndRedirect(userRecord.UserId, expiry, "~/Projects");
                        }
                    }
                    Model.Page.Notifications.Add(new NotificationModel
                    {
                        Message = "Confirmation link invalid",
                        Type = NotificationType.Error
                    });
                    Model.LoginModel = new LoginModel();
                    return View["LogIn", Model];
                };
        }
    }
}