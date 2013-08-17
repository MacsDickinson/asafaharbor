using System;
using Nancy.Responses;
using Raven.Imports.Newtonsoft.Json;
using HttpStatusCode = Nancy.HttpStatusCode;

namespace Asafaharbor.Web.Responses
{
    public class ErrorResponse : JsonResponse
    {
        readonly Error _error;

        private ErrorResponse(Error error)
            : base(error, new DefaultJsonSerializer())
        {
            _error = error;
        }

        public string ErrorMessage { get { return _error.ErrorMessage; } }
        public string FullException { get { return _error.FullException; } }
        public string[] Errors { get { return _error.Errors; } }

        public static ErrorResponse FromMessage(string message)
        {
            return new ErrorResponse(new Error { ErrorMessage = message });
        }

        public static ErrorResponse FromException(Exception exception)
        {
            var error = new Error { ErrorMessage = exception.Message, FullException = exception.ToString() };

            return new ErrorResponse(error)
                {
                    StatusCode = HttpStatusCode.InternalServerError
                };
        }

        class Error
        {
            public string ErrorMessage { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string FullException { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string[] Errors { get; set; }
        }
    }
}