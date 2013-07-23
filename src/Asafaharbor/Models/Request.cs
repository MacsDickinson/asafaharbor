using System;

namespace Asafaharbor.Web.Models
{
    public class Request
    {
        public string RequrestUri { get; set; }
        public string ResponseUri { get; set; }
        public int DurationMs { get; set; }
        public int ResponseBytes { get; set; }
        public int HttpStatusCode { get; set; }
        public string HttpStatusDescription { get; set; }
        public bool IsResponseGzipped { get; set; }
        public string PageTitle { get; set; }
        public string RequestType { get; set; }
        public Exception Exception { get; set; }
        public string HttpMethod { get; set; }
    }
}