using System;
using System.Collections.Generic;
using System.Linq;
using ASafaWeb.Library.DomainModels;
using ASafaWeb.Library.DomainModels.Enums;
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

            Post["/"] = parameters =>
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

            Get["/EditSettings/{projectId}"] = parameters =>
                {
                    int projectId;
                    if (Int32.TryParse(parameters.projectId, out projectId))
                    {
                        var project = documentSession.Load<Project>(string.Format("projects/{0}", projectId));
                        if (project != null && project.UserId == Page.CurrentUser)
                        {
                            Page.Title = project.Name;
                            EditSettingsModel model = new EditSettingsModel
                                {
                                    FailOnWarnings = project.Settings.FailOnWarnings,
                                    FailOnNotTested = project.Settings.FailOnNotTested,
                                    EmailOnFailure = project.Settings.EmailOnFailure,
                                    FailureEmail = project.Settings.FailureEmail,
                                    TweetOnFailure = project.Settings.TweetOnFailure,
                                    FailureTwitter = project.Settings.FailureTwitter,
                                    IgnoredTests = project.Settings.IgnoredTests,
                                    ProjectId = project.Id,
                                    ProjectName = project.Name,
                                    UrlToScan = project.Url
                                };
                            Model.EditSettingsModel = model;
                            return View["EditSettings", Model];
                        }
                    }

                    Page.Notifications.Add(new NotificationModel { Message = "Project not found", Type = NotificationType.Error });
                    Page.Title = "Project not found";
                    Model.Project = new Project();
                    return View["View", Model];
                };

            Post["/EditSettings/{projectId}"] = parameters =>
                {
                    var model = this.Bind<EditSettingsModel>();
                    var result = this.Validate(model);
                    
                    if (!result.IsValid)
                    {
                        AddPageErrors(result);

                        Page.Title = model.ProjectName;
                        Model.EditSettingsModel = model;
                        return View["EditSettings", Model];
                    }
                    int projectId;
                    if (Int32.TryParse(parameters.projectId, out projectId))
                    {
                        var project = documentSession.Load<Project>(string.Format("projects/{0}", projectId));
                        if (project != null && project.UserId == Page.CurrentUser)
                        {
                            Page.Title = project.Name;

                            project.Settings.FailOnWarnings = model.FailOnWarnings;
                            project.Settings.FailOnNotTested = model.FailOnNotTested;
                            project.Settings.EmailOnFailure = model.EmailOnFailure;
                            project.Settings.FailureEmail = model.FailureEmail;
                            project.Settings.TweetOnFailure = model.TweetOnFailure;
                            project.Settings.FailureTwitter = model.FailureTwitter;
                            project.Settings.IgnoredTests = model.IgnoredTests;
                            project.Name = model.ProjectName;
                            project.Url = model.UrlToScan;

                            documentSession.SaveChanges();

                            Page.Notifications.Add(new NotificationModel
                                {
                                    Message = "Settings Updated!",
                                    Type = NotificationType.Success
                                });

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