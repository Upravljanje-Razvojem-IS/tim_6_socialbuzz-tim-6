using System;

namespace SasaMessagingService.CustomException
{
    public class BussinessException : Exception
    {
        public int StatusCode { get; set; }
        public BussinessException(string message, int statusCode = 500) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
