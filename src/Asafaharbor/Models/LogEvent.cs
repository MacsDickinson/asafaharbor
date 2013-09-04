﻿using System;
using System.Web.Management;

namespace Asafaharbor.Web.Models
{
    public class LogEvent : WebRequestErrorEvent
    {
        public LogEvent(string message)
            : base(null, null, 100001, new Exception(message))
        {
        }
    }
}