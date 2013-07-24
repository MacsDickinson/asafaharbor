using System.Collections.Generic;
using asafaweb.console.Enums;

namespace asafaweb.console.Models
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
        public AsafaResult OverallScanStatus { get; set; }
    }
}
