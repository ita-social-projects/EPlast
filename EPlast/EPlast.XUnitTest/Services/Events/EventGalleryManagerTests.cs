using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Events;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Services.Events;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Xunit;

namespace EPlast.XUnitTest.Services.Events
{
    public class EventGalleryManagerTests
    {
        private readonly Mock<IRepositoryWrapper> _repoWrapper;
        private readonly Mock<IEventBlobStorageRepository> _eventBlobStorage;

        public EventGalleryManagerTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _eventBlobStorage = new Mock<IEventBlobStorageRepository>();
        }

        [Fact]
        public async Task DeletePictureSuccessTest()
        {
            //Arrange
            int eventId = 145;
            _repoWrapper.Setup(x =>
                    x.Gallary.GetFirstAsync(It.IsAny<Expression<Func<Gallary, bool>>>(), null))
                .ReturnsAsync(new Gallary { ID = 2, GalaryFileName = "picture.jpj" });
            _eventBlobStorage.Setup(x => x.DeleteBlobAsync(It.IsAny<string>()));
            //Act
            var eventGalleryManager = new EventGalleryManager(
                _repoWrapper.Object,
                _eventBlobStorage.Object
            );
            var methodResult = await eventGalleryManager.DeletePictureAsync(eventId);
            //Assert
            _repoWrapper.Verify(r => r.Gallary.Delete(It.IsAny<Gallary>()), Times.Once());
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once());
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }

        [Fact]
        public async Task DeletePictureFailTest()
        {
            //Arrange
            int eventId = 145;
            _repoWrapper.Setup(x =>
                    x.Gallary.GetFirstAsync(It.IsAny<Expression<Func<Gallary, bool>>>(), null))
                .ThrowsAsync(new Exception());
            _eventBlobStorage.Setup(x => x.DeleteBlobAsync(It.IsAny<string>()));
            //Act
            var eventGalleryManager = new EventGalleryManager(
                _repoWrapper.Object,
                _eventBlobStorage.Object
            );
            var methodResult = await eventGalleryManager.DeletePictureAsync(eventId);
            //Assert
            Assert.Equal(StatusCodes.Status400BadRequest, methodResult);
        }

        [Fact]
        public async Task GetPicturesInBase64Test()
        {
            //Arrange
            int eventId = 145;
            var picture = "StringInBase64";
            _repoWrapper.Setup(x =>
                    x.EventGallary.GetAllAsync(It.IsAny<Expression<Func<EventGallary, bool>>>(), It.IsAny<Func<IQueryable<EventGallary>, IIncludableQueryable<EventGallary, object>>>()))
                .ReturnsAsync(GetEventsGallaries());
            _eventBlobStorage.Setup(x => x.GetBlobBase64Async(It.IsAny<string>()))
                .ReturnsAsync(picture);
            //Act
            var eventGalleryManager = new EventGalleryManager(
                _repoWrapper.Object,
                _eventBlobStorage.Object
            );
            var methodResult = await eventGalleryManager.GetPicturesInBase64(eventId);
            //Assert
            Assert.NotNull(methodResult);
            Assert.IsType<List<EventGalleryDto>>(methodResult);
            Assert.Equal(GetEventsGallaries().Count(), methodResult.ToList().Count);
        }

        [Fact]
        public async Task FillEventGalleryTest()
        {
            //Arrange
            int eventId = 145;
            var picture = "StringInBase64";
            _repoWrapper.Setup(x => x.Gallary.CreateAsync((It.IsAny<Gallary>())));
            _repoWrapper.Setup(x => x.EventGallary.CreateAsync((It.IsAny<EventGallary>())));
            _eventBlobStorage.Setup(x => x.UploadBlobAsync(It.IsAny<IFormFile>(), It.IsAny<string>()));
            _eventBlobStorage.Setup(x => x.GetBlobBase64Async(It.IsAny<string>()))
                .ReturnsAsync(picture);
            //Act  
            var eventGalleryManager = new EventGalleryManager(
                _repoWrapper.Object,
                _eventBlobStorage.Object
            );
            var methodResult = await eventGalleryManager.AddPicturesAsync(eventId, FakeFiles());
            //Assert
            Assert.NotNull(methodResult);
            Assert.IsType<List<EventGalleryDto>>(methodResult);
        }

        public static IList<IFormFile> FakeFiles()
        {
            var fileMock = new Mock<IFormFile>();
            Icon icon1 = new Icon(SystemIcons.Exclamation, 40, 40);
            var content = icon1.ToBitmap();
            var fileName = "picture.png";
            var ms = new MemoryStream();
            content.Save(ms, ImageFormat.Png);
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            var arr = new[] { fileMock.Object, fileMock.Object };
            return arr;
        }

        public IQueryable<EventGallary> GetEventsGallaries()
        {
            var eventsGallaries = new List<EventGallary>
            {
                new EventGallary{
                    EventID=145,
                    GallaryID=145,
                    Gallary=new Gallary{ID=145,GalaryFileName="FileName"}
                }
            }.AsQueryable();
            return eventsGallaries;
        }
    }

}
