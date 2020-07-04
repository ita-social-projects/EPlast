using EPlast.BLL.DTO.Events;
using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Services.Events;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Moq;
using System.Collections.Generic;
using System.Linq;
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
        public async void GetDTOTest()
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
    }
}
