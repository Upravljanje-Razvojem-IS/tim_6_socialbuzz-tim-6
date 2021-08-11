using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FollowingService.Exceptions
{
    public class SameAccountException :Exception
    {
        public SameAccountException(string message)
           : base(message)
        {

        }
    }
}
