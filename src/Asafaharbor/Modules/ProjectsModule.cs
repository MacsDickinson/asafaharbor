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


                    return View["Dashboard", Model];
                };
        }
    }
}