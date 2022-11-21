using EPlast.BLL.Interfaces.AzureStorage.Base;
using EPlast.BLL.Services.AzureStorage;
using EPlast.BLL.Services.Logging;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace EPlast.XUnitTest.Services.Logging
{
    public class LoggerServiceTests
    {
        private readonly Mock<ILogger<LoggerServiceTests>> _Logger;
   
        public LoggerServiceTests()
        {
           _Logger = new Mock<ILogger<LoggerServiceTests>>();
       
        }
       
        [Fact]
        public void LogInformationTest()
        {
            // Arrange
            var service = new LoggerService<LoggerServiceTests>(_Logger.Object);

            // Act
            service.LogInformation("LogInformation");

            // Assert
            _Logger.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
          
        }

        [Fact]
        public void LogWarningTest()
        {
            // Arrange
            var service = new LoggerService<LoggerServiceTests>(_Logger.Object);

            // Act
            service.LogWarning("LogWarning");

            // Assert
            _Logger.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));

        }
        [Fact]
        public void LogTraceTest()
        {
            // Arrange
            var service = new LoggerService<LoggerServiceTests>(_Logger.Object);

            // Act
            service.LogTrace("LogTrace");

            // Assert
            _Logger.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }
        [Fact]
        public void LogDebugTest()
        {
            // Arrange
            var service = new LoggerService<LoggerServiceTests>(_Logger.Object);

            // Act
            service.LogDebug("LogDebug");

            // Assert
            _Logger.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }
        [Fact]
        public void LogErrorTest()
        {
            // Arrange
            var service = new LoggerService<LoggerServiceTests>(_Logger.Object);

            // Act
            service.LogError("LogError");

            // Assert
            _Logger.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }

    }
}
