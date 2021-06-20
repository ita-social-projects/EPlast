using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Services.Events;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services.Events
{
    class EventCategoryManagerTests
    {
        private IEventCategoryManager _eventCategoryManager;
        private Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private Mock<IEventTypeManager> _mockEventTypeManager;
        [SetUp]
        public void SetUp()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockEventTypeManager = new Mock<IEventTypeManager>();
            _eventCategoryManager = new EventCategoryManager(
                _mockRepositoryWrapper.Object,
                _mockEventTypeManager.Object);
        }
        [Test]
        public void GetDTOByEventPageAsync_Valid()
        {
            //Arrange
            int testEventTypeId = 1;
            int testPage = 1;
            int testPageSize = 1;
            _mockEventTypeManager.Setup(x => x.GetTypeByIdAsync(It.IsAny<int>()))
                .ReturnsAsync( new EventType());

            //Act
             var result =
                 _eventCategoryManager.GetDTOByEventPageAsync(testEventTypeId, testPage, testPageSize, "CategoryName");

            //Assert
            Assert.IsNotNull(result);

        }
    }
}
