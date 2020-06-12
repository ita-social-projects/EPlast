using EPlast.BussinessLayer.DTO.Events;
using EPlast.BussinessLayer.Services.Events;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Moq;
using System.Collections.Generic;
using System.Linq;
using EPlast.DataAccess.Entities.Event;
using Xunit;
using System.Threading.Tasks;

namespace EPlast.XUnitTest.Services.Events
{
    public class EventCategoryManagerTests
    {
        private readonly Mock<IRepositoryWrapper> _repoWrapper;

        public EventCategoryManagerTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
        }

        //[Fact]
        //public async Task GetDTOTest()
        //{
        //    //Arrange
        //    _repoWrapper.Setup(x=>x.EventCategory.FindAll())
        //        .Returns(GetEventCategories());
        //    //Act
        //    var eventCategoryManager = new EventCategoryManager(_repoWrapper.Object);
        //    var methodResult =await eventCategoryManager.GetDTOAsync();
        //    //Assert
        //    Assert.NotNull(methodResult);
        //    Assert.IsType<List<EventCategoryDTO>>(methodResult);
        //    Assert.Equal(GetEventCategories().Count(),methodResult.Count);
        //}
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
