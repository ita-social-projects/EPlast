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
        {_mockRepoWrapper = new Mock<IRepositoryWrapper>();
            var store = new Mock<IUserStore<User>>();
            _eventTypeManager = new EventTypeManager(
                _mockRepoWrapper.Object
            );
        }

        [Test]
        public void GetEventTypes_Valid()
        { 
            var eventToCheck = _mockRepoWrapper.Setup(x => x.EventType.GetAllAsync(
                    It.IsAny<Expression<Func<DataAccess.Entities.Event.EventType, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.Event.EventType>,
                        IIncludableQueryable<DataAccess.Entities.Event.EventType, object>>>()))
                .ReturnsAsync(new List<DataAccess.Entities.Event.EventType>());
            
            var result = _eventTypeManager.GetDTOAsync();
            
            Assert.IsNotNull(result);
        }
        
        [Test]
        public void GetTypeByIdAsync_Valid()
        {
            var eventType = _mockRepoWrapper.Setup(x => x.EventType.GetFirstAsync(
                       It.IsAny<Expression<Func<DataAccess.Entities.Event.EventType, bool>>>(),
                       It.IsAny<Func<IQueryable<DataAccess.Entities.Event.EventType>,
                           IIncludableQueryable<DataAccess.Entities.Event.EventType, object>>>()))
                    .ReturnsAsync(new DataAccess.Entities.Event.EventType());
            
            var result = _eventTypeManager.GetTypeByIdAsync(1);

            Assert.IsAssignableFrom<Task<EventType>>(result);
            Assert.IsNotNull(result);
        }
        
        [Test]
        public void GetTypeIdAsync_Valid()
        {
            var eventType = _mockRepoWrapper.Setup(x => x.EventType.GetFirstAsync(
                    It.IsAny<Expression<Func<DataAccess.Entities.Event.EventType, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.Event.EventType>,
                        IIncludableQueryable<DataAccess.Entities.Event.EventType, object>>>()))
                .ReturnsAsync(new DataAccess.Entities.Event.EventType());

            var result = _eventTypeManager.GetTypeByIdAsync(1);

            Assert.IsAssignableFrom<Task<EventType>>(result);
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetTypeIdAsync()
        {
            var type = _mockRepoWrapper.Setup(x => x.EventType.GetFirstAsync(
                    It.IsAny<Expression<Func<DataAccess.Entities.Event.EventType, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.Event.EventType>,
                        IIncludableQueryable<DataAccess.Entities.Event.EventType, object>>>()))
                .ReturnsAsync(new EventType() { ID = 1 });

            var result = _eventTypeManager.GetTypeIdAsync("");

            Assert.IsAssignableFrom<Task<int>>(result);
            Assert.IsNotNull(result);
        }


    }

}