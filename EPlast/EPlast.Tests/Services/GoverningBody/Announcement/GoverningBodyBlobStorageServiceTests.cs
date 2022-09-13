using System.Threading.Tasks;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Services.GoverningBodies.Announcement;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services.GoverningBody.Announcement
{
    internal class GoverningBodyBlobStorageServiceTests
    {
        private Mock<IGoverningBodyBlobStorageRepository> _blobStorage;
        private GoverningBodyBlobStorageService _governingBodyBlobStorageService;

        [SetUp]
        public void SetUp()
        {
            _blobStorage = new Mock<IGoverningBodyBlobStorageRepository>();
            _governingBodyBlobStorageService = new GoverningBodyBlobStorageService(
                _blobStorage.Object
            );
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
            string fileName = "fileName/filepath,extention";
            //Act
            var result = await _governingBodyBlobStorageService.UploadImageAsync(fileName);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<string>(result);
        }


    }
}
