using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Events;
using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.BLL.Services.EventUser.EventUserAccess;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services.Event
{
    class EventUserAccessServiceTests
    {
        private IEventUserAccessService _eventUserAccessService;
        private Mock<IEventAdmininistrationManager> _eventAdministrationManager;
        private Mock<UserManager<User>> _userManager;
        private Mock<IActionManager> _actionManager;
        private Mock<IRepositoryWrapper> _repositoryWrapper;

        [SetUp]
        public void SetUp()
        {
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _eventAdministrationManager = new Mock<IEventAdmininistrationManager>();
            _actionManager = new Mock<IActionManager>();
            _repositoryWrapper = new Mock<IRepositoryWrapper>();
            _eventUserAccessService = new EventUserAccessService(_eventAdministrationManager.Object, _userManager.Object, _actionManager.Object, _repositoryWrapper.Object);
        }

        [Test]
        public async Task IsUserAdminOfEvent_ReturnsBool()
        {
            //Arrange
            _eventAdministrationManager
                .Setup(x => x.GetEventAdmininistrationByUserIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<EventAdministration>() { new EventAdministration { ID = 1 } });

            //Act
            var result = await _eventUserAccessService.IsUserAdminOfEvent(new User { Id = "1" }, It.IsAny<int>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<bool>(result);
        }

        [Test]
        public async Task GetEventStatusAsync_ReturnsString()
        {
            //Arrange
            _actionManager
                .Setup(x => x.GetEventInfoAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(CreateFakeApprovedEvent());

            //Act
            var result = await _eventUserAccessService.GetEventStatusAsync(It.IsAny<User>(), It.IsAny<int>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<string>(result);
        }

        [Test]
        public async Task RedefineAccessesAsync_EventIdNull_ReturnsListOfEventAccesses()
        {
            //Arrange
            Dictionary<string, bool> dict = new Dictionary<string, bool>();
            dict.Add("action", It.IsAny<bool>());

            //Act
            var result = await _eventUserAccessService.RedefineAccessesAsync(dict, It.IsAny<User>());

            //Assert
            Assert.IsNotEmpty(result);
            Assert.IsInstanceOf<Dictionary<string, bool>>(result);
            _eventAdministrationManager.Verify(v => v.GetEventAdmininistrationByUserIdAsync(It.IsAny<string>()), Times.Never());
        }

        [Test]
        public async Task RedefineAccessesAsync_EventIdNotNullAndRoleIsPlastMemberAndEventIsApproved_ReturnsListOfEventAccesses()
        {
            //Arrange
            int? eventId = CreateFakeApprovedEvent().Event.EventId;
            var user = CreateFakeUser();
            Dictionary<string, bool> dict = new Dictionary<string, bool>();
            dict.Add("action", It.IsAny<bool>());

            _eventAdministrationManager
                .Setup(x => x.GetEventAdmininistrationByUserIdAsync(user.Id))
                .ReturnsAsync(new List<EventAdministration>() { new EventAdministration { ID = 1, EventID = 1 } });
            _actionManager
                .Setup(x => x.GetEventInfoAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(CreateFakeApprovedEvent());

            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.PlastMember });

            //Act
            var result = await _eventUserAccessService.RedefineAccessesAsync(dict, CreateFakeUser(), eventId);

            //Assert
            Assert.IsNotEmpty(result);
            Assert.IsInstanceOf<Dictionary<string, bool>>(result);
        }

        [Test]
        public async Task RedefineAccessesAsync_EventIdNotNullAndRoleIsAdminAndEventIsFinished_ReturnsListOfEventAccesses()
        {
            //Arrange
            int? eventId = CreateFakeFinishedEvent().Event.EventId;
            var user = CreateFakeUser();
            Dictionary<string, bool> dict = new Dictionary<string, bool>();
            dict.Add("action", It.IsAny<bool>());

            _eventAdministrationManager
                .Setup(x => x.GetEventAdmininistrationByUserIdAsync(user.Id))
                .ReturnsAsync(new List<EventAdministration>() { new EventAdministration { ID = 1, EventID = 1 } });
            _actionManager
                .Setup(x => x.GetEventInfoAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(CreateFakeFinishedEvent());

            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Admin });

            //Act
            var result = await _eventUserAccessService.RedefineAccessesAsync(dict, user, eventId);

            //Assert
            Assert.IsNotEmpty(result);
            Assert.IsInstanceOf<Dictionary<string, bool>>(result);
        }

        private User CreateFakeUser()
            => new User()
            {
                Id = "1",
                FirstName = "SomeFirstName",
                LastName = "SomeLastName",
                Events = new List<EventAdmin>()
                {
                    new EventAdmin()
                    {
                        EventID = 1
                    }
                }
            };

        private EventDto CreateFakeFinishedEvent()
            => new EventDto()
            {
                Event = new EventInfoDto()
                {
                    EventId = 1,
                    EventName = "SomeEventName",
                    EventStatus = "Завершено"
                },

            };

        private EventDto CreateFakeApprovedEvent()
            => new EventDto()
            {
                Event = new EventInfoDto()
                {
                    EventId = 1,
                    EventName = "SomeEventName",
                    EventStatus = "Затверджено"
                },

            };
    }
}

