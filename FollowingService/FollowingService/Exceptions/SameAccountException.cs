using System;
using System.Runtime.Serialization;

namespace FollowingService.Exceptions
{
    public class SameAccountException :Exception
    {
        public SameAccountException(string message)
           : base(message)
        {

        }

        protected SameAccountException(SerializationInfo info, StreamingContext context)
          : base(info, context)
        {

        }
        public SameAccountException()
        {

        }
    }
}
