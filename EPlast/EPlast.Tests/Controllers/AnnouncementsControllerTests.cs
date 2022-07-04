using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EPlast.BLL.DTO.GoverningBody.Announcement;
using EPlast.BLL.Interfaces.Announcements;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Controllers
{
    internal class AnnouncementsControllerTests
    {
        private Mock<IAnnouncemetsService> _announcemetsService;

        private AnnouncementsController _announcemetsController;


        [SetUp]
        public void SetUp()
        {
            _announcemetsService = new Mock<IAnnouncemetsService>();
            _announcemetsController = new AnnouncementsController(
               _announcemetsService.Object
            );
        }

        [Test]
        public async Task GetById_Valid()
        {
            //Arrange
            _announcemetsService.Setup(g => g.GetAnnouncementByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new GoverningBodyAnnouncementUserWithImagesDto());

            //Act
            var result = await _announcemetsController.GetById(It.IsAny<int>());
            var resultValue = (result as ObjectResult).Value;

            //Assert
            _announcemetsService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<GoverningBodyAnnouncementUserWithImagesDto>(resultValue);
        }

        [Test]
        public async Task GetById_ReturnNoContent()
        {
            //Arrange
            _announcemetsService.Setup(g => g.GetAnnouncementByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(null as GoverningBodyAnnouncementUserWithImagesDto);

            //Act
            var result = await _announcemetsController.GetById(It.IsAny<int>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task PinAnnouncementAsync_Invalid()
        {
            //Arrange
            _announcemetsService.Setup(g => g.PinAnnouncementAsync(It.IsAny<int>()));

            //Act
            var result = await _announcemetsController.PinAnnouncement(It.IsAny<int>());

            //Assert
            _announcemetsService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [TestCase(1, 5, 1)]
        public async Task GetAnnouncementsByPage_Valid(int page, int pageSize, int governingBodyId)
        {
            //Arrange
            _announcemetsService.Setup(g => g.GetAnnouncementsByPageAsync(page, pageSize));

            //Act
            var result = await _announcemetsController.GetAnnouncementsByPage(page, pageSize);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
    }
}
