using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EPlast.BLL;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace EPlast.XUnitTest.Services.FileManager
{
    public class FileStreamManagerTests
    {
        private Mock<IFileStreamManager> mockedFileStreamManager;
        private IFileStreamManager fileStreamManager;

        public FileStreamManagerTests()
        {
            mockedFileStreamManager = new Mock<IFileStreamManager>();
            fileStreamManager = new FileStreamManager();
        }

        public bool ExceptionHandlerMethod(Action action)
        {
            bool exceptionOccured = false;
            try
            {
                action();
            }
            catch
            {
                exceptionOccured = true;
            }
            return exceptionOccured;
        }


        [Fact]
        public void GetStreamTest_ReturnsNull()
        { 
            // Arrange
            mockedFileStreamManager.Setup(m => m.GetStream()).Returns(It.IsAny<Stream>());

            // Act
            var streamInstance = mockedFileStreamManager.Object.GetStream();

            // Assert 
            Assert.Null(streamInstance);
        }

        [Fact]
        public void GenerateFileStreamManagerTest_ReturnsNewFileStreamManagerInstance()
        {
            // Arrange
            const string anyFilePath = "TestPath";
            const FileMode anyFileMode = FileMode.Open;
 
            mockedFileStreamManager.Setup(x => x.GenerateFileStreamManager(anyFilePath, anyFileMode)).Returns(It.IsAny<FileStreamManager>());

            // Act
            Action actionToAssert = () => mockedFileStreamManager.Object.GenerateFileStreamManager(anyFilePath, anyFileMode);
            bool exceptionOccured=ExceptionHandlerMethod(actionToAssert);

            // Assert
            Assert.False(exceptionOccured);
        }

        [Fact]
        public void CopyToAsyncTest_MemoryStream_DoesNotThrowException()
        {
            // Arrange
            MemoryStream anyMemoryStream = null;

            // Act
            Action action = () => fileStreamManager.CopyToAsync(anyMemoryStream);           
            bool exceptionOccured = ExceptionHandlerMethod(action);

            // Assert
            Assert.False(exceptionOccured);
        }

        [Fact]
        public void CopyToAsyncTest_IFormFileStream_DoesNotThrowException()
        {
            // Arrange
            IFormFile anyFormFile = null;
            Stream anyStream = null;

            // Act
            Action action = () => fileStreamManager.CopyToAsync(anyFormFile, anyStream);
            bool exceptionOccured = ExceptionHandlerMethod(action);

            // Assert
            Assert.False(exceptionOccured);
        }

        [Fact]
        public void CopyToAsyncTest_Streams_DoesNotThrowException()
        {
            // Arrange
            Stream anyInputStream = null;
            Stream anyOutputStream = null;

            // Act
            Action action = () => fileStreamManager.CopyToAsync(anyInputStream, anyOutputStream);
            bool exceptionOccured = ExceptionHandlerMethod(action);

            // Assert
            Assert.False(exceptionOccured);
        }
    }
}
