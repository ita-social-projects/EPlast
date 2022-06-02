using AutoMapper;
using EPlast.BLL.DTO.Events;
using EPlast.BLL.DTO.EventUser;
using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Interfaces.Notifications;
using EPlast.BLL.Services.Events;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
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
    public class ActionManagerTests
    {
        private readonly Mock<IRepositoryWrapper> _repoWrapper;
        private readonly Mock<UserManager<User>> _userManager;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IParticipantStatusManager> _participantStatusManager;
        private readonly Mock<IParticipantManager> _participantManager;
        private readonly Mock<IEventWrapper> _eventWrapper;
        private readonly Mock<INotificationService> _mockNotificationService;

        public ActionManagerTests()
        {
            var userStore = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _participantStatusManager = new Mock<IParticipantStatusManager>();
            _participantManager = new Mock<IParticipantManager>();
            _eventWrapper = new Mock<IEventWrapper>();
            _mockNotificationService = new Mock<INotificationService>();
        }

        [Fact]
        public async Task GetEventTypesTestAsync()
        {
            //Arrange

            _eventWrapper.Setup(x => x.EventTypeManager.GetEventTypesDTOAsync())
                .ReturnsAsync(new List<EventTypeDTO>());
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _participantStatusManager.Object, _participantManager.Object, _eventWrapper.Object, _mockNotificationService.Object);
            var methodResult = await actionManager.GetEventTypesAsync();
            //Assert
            Assert.NotNull(methodResult);
            Assert.IsAssignableFrom<IEnumerable<EventTypeDTO>>(methodResult);
        }

        [Fact]
        public async Task GetActionCategoriesTestAsync()
        {
            //Arrange
            _eventWrapper.Setup(x => x.EventCategoryManager.GetDTOAsync())
                .ReturnsAsync(new List<EventCategoryDTO>());
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _participantStatusManager.Object, _participantManager.Object, _eventWrapper.Object, _mockNotificationService.Object);
            var methodResult = await actionManager.GetActionCategoriesAsync();
            //Assert
            Assert.NotNull(methodResult);
            Assert.IsType<List<EventCategoryDTO>>(methodResult);
        }

        [Fact]
        public async Task GetCategoriesByTypeIdTestAsync()
        {
            //Arrange
            var eventTypeId = 1;
            _eventWrapper.Setup(x => x.EventCategoryManager.GetDTOByEventTypeIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<EventCategoryDTO>());
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _participantStatusManager.Object, _participantManager.Object, _eventWrapper.Object, _mockNotificationService.Object);
            var methodResult = await actionManager.GetCategoriesByTypeIdAsync(eventTypeId);
            //Assert
            Assert.NotNull(methodResult);
            Assert.IsAssignableFrom<IEnumerable<EventCategoryDTO>>(methodResult);
        }

        [Fact]
        public async Task GetEventSectionsTestAsync()
        {
            //Arrange
            _eventWrapper.Setup(x => x.EventSectionManager.GetEventSectionsDTOAsync())
                .ReturnsAsync(new List<EventSectionDTO>());
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _participantStatusManager.Object, _participantManager.Object, _eventWrapper.Object, _mockNotificationService.Object);
            var methodResult = await actionManager.GetEventSectionsAsync();
            //Assert
            Assert.NotNull(methodResult);
            Assert.IsAssignableFrom<IEnumerable<EventSectionDTO>>(methodResult);
        }

        [Fact]
        public async Task GetEventsTestAsync()
        {
            //Arrange
            string expectedID = "abc-1";
            int typeId = 3;
            int categoryId = 3;
            int fakeId = 3;
            User user = new User();
            user.Id = "abc-1";
            var adm = new List<EventAdministration>()
                { new EventAdministration()
                      { Event = new Event(),
                        EventID = fakeId } }.ToList();
            _eventWrapper.Setup(x => x.EventStatusManager.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(fakeId);
            _participantStatusManager.Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(fakeId);
            _eventWrapper.Setup(x => x.EventTypeManager.GetTypeIdAsync(It.IsAny<string>()))
                .ReturnsAsync(fakeId);
            _userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(expectedID);
            _repoWrapper.Setup(x => x.Event.GetAllAsync(It.IsAny<Expression<Func<Event, bool>>>(), It.IsAny<Func<IQueryable<Event>, IIncludableQueryable<Event, object>>>()))
                .ReturnsAsync(GetEvents());
            _repoWrapper.Setup(x => x.EventAdministration.GetAllAsync(It.IsAny<Expression<Func<EventAdministration, bool>>>(), It.IsAny<Func<IQueryable<EventAdministration>, IIncludableQueryable<EventAdministration, object>>>()))
                .ReturnsAsync(adm);
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _participantStatusManager.Object, _participantManager.Object, _eventWrapper.Object, _mockNotificationService.Object);
            var methodResult = await actionManager.GetEventsAsync(categoryId, typeId, user);
            //Assert
            Assert.NotNull(methodResult);
            Assert.IsType<List<GeneralEventDTO>>(methodResult);
            Assert.Equal(GetEvents().Count(), methodResult.Count());
        }
        [Fact]
        public async Task GetEventInfoTestAsync()
        {
            //Arrange
            string expectedID = "abc-1";
            int eventId = 3;
            int fakeId = 3;
            _eventWrapper.Setup(x => x.EventStatusManager.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(fakeId);
            _participantStatusManager.Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(fakeId);
            _userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(expectedID);
            _mapper.Setup(m => m.Map<Event, EventInfoDTO>(It.IsAny<Event>())).Returns(new EventInfoDTO());
            _repoWrapper.Setup(x => x.Event.GetFirstAsync(It.IsAny<Expression<Func<Event, bool>>>(), It.IsAny<Func<IQueryable<Event>, IIncludableQueryable<Event, object>>>()))
                .ReturnsAsync(GetEvents().First());
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _participantStatusManager.Object, _participantManager.Object, _eventWrapper.Object, _mockNotificationService.Object);
            var methodResult = await actionManager.GetEventInfoAsync(eventId, new User());
            //Assert
            Assert.NotNull(methodResult);
            Assert.IsType<EventDTO>(methodResult);
        }

        [Fact]
        public async Task DeleteEventSuccessTestAsync()
        {
            //Arrange
            int testEventId = 2;
            _repoWrapper.Setup(x => x.Event.GetFirstAsync(It.IsAny<Expression<Func<Event, bool>>>(), null))
                .ReturnsAsync(GetEvents().First());
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _participantStatusManager.Object, _participantManager.Object, _eventWrapper.Object, _mockNotificationService.Object);
            var methodResult = await actionManager.DeleteEventAsync(testEventId);
            //Assert
            _repoWrapper.Verify(r => r.Event.Delete(It.IsAny<Event>()), Times.Once());
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once());
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }

        [Fact]
        public async Task DeleteEventFailTestAsync()
        {
            //Arrange
            int testEventId = 2;
            _repoWrapper.Setup(x => x.Event.GetFirstAsync(It.IsAny<Expression<Func<Event, bool>>>(), null))
                .ThrowsAsync(new Exception());
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _participantStatusManager.Object, _participantManager.Object, _eventWrapper.Object, _mockNotificationService.Object);
            var methodResult = await actionManager.DeleteEventAsync(testEventId);
            //Assert
            Assert.Equal(StatusCodes.Status400BadRequest, methodResult);
        }

        [Fact]
        public async Task SubscribeOnEventSuccessTestAsync()
        {
            //Arrange
            int testEventId = 2;
            string userId = "051fe5ee6ge";
            _repoWrapper.Setup(x => x.Event.GetFirstAsync(It.IsAny<Expression<Func<Event, bool>>>(), null))
                .ReturnsAsync(GetEvents().First());
            _userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);
            _participantManager.Setup(x => x.SubscribeOnEventAsync(It.IsAny<Event>(), It.IsAny<string>()))
                .ReturnsAsync(StatusCodes.Status200OK);
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _participantStatusManager.Object, _participantManager.Object, _eventWrapper.Object, _mockNotificationService.Object);
            var methodResult = await actionManager.SubscribeOnEventAsync(testEventId, new User());
            //Assert
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }
        [Fact]
        public async Task SubscribeOnEventFailTestAsync()
        {
            //Arrange
            int testEventId = 2;
            string userId = "051fe5ee6ge";
            _repoWrapper.Setup(x => x.Event.GetFirstAsync(It.IsAny<Expression<Func<Event, bool>>>(), null))
                .ReturnsAsync(GetEvents().First());
            _userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);
            _participantManager.Setup(x => x.SubscribeOnEventAsync(It.IsAny<Event>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception());
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _participantStatusManager.Object, _participantManager.Object, _eventWrapper.Object, _mockNotificationService.Object);
            var methodResult = await actionManager.SubscribeOnEventAsync(testEventId, new User());
            //Assert
            Assert.Equal(StatusCodes.Status400BadRequest, methodResult);
        }

        [Fact]
        public async Task UnSubscribeOnEventSuccessTestAsync()
        {
            //Arrange
            int testEventId = 2;
            string userId = "051fe5ee6ge";
            _repoWrapper.Setup(x => x.Event.GetFirstAsync(It.IsAny<Expression<Func<Event, bool>>>(), null))
                .ReturnsAsync(GetEvents().First());
            _userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);
            _participantManager.Setup(x => x.UnSubscribeOnEventAsync(It.IsAny<Event>(), It.IsAny<string>()))
                .ReturnsAsync(StatusCodes.Status200OK);
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _participantStatusManager.Object, _participantManager.Object, _eventWrapper.Object, _mockNotificationService.Object);
            var methodResult = await actionManager.UnSubscribeOnEventAsync(testEventId, new User());
            //Assert
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }
        [Fact]
        public async Task UnSubscribeOnEventFailTestAsync()
        {
            //Arrange
            int testEventId = 2;
            string userId = "051fe5ee6ge";
            _repoWrapper.Setup(x => x.Event.GetFirstAsync(It.IsAny<Expression<Func<Event, bool>>>(), null))
                .ReturnsAsync(GetEvents().First());
            _userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);
            _participantManager.Setup(x => x.UnSubscribeOnEventAsync(It.IsAny<Event>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception());
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _participantStatusManager.Object, _participantManager.Object, _eventWrapper.Object, _mockNotificationService.Object);
            var methodResult = await actionManager.UnSubscribeOnEventAsync(testEventId, new User());
            //Assert
            Assert.Equal(StatusCodes.Status400BadRequest, methodResult);
        }

        [Fact]
        public async Task ApproveParticipantTestAsync()
        {
            //Arrange
            int testParticipantId = 3;
            _participantManager.Setup(x => x.ChangeStatusToApprovedAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status200OK);
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _participantStatusManager.Object, _participantManager.Object, _eventWrapper.Object, _mockNotificationService.Object);
            var methodResult = await actionManager.ApproveParticipantAsync(testParticipantId);
            //Assert
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }
        [Fact]
        public async Task UnderReviewParticipantTestAsync()
        {
            //Arrange
            int testParticipantId = 3;
            _participantManager.Setup(x => x.ChangeStatusToUnderReviewAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status200OK);
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _participantStatusManager.Object, _participantManager.Object, _eventWrapper.Object, _mockNotificationService.Object);
            var methodResult = await actionManager.UnderReviewParticipantAsync(testParticipantId);
            //Assert
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }
        [Fact]
        public async Task RejectParticipantTestAsync()
        {
            //Arrange
            int testParticipantId = 3;
            _participantManager.Setup(x => x.ChangeStatusToRejectedAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status200OK);
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _participantStatusManager.Object, _participantManager.Object, _eventWrapper.Object, _mockNotificationService.Object);
            var methodResult = await actionManager.RejectParticipantAsync(testParticipantId);
            //Assert
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }
        [Fact]
        public async Task GetPicturesTestAsync()
        {
            //Arrange
            int eventId = 3;
            _eventWrapper.Setup(x => x.EventGalleryManager.GetPicturesInBase64(It.IsAny<int>()))
                .ReturnsAsync(new List<EventGalleryDTO>());
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _participantStatusManager.Object, _participantManager.Object, _eventWrapper.Object, _mockNotificationService.Object);
            var methodResult = await actionManager.GetPicturesAsync(eventId);
            //Assert
            Assert.NotNull(methodResult);
            Assert.IsType<List<EventGalleryDTO>>(methodResult);
        }
        [Fact]
        public async Task FillEventGalleryTestAsync()
        {
            //Arrange
            int eventId = 3;
            _eventWrapper.Setup(x => x.EventGalleryManager.AddPicturesAsync(It.IsAny<int>(), It.IsAny<IList<IFormFile>>()))
                .ReturnsAsync(new List<EventGalleryDTO>());
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _participantStatusManager.Object, _participantManager.Object, _eventWrapper.Object, _mockNotificationService.Object);
            var methodResult = await actionManager.FillEventGalleryAsync(eventId, new List<IFormFile>());
            //Assert
            Assert.NotNull(methodResult);
            Assert.IsType<List<EventGalleryDTO>>(methodResult);
        }
        [Fact]
        public async Task DeletePictureTestAsync()
        {
            //Arrange
            int eventId = 3;
            _eventWrapper.Setup(x => x.EventGalleryManager.DeletePictureAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status200OK);
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _participantStatusManager.Object, _participantManager.Object, _eventWrapper.Object, _mockNotificationService.Object);
            var methodResult = await actionManager.DeletePictureAsync(eventId);
            //Assert
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }
        public IQueryable<Event> GetEvents()
        {
            var events = new List<Event>
            {
                new Event{
                    ID=1,
                    EventName="Крайовий Пластовий З`їзд 2015",
                    EventDateStart=new DateTime(),
                    EventDateEnd=new DateTime(),
                    Eventlocation="Київ",
                    EventTypeID=1,
                    EventCategoryID=3,
                    EventStatusID=2,
                    Description="Event Description",
                    Participants= new List<Participant>
                    {
                      new Participant{ID=1,ParticipantStatusId=3,EventId=1,UserId="abc-1"},
                      new Participant{ID=2,ParticipantStatusId=3,EventId=1,UserId="abc-2"},
                      new Participant{ID=3,ParticipantStatusId=1,EventId=1,UserId="abc-3"}
                    },
                    EventAdministrations = new List<EventAdministration>()
                    {
                        new EventAdministration(){EventID = 1,UserID ="abc-1",EventAdministrationTypeID = 1}
                    }
                },
                 new Event{
                    ID=2,
                    EventName="Крайовий Пластовий З`їзд 2020",
                    EventDateStart=new DateTime(),
                    EventDateEnd=new DateTime(),
                    Eventlocation="Львів",
                    EventTypeID=1,
                    EventCategoryID=3,
                    EventStatusID=3,
                    Description="Event Description",
                    Participants= new List<Participant>
                    {
                      new Participant{ID=1,ParticipantStatusId=3,EventId=2,UserId="abc-1"},
                      new Participant{ID=2,ParticipantStatusId=3,EventId=2,UserId="abc-2"},
                      new Participant{ID=3,ParticipantStatusId=1,EventId=2,UserId="abc-3"}
                    },
                    EventAdministrations = new List<EventAdministration>()
                    {
                        new EventAdministration(){EventID = 2,UserID ="abc-1",EventAdministrationTypeID = 1}
                    }
                 }
            }.AsQueryable();
            return events;
        }
    }
}