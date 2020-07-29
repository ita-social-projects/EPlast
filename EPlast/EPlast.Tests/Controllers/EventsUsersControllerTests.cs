using EPlast.BLL.DTO.EventUser;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.Tests.Controllers
{
    [TestFixture]
    class EventsUsersControllerTests
    {
        private Mock<IEventUserManager> _actionManager;

        private EventsUsersController _eventsUsersController;

        [SetUp]
        public void SetUp()
        {
            _actionManager = new Mock<IEventUserManager>();

            _eventsUsersController = new EventsUsersController(
                _actionManager.Object);
        }

        [Test]
        public async Task GetEventUserByUserId_ReturnsOkObjectResult()
        {
            // Arrange

            _actionManager
                .Setup((x) => x.EventUserAsync(It.IsAny<string>(), It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new EventUserDTO());

            // Act

            var result = await _eventsUsersController.GetEventUserByUserId(It.IsAny<string>());

            // Assert

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetEventUserByUserId_ActionManagerEventUserAsyncThrowsException_ReturnsBadRequestResult()
        {
            // Arrange

            _actionManager
                .Setup((x) => x.EventUserAsync(It.IsAny<string>(), It.IsAny<ClaimsPrincipal>()))
                .ThrowsAsync(new Exception());

            // Act

            var result = await _eventsUsersController.GetEventUserByUserId(It.IsAny<string>());

            // Assert

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task GetEventsDataForCreate_InitializeEventCreateDTOAsyncReturnsEventCreateDTOWithIdOne_ReturnsReturnsEventCreateDTOWithIdOne()
        {
            // Arrange

            var id = 1;

            _actionManager
                .Setup((x) => x.InitializeEventCreateDTOAsync())
                .ReturnsAsync(new EventCreateDTO() { Event = new EventCreationDTO() { ID = id, }, });

            var expected = id;

            // Act

            var result = await _eventsUsersController.GetEventsDataForCreate();

            var actual = ((result as ObjectResult).Value as EventCreateDTO).Event.ID;

            // Assert

            Assert.AreEqual(expected, actual);
        }
    }
}
