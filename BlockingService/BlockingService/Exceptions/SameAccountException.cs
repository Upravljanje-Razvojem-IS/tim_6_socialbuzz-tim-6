using System;
namespace BlockingService.Exceptions
{
    public class SameAccountException :Exception
    {
        public SameAccountException(string message)
           : base(message)
        {

        }
        public SameAccountException()
        {

        }
    }
}
