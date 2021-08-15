using Microsoft.Extensions.Logging;
using System;

namespace BlockingService.Logger
{
    public interface IFakeLogger
    {
        void Log(LogLevel logLevel, string requestId, string previousRequestId, string message, Exception exception);
    }
}
