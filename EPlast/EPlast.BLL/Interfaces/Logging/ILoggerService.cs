using Microsoft.Extensions.Logging;

namespace EPlast.BLL.Interfaces.Logging
{
    public interface ILoggerService<out T>
    {
        ILogger<T> Logger { get; }
        void LogInformation(string msg);
        void LogWarning(string msg);
        void LogTrace(string msg);
        void LogDebug(string msg);
        void LogError(string msg);
    }
}
