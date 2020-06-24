using System;

namespace EPlast.BussinessLayer.Interfaces.Logging
{
    public interface IGlobalLoggerService
    {
        void LogError(Exception ex);
    }
}
