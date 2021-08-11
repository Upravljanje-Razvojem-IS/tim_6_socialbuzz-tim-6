using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FollowingService.Logger
{
    public interface IFakeLogger
    {
        void Log(LogLevel logLevel, string requestId, string previousRequestId, string message, Exception exception);
    }
}
