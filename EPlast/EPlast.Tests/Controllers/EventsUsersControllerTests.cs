using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EPlast.BLL.DTO.EventUser;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Controllers
{
    [TestFixture]
    internal class EventsUsersControllerTests
    {
        private Mock<IEventUserManager> eventUserManager;
        private Mock<IEventUserService> eventUserService;

        private EventsUsersController eventsUsersController;
        private Mock<UserManager<User>> userManager;

        [SetUp]
        public void SetUp()
        {
            eventUserManager = new Mock<IEventUserManager>();
            eventUserService = new Mock<IEventUserService>();
            var store = new Mock<IUserStore<User>>();
            userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            eventsUsersController = new EventsUsersController(eventUserManager.Object, eventUserService.Object, userManager.Object);
        }

        [Test]
        public async Task GetEventUserByUserId_ReturnsOkObjectResult()
        {
            // Arrange
            userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("1");
            userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>(){Roles.Admin});
            eventUserService
                .Setup((x) => x.EventUserAsync(It.IsAny<string>(), It.IsAny<User>()))
                .ReturnsAsync(CreateFakeEventUser());

            // Act
            var result = await eventsUsersController.GetEventUserByUserId("2");
            var resultValue = (result as ObjectResult).Value;

            // Assert
            Assert.NotNull(resultValue);
            Assert.IsInstanceOf<EventUserDto>(resultValue);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetEventUserByUserId_Returns403Forbidden()
        {
            // Arrange
            userManager.Setup(x=>x.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("1");
            userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>());
            eventUserService
                .Setup((x) => x.EventUserAsync(It.IsAny<string>(), It.IsAny<User>()))
                .ReturnsAsync(CreateFakeEventUser());

            var expected = StatusCodes.Status403Forbidden;

            // Act
            var result = await eventsUsersController.GetEventUserByUserId("2");
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetEventUserByUserId_EventUserWithIdOne_ReturnsEventUserWithIdOne()
        {
            // Arrange
            var expectedId = "1";

            eventUserService
                .Setup((x) => x.EventUserAsync(It.IsAny<string>(), It.IsAny<User>()))
                .ReturnsAsync(CreateFakeEventUser());

            // Act
            var result = await eventsUsersController.GetEventUserByUserId(It.IsAny<string>());

            var actual = ((result as ObjectResult).Value as EventUserDto).User.Id;

            // Assert
            Assert.NotNull((result as ObjectResult).Value);
            Assert.AreEqual(expectedId, actual);
        }

        [Test]
        public async Task GetEventsDataForCreate_ReturnsOkObjectResult()
        {
            // Arrange
            eventUserManager
                .Setup((x) => x.InitializeEventCreateDTOAsync())
                .ReturnsAsync(CreateFakeEventCreate());

            // Act
            var result = await eventsUsersController.GetEventsDataForCreate();
            var resultValue = (result as ObjectResult).Value;

            // Assert
            Assert.NotNull(resultValue);
            Assert.IsInstanceOf<EventCreateDto>(resultValue);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetEventsDataForCreate_EventCreateWithIdOne_ReturnsEventCreateWithIdOne()
        {
            // Arrange
            var expectedId = 1;

            eventUserManager
                .Setup((x) => x.InitializeEventCreateDTOAsync())
                .ReturnsAsync(CreateFakeEventCreate());

            // Act
            var result = await eventsUsersController.GetEventsDataForCreate();

            var actual = ((result as ObjectResult).Value as EventCreateDto).Event.ID;

            // Assert
            Assert.NotNull((result as ObjectResult).Value);
            Assert.AreEqual(expectedId, actual);
        }
        
        [Test]
        public async Task EventCreate_Returns400BadRequest()
        {
            // Arrange
            eventUserManager
                .Setup((x) => x.CreateEventAsync(It.IsAny<EventCreateDto>()))
                .Throws(new InvalidOperationException());

            // Act
            var result = await eventsUsersController.EventCreate(CreateFakeEventCreateDates());
            var expected = StatusCodes.Status400BadRequest;
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public async Task EventCreate_ReturnNullReference()
        {
            // Arrange
            eventUserManager
                .Setup((x) => x.CreateEventAsync(null))
                .ReturnsAsync(null);

            // Act
            var result = await eventsUsersController.EventCreate(CreateFakeEventCreate());
            var resultValue = (result as CreatedResult).Value;

            // Assert
            Assert.IsInstanceOf<EventCreateDto>(resultValue);
            Assert.IsInstanceOf<CreatedResult>(result);
        }

        [Test]
        public async Task EventCreate_ReturnsCreatedResult()
        {
            // Arrange
            eventUserManager
                .Setup((x) => x.CreateEventAsync(CreateFakeEventCreate()))
                .ReturnsAsync(It.IsAny<int>());

            // Act
            var result = await eventsUsersController.EventCreate(CreateFakeEventCreate());
            var resultValue = (result as CreatedResult).Value;

            // Assert
            Assert.NotNull(resultValue);
            Assert.IsInstanceOf<EventCreateDto>(resultValue);
            Assert.IsInstanceOf<CreatedResult>(result);
        }

        [Test]
        public async Task EventCreate_EventCreateWithIdOne_ReturnsEventCreateWithIdOne()
        {
            // Arrange
            var expectedId = 1;
            var FakeEvent = CreateFakeEventCreate();
            FakeEvent.Event.ID = 0;

            eventUserManager
                .Setup((x) => x.CreateEventAsync(FakeEvent))
                .ReturnsAsync(expectedId);

            // Act
            var result = await eventsUsersController.EventCreate(FakeEvent);
            var actual = ((result as ObjectResult).Value as EventCreateDto).Event.ID;

            // Assert
            Assert.NotNull((result as ObjectResult).Value);
            Assert.AreEqual(expectedId, actual);
        }

        [Test]
        public async Task EventEdit_ReturnsOkObjectResult()
        {
            // Arrange
            eventUserManager
                .Setup((x) => x.InitializeEventEditDTOAsync(It.IsAny<int>()))
                .ReturnsAsync(CreateFakeEventCreate());

            // Act
            var result = await eventsUsersController.EventEdit(It.IsAny<int>());

            // Assert
            Assert.NotNull((result as ObjectResult).Value as EventCreateDto);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task EventEdit_EventCreateWithIdOne_ReturnsEventCreateWithIdOne()
        {
            // Arrange
            var expectedId = 1;

            eventUserManager
                .Setup((x) => x.InitializeEventEditDTOAsync(It.IsAny<int>()))
                .ReturnsAsync(CreateFakeEventCreate());

            // Act
            var result = await eventsUsersController.EventEdit(It.IsAny<int>());

            var actual = ((result as ObjectResult).Value as EventCreateDto).Event.ID;

            // Assert
            Assert.NotNull((result as ObjectResult).Value);
            Assert.AreEqual(expectedId, actual);
        }

        [Test]
        public async Task EventEdit_EventCreate_ReturnsNoContentResult()
        {
            // Arrange
            eventUserManager
                .Setup((x) => x.EditEventAsync(CreateFakeEventCreate()));

            // Act
            var result = await eventsUsersController.EventEdit(CreateFakeEventCreate());

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [TestCase(2, 1)]
        public async Task ApproveEvent_ReturnsOkObjectResult_Test(int eventId, int expectedId)
        {
            // Arrange
            eventUserManager.Setup(x => x.ApproveEventAsync(It.IsAny<int>()))
                .ReturnsAsync(expectedId);

            // Act
            var result = await eventsUsersController.ApproveEvent(eventId);

            var actual = (result as ObjectResult).Value;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedId, actual);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        private EventUserDto CreateFakeEventUser()
            => new EventUserDto()
            {
                User = new UserDto()
                {
                    Id = "1",
                    FirstName = "SomeFirstName",
                    LastName = "SomeLastName",
                }
            };

        private EventCreateDto CreateFakeEventCreate()
            => new EventCreateDto()
            {
                Event = new EventCreationDto()
                {
                    ID = 1,
                },
            };
        private EventCreateDto CreateFakeEventCreateDates()
            => new EventCreateDto()
            {
                Event = new EventCreationDto
                {
                    EventDateStart = new DateTime(2021, 04, 30), EventDateEnd = new DateTime(2020, 04, 30) 
                },
            };
    }
}
