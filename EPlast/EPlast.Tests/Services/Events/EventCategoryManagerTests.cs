using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Events;
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
            Assert.IsAssignableFrom<Task<IEnumerable<EventCategoryDto>>>(result);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task CreateEventCategoryAsync_ReturnsIntIdOfCreatedCategory()
        {
            //Arrange
            _mockMapper.Setup(m => m.Map<EventCategoryDto, EventCategory>(It.IsAny<EventCategoryDto>()))
                       .Returns(new EventCategory());
            _mockRepositoryWrapper
                .Setup(r => r.EventCategory.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<EventCategory, bool>>>(), null))
                .ReturnsAsync(new EventCategory());
            _mockRepositoryWrapper.Setup(r => r.EventCategory.CreateAsync(It.IsAny<EventCategory>()));
            _mockRepositoryWrapper.Setup(r => r.EventCategoryType.CreateAsync(It.IsAny<EventCategoryType>()));
            _mockRepositoryWrapper.Setup(r => r.SaveAsync());

            //Act
            var methodResult = await _eventCategoryManager.CreateEventCategoryAsync(CreateFakeEventCategory());

            //Assert
            Assert.IsNotNull(methodResult);
            Assert.IsInstanceOf<int>(methodResult);
        }

        private EventCategoryCreateDto CreateFakeEventCategory()
            => new EventCategoryCreateDto()
            {
                EventCategory = new EventCategoryDto()
                {
                    EventCategoryId = 1,
                    EventCategoryName = "new category",
                    EventSectionId = 2
                },
                EventTypeId = 3
            };
    }
}
