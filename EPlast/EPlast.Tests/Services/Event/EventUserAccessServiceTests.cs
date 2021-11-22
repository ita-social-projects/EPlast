using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.BLL.Services.EventUser.EventUserAccess;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.Tests.Services.Event
{
    class EventUserAccessServiceTests
    {
        private IEventUserAccessService _eventUserAccessService;
        private Mock<IEventAdmininistrationManager> _eventAdministrationManager;

        [SetUp]
        public void SetUp()
        {
            _eventAdministrationManager = new Mock<IEventAdmininistrationManager>();
            _eventUserAccessService = new EventUserAccessService(_eventAdministrationManager.Object);
        }

        [Test]
        public async Task HasAccessAsync_ReturnsBool()
        {
            //Arrange
            _eventAdministrationManager
                .Setup(x => x.GetEventAdmininistrationByUserIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<EventAdministration>() { new EventAdministration { ID = 1 } });

            //Act
            var result = await _eventUserAccessService.HasAccessAsync(new User { Id = "1" }, It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<bool>(result);
        }
    }
}

