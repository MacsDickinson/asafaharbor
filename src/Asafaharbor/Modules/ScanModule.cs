using System;
using System.Collections.Generic;
using System.Linq;
using Asafaharbor.Web.Models;
using Asafaharbor.Web.Models.Enums;
using Asafaharbor.Web.ViewModels.Scan;
using Nancy.Security;
using Raven.Client;

namespace Asafaharbor.Web.Modules
{
    public class ScanModule : BaseModule
    {
        public ScanModule(IDocumentSession documentSession)
            : base("/scan")
        {
            Get["/{projectId}/{key}"] = parameters =>
            {
                bool linkValid = true;
                Guid projectId;
                if (!Guid.TryParse(parameters.projectId, out projectId))
                    linkValid = false;
                Guid key;
                if (!Guid.TryParse(parameters.key, out key))
                    linkValid = false;
                if (linkValid)
                {
                    var project = documentSession.Query<Project>().FirstOrDefault(x => x.ProjectId == projectId);

                    if (project != null && project.Key == key)
                    {
                        var user = documentSession.Load<UserAccount>(project.UserId);

                        if (user != null)
                        {
                            var results = project.Scan(user.ASafaApiUsername, user.ASafaApiKey);

                            if (project.Results == null)
                                project.Results = new List<ScanResults>();

                            project.Results.Add(results);

                            documentSession.SaveChanges();

                            ScanModel model = new ScanModel
                            {
                                DateRun = results.DateRun,
                                ProjectName = project.Name,
                                Results = results.Results
                            };
                            Page.Title = "Scan Results";
                            Model.Scan = model;
                            return View["View", Model];
                        }
                    }
                }
                return null;
            };

            Post["/{projectId}/{key}"] = parameters =>
                {
                    bool linkValid = true;
                    Guid projectId;
                    if (!Guid.TryParse(parameters.projectId, out projectId))
                        linkValid = false;
                    Guid key;
                    if (!Guid.TryParse(parameters.key, out key))
                        linkValid = false;
                    if (linkValid)
                    {
                        var project = documentSession.Query<Project>().FirstOrDefault(x => x.ProjectId == projectId);

                        if (project != null && project.Key == key)
                        {
                            var user = documentSession.Load<UserAccount>(project.UserId);

                            if (user != null)
                            {
                                var results = project.Scan(user.ASafaApiUsername, user.ASafaApiKey);

                                if (project.Results == null)
                                    project.Results = new List<ScanResults>();

                                project.Results.Add(results);

                                documentSession.SaveChanges();
                            }
                        }
                    }
                    return null;
                };

            Get["/View/{projectId}/{scanId}"] = parameters =>
                {
                    this.RequiresAuthentication();

                    Page.Title = "Scan Details";

                    bool linkValid = true;
                    int projectId;
                    if (!Int32.TryParse(parameters.projectId, out projectId))
                        linkValid = false;
                    Guid scanId;
                    if (!Guid.TryParse(parameters.scanId, out scanId))
                        linkValid = false;

                    if (linkValid)
                    {
                        var project = documentSession.Load<Project>("projects/" + projectId);
                        if (project != null)
                        {
                            var scans = project.Results.Where(r => r.ScanResultId == scanId).ToList();
                            if (scans.Count() == 1)
                            {
                                ScanResults scan = scans.Take(1).FirstOrDefault();
                                if (scan != null)
                                {
                                    ScanModel model = new ScanModel
                                        {
                                            DateRun = scan.DateRun,
                                            ProjectName = project.Name,
                                            Results = scan.Results,
                                            ProjectId = project.Id.Replace("projects/", "")
                                        };
                                    Page.Title = "Scan Results";
                                    Model.Scan = model;
                                    return View["View", Model];
                                }
                            }
                        }
                    }

                    Page.Notifications.Add(new NotificationModel { Message = "Unexpected Scan ID", Type = NotificationType.Error});
                    return View["View", Model];
                };
        }
    }
}