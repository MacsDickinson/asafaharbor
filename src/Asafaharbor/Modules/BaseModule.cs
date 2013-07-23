using System.Collections.Generic;
using System.Dynamic;
using Asafaharbor.Web.Models;
using Nancy;

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
    }
}