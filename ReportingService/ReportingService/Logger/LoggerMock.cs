using Microsoft.Extensions.Logging;

namespace ReportingService.Logger
{
    public class LoggerMock
    {
        private readonly ILogger<LoggerMock> _logger;

        public LoggerMock(ILogger<LoggerMock> logger)
        {
            _logger = logger;
        }

        public void Log(string message)
        {
            _logger.LogInformation(message);
        }
    }
}
