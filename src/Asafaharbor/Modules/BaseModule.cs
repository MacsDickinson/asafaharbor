using System.Collections.Generic;
using System.Dynamic;
using ASafaWeb.Library.DomainModels;
using Nancy;
using Nancy.Validation;

namespace Asafaharbor.Web.Modules
{
    public class BaseModule : NancyModule
    {
        public dynamic Model = new ExpandoObject();

        protected PageModel Page { get; set; }

        public BaseModule()
        {
            SetupModelDefaults();
        }

        public BaseModule(string modulepath)
            : base(modulepath)
        {
            SetupModelDefaults();
        }

        private void SetupModelDefaults()
        {
            Before += ctx =>
            {
                Page = new PageModel
                    {
                    IsAuthenticated = ctx.CurrentUser != null,
                    TitleSuffix = " | ASafaHarbor",
                    FriendlyName = ctx.CurrentUser != null ? ((UserIdentity)ctx.CurrentUser).FriendlyName : "",
                    CurrentUser = ctx.CurrentUser != null ? ((UserIdentity)ctx.CurrentUser).UserId : "",
                    ImageUrl = ctx.CurrentUser != null ? ((UserIdentity)ctx.CurrentUser).ImageUrl : "",
                    Errors = new List<ErrorModel>(),
                    Notifications = new List<NotificationModel>()
                };

                Model.Page = Page;

                return null;
            };
        }

        internal void AddPageErrors(ModelValidationResult result)
        {
            foreach (var item in result.Errors)
            {
                foreach (var member in item.MemberNames)
                {
                    Page.Errors.Add(new ErrorModel { Name = member, ErrorMessage = item.GetMessage(member) });
                }
            }
        }
    }
}