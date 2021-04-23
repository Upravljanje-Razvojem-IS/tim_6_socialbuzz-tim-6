using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaluationsService.Auth
{
    public class Authorization
    {
        public static bool Authorize(string key, IConfiguration configuration)
        {
            if (key == null)
            {
                return false;
            }

            if (!key.StartsWith("Bearer"))
            {
                return false;
            }

            var keyOnly = key.Substring(key.IndexOf("Bearer") + 7);
            var username = configuration.GetValue<string>("Authorization:Username");
            var password = configuration.GetValue<string>("Authorization:Password");
            var base64EncodedBytes = System.Convert.FromBase64String(keyOnly);
            var user = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

            if ((username + ":" + password) != user)
            {
                return false;
            }

            return true;
        }
    }
}
