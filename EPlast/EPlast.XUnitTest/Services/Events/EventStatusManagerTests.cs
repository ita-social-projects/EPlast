using EPlast.BussinessLayer.Services.Events;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace EPlast.XUnitTest.Services.Events
{
    public class EventStatusManagerTests
    {
        private readonly Mock<IRepositoryWrapper> _repoWrapper;

        public EventStatusManagerTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
        }
        [Fact]
        public void GetStatusIdTest()
        {
            //Arrange
            string statusName = "Status Name";
            _repoWrapper.Setup(x => x.EventStatus.FindByCondition(It.IsAny<Expression<Func<EventStatus, bool>>>()))
                .Returns(GetEventStatuses());
            //Act
            var eventStatusManager = new EventStatusManager(_repoWrapper.Object);
            var methodResult = eventStatusManager.GetStatusId(statusName);
            //Assert
            Assert.Equal(1, methodResult);
        }
        public IQueryable<EventStatus> GetEventStatuses()
        {
            var eventStatuses = new List<EventStatus>
            {
                new EventStatus(){
                    ID = 1,
                    EventStatusName = "Status 1"
                }
            }.AsQueryable();
            return eventStatuses;
        }
    }
}
