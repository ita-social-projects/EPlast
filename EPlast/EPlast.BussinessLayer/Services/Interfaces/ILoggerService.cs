﻿namespace EPlast.BussinessLayer.Services.Interfaces
{
    public interface ILoggerService<T>
    {
        void LogInformation(string msg);
        void LogWarning(string msg);
        void LogTrace(string msg);
        void LogDebug(string msg);
        void LogError(string msg);

    }
}
