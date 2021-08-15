using System;

namespace FollowingService.Exceptions
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
