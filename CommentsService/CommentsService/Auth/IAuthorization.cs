using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentsService.Auth
{
    public interface IAuthorization
    {
        bool Authorize(string key);
    }
}
