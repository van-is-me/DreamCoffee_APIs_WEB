using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class HttpResponseException : Exception
    {
        public int StatusCode { get; }

        public HttpResponseException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
