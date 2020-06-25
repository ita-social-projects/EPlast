using System;

namespace EPlast.BLL.Interfaces.Logging
{
    public interface IGlobalLoggerService
    {
        void LogError(Exception ex);
    }
}
