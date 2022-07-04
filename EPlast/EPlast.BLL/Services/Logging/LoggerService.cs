using EPlast.BLL.Interfaces.Logging;
using Microsoft.Extensions.Logging;

namespace EPlast.BLL.Services.Logging
{
    public class LoggerService : ILoggerService
    {
        protected readonly ILogger _logger;
        public LoggerService(ILogger logger)
        {
            _logger = logger;
        }
        public void LogInformation(string msg)
        {
            _logger.Log(LogLevel.Information, $"{msg}");
        }
        public void LogWarning(string msg)
        {
            _logger.Log(LogLevel.Warning, $"{msg}");
        }
        public void LogTrace(string msg)
        {
            _logger.Log(LogLevel.Trace, $"{msg}");
        }
        public void LogDebug(string msg)
        {
            _logger.Log(LogLevel.Debug, $"{msg}");
        }
        public void LogError(string msg)
        {
            _logger.Log(LogLevel.Error, $"{msg}");
        }
    }
}
