using System;
using System.Linq;
using Asafaharbor.Web.Models;
using Asafaharbor.Web.ViewModels.Projects;
using Nancy.ModelBinding;
using Nancy.Security;
using Nancy.Validation;
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

            Get["/New/"] = parameters =>
                {
                    Page.Title = "New Project";

                    Model.NewProjectModel = new NewProjectModel();

                    return View["New", Model];
                };

            Post["/New/"] = parameters =>
                {
                    var model = this.Bind<NewProjectModel>();
                    var result = this.Validate(model);
                    if (!result.IsValid)
                    {
                        AddPageErrors(result);

                        Page.Title = "New Project";
                        Model.NewProjectModel = model;
                        return View["New", Model];
                    }
                    Project newProject = new Project
                        {
                            Name = model.Name,
                            Url = model.Url,
                            Key = Guid.NewGuid(),
                            UserId = Page.CurrentUser,
                            
                        };
                    documentSession.Store(newProject);
                    documentSession.SaveChanges();

                    Page.Title = newProject.Name;
                    Model.Project = newProject;

                    return View["View", Model];
                };
        }
    }
}