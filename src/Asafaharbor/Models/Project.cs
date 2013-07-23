﻿using System;
using System.Collections.Generic;
using System.Linq;
using Asafaharbor.Web.Models.Enums;

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
        public List<ScanResults> Results { get; set; }
        public ASafaResult Status
        {
            get
            {
                return Results.Count == 0
                           ? ASafaResult.NotTested
                           : Results.OrderByDescending(x => x.DateRun).First().OverallScanStatus;
            }
        }
    }
}