using EPlast.BussinessLayer.Services.Events;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EPlast.DataAccess.Entities.Event;
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
        public void GetTypeIdTest()
        {
            //Arrange
            string typeName = "Status Name";
            _repoWrapper.Setup(x => x.EventType.FindByCondition(It.IsAny<Expression<Func<EventType, bool>>>()))
                .Returns(GetEventTypes());
            //Act
            var eventTypeManager = new EventTypeManager(_repoWrapper.Object);
            var methodResult = eventTypeManager.GetTypeId(typeName);
            //Assert
            Assert.Equal(1, methodResult);
        }
        public IQueryable<EventType> GetEventTypes()
        {
            var eventTypes = new List<EventType>
            {
                new EventType(){
                    ID = 1,
                    EventTypeName = "Type 1"
                }
            }.AsQueryable();
            return eventTypes;
        }
    }
}
