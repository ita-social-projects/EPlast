using System.Collections.Generic;
using System.Security.Claims;
using EPlast.BLL.DTO.EventUser;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.DataAccess.Entities;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using EPlast.Resources;
using Microsoft.AspNetCore.Http;

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
            Assert.IsInstanceOf<EventUserDTO>(resultValue);
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

            var actual = ((result as ObjectResult).Value as EventUserDTO).User.Id;

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
            Assert.IsInstanceOf<EventCreateDTO>(resultValue);
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

            var actual = ((result as ObjectResult).Value as EventCreateDTO).Event.ID;

            // Assert
            Assert.NotNull((result as ObjectResult).Value);
            Assert.AreEqual(expectedId, actual);
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
            Assert.IsInstanceOf<EventCreateDTO>(resultValue);
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
            Assert.IsInstanceOf<EventCreateDTO>(resultValue);
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
            var actual = ((result as ObjectResult).Value as EventCreateDTO).Event.ID;

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
            Assert.NotNull((result as ObjectResult).Value as EventCreateDTO);
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

            var actual = ((result as ObjectResult).Value as EventCreateDTO).Event.ID;

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

        private EventUserDTO CreateFakeEventUser()
            => new EventUserDTO()
            {
                User = new UserDTO()
                {
                    Id = "1",
                    FirstName = "SomeFirstName",
                    LastName = "SomeLastName",
                }
            };

        private EventCreateDTO CreateFakeEventCreate()
            => new EventCreateDTO()
            {
                Event = new EventCreationDTO()
                {
                    ID = 1,
                },
            };
    }
}
