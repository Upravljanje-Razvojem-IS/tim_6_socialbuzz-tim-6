using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Exceptions
{
    [Serializable]
    public class ForeignKeyConstraintException : Exception
    {
        public ForeignKeyConstraintException(string message) 
            : base(message)
        {

        }
        public ForeignKeyConstraintException(string message, Exception inner) 
            : base(message, inner)
        {

        }

    }
}
