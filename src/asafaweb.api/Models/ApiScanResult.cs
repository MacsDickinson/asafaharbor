using System.Collections.Generic;
using System.Linq;
using asafaweb.api.Enums;

namespace asafaweb.api.Models
{
    public class ApiScanResult
    {
        public string ScanUri { get; set; }
        public List<Request> Requests { get; set; }
        public List<Scan> Scans { get; set; }
        public string SiteTitle { get; set; }
        public bool UsWebFormsApp { get; set; }
        public bool IsAspNetSite { get; set; }
        public string ServerHeader { get; set; }
        public string XAspNetVersion { get; set; }
        public string XAspNetMvcVersion { get; set; }
        public string[] XPoweredBy { get; set; }
        public string AspNetVersion { get; set; }
        public ASafaResult OverallScanStatus { get; set; }
        
        public List<Scan> FailedScans
        {
            get { return Scans.Where(x => x.ScanStatus == ASafaResult.Fail).ToList(); }
        }

        public List<Scan> WarningScans
        {
            get { return Scans.Where(x => x.ScanStatus == ASafaResult.Warning).ToList(); }
        }

        public List<Scan> NotRunScans
        {
            get { return Scans.Where(x => x.ScanStatus == ASafaResult.NotTested).ToList(); }
        }
    }
}
