using AutoMapper;
using EPlast.BussinessLayer.DTO.Events;
using EPlast.BussinessLayer.DTO.EventUser;
using EPlast.BussinessLayer.DTO.UserProfiles;
using EPlast.BussinessLayer.Interfaces.Events;
using EPlast.BussinessLayer.Services.EventUser;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using Xunit;

namespace EPlast.XUnitTest.Services.Events
{
    public class EventUserManagerTests
    {
        private readonly Mock<IRepositoryWrapper> _repoWrapper;
        private readonly Mock<UserManager<User>> _userManager;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IEventAdminManager> _eventAdminManager;
        private readonly Mock<IParticipantManager> _participantManager;
        private readonly Mock<IParticipantStatusManager> _participantStatusManager;
        private readonly Mock<IEventCategoryManager> _eventCategoryManager;
        private readonly Mock<IEventStatusManager> _eventStatusManager;


        public EventUserManagerTests()
        {
            var userStore = new Mock<IUserStore<User>>();
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _userManager = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);
            _participantStatusManager = new Mock<IParticipantStatusManager>();
            _mapper = new Mock<IMapper>();
            _participantManager = new Mock<IParticipantManager>();
            _eventAdminManager = new Mock<IEventAdminManager>();
            _eventCategoryManager = new Mock<IEventCategoryManager>();
            _eventStatusManager = new Mock<IEventStatusManager>();

        }

