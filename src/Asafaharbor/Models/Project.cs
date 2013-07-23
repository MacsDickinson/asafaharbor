using System;
using System.Collections.Generic;

namespace Asafaharbor.Web.Models
{
    public class Project
    {
        public string Id { get; set; }
        public Guid ProjectId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public List<ScanResults> Results { get; set; }
    }
}