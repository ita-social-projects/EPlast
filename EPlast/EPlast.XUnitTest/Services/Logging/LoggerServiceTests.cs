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
        private const string LogText = "0";
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
            service.LogInformation(LogText);

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
            service.LogWarning(LogText);

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
            service.LogTrace(LogText);

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
            service.LogDebug(LogText);

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
            service.LogError(LogText);

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
