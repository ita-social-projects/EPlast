//using AutoMapper;
//using EPlast.BussinessLayer.DTO.EventUser;
//using EPlast.BussinessLayer.DTO.UserProfiles;
//using EPlast.BussinessLayer.Interfaces.Events;
//using EPlast.BussinessLayer.Services.EventUser;
//using EPlast.DataAccess.Entities;
//using EPlast.DataAccess.Repositories;
//using EPlast.ViewModels.EventUser;
//using Microsoft.AspNetCore.Identity;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Security.Claims;
//using System.Security.Cryptography.X509Certificates;
//using Xunit;

//namespace EPlast.XUnitTest.Services.Events
//{
//    public class EventUserManagerTests
//    {
//        private readonly Mock<IRepositoryWrapper> _repoWrapper;
//        private readonly Mock<UserManager<User>> _userManager;
//        private readonly Mock<IMapper> _mapper;
//        private readonly Mock<IEventAdminManager> _eventAdminManager;
//        private readonly Mock<IParticipantStatusManager> _participantStatusManager;
//        private readonly Mock<IParticipantManager> _participantManager;
//        private readonly Mock<IEventCategoryManager> _eventCategoryManager;
//        private readonly Mock<IEventStatusManager> _eventStatusManager;


//        public EventUserManagerTests()
//        {
//            var userStore = new Mock<IUserStore<User>>();
//            _repoWrapper = new Mock<IRepositoryWrapper>();
//            _userManager = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);
//            _mapper = new Mock<IMapper>();
//            _eventAdminManager = new Mock<IEventAdminManager>();
//            _participantStatusManager = new Mock<IParticipantStatusManager>();
//            _participantManager = new Mock<IParticipantManager>();
//            _eventCategoryManager = new Mock<IEventCategoryManager>();
//            _eventStatusManager = new Mock<IEventStatusManager>();

//        }

//        [Fact]
//        public void EventUserSuccessTest()
//        {
//            string expectedID = "abc-1";
//            string eventId = "abc";
            

//            _eventAdminManager.Setup(x => x.GetEventAdminsByUserId(It.IsAny<string>()));
//            _userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
//                .Returns(expectedID);
//            _mapper.Setup(m => m.Map<User, UserDTO>(It.IsAny<User>())).Returns(new UserDTO());
//            _repoWrapper.Setup(x => x.User.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()))
//                .Returns(new );


//            var eventUserManager = new EventUserManager(_repoWrapper.Object, _userManager.Object, _mapper.Object, _eventAdminManager.Object,
//                 _participantStatusManager.Object, _participantManager.Object, _eventCategoryManager.Object, _eventStatusManager.Object);
//            var methodResult = eventUserManager.EventUser(eventId, new ClaimsPrincipal());
//            //Assert
//            Assert.NotNull(methodResult);
//            Assert.IsType<List<EventUserDTO>>(methodResult);
//            //Assert.Equal(GetEventsUser().Count(), methodResult.);

//        }


//        public IQueryable<EventUserDTO> GetEventsUserDTO()
//        {
//            var events = new List<EventUserDTO>
//            {
//                new EventUserDTO
//                {
//                    User = new UserDTO
//                    {
//                        Id = "1",
//                        FirstName = "Ігор",
//                        LastName = "Ігоренко",
//                        ImagePath = "picture.jpg",
//                    },
//                     PlanedEvents = new List<EventGeneralInfoDTO>
//                     {
//                         new EventGeneralInfoDTO{ID = 1, EventDateStart = DateTime.Now, EventDateEnd = DateTime.Now }
//                     },
//                     CreatedEvents = new List<EventGeneralInfoDTO>
//                     {
//                         new EventGeneralInfoDTO{ID = 1, EventDateStart = DateTime.Now, EventDateEnd = DateTime.Now }
//                     },
//                     VisitedEvents = new List<EventGeneralInfoDTO>
//                     {
//                         new EventGeneralInfoDTO{ID = 1, EventDateStart = DateTime.Now, EventDateEnd = DateTime.Now }
//                     },
//                },
//                 new EventUserDTO
//                {
//                    User = new UserDTO
//                    {
//                        Id = "2",
//                        FirstName = "Іван",
//                        LastName = "Іваненко",
//                        ImagePath = "picture.jpg",
//                    },
//                     PlanedEvents = new List<EventGeneralInfoDTO>
//                     {
//                         new EventGeneralInfoDTO{ID = 1, EventDateStart = DateTime.Now, EventDateEnd = DateTime.Now}
//                     },
//                     CreatedEvents = new List<EventGeneralInfoDTO>
//                     {
//                         new EventGeneralInfoDTO{ID = 1,EventDateStart = DateTime.Now, EventDateEnd = DateTime.Now}
//                     },
//                     VisitedEvents = new List<EventGeneralInfoDTO>
//                     {
//                         new EventGeneralInfoDTO{ID = 1, EventDateStart = DateTime.Now, EventDateEnd = DateTime.Now}
//                     }
//                }
//            }.AsQueryable();
//            return events;


//        }







//    }
//}
