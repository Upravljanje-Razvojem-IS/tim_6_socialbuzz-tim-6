using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Exceptions
{
    public class ReferentialConstraintException : Exception
    {
        public ReferentialConstraintException()
        {

        }

        public ReferentialConstraintException(string message)
            : base(message)
        {

        }

        public ReferentialConstraintException(string message, Exception inner)
             : base(message, inner)
        {

        }
    }
}
