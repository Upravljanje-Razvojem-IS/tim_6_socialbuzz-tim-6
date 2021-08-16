using System;
using System.Runtime.Serialization;

namespace FollowingService.Exceptions
{
    [Serializable]
    public class SameAccountException :Exception
    {

        public SameAccountException(string message)
           : base(message)
        {

        }
        public SameAccountException()
        {

        }
        public SameAccountException(string message, Exception innerException)
           : base(message, innerException)
        {
        }

        // Without this constructor, deserialization will fail
        protected SameAccountException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
