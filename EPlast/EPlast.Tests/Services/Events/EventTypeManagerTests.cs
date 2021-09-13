using EPlast.BLL.Interfaces.Events;
using EPlast.DataAccess.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using EPlast.BLL.Services.Events;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities.Event;


namespace EPlast.Tests.Services.Events
{
    internal class EventTypeManagerTests
    {

        private IEventTypeManager _eventTypeManager;
        private Mock<IRepositoryWrapper> _mockRepoWrapper;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            var store = new Mock<IUserStore<User>>();
            _eventTypeManager = new EventTypeManager(
                _mockRepoWrapper.Object
            );
        }

        [Test]
        public async Task GetEventTypes_ReturnsEventTypes()
        {
            //Arrange
           _mockRepoWrapper.Setup(x => x.EventType.GetAllAsync(
                    It.IsAny<Expression<Func<EventType, bool>>>(),
                    It.IsAny<Func<IQueryable<EventType>,
                        IIncludableQueryable<EventType, object>>>()))
                .ReturnsAsync(new List<EventType>() { new EventType() { ID = 1 } });

            //Act
            var result = await _eventTypeManager.GetEventTypesDTOAsync();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetTypeByIdAsync_ReturnsType()
        {   //Arrange
            _mockRepoWrapper.Setup(x => x.EventType.GetFirstAsync(
                       It.IsAny<Expression<Func<EventType, bool>>>(),
                       It.IsAny<Func<IQueryable<EventType>,
                           IIncludableQueryable<EventType, object>>>()))
                    .ReturnsAsync(new EventType() { ID = 1 });

            //Act
            var result = await _eventTypeManager.GetTypeByIdAsync(1);

            //Assert
            Assert.IsAssignableFrom<EventType>(result);
            Assert.AreEqual(1, result.ID);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetTypeIdAsync_ReturnsTypeId()
        { 
            //Arrange
            _mockRepoWrapper.Setup(x => x.EventType.GetFirstAsync(
                    It.IsAny<Expression<Func<EventType, bool>>>(),
                    It.IsAny<Func<IQueryable<EventType>,
                        IIncludableQueryable<EventType, object>>>()))
                .ReturnsAsync(new EventType());

            //Act
            var result = await _eventTypeManager.GetTypeByIdAsync(1);

            //Assert
            Assert.IsInstanceOf<EventType>(result);
            Assert.IsNotNull(result);
        }
    }
}
