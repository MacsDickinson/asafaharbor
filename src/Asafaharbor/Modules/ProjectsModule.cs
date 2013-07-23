using System;
using System.Collections.Generic;
using System.Linq;
using Asafaharbor.Web.Models;
using Asafaharbor.Web.Models.Enums;
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
                            ProjectId = Guid.NewGuid(),
                            Settings = new ProjectSettings
                                {
                                    FailOnWarnings = false,
                                    FailOnNotTested = false,
                                    EmailOnFailure = false,
                                    TweetOnFailure = false,
                                    IgnoredTests = new List<ASafaTest>()
                                }
                        };
                    documentSession.Store(newProject);
                    documentSession.SaveChanges();

                    Page.Notifications.Add(new NotificationModel { Message = "Project \"" + newProject.Name + "\" Created", Type = NotificationType.Success });

                    Page.Title = newProject.Name;
                    Model.Project = newProject;

                    return View["View", Model];
                };

            Get["/View/{projectId}"] = parameters =>
                {
                    int projectId;
                    if (Int32.TryParse(parameters.projectId, out projectId))
                    {
                        var project = documentSession.Load<Project>(string.Format("projects/{0}", projectId));
                        if (project != null && project.UserId == Page.CurrentUser)
                        {
                            Page.Title = project.Name;
                            Model.Project = project;
                            return View["View", Model];
                        }
                    }

                    Page.Notifications.Add(new NotificationModel { Message = "Project not found", Type = NotificationType.Error });
                    Page.Title = "Project not found";
                    Model.Project = new Project();
                    return View["View", Model];
                };
        }
    }
}