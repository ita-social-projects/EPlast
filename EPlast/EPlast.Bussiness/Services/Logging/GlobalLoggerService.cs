using EPlast.BusinessLogicLayer.Interfaces.Logging;
using NLog;
using System;

namespace EPlast.BusinessLogicLayer.Services.Logging
{
    public class GlobalLoggerService : IGlobalLoggerService
    {
        private readonly ILogger _logger = LogManager.GetLogger("Global Exception Handling");
        public void LogError(Exception ex)
        {
            _logger.Error($"Something went wrong: {ex}");
        }
    }
}
