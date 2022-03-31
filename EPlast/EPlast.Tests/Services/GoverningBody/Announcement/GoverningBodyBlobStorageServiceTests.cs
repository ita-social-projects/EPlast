using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Services.GoverningBodies.Announcement;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.Tests.Services.GoverningBody.Announcement
{
    internal class GoverningBodyBlobStorageServiceTests
    {
        private Mock<IGoverningBodyBlobStorageRepository> _blobStorage;
        private Mock<IUniqueIdService> _uniqueId;
        private GoverningBodyBlobStorageService _governingBodyBlobStorageService;

        [SetUp]
        public void SetUp()
        {
            _blobStorage = new Mock<IGoverningBodyBlobStorageRepository>();
            _uniqueId = new Mock<IUniqueIdService>();
            _governingBodyBlobStorageService = new GoverningBodyBlobStorageService(
                _blobStorage.Object,
                _uniqueId.Object);
        }

        [Test]
        public async Task GetImageAsync__Valid()
        {
            //Arrange
            _blobStorage.Setup(x => x.GetBlobBase64Async(It.IsAny<string>())).ReturnsAsync("success");
            string fileName = "fileName";
            //Act
            var result = await _governingBodyBlobStorageService.GetImageAsync(fileName);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<string>(result);
        }

        [Test]
        public async Task UploadImageAsync__Valid()
        {
            //Arrange
            _blobStorage.Setup(x => x.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()));
            _uniqueId.Setup(x => x.GetUniqueId());
            string fileName = "fileName/filepath,extention";
            //Act
            var result = await _governingBodyBlobStorageService.UploadImageAsync(fileName);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<string>(result);
        }


    }
}
