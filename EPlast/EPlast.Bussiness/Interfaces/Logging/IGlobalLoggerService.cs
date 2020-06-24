using System;

namespace EPlast.Bussiness.Interfaces.Logging
{
    public interface IGlobalLoggerService
    {
        void LogError(Exception ex);
    }
}
