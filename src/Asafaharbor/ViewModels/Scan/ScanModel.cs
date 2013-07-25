using System;
using asafaweb.api.Models;

namespace Asafaharbor.Web.ViewModels.Scan
{
    public class ScanModel
    {
        public string ProjectName { get; set; }
        public string ProjectId { get; set; }
        public DateTime DateRun { get; set; }
        public ApiScanResult Results { get; set; }
    }
}