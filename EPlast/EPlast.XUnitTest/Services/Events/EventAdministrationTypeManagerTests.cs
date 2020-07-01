using EPlast.BLL.Interfaces.EventUser;
using EPlast.BLL.Services.EventUser;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EPlast.XUnitTest.Services.Events
{
    public class EventAdministrationTypeManagerTests
    {
        private readonly Mock<IRepositoryWrapper> _repoWrapper;

        public EventAdministrationTypeManagerTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
        }

        [Fact]
        public async Task GetTypeID()
        {
            //Arrange
            _repoWrapper.Setup(x => x.EventAdministrationType.GetFirstAsync(It.IsAny<Expression<Func<EventAdministrationType,
                bool>>>(), null)).ReturnsAsync(new EventAdministrationType());
            var typeName = "Комендант";
            //Act
            var eventAdministrationTypeManager = new EventAdministrationTypeManager(_repoWrapper.Object);
            var methodResult = await eventAdministrationTypeManager.GetTypeIdAsync(typeName);
            //Assert
            Assert.IsType<int>(methodResult);
        }
    }
}
