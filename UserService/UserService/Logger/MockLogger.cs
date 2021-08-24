using Microsoft.Extensions.Logging;

namespace UserService.Logger
{
    public class MockLogger
    {
        private readonly ILogger<MockLogger> _logger;

        public MockLogger(ILogger<MockLogger> logger)
        {
            _logger = logger;
        }

        public void Log(string message)
        {
            _logger.LogInformation(message);
        }
    }
}
