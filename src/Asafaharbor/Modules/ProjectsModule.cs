using System.Collections.Generic;
using System.Linq;
using Asafaharbor.Web.Models;
using Nancy.Security;
using Raven.Client;

namespace Asafaharbor.Web.Modules
{
    public class ProjectsModule : BaseModule
    {

        public ProjectsModule(IDocumentSession documentSession)
            : base("/projects")
        {
            this.RequiresAuthentication();

            Get["/"] = parameters =>
                {
                    Page.Title = "Projects";

                    Model.Projects = documentSession.Query<Project>().Where(x => x.UserId == Page.CurrentUser).ToList();
                    return View["Dashboard", Model];
                };
        }
    }
}