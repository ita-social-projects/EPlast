using EPlast.BLL.Services.Events;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
        public async Task GetStatusIdTest()
        {
            //Arrange
            string statusName = "Status Name";
            _repoWrapper.Setup(x =>
                    x.EventStatus.GetFirstAsync(It.IsAny<Expression<Func<EventStatus, bool>>>(), null))
                .ReturnsAsync(GetEventStatus());
            //Act
            var eventStatusManager = new EventStatusManager(_repoWrapper.Object);
            var methodResult =await eventStatusManager.GetStatusIdAsync(statusName);
            //Assert
            Assert.Equal(1, methodResult);
        }
        public EventStatus GetEventStatus()
        {
            return new EventStatus()
            {
                ID = 1,
                EventStatusName = "Status 1"
            };
        }
    }
}
