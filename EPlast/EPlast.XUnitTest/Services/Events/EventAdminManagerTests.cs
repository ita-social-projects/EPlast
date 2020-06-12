using EPlast.BussinessLayer.Services.Events;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace EPlast.XUnitTest.Services.Events
{
    public class EventAdminManagerTests
    {
        private readonly Mock<IRepositoryWrapper> _repoWrapper;

        public EventAdminManagerTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
        }

        [Fact]
        public async Task GetEventAdminByUserId()
        {
            //Arrange
            string userId = "1";
            _repoWrapper.Setup(x => x.EventAdmin.GetFirstAsync(It.IsAny<Expression<Func<EventAdmin, bool>>>(), null))
                .ReturnsAsync(new EventAdmin());
            //Act
            var eventAdminManager = new EventAdminManager(_repoWrapper.Object);
            var methodResult = await eventAdminManager.GetEventAdminsByUserIdAsync(userId);

            //Assert
            Assert.NotNull(methodResult);
            Assert.IsType<List<EventAdmin>>(methodResult);
        }
    }
}
