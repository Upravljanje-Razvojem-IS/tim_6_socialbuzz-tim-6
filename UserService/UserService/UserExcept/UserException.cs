using System;

namespace UserService.UserExcept
{
    public class UserException : Exception
    {
        public int StatusCode { get; set; }
        public UserException(string message, int statusCode = 500) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
