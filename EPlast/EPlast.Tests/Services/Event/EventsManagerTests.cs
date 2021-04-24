using EPlast.BLL.DTO.EventCalendar;
using EPlast.BLL.Services.EventUser;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace EPlast.Tests.Services
{
    class EventsManagerTests
    {

        private Mock<IRepositoryWrapper> _repoWrapper;
        private EventsManager manager;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            manager = new EventsManager(_repoWrapper.Object);
        }

        [Test]
        public async Task GetActionsAsync_ReturnsCorrect()
        {
            // Arrange
            _repoWrapper
                .Setup(x => x.Event.GetAllAsync(It.IsAny<Expression<Func<DataAccess.Entities.Event.Event, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccess.Entities.Event.Event>, IIncludableQueryable<DataAccess.Entities.Event.Event, object>>>()))
                .ReturnsAsync(EventsList);
            // Act
            var result = await manager.GetActionsAsync();
            // Assert
            Assert.IsInstanceOf<List<EventCalendarInfoDTO>>(result);
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public async Task GetEducationsAsync_ReturnsList()
        {
            // Arrange
             _repoWrapper
                .Setup(x => x.Event.GetAllAsync(It.IsAny<Expression<Func<DataAccess.Entities.Event.Event, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccess.Entities.Event.Event>, IIncludableQueryable<DataAccess.Entities.Event.Event, object>>>()))
                .ReturnsAsync(EventsList);
            // Act
            var result = await manager.GetEducationsAsync();
            // Assert
            Assert.IsInstanceOf<List<EventCalendarInfoDTO>>(result);
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public async Task GetCampsAsync_ReturnsList()
        {
            // Arrange
            _repoWrapper
                .Setup(x => x.Event.GetAllAsync(It.IsAny<Expression<Func<DataAccess.Entities.Event.Event, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccess.Entities.Event.Event>, IIncludableQueryable<DataAccess.Entities.Event.Event, object>>>()))
                .ReturnsAsync(EventsList);
            // Act
            var result = await manager.GetCampsAsync();
            // Assert
            Assert.IsInstanceOf<List<EventCalendarInfoDTO>>(result);
            Assert.AreEqual(1, result.Count);
        }

        private readonly List<DataAccess.Entities.Event.Event> EventsList =
            new List<DataAccess.Entities.Event.Event>()
            { 
                new DataAccess.Entities.Event.Event() { ID = 2 } 
            };
    }
}
