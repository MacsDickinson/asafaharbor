using System;
using System.Collections.Generic;
using System.Linq;
using Asafaharbor.Web.Models;
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
                        }
                    }
                }
                return null;
            };
        }
    }
}