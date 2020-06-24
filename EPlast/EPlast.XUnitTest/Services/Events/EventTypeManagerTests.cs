using EPlast.BusinessLogicLayer.Services.Events;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Moq;
using System;
using System.Linq.Expressions;
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
        public async void GetTypeIdTest()
        {
            //Arrange
            string typeName = "Status Name";
            _repoWrapper.Setup(x =>
                    x.EventType.GetFirstAsync(It.IsAny<Expression<Func<EventType, bool>>>(), null))
                .ReturnsAsync(GetEventType());
            //Act
            var eventTypeManager = new EventTypeManager(_repoWrapper.Object);
            var methodResult = await eventTypeManager.GetTypeIdAsync(typeName);
            //Assert
            Assert.Equal(1, methodResult);
        }

        public EventType GetEventType()
        {
            return new EventType()
            {
                ID = 1,
                EventTypeName = "Type 1"
            };
        }
    }
}  
