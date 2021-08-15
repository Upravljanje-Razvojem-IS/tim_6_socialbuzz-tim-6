using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace BlockingService.Logger
{
    public class FakeLogger :IFakeLogger
    {
        public void Log(LogLevel logLevel, string requestId, string previousRequestId, string message, Exception exception)
        {
            string directory = System.IO.Directory.GetParent(System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString()).ToString() + "\\FakeLogs\\fakeLogs.txt";
            var log = "Blocking Service - " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + " - " + logLevel + " " + requestId + " " + previousRequestId + " " + message + " " + exception;
            using (StreamWriter writer = System.IO.File.AppendText(directory))
            {
                writer.WriteLine(log);
            }
        }
    }
}
