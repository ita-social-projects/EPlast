using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Events;
using EPlast.BLL.DTO.EventUser;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.BLL.Interfaces.UserAccess;
using EPlast.BLL.Services.EventUser;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace EPlast.XUnitTest.Services.EventUser
{
    public class EventUserManagerTests
    {
        private readonly Mock<IRepositoryWrapper> _repoWrapper;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IEventCategoryManager> _eventCategoryManager;
        private readonly Mock<IEventStatusManager> _eventStatusManager;
        private readonly Mock<IEventAdministrationTypeManager> _eventAdministrationTypeManager;
        private readonly Mock<IUserAccessService> _userAccessService;
        private readonly Mock<UserManager<User>> _userManager;
        private EventUserManager eventUserManager;
        private class DateTimeData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { "06/03/2023", "06/03/2022" };
                yield return new object[] { "06/03/2022", DateTime.Now.ToString() };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public EventUserManagerTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _eventCategoryManager = new Mock<IEventCategoryManager>();
            _eventStatusManager = new Mock<IEventStatusManager>();
            _eventAdministrationTypeManager = new Mock<IEventAdministrationTypeManager>();
            _userAccessService = new Mock<IUserAccessService>();
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            eventUserManager = new EventUserManager(_repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object,
                _eventStatusManager.Object, _eventAdministrationTypeManager.Object, _userManager.Object, _userAccessService.Object);
        }

        [Fact]
        public async Task InitializeEventCreateDTOTest()
        {
            //Arrange
            _eventCategoryManager.Setup(x => x.GetDTOAsync());
            _mapper.Setup(m => m.Map<List<User>, IEnumerable<UserDto>>(It.IsAny<List<User>>()))
                .Returns(new List<UserDto>());
            _mapper.Setup(a => a.Map<List<EventType>, IEnumerable<EventTypeDto>>(It.IsAny<List<EventType>>()))
                .Returns(new List<EventTypeDto>());
            _repoWrapper.Setup(x => x.User.GetFirstAsync(It.IsAny<Expression<Func<User, bool>>>(), null))
                .ReturnsAsync(new User());
            _repoWrapper.Setup(e => e.EventType.GetFirstAsync(It.IsAny<Expression<Func<EventType, bool>>>(), null))
                .ReturnsAsync(new EventType());

            //Act
            var methodResult = await eventUserManager.InitializeEventCreateDTOAsync();

            //Assert
            Assert.NotNull(methodResult);
            Assert.IsType<EventCreateDto>(methodResult);
        }

        [Fact]
        public async Task CreateEventTest()
        {
            int statusId = 1;
            _eventStatusManager.Setup(s => s.GetStatusIdAsync(It.IsAny<string>())).ReturnsAsync(statusId);

            _mapper.Setup(m => m.Map<EventCreationDto, Event>(It.IsAny<EventCreationDto>()))
                .Returns(new Event());
            _repoWrapper.Setup(r => r.EventAdmin.CreateAsync(It.IsAny<EventAdmin>()));
            _repoWrapper.Setup(r => r.EventAdministration.CreateAsync(It.IsAny<EventAdministration>()));
            _repoWrapper.Setup(r => r.Event.CreateAsync(It.IsAny<Event>()));

            //Act
            var methodResult = await eventUserManager.CreateEventAsync(GetEventCreateDTO());

            //Assert
            Assert.IsType<int>(methodResult);
        }

        [Fact]
        public async Task CreateEventTestWithoutAlternate()
        {
            int statusId = 1;
            _eventStatusManager.Setup(s => s.GetStatusIdAsync(It.IsAny<string>())).ReturnsAsync(statusId);

            _mapper.Setup(m => m.Map<EventCreationDto, Event>(It.IsAny<EventCreationDto>()))
                .Returns(new Event());
            _repoWrapper.Setup(r => r.EventAdmin.CreateAsync(It.IsAny<EventAdmin>()));
            _repoWrapper.Setup(r => r.EventAdministration.CreateAsync(It.IsAny<EventAdministration>()));
            _repoWrapper.Setup(r => r.Event.CreateAsync(It.IsAny<Event>()));

            //Act
            var methodResult = await eventUserManager.CreateEventAsync(GetEventCreateDTOWithoutAlternate());

            //Assert
            Assert.IsType<int>(methodResult);
        }

        [Fact]
        public async Task CreateEventTestWithAlternate()
        {
            int statusId = 1;
            _eventStatusManager.Setup(s => s.GetStatusIdAsync(It.IsAny<string>())).ReturnsAsync(statusId);

            _mapper.Setup(m => m.Map<EventCreationDto, Event>(It.IsAny<EventCreationDto>()))
                .Returns(new Event());
            _repoWrapper.Setup(r => r.EventAdmin.CreateAsync(It.IsAny<EventAdmin>()));
            _repoWrapper.Setup(r => r.EventAdministration.CreateAsync(It.IsAny<EventAdministration>()));
            _repoWrapper.Setup(r => r.Event.CreateAsync(It.IsAny<Event>()));

            //Act
            var param = GetEventCreateDTOWithoutAlternate();
            param.Alternate.UserId = "0";
            var methodResult = await eventUserManager.CreateEventAsync(param);

            //Assert
            Assert.IsType<int>(methodResult);
        }


        [Theory]
        [ClassData(typeof(DateTimeData))]
        public async Task CreateEventExceptionTest(string dateStart, string dateEnd)
        {
            var expectedStartDate = DateTime.Parse(dateStart);
            var expectedEndDate = DateTime.Parse(dateEnd);
            int statusId = 1;
            _eventStatusManager.Setup(s => s.GetStatusIdAsync(It.IsAny<string>())).ReturnsAsync(statusId);

            _mapper.Setup(m => m.Map<EventCreationDto, Event>(It.IsAny<EventCreationDto>()))
                .Returns(new Event());
            _repoWrapper.Setup(r => r.EventAdmin.CreateAsync(It.IsAny<EventAdmin>()));
            _repoWrapper.Setup(r => r.EventAdministration.CreateAsync(It.IsAny<EventAdministration>()));
            _repoWrapper.Setup(r => r.Event.CreateAsync(It.IsAny<Event>()));

            //Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => eventUserManager.CreateEventAsync(GetEventCreateDTOException(expectedStartDate, expectedEndDate)));
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
            _repoWrapper.Setup(x => x.EventAdministration.GetFirstAsync(It.IsAny<Expression<Func<EventAdministration, bool>>>(), null))
                .ReturnsAsync(new EventAdministration());

            //Act
            var methodResult = await eventUserManager.InitializeEventEditDTOAsync(eventId);

            //Assert
            Assert.NotNull(methodResult);
            Assert.IsType<EventCreateDto>(methodResult);
        }

        public EventCreateDto GetEventCreateDTO()
        {
            var eventCreate = new EventCreateDto
            {
                Event = new EventCreationDto { EventDateStart = DateTime.Now.AddDays(2), EventDateEnd = DateTime.Now.AddDays(5) },
                Сommandant = new EventAdministrationDto { },
                Alternate = new EventAdministrationDto { },
                Bunchuzhnyi = new EventAdministrationDto { },
                Pysar = new EventAdministrationDto { },
                EventCategories = new List<EventCategoryDto>
                {
                    new EventCategoryDto { }
                },
                EventTypes = new List<EventTypeDto>
                {
                    new EventTypeDto { }
                },
                Users = new List<UserInfoDto>
                {
                    new UserInfoDto { }
                }
            };
            return eventCreate;
        }

        public EventCreateDto GetEventCreateDTOWithoutAlternate()
        {
            var eventCreate = new EventCreateDto
            {
                Event = new EventCreationDto { EventDateStart = DateTime.Now.AddDays(2), EventDateEnd = DateTime.Now.AddDays(5) },
                Сommandant = new EventAdministrationDto { },
                Alternate = new EventAdministrationDto { UserId = null },
                Bunchuzhnyi = new EventAdministrationDto { },
                Pysar = new EventAdministrationDto { },
                EventCategories = new List<EventCategoryDto>
                {
                    new EventCategoryDto { }
                },
                EventTypes = new List<EventTypeDto>
                {
                    new EventTypeDto { }
                },
                Users = new List<UserInfoDto>
                {
                    new UserInfoDto { }
                }
            };
            return eventCreate;
        }

        public EventCreateDto GetEventCreateDTOException(DateTime startDate, DateTime endDate)
        {
            var eventCreate = new EventCreateDto
            {
                Event = new EventCreationDto { EventDateStart = startDate, EventDateEnd = endDate },
                Сommandant = new EventAdministrationDto { },
                Alternate = new EventAdministrationDto { },
                Bunchuzhnyi = new EventAdministrationDto { },
                Pysar = new EventAdministrationDto { },
                EventCategories = new List<EventCategoryDto>
                {
                    new EventCategoryDto { }
                },
                EventTypes = new List<EventTypeDto>
                {
                    new EventTypeDto { }
                },
                Users = new List<UserInfoDto>
                {
                    new UserInfoDto { }
                }
            };
            return eventCreate;
        }

    }
}
