using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPlast.BLL.DTO.EventCalendar;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.BLL.Services;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services.Event
{
    class EventCalendarServiceTests
    {
        private Mock<IEventsManager> _eventsManager;
        private EventCalendarService service;

        [SetUp]
        public void SetUp()
        {
            _eventsManager = new Mock<IEventsManager>();
            service = new EventCalendarService(_eventsManager.Object);
        }

        [Test]
        public async Task GetAllActions_ReturnsCorrect()
        {
            // Arrange
            _eventsManager
                .Setup(x => x.GetActionsAsync()).ReturnsAsync(new List<EventCalendarInfoDto>() { new EventCalendarInfoDto() { ID = 2 } });
            // Act
            var result = await service.GetAllActions();
            // Assert
            Assert.IsInstanceOf<IEnumerable<EventCalendarInfoDto>>(result);
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public async Task GetAllActions_ReturnsNull()
        {
            // Arrange
            List<EventCalendarInfoDto> list = null;
            _eventsManager
                .Setup(x => x.GetActionsAsync())
                .ReturnsAsync(list);
            // Act
            var result = await service.GetAllActions();
            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllEducations_ReturnsList()
        {
            // Arrange
            _eventsManager
                .Setup(x => x.GetEducationsAsync())
                .ReturnsAsync(new List<EventCalendarInfoDto>() { new EventCalendarInfoDto() { ID = 2 } });
            // Act
            var result = await service.GetAllEducations();
            // Assert
            Assert.IsInstanceOf<IEnumerable<EventCalendarInfoDto>>(result);
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public async Task GetAllEducations_ReturnsNull()
        {
            // Arrange
            List<EventCalendarInfoDto> list = null;
            _eventsManager
                .Setup(x => x.GetEducationsAsync())
                .ReturnsAsync(list);
            // Act
            var result = await service.GetAllEducations();
            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllCamps_ReturnsList()
        {
            // Arrange
            _eventsManager
                .Setup(x => x.GetCampsAsync()).ReturnsAsync(new List<EventCalendarInfoDto>() { new EventCalendarInfoDto() { ID = 2 } });
            // Act
            var result = await service.GetAllCamps();
            // Assert
            Assert.IsInstanceOf<IEnumerable<EventCalendarInfoDto>>(result);
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public async Task GetAllCamps_ReturnsNull()
        {
            // Arrange
            List<EventCalendarInfoDto> list = null;
            _eventsManager
                .Setup(x => x.GetCampsAsync()).ReturnsAsync(list);
            // Act
            var result = await service.GetAllCamps();
            // Assert
            Assert.IsNull(result);
        }
    }
}
