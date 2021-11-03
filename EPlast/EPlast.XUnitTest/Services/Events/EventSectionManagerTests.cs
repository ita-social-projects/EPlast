using EPlast.BLL.DTO.Events;
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
    public class EventSectionManagerTests
    {
        private readonly Mock<IRepositoryWrapper> _repoWrapper;

        public EventSectionManagerTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
        }

        [Fact]
        public async Task GetEventSectionsDTOAsync_ReturnIEnumerableDto()
        {
            //Arrange
            _repoWrapper.Setup(x => x.EventSection.GetAllAsync(null, null))
                .ReturnsAsync(GetEventSections());

            //Act
            var eventSectionManager = new EventSectionManager(_repoWrapper.Object);
            var methodResult = await eventSectionManager.GetEventSectionsDTOAsync();

            //Assert
            Assert.NotNull(methodResult);
            Assert.IsAssignableFrom<IEnumerable<EventSectionDTO>>(methodResult);
            Assert.Equal(GetEventSections().Count(), methodResult.ToList().Count);
        }

        public IQueryable<EventSection> GetEventSections()
        {
            var types = new List<EventSection>
            {
                new EventSection()
                {
                    ID = 1,
                    EventSectionName = "Type 1"
                },
                new EventSection()
                {
                    ID = 2,
                    EventSectionName = "Type 2"
                },
                new EventSection()
                {
                    ID = 1,
                    EventSectionName = "Type 3"
                },
            }.AsQueryable();
            return types;
        }
    }
}
