using System;
using System.Collections.Generic;
using System.Dynamic;
using Asafaharbor.Web.Models;
using Asafaharbor.Web.Models.Enums;
using Asafaharbor.Web.Responses;
using Asafaharbor.Web.ViewModels;
using Asafaharbor.Web.ViewModels.Account;
using Nancy;
using Nancy.ViewEngines;

namespace Asafaharbor.Web.Renderers
{
    public class ErrorViewRenderer : DefaultViewRenderer
    {
        public dynamic Model = new ExpandoObject();
        protected PageModel Page { get; set; }

        public ErrorViewRenderer(IViewFactory factory)
            : base(factory)
        {
            SetupModelDefaults();
        }

        public Response RenderResponse(HttpStatusCode statusCode, NancyContext context)
        {
            SetupModelDefaults();

            Response response = RenderView(context, "Error/SeriousError", Model);

            string errorMessage = "Something went horribly wrong!";

            Page.IsAuthenticated = context.CurrentUser != null;
            if (context.CurrentUser != null)
            {
                var user = (UserIdentity) context.CurrentUser;
                Page.CurrentUser = user.UserName;
                Page.ImageUrl = user.ImageUrl;
                Page.FriendlyName = user.FriendlyName;
            }
            else
            {
                Page.Notifications.Add(new NotificationModel
                    {
                        Message = "Please log in to access this page.",
                        Type = NotificationType.Error
                    });
                Model.LoginModel = new LoginModel();
                return RenderView(context, "Account/Login", Model);
            }
            response.StatusCode = statusCode;

            switch (statusCode)
            {
                case HttpStatusCode.BadRequest:
                    errorMessage = "Sorry - we were unable to process your request";
                    Page.Title = "Bad Request";
                    response = RenderView(context, "Error/BadRequest", Model);
                    break;
                case HttpStatusCode.NotFound:
                    errorMessage = "We couldn't find what you were looking for";
                    Page.Title = "Page Not Found";
                    response = RenderView(context, "Error/PageNotFound", Model);
                    break;
                case HttpStatusCode.InternalServerError:
                    errorMessage = "Looks like we've come into a little bother";
                    Page.Title = "Something went wrong";
                    var error = context.Response as ErrorResponse;
                    if (error != null)
                    {
                        Model.Error = new InternalServerErrorViewModel
                            {
                                ErrorId = DateTime.Now.ToString("yyyyMMddhhmmssss"),
                                Summary = error.ErrorMessage,
                                Exceptions = error.Errors,
                                StackTrace = error.FullException
                            };
                    }
                    response = RenderView(context, "Error/internalServerError", Model);
                    break;
            }
            Page.Notifications.Add(new NotificationModel
            {
                Type = NotificationType.Error,
                Message = errorMessage
            });
            return response;
        }

        private void SetupModelDefaults()
        {
            Page = new PageModel
            {
                Title = "Error",
                TitleSuffix = " | ASafaHarbor",
                Errors = new List<ErrorModel>(),
                Notifications = new List<NotificationModel>()
            };

            Model.Page = Page;
        }
    }
}