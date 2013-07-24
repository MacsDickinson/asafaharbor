using System;
using System.Collections.Generic;
using System.Linq;
using Asafaharbor.Web.Models.Enums;
using asafaweb.api.Enums;
using asafaweb.api.Logic;

namespace Asafaharbor.Web.Models
{
    public class Project
    {
        public string Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid Key { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public ProjectSettings Settings { get; set; }
        public List<ScanResults> Results { get; set; }
        public ASafaResult Status
        {
            get
            {
                if (Results == null)
                    return ASafaResult.NotTested;
                return Results.Count == 0
                           ? ASafaResult.NotTested
                           : Results.OrderByDescending(x => x.DateRun).First().Results.OverallScanStatus;
            }
        }
        public DateTime? LastScanned
        {
            get
            {
                if (Results == null)
                    return null;
                if (Results.Count == 0)
                    return null;
                return Results.OrderByDescending(x => x.DateRun).First().DateRun;
            }
        }

        public ScanResults Scan(string name, string key)
        {

            ApiLogic api = new ApiLogic(name, key, Url);

            ScanResults scanResults = new ScanResults
                {
                    DateRun = DateTime.Now,
                    ScanResultId = Guid.NewGuid(),
                    Results = api.Scan()
                };
            return scanResults;
        }
    }
}