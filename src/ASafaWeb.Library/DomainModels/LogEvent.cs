using System;
using System.Web.Management;

namespace ASafaWeb.Library.DomainModels
{
    public class LogEvent : WebRequestErrorEvent
    {
        public LogEvent(string message)
            : base(null, null, 100001, new Exception(message))
        {
        }
        
        public LogEvent(string message, Exception exception)
            : base(null, null, 100001, new Exception(message, exception))
        {
        }
    }
}