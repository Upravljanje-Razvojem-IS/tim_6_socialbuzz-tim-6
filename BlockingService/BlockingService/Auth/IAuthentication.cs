using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockingService.Auth
{
    public interface IAuthentication
    {
        string GenerateToken(string username, int expireMinutes = 60);
        bool ValidateToken(string token, out string username);
    }
}
