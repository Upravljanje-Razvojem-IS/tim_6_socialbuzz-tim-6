using LoggingClassLibrary;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;


namespace EvaluationsService.FakeLoggerService
{
    public class FakeLogger : Logger
    {
        public FakeLogger(IConfiguration configuration) : base(configuration)
        {
        }

        public override void Log(LogLevel logLevel, string requestId, string previousRequestId, string message, Exception exception)
        {
            string directory = System.IO.Directory.GetParent(System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString()).ToString() + "\\FakeLogs\\logs.txt";
            var log = "Evaluations Service - " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + " - " + logLevel + " " + requestId + " " + previousRequestId + " " + message + " " + exception;
            using (StreamWriter writer = System.IO.File.AppendText(directory))
            {
                writer.WriteLine(log);
            }
        }
    }
}
