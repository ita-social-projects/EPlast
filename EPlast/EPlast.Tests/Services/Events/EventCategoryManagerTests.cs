﻿using AutoMapper;
using EPlast.BLL.DTO.Events;
using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Services.Events;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.Tests.Services.Events
{
    class EventCategoryManagerTests
    {
        private IEventCategoryManager _eventCategoryManager;
        private Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private Mock<IEventTypeManager> _mockEventTypeManager;
        private Mock<IMapper> _mockMapper;

        [SetUp]
        public void SetUp()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockEventTypeManager = new Mock<IEventTypeManager>();
            _mockMapper = new Mock<IMapper>();
            _eventCategoryManager = new EventCategoryManager(
                _mockRepositoryWrapper.Object,
                _mockEventTypeManager.Object,
                _mockMapper.Object);
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
            Assert.IsAssignableFrom<Task<IEnumerable<EventCategoryDTO>>>(result);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task CreateEventCategoryAsync_ReturnsIntIdOfCreatedCategory()
        {
            //Arrange
            _mockMapper.Setup(m => m.Map<EventCategoryDTO, EventCategory>(It.IsAny<EventCategoryDTO>()))
                       .Returns(new EventCategory());
            _mockRepositoryWrapper.Setup(r => r.EventCategory.CreateAsync(It.IsAny<EventCategory>()));
            _mockRepositoryWrapper.Setup(r => r.EventCategoryType.CreateAsync(It.IsAny<EventCategoryType>()));
            _mockRepositoryWrapper.Setup(r => r.SaveAsync());

            //Act
            var methodResult = await _eventCategoryManager.CreateEventCategoryAsync(CreateFakeEventCategory());

            //Assert
            Assert.IsNotNull(methodResult);
            Assert.IsInstanceOf<int>(methodResult);
        }

        private EventCategoryCreateDTO CreateFakeEventCategory()
            => new EventCategoryCreateDTO()
            {
                EventCategory = new EventCategoryDTO()
                {
                    EventCategoryId = 1,
                    EventCategoryName = "new category",
                    EventSectionId = 2
                },
                EventTypeId = 3
            };
    }
}
