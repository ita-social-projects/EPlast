using EPlast.BLL.Services.Events;
using EPlast.DataAccess.Entities.Event;
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

namespace EPlast.Tests.Services.Event
{
    class EventAdministrationManagerTests
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private EventAdministrationManager _manager;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _manager = new EventAdministrationManager(_repoWrapper.Object);
        }

        [Test]
        public async Task GetEventAdmininistrationByUserIdAsync_ReturnsCorrect()
        {
            // Arrange
            _repoWrapper
                .Setup(x => x.EventAdministration.GetAllAsync(It.IsAny<Expression<Func<EventAdministration, bool>>>(),
                It.IsAny<Func<IQueryable<EventAdministration>,
                IIncludableQueryable<EventAdministration, object>>>()))
                .ReturnsAsync(new List<EventAdministration>() { new EventAdministration() { ID=2} });
            // Act
            var result = await _manager.GetEventAdmininistrationByUserIdAsync(It.IsAny<string>());
            // Assert
            Assert.IsInstanceOf<IEnumerable<EventAdministration>>(result);
            Assert.AreEqual(1, result.Count());
        }
    }
}
