using AutoMapper;
using EPlast.BLL.DTO.Events;
using EPlast.BLL.DTO.EventUser;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Services.EventUser;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
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
            _userManager =
                new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);
            _participantStatusManager = new Mock<IParticipantStatusManager>();
            _mapper = new Mock<IMapper>();
            _participantManager = new Mock<IParticipantManager>();
            _eventAdminManager = new Mock<IEventAdminManager>();
            _eventCategoryManager = new Mock<IEventCategoryManager>();
            _eventStatusManager = new Mock<IEventStatusManager>();

        }

        [Fact]
        public async Task EventUserSuccessTest()
        {
            //Arrange
            string expectedID = "abc-1";
            string eventUserId = "abc";
            _userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(expectedID);
            _repoWrapper.Setup(x => x.User.GetFirstAsync(It.IsAny<Expression<Func<User, bool>>>(), null))
                .ReturnsAsync(new User());
            _mapper.Setup(m => m.Map<User, UserDTO>(It.IsAny<User>())).Returns(new UserDTO());
            _eventAdminManager.Setup(x => x.GetEventAdminsByUserIdAsync(eventUserId));
            _participantManager.Setup(x => x.GetParticipantsByUserIdAsync(eventUserId));

            //Act
            var eventUserManager = new EventUserManager(_repoWrapper.Object, _userManager.Object,
                _participantStatusManager.Object,
                _mapper.Object, _participantManager.Object, _eventAdminManager.Object, _eventCategoryManager.Object,
                _eventStatusManager.Object);
            var methodResult = await eventUserManager.EventUserAsync(eventUserId, new ClaimsPrincipal());
            //Assert
            Assert.NotNull(methodResult);
            Assert.IsType<EventUserDTO>(methodResult);
        }

        [Fact]
        public async Task InitializeEventCreateDTOTest()
        {
            //Arrange
            _eventCategoryManager.Setup(x => x.GetDTOAsync());
            _mapper.Setup(m => m.Map<List<User>, IEnumerable<UserDTO>>(It.IsAny<List<User>>()))
                .Returns(new List<UserDTO>());
            _mapper.Setup(a => a.Map<List<EventType>, IEnumerable<EventTypeDTO>>(It.IsAny<List<EventType>>()))
                .Returns(new List<EventTypeDTO>());
            _repoWrapper.Setup(x => x.User.GetFirstAsync(It.IsAny<Expression<Func<User, bool>>>(), null))
                .ReturnsAsync(new User());
            _repoWrapper.Setup(e => e.EventType.GetFirstAsync(It.IsAny<Expression<Func<EventType, bool>>>(), null))
                .ReturnsAsync(new EventType());

            //Act
            var eventUserManager = new EventUserManager(_repoWrapper.Object, _userManager.Object,
                _participantStatusManager.Object,
                _mapper.Object, _participantManager.Object, _eventAdminManager.Object, _eventCategoryManager.Object,
                _eventStatusManager.Object);
            var methodResult = await eventUserManager.InitializeEventCreateDTOAsync();

            //Assert
            Assert.NotNull(methodResult);
            Assert.IsType<EventCreateDTO>(methodResult);
        }

        [Fact]
        public async Task InitializeEventCreateDTOTSetAdministrationTest()
        {
            //Assert
            _mapper.Setup(m => m.Map<List<User>, IEnumerable<UserDTO>>(It.IsAny<List<User>>()))
                .Returns(new List<UserDTO>());
            _mapper.Setup(m => m.Map<Event, EventCreationDTO>(It.IsAny<Event>()))
                .Returns(new EventCreationDTO());

            _repoWrapper.Setup(r => r.Event.GetFirstAsync(It.IsAny<Expression<Func<Event, bool>>>(), It
                .IsAny<Func<IQueryable<Event>,
                    IIncludableQueryable<Event, object>>>())).ReturnsAsync(
                new Event()
                {
                    EventAdmins = new List<EventAdmin>() { GetEventAdmins() }
                }
                );
            _repoWrapper.Setup(x => x.User.GetFirstAsync(It.IsAny<Expression<Func<User, bool>>>(), null))
                .ReturnsAsync(new User());

            int eventId = 1;

            //Act
            var eventUserManager = new EventUserManager(_repoWrapper.Object, _userManager.Object,
                _participantStatusManager.Object,
                _mapper.Object, _participantManager.Object, _eventAdminManager.Object, _eventCategoryManager.Object,
                _eventStatusManager.Object);
            var methodResult = await eventUserManager.InitializeEventCreateDTOAsync(eventId);

            //Assert
            Assert.NotNull(methodResult);
            Assert.IsType<EventCreateDTO>(methodResult);
        }

        [Fact]
        public async Task CreateEventTest()
        {
            int statusId = 1;
            _eventStatusManager.Setup(s => s.GetStatusIdAsync(It.IsAny<string>())).ReturnsAsync(statusId);

            _mapper.Setup(m => m.Map<EventCreationDTO, Event>(It.IsAny<EventCreationDTO>()))
                .Returns(new Event());
            _repoWrapper.Setup(r => r.EventAdmin.CreateAsync(It.IsAny<EventAdmin>()));
            _repoWrapper.Setup(r => r.EventAdministration.CreateAsync(It.IsAny<EventAdministration>()));
            _repoWrapper.Setup(r => r.Event.CreateAsync(It.IsAny<Event>()));

            //Act
            var eventUserManager = new EventUserManager(_repoWrapper.Object, _userManager.Object,
                _participantStatusManager.Object,
                _mapper.Object, _participantManager.Object, _eventAdminManager.Object, _eventCategoryManager.Object,
                _eventStatusManager.Object);
            var methodResult = await eventUserManager.CreateEventAsync(GetEventCreateDTO());

            //Assert
            Assert.IsType<int>(methodResult);

        }

        [Fact]
        public async Task SetAdministrationTest()
        {
            //Assert
            _repoWrapper.Setup(r => r.EventAdmin.CreateAsync(It.IsAny<EventAdmin>()));
            _repoWrapper.Setup(r => r.EventAdministration.CreateAsync(It.IsAny<EventAdministration>()));

            //Act
            var eventUserManager = new EventUserManager(_repoWrapper.Object, _userManager.Object,
                _participantStatusManager.Object,
                _mapper.Object, _participantManager.Object, _eventAdminManager.Object, _eventCategoryManager.Object,
                _eventStatusManager.Object);
            await eventUserManager.SetAdministrationAsync(GetEventCreateDTO());
            //Assert
        }

        [Fact]
        public async Task InitializeEventEditDTOTest()
        {
            //Arrange
            int eventId = 1;

            _repoWrapper.Setup(r => r.Event.GetFirstAsync(It.IsAny<Expression<Func<Event, bool>>>(), null))
                .ReturnsAsync(new Event());
            _repoWrapper.Setup(r => r.EventType.GetFirstAsync(It.IsAny<Expression<Func<EventType, bool>>>(), null))
                .ReturnsAsync(new EventType());
            _repoWrapper.Setup(x => x.User.GetFirstAsync(It.IsAny<Expression<Func<User, bool>>>(), null))
                .ReturnsAsync(new User());
            _eventCategoryManager.Setup(x => x.GetDTOAsync());

            var eventUserManager = new EventUserManager(_repoWrapper.Object, _userManager.Object,
                _participantStatusManager.Object,
                _mapper.Object, _participantManager.Object, _eventAdminManager.Object, _eventCategoryManager.Object,
                _eventStatusManager.Object);
            var methodResult = await eventUserManager.InitializeEventEditDTOAsync(eventId);

            //Assert
            Assert.NotNull(methodResult);
            Assert.IsType<EventCreateDTO>(methodResult);
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
                    new EventCategoryDTO { }
                },
                EventTypes = new List<EventTypeDTO>
                {
                    new EventTypeDTO { }
                },
                Users = new List<UserDTO>
                {
                    new UserDTO { }
                }

            };

            return eventCreate;
        }

        public EventAdmin GetEventAdmins()
        {

            return new EventAdmin
            {
                Event = new Event { },
                EventID = 1,
                User = new User { },
                UserID = "1",
            };
        }
    }
}