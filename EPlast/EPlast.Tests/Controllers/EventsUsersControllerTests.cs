using EPlast.BLL.DTO.EventUser;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.Tests.Controllers
{
    [TestFixture]
    class EventsUsersControllerTests
    {
        private Mock<IEventUserManager> _eventUserManager;

        private EventsUsersController _eventsUsersController;

        [SetUp]
        public void SetUp()
        {
            _eventUserManager = new Mock<IEventUserManager>();

            _eventsUsersController = new EventsUsersController(
                _eventUserManager.Object);
        }

        [Test]
        public async Task GetEventUserByUserId_ReturnsOkObjectResult()
        {
            // Arrange
            _eventUserManager
                .Setup((x) => x.EventUserAsync(It.IsAny<string>(), It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(CreateFakeEventUser());

            // Act
            var result = await _eventsUsersController.GetEventUserByUserId(It.IsAny<string>());

            // Assert
            Assert.NotNull((result as ObjectResult).Value);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetEventUserByUserId_EventUserWithIdOne_ReturnsEventUserWithIdOne()
        {
            // Arrange
            var expectedId = "1";

            _eventUserManager
                .Setup((x) => x.EventUserAsync(It.IsAny<string>(), It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(CreateFakeEventUser());

            // Act
            var result = await _eventsUsersController.GetEventUserByUserId(It.IsAny<string>());

            var actual = ((result as ObjectResult).Value as EventUserDTO).User.Id;

            // Assert
            Assert.NotNull((result as ObjectResult).Value);
            Assert.AreEqual(expectedId, actual);
        }

        [Test]
        public async Task GetEventsDataForCreate_ReturnsOkObjectResult()
        {
            // Arrange
            _eventUserManager
                .Setup((x) => x.InitializeEventCreateDTOAsync())
                .ReturnsAsync(CreateFakeEventCreate());

            // Act
            var result = await _eventsUsersController.GetEventsDataForCreate();

            // Assert
            Assert.NotNull((result as ObjectResult).Value);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetEventsDataForCreate_EventCreateWithIdOne_ReturnsEventCreateWithIdOne()
        {
            // Arrange
            var expectedId = 1;

            _eventUserManager
                .Setup((x) => x.InitializeEventCreateDTOAsync())
                .ReturnsAsync(CreateFakeEventCreate());

            // Act
            var result = await _eventsUsersController.GetEventsDataForCreate();

            var actual = ((result as ObjectResult).Value as EventCreateDTO).Event.ID;

            // Assert
            Assert.NotNull((result as ObjectResult).Value);
            Assert.AreEqual(expectedId, actual);
        }

        [Test]
        public async Task EventCreate_ReturnsCreatedResult()
        {
            // Arrange
            _eventUserManager
                .Setup((x) => x.CreateEventAsync(CreateFakeEventCreate()))
                .ReturnsAsync(It.IsAny<int>());

            // Act
            var result = await _eventsUsersController.EventCreate(CreateFakeEventCreate());

            // Assert
            Assert.NotNull((result as ObjectResult).Value as EventCreateDTO);
            Assert.IsInstanceOf<CreatedResult>(result);
        }

        [Test]
        public async Task EventCreate_EventCreateWithIdOne_ReturnsEventCreateWithIdOne()
        {
            // Arrange
            var expectedId = 1;

            _eventUserManager
                .Setup((x) => x.CreateEventAsync(CreateFakeEventCreate()))
                .ReturnsAsync(It.IsAny<int>());

            // Act
            var result = await _eventsUsersController.EventCreate(CreateFakeEventCreate());

            var actual = ((result as ObjectResult).Value as EventCreateDTO).Event.ID;

            // Assert
            Assert.NotNull((result as ObjectResult).Value);
            Assert.AreEqual(expectedId, actual);
        }

        [Test]
        public async Task EventEdit_ReturnsOkObjectResult()
        {
            // Arrange
            _eventUserManager
                .Setup((x) => x.InitializeEventEditDTOAsync(It.IsAny<int>()))
                .ReturnsAsync(CreateFakeEventCreate());

            // Act
            var result = await _eventsUsersController.EventEdit(It.IsAny<int>());

            // Assert
            Assert.NotNull((result as ObjectResult).Value as EventCreateDTO);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task EventEdit_EventCreateWithIdOne_ReturnsEventCreateWithIdOne()
        {
            // Arrange
            var expectedId = 1;

            _eventUserManager
                .Setup((x) => x.InitializeEventEditDTOAsync(It.IsAny<int>()))
                .ReturnsAsync(CreateFakeEventCreate());

            // Act
            var result = await _eventsUsersController.EventEdit(It.IsAny<int>());

            var actual = ((result as ObjectResult).Value as EventCreateDTO).Event.ID;

            // Assert
            Assert.NotNull((result as ObjectResult).Value);
            Assert.AreEqual(expectedId, actual);
        }

        [Test]
        public async Task EventEdit_EventCreate_ReturnsOkObjectResult()
        {
            // Arrange
            _eventUserManager
                .Setup((x) => x.EditEventAsync(CreateFakeEventCreate()));

            // Act
            var result = await _eventsUsersController.EventEdit(CreateFakeEventCreate());

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
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
