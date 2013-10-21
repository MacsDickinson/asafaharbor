using asafaweb.api.Enums;

namespace ASafaWeb.Library.DomainModels
{
    public class Scan
    {
        public ASafaResult ScanStatus { get; set; }
        public string ScanOutcome { get; set; }
        public Request Request { get; set; }
        public string ScanType { get; set; }
    }
}