using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FollowingService.Auth
{
    public interface IAuthorization
    {
        string GenerateToken(string username, int expireMinutes=60);
        bool ValidateToken(string token, out string username);

    }
}
