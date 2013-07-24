using System;
using asafaweb.api.Models;

namespace Asafaharbor.Web.Models
{
    public class ScanResults
    {
        public string Id { get; set; }
        public Guid ScanResultId { get; set; }
        public DateTime DateRun { get; set; }
        public ApiScanResult Results { get; set; }
    }
}