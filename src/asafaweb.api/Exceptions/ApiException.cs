using System;

namespace asafaweb.console.Exceptions
{
    public class ApiException : Exception
    {
        public ApiException()
        {
        }

        public ApiException(string message)
            : base(message)
        {
        }
    }
}
