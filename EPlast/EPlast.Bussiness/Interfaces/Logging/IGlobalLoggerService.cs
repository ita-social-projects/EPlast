using System;

namespace EPlast.BusinessLogicLayer.Interfaces.Logging
{
    public interface IGlobalLoggerService
    {
        void LogError(Exception ex);
    }
}
