using System;

namespace ReportingService.ExceptionReport
{
    public class ReportException : Exception
    {
        public int StatusCode { get; set; }
        public ReportException(string message, int statusCode = 500) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
