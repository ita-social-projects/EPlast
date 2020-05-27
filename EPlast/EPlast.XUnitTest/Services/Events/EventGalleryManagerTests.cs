﻿using EPlast.BussinessLayer.Services.Events;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace EPlast.XUnitTest.Services.Events
{
    public class EventGalleryManagerTests
    {
        private readonly Mock<IRepositoryWrapper> _repoWrapper;
        private readonly Mock<IHostingEnvironment> _env;

        public EventGalleryManagerTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _env = new Mock<IHostingEnvironment>();
        }

        [Fact]
        public void DeletePictureSuccessTest()
        {
            //Arrange
            int eventId = 5;
            _repoWrapper.Setup(x => x.Gallary.FindByCondition(It.IsAny<Expression<Func<Gallary, bool>>>()))
                .Returns(new List<Gallary> { new Gallary { ID = 2, GalaryFileName = "picture.jpj" } }.AsQueryable());
            _env.Setup(e => e.WebRootPath).Returns("Webroot\\");
            //Act
            var eventGalleryManager = new EventGalleryManager(_repoWrapper.Object, _env.Object);
            var methodResult = eventGalleryManager.DeletePicture(eventId);
            //Assert
            _repoWrapper.Verify(r => r.Gallary.Delete(It.IsAny<Gallary>()), Times.Once());
            _repoWrapper.Verify(r => r.Save(), Times.Once());
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }

        [Fact]
        public void DeletePictureFailTest()
        {
            //Arrange
            int eventId = 5;
            _repoWrapper.Setup(x => x.Gallary.FindByCondition(It.IsAny<Expression<Func<Gallary, bool>>>()))
                .Throws(new Exception());
            _env.Setup(e => e.WebRootPath).Returns("Webroot\\");
            //Act
            var eventGalleryManager = new EventGalleryManager(_repoWrapper.Object, _env.Object);
            var methodResult = eventGalleryManager.DeletePicture(eventId);
            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, methodResult);
        }

        [Fact]
        public void FillEventGalleryTest()
        {
            //Arrange
            int eventId = 5;
            _repoWrapper.Setup(x => x.Gallary.Create((It.IsAny<Gallary>())));
            _repoWrapper.Setup(x => x.EventGallary.Create((It.IsAny<EventGallary>())));
            _env.Setup(e => e.WebRootPath).Returns("Webroot\\");
            //Act  
            var eventGalleryManager = new EventGalleryManager(_repoWrapper.Object,_env.Object);
            var methodResult = eventGalleryManager.AddPictures(eventId, FakeFiles());
            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, methodResult);
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
    }

}
