//using EPlast.BussinessLayer.Services.Events;
//using EPlast.DataAccess.Entities.Event;
//using EPlast.DataAccess.Repositories;
//using Moq;
//using System.Collections.Generic;
//using Xunit;

//namespace EPlast.XUnitTest.Services.Events
//{
//    public class EventAdminManagerTests
//    {
//        private readonly Mock<IRepositoryWrapper> _repoWrapper;

//        public EventAdminManagerTests()
//        {
//            _repoWrapper = new Mock<IRepositoryWrapper>();
//        }

//        [Fact]
//        public void GetEventAdminByUserId()
//        {
//            //Arrange
//            string userId = "1";
//            _repoWrapper.Setup(x => x.EventAdmin.FindByCondition(q => q.UserID == userId));
//            //Act
//            var eventAdminManager = new EventAdminManager(_repoWrapper.Object);
//            var methodResult = eventAdminManager.GetEventAdminsByUserId(userId);

//            //Assert
//            Assert.NotNull(methodResult);
//            Assert.IsType<List<EventAdmin>>(methodResult);
//        }
//    }
//}
