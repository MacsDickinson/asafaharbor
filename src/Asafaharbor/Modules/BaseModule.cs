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
                Page = new PageModel()
                {
                    IsAuthenticated = ctx.CurrentUser != null,
                    TitleSuffix = " | ASafaHarbor",
                    CurrentUser = ctx.CurrentUser != null ? ctx.CurrentUser.UserName : "",
                    Errors = new List<ErrorModel>()
                };

                Model.Page = Page;

                return null;
            };
        }
    }
}