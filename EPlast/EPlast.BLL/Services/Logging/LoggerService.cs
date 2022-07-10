using EPlast.BLL.Interfaces.Logging;
using Microsoft.Extensions.Logging;

namespace EPlast.BLL.Services.Logging
{
    public class LoggerService<T> : ILoggerService<T>
    {
        public ILogger<T> Logger { get; }
        public LoggerService(ILogger<T> logger)
        {
            Logger = logger;
        }
        public void LogInformation(string msg)
        {
            Logger.Log(LogLevel.Information, $"{msg}");
        }
        public void LogWarning(string msg)
        {
            Logger.Log(LogLevel.Warning, $"{msg}");
        }
        public void LogTrace(string msg)
        {
            Logger.Log(LogLevel.Trace, $"{msg}");
        }
        public void LogDebug(string msg)
        {
            Logger.Log(LogLevel.Debug, $"{msg}");
        }
        public void LogError(string msg)
        {
            Logger.Log(LogLevel.Error, $"{msg}");
        }
    }
}
