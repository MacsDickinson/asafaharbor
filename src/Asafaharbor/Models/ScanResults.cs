using System;
using System.Collections.Generic;
using Asafaharbor.Web.Models.Enums;

namespace Asafaharbor.Web.Models
{
    public class ScanResults
    {
        public string Id { get; set; }
        public Guid ScanResultId { get; set; }
        public DateTime DateRun { get; set; }
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
    }
}