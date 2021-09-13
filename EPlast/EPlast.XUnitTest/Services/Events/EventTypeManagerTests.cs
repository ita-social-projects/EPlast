using EPlast.BLL.Services.Events;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EPlast.BLL.DTO.EventUser;
using Microsoft.EntityFrameworkCore.Query;
using Xunit;

namespace EPlast.XUnitTest.Services.Events
{
    public class EventTypeManagerTests
    {
        private readonly Mock<IRepositoryWrapper> _repoWrapper;

        public EventTypeManagerTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
        }

        [Fact]
        public async Task GetTypeIdTest()
        {
            //Arrange
            string typeName = "Status Name";
            _repoWrapper.Setup(x =>
                    x.EventType.GetFirstAsync(It.IsAny<Expression<Func<EventType, bool>>>(), null))
                .ReturnsAsync(GetEventTypes().First());
            //Act
            var eventTypeManager = new EventTypeManager(_repoWrapper.Object);
            var methodResult = await eventTypeManager.GetTypeIdAsync(typeName);
            //Assert
            Assert.Equal(1, methodResult);
        }

        [Fact]
        public async Task GetDTOTest()
        {
            //Arrange
            _repoWrapper.Setup(x => x.EventType.GetAllAsync(null, null))
                .ReturnsAsync(GetEventTypes());
            //Act
            var eventTypeManager = new EventTypeManager(_repoWrapper.Object);
            var methodResult = await eventTypeManager.GetEventTypesDTOAsync();
            //Assert
            Assert.NotNull(methodResult);
            Assert.IsAssignableFrom<IEnumerable<EventTypeDTO>>(methodResult);
            Assert.Equal(GetEventTypes().Count(), methodResult.ToList().Count);
        }

        [Fact]
        public async Task GetTypeByIdTest()
        {
            //Arrange
            var eventTypeId = 1;
            _repoWrapper.Setup(x => x.EventType.GetFirstAsync(It.IsAny<Expression<Func<EventType, bool>>>(), It.IsAny<Func<IQueryable<EventType>, IIncludableQueryable<EventType, object>>>()))
                .ReturnsAsync(GetEventTypes().First());
            //Act
            var eventTypeManager = new EventTypeManager(_repoWrapper.Object);
            var methodResult = await eventTypeManager.GetTypeByIdAsync(eventTypeId);
            //Assert
            Assert.NotNull(methodResult);
            Assert.Equal(eventTypeId, methodResult.ID);
        }

        public IQueryable<EventType> GetEventTypes()
        {
            var types = new List<EventType>
            {
                new EventType()
                {
                    ID = 1,
                    EventTypeName = "Type 1"
                },
                new EventType()
                {
                    ID = 2,
                    EventTypeName = "Type 2"
                },
                new EventType()
                {
                    ID = 1,
                    EventTypeName = "Type 3"
                },
            }.AsQueryable();
            return types;
        }
    }
}  
