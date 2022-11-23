using EPlast.BLL.Interfaces.AzureStorage.Base;
using EPlast.BLL.Services.AzureStorage;
using EPlast.BLL.Services.AzureStorage.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage.Blob;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EPlast.XUnitTest.Services.AzureStorage.Base
{
    public class BlobStorageRepositoryTests
    {
        private readonly Mock<IAzureBlobConnectionFactory> _connectionFactory;
        

        public BlobStorageRepositoryTests()
        {
            _connectionFactory = new Mock<IAzureBlobConnectionFactory>();
        }
       

        [Fact]
        public async Task GetBlobTest()
        {
            // Arrange
            _connectionFactory.Setup(c => c.GetBlobContainer(It.IsAny<string>())).ReturnsAsync(new CloudBlobContainer(new Uri("https://www.google.com")));
            var service = new AboutBaseBlobStorageRepository(_connectionFactory.Object);
            const string nameKey = "NameKey";
            // Act
            var result = await service.GetBlobAsync(nameKey);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<CloudBlockBlob>(result);
        }

        [Fact]
        public async Task GetBlobBase64Test()
        {
            // Arrange
            _connectionFactory.Setup(c => c.GetBlobContainer(It.IsAny<string>())).ReturnsAsync(new CloudBlobContainer(new Uri("https://www.google.com")));
            var service = new AboutBaseBlobStorageRepository(_connectionFactory.Object);
            const string blobName = "blobName";
            // Act
            var result = service.GetBlobBase64Async(blobName);
            // Assert
            await Assert.ThrowsAsync<Microsoft.Azure.Storage.StorageException>( () => result);
          
        }

        [Fact]
        public async Task DeleteBlobTest()
        {
            // Arrange
            _connectionFactory.Setup(c => c.GetBlobContainer(It.IsAny<string>())).ReturnsAsync(new CloudBlobContainer(new Uri("https://www.google.com")));
            var service = new AboutBaseBlobStorageRepository(_connectionFactory.Object);
            const string blobName = "blobName";
            // Act
            await service.DeleteBlobAsync(blobName);
            // Assert
            _connectionFactory.Verify(r => r.GetBlobContainer(It.IsAny<string>()));
           
          
        }
 
        [Fact]
        public async Task UploadBlobTest()
        {
            // Arrange
            _connectionFactory.Setup(c => c.GetBlobContainer(It.IsAny<string>())).ReturnsAsync(new CloudBlobContainer(new Uri("https://www.google.com")));
            var service = new AboutBaseBlobStorageRepository(_connectionFactory.Object);
            var mockFile = new Mock<IFormFile>();
            const string fileName = "picture.png";
            // Act
            var ms = new MemoryStream();
            mockFile.Setup(_ => _.OpenReadStream()).Returns(ms);
            mockFile.Setup(_ => _.FileName).Returns(fileName);
            mockFile.Setup(_ => _.Length).Returns(ms.Length);
            // Assert
            await Assert.ThrowsAsync<Microsoft.Azure.Storage.StorageException>(() => service.UploadBlobAsync(mockFile.Object, "blobName"));
        }
        [Fact]
        public async Task UploadBlobForBase64Test()
        {
            //Arrange
            _connectionFactory.Setup(c => c.GetBlobContainer(It.IsAny<string>())).ReturnsAsync(new CloudBlobContainer(new Uri("https://www.google.com")));
            var service = new AboutBaseBlobStorageRepository(_connectionFactory.Object);
            const string blobForBase64 = "qwertyuiopqwertyuiopqwertyuiopqwertyuiopqwertyuiopqwertyuiopqwer";
            // Act
            var result = service.UploadBlobForBase64Async(blobForBase64, "blobName");
            // Assert
            await Assert.ThrowsAsync<Microsoft.Azure.Storage.StorageException>(() => result);

        }
       
    }
}
