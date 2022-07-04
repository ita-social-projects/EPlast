namespace EPlast.BLL.Interfaces.Logging
{
    public interface ILoggerService
    {
        void LogInformation(string msg);
        void LogWarning(string msg);
        void LogTrace(string msg);
        void LogDebug(string msg);
        void LogError(string msg);
    }
}
