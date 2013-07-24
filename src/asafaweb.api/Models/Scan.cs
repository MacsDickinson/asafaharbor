using asafaweb.console.Enums;

namespace asafaweb.console.Models
{
    public class Scan
    {
        public AsafaResult ScanStatus { get; set; }
        public string ScanOutcome { get; set; }
        public Request Request { get; set; }
        public string ScanType { get; set; }
    }
}
