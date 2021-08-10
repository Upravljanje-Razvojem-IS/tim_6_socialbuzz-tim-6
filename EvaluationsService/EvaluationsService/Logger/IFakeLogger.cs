using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaluationsService.Logger
{
    public interface IFakeLogger
    {
        public void Log(LogLevel logLevel, string requestId, string previousRequestId, string message, Exception exception);
    }
}
