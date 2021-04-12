using EPlast.BLL.DTO.Events;
using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Services.Events;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EPlast.XUnitTest.Services.Events
{
    public class EventCategoryManagerTests
    {
        private readonly Mock<IRepositoryWrapper> _repoWrapper;
        private readonly Mock<IEventTypeManager> _eventTypeManager;


        public EventCategoryManagerTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _eventTypeManager = new Mock<IEventTypeManager>();
        }

        [Fact]
        public async Task GetDTOTest()
        {
            //Arrange
            _repoWrapper.Setup(x => x.EventCategory.GetAllAsync(null, null))
                .ReturnsAsync(GetEventCategories());
            //Act
            var eventCategoryManager = new EventCategoryManager(_repoWrapper.Object, _eventTypeManager.Object);
            var methodResult = await eventCategoryManager.GetDTOAsync();
            //Assert
            Assert.NotNull(methodResult);
            Assert.IsAssignableFrom<IEnumerable<EventCategoryDTO>>(methodResult);
            Assert.Equal(GetEventCategories().Count(), methodResult.ToList().Count);
        }

        [Fact]
        public async Task GetDTOByEventTypeIdTest()
        {
            //Arrange
            _eventTypeManager.Setup(et => et.GetTypeByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(GeEventType);
            var eventTypeId = 1;
            //Act
            var eventCategoryManager = new EventCategoryManager(_repoWrapper.Object, _eventTypeManager.Object);
            var methodResult = await eventCategoryManager.GetDTOByEventTypeIdAsync(eventTypeId);
            //Assert
            Assert.NotNull(methodResult);
            Assert.IsAssignableFrom<IEnumerable<EventCategoryDTO>>(methodResult);
            Assert.Equal(GeEventType().EventCategories.Count, methodResult.ToList().Count);
        }

        public IQueryable<EventCategory> GetEventCategories()
        {
            var events = new List<EventCategory>
            {
                new EventCategory(){
                    ID = 1,
                    EventCategoryName = "Category 1"
                },
                new EventCategory(){
                    ID = 2,
                    EventCategoryName = "Category 2"
                },
                new EventCategory(){
                    ID = 3,
                    EventCategoryName = "Category 3"
                },
            }.AsQueryable();
            return events;
        }

        public EventType GeEventType()
        {
            return new EventType()
            {
                ID = 1,
                EventTypeName = "Type 1",
                EventCategories = new List<EventCategoryType>()
                {
                    new EventCategoryType()
                    {
                        EventTypeId = 1,
                        EventCategoryId = 1,
                        EventCategory = new EventCategory()
                        {
                            ID = 1,
                            EventCategoryName = "Category 1"
                        }
                    },
                    new EventCategoryType()
                    {
                        EventTypeId = 1,
                        EventCategoryId = 2,
                        EventCategory = new EventCategory()
                        {
                            ID = 2,
                            EventCategoryName = "Category 2"
                        }
                    }
                }
            };
        }
    }
}