        [Fact]
        public void EventUserSuccessTest()
        {
            //Arrange
            string expectedID = "abc-1";
            string eventUserId = "abc";

            _userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(expectedID);
            _eventAdminManager.Setup(x => x.GetEventAdminsByUserId(It.IsAny<string>()));
            _participantManager.Setup(x => x.GetParticipantsByUserId(eventUserId));
            _repoWrapper.Setup(x => x.User.FindByCondition(q => q.Id == eventUserId));

            _mapper.Setup(m => m.Map<User, UserDTO>(It.IsAny<User>())).Returns(new UserDTO());
            _repoWrapper.Setup(x => x.User.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(GetUsers());
            //Act
            var eventUserManager = new EventUserManager(_repoWrapper.Object, _userManager.Object, _participantStatusManager.Object,
                _mapper.Object, _participantManager.Object, _eventAdminManager.Object, _eventCategoryManager.Object,
                _eventStatusManager.Object);
            var methodResult = eventUserManager.EventUser(eventUserId, new ClaimsPrincipal());
            //Assert
            Assert.NotNull(methodResult);
            Assert.IsType<EventUserDTO>(methodResult);
        }

        [Fact]
        public void InitializeEventCreateDTOTest()
        {
            //Arrange
            _eventCategoryManager.Setup(x => x.GetDTO());
            _mapper.Setup(m => m.Map<List<User>, IEnumerable<UserDTO>>(It.IsAny<List<User>>())).Returns(new List<UserDTO>());
            _mapper.Setup(a => a.Map<List<EventType>, IEnumerable<EventTypeDTO>>(It.IsAny<List<EventType>>()))
                .Returns(new List<EventTypeDTO>());
            _repoWrapper.Setup(r => r.User.FindAll()).Returns(GetUsers());
            _repoWrapper.Setup(e => e.EventType.FindAll()).Returns(GetEventTypes());

            //Act
            var eventUserManager = new EventUserManager(_repoWrapper.Object, _userManager.Object, _participantStatusManager.Object,
               _mapper.Object, _participantManager.Object, _eventAdminManager.Object, _eventCategoryManager.Object,
               _eventStatusManager.Object);
            var methodResult = eventUserManager.InitializeEventCreateDTO();

            //Assert
            Assert.NotNull(methodResult);
            Assert.IsType<EventCreateDTO>(methodResult);
        }

        [Fact]
        public void InitializeEventCreateDTOTSetAdministrationTest()
        {
            //Assert
            _mapper.Setup(m => m.Map<Event, EventDTO>(It.IsAny<Event>())).Returns(new EventDTO());
            _mapper.Setup(m => m.Map<List<User>, IEnumerable<UserDTO>>(It.IsAny<List<User>>())).Returns(new List<UserDTO>());
            _mapper.Setup(m => m.Map<List<Event>, IEnumerable<EventAdminDTO>>(It.IsAny<List<Event>>())).
                Returns(new List<EventAdminDTO>());

            _repoWrapper.Setup(r => r.Event.FindByCondition(It.IsAny<Expression<Func<Event, bool>>>()))
                .Returns(GetEvents());
            _repoWrapper.Setup(r => r.EventAdmin.FindByCondition(It.IsAny<Expression<Func<EventAdmin, bool>>>()))
                .Returns(GetEventAdmins());

            _repoWrapper.Setup(r => r.User.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).Returns(GetUsers());

            int eventId = 1;

            //Act
            var eventUserManager = new EventUserManager(_repoWrapper.Object, _userManager.Object, _participantStatusManager.Object,
              _mapper.Object, _participantManager.Object, _eventAdminManager.Object, _eventCategoryManager.Object,
              _eventStatusManager.Object);
            var methodResult = eventUserManager.InitializeEventCreateDTO(eventId);

            //Assert
            Assert.NotNull(methodResult);
            Assert.IsType<EventCreateDTO>(methodResult);
        }

        [Fact]
        public void CreateEventTest()
        {
            int statusId = 1;
            _eventStatusManager.Setup(s => s.GetStatusId(It.IsAny<string>())).Returns(statusId);

            _mapper.Setup(m => m.Map<EventCreationDTO, Event>(It.IsAny<EventCreationDTO>()))
                .Returns(new Event());
            _repoWrapper.Setup(r => r.EventAdmin.Create(It.IsAny<EventAdmin>()));
            _repoWrapper.Setup(r => r.EventAdministration.Create(It.IsAny<EventAdministration>()));
            _repoWrapper.Setup(r => r.Event.Create(It.IsAny<Event>()));

            //Act
            var eventUserManager = new EventUserManager(_repoWrapper.Object, _userManager.Object, _participantStatusManager.Object,
              _mapper.Object, _participantManager.Object, _eventAdminManager.Object, _eventCategoryManager.Object,
              _eventStatusManager.Object);
            var methodResult = eventUserManager.CreateEvent(GetEventCreateDTO());

            //Assert
            Assert.IsType<int>(methodResult);

        }

        [Fact]
        public void SetAdministrationTest()
        {
            //Assert
            EventAdmin eventAdmin = new EventAdmin
            {
                Event = new Event { },
                EventID = 1,
                User = new User { },
                UserID = "1"
            };

            EventAdministration eventAdministration = new EventAdministration
            {
                Event = new Event { },
                EventID = 1,
                AdministrationType = "АБВ",
                ID = 2,
                User = new User { },
                UserID = "2"
            };
            _repoWrapper.Setup(r => r.EventAdmin.Create(eventAdmin));
            _repoWrapper.Setup(r => r.EventAdministration.Create(eventAdministration));

            //Act
            var eventUserManager = new EventUserManager(_repoWrapper.Object, _userManager.Object, _participantStatusManager.Object,
               _mapper.Object, _participantManager.Object, _eventAdminManager.Object, _eventCategoryManager.Object,
               _eventStatusManager.Object);
            eventUserManager.SetAdministration(GetEventCreateDTO());
            //Assert

        }

        public EventCreateDTO GetEventCreateDTO()
        {
            var eventCreate = new EventCreateDTO
            {

                Event = new EventCreationDTO { },
                EventAdmin = new CreateEventAdminDTO { },
                EventAdministration = new EventAdministrationDTO { },
                EventCategories = new List<EventCategoryDTO>
                    {
                        new EventCategoryDTO{}
                    },
                EventTypes = new List<EventTypeDTO>
                    {
                        new EventTypeDTO{}
                    },
                Users = new List<UserDTO>
                    {
                        new UserDTO{}
                    }

            };
            return eventCreate;
        }


        public IQueryable<EventUserDTO> GetEventsUserDTO()
        {
            var events = new List<EventUserDTO>
            {
                new EventUserDTO
                {
                    User = new UserDTO
                    {
                        Id = "1",
                        FirstName = "Ігор",
                        LastName = "Ігоренко",
                        ImagePath = "picture.jpg",
                    },
                     PlanedEvents = new List<EventGeneralInfoDTO>
                     {
                         new EventGeneralInfoDTO{ID = 1, EventDateStart = DateTime.Now, EventDateEnd = DateTime.Now }
                     },
                     CreatedEvents = new List<EventGeneralInfoDTO>
                     {
                         new EventGeneralInfoDTO{ID = 1, EventDateStart = DateTime.Now, EventDateEnd = DateTime.Now }
                     },
                     VisitedEvents = new List<EventGeneralInfoDTO>
                     {
                         new EventGeneralInfoDTO{ID = 1, EventDateStart = DateTime.Now, EventDateEnd = DateTime.Now }
                     },
                },
                 new EventUserDTO
                {
                    User = new UserDTO
                    {
                        Id = "2",
                        FirstName = "Іван",
                        LastName = "Іваненко",
                        ImagePath = "picture.jpg",
                    },
                     PlanedEvents = new List<EventGeneralInfoDTO>
                     {
                         new EventGeneralInfoDTO{ID = 1, EventDateStart = DateTime.Now, EventDateEnd = DateTime.Now}
                     },
                     CreatedEvents = new List<EventGeneralInfoDTO>
                     {
                         new EventGeneralInfoDTO{ID = 1,EventDateStart = DateTime.Now, EventDateEnd = DateTime.Now}
                     },
                     VisitedEvents = new List<EventGeneralInfoDTO>
                     {
                         new EventGeneralInfoDTO{ID = 1, EventDateStart = DateTime.Now, EventDateEnd = DateTime.Now}
                     }
                }
            }.AsQueryable();
            return events;
        }

        public IQueryable<User> GetUsers()
        {
            var users = new List<User>
            {
                new User
                {
                    FirstName = "Іван",
                    LastName = "Іваненко",
                    FatherName = "Іванович",
                    PhoneNumber = "0930146434",
                    RegistredOn = DateTime.Now,
                    EmailSendedOnRegister = DateTime.Now,
                    EmailSendedOnForgotPassword = DateTime.Now,
                    ImagePath = "picture.jpg",
                    SocialNetworking = true
                },
            }.AsQueryable();
            return users;
        }

        public IQueryable<EventType> GetEventTypes()
        {
            var types = new List<EventType>
            {
                new EventType
                {
                    ID = 1,
                    EventTypeName = "Тестова подія",
                    Events = new List<Event> { }
                }
            }.AsQueryable();
            return types;
        }

        public IQueryable<Event> GetEvents()
        {
            var events = new List<Event>
            {
                new Event
                {
                ID = 1,
                EventName = "тест",
                Description = "опис",
                Questions = "питання",
                EventDateStart = DateTime.Now,
                EventDateEnd = DateTime.Now,
                Eventlocation = "Львів",
                EventCategoryID = 3,
                EventStatusID = 2,
                EventTypeID = 1,
                FormOfHolding = "абв",
                ForWhom = "дітей",
                NumberOfPartisipants = 1,

                    EventAdmins = new  List<EventAdmin>
                   {
                       new EventAdmin
                       {
                       Event = new Event{},
                       EventID = 1,
                       User = new User{},
                       UserID = "1",
                       }

                   }
                }
            }.AsQueryable();
            return events;
        }

        public IQueryable<EventAdmin> GetEventAdmins()
        {
            var events = new List<EventAdmin>
            {
               new EventAdmin
               {
                       Event = new Event{},
                       EventID = 1,
                       User = new User{},
                       UserID = "1",
               }

            }.AsQueryable();
            return events;
        }
    }
}
