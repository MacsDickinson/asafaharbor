using System;
using System.Collections.Generic;
using Asafaharbor.Web.Models;
using Nancy;

namespace Asafaharbor.Web.Modules
{
    public class ErrorModule : BaseModule
    {
        public ErrorModule()
            : base("/error")
        {
            Get["/400"] = parameters => HttpStatusCode.BadRequest;
            Get["/404"] = parameters => HttpStatusCode.NotFound;
            Get["/500"] = parameters =>
                {
                    throw new Exception("Kaboom! It's all broke!!!");
                };
        }
    }
}