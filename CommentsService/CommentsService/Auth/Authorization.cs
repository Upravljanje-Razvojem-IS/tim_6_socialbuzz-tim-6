using CommentsService.Auth;
using CommentsService.Logger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaluationsService.Auth
{
    public class Authorization : IAuthorization
    {
        private readonly IConfiguration configuration;
        private readonly IFakeLogger logger;


        public Authorization(IConfiguration configuration, IFakeLogger logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }
        public bool Authorize(string key)
        {
            if (key == null)
            {
                logger.Log(LogLevel.Error, "", "", "User not authorized, authorization missing in header.", null);
                return false;
            }

            if (!key.StartsWith("Bearer"))
            {
                logger.Log(LogLevel.Error, "", "", "User not authorized, Bearer missing in header.", null);
                return false;
            }

            try
            {
                var keyOnly = key.Substring(key.IndexOf("Bearer") + 7);
                var username = configuration.GetValue<string>("Authorization:Username");
                var password = configuration.GetValue<string>("Authorization:Password");
                var base64EncodedBytes = System.Convert.FromBase64String(keyOnly);
                var user = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

                if ((username + ":" + password) != user)
                {
                    logger.Log(LogLevel.Error, "", "", "User not authorized, wrong credentials.", null);
                    return false;
                }

                logger.Log(LogLevel.Information, "", "", String.Format("User {0} succesfully authorized.", username), null);
                return true;
            }
            catch (System.FormatException ex)
            {
                logger.Log(LogLevel.Error, "", "", String.Format("User not authorized, message: {0}", ex.Message), null);
                return false;
            }
        }
    }
}
