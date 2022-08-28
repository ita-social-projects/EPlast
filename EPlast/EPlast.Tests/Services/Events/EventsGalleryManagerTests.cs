using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Events;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Services.Events;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Moq;
using NUnit.Framework;


namespace EPlast.Tests.Services
{
    class EventsGalleryManagerTests
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IEventBlobStorageRepository> _blobStorage;
        private EventGalleryManager _galleryManager;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _blobStorage = new Mock<IEventBlobStorageRepository>();
            _galleryManager = new EventGalleryManager(_repoWrapper.Object, _blobStorage.Object);
        }

        [Test]
        public async Task GetPictureByIdAsync_PictureExists_ReturnsPicture()
        {
            // Arrange
            int pictureId = 1;
            string base64data = "SSBsb3ZlIHVuaXQgdGVzdGluZyE=";

            _repoWrapper
                .Setup(x => x.Gallary.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Gallary, bool>>>(), null))
                .ReturnsAsync(new Gallary() { ID = pictureId });

            _blobStorage
                .Setup(x => x.GetBlobBase64Async(It.IsAny<string>()))
                .ReturnsAsync(base64data);

            // Act
            var result = await _galleryManager.GetPictureByIdAsync(pictureId);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<EventGalleryDto>(result);
            Assert.AreEqual(pictureId, result.GalleryId);
        }

        [Test]
        public async Task GetPictureByIdAsync_PictureDoesntExist_ReturnsNull()
        {
            // Arrange
            int pictureId = 1;

            _repoWrapper
                .Setup(x => x.Gallary.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Gallary, bool>>>(), null))
                .ReturnsAsync((Gallary)null);

            // Act
            var result = await _galleryManager.GetPictureByIdAsync(pictureId);

            // Assert
            Assert.Null(result);
        }
    }
}
