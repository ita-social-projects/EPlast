using AutoMapper;
using EPlast.BussinessLayer.DTO.Events;
using EPlast.BussinessLayer.Interfaces.Events;
using EPlast.BussinessLayer.Services.Events;
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
using Xunit;

namespace EPlast.XUnitTest.Services.Events
{
    public class ActionManagerTests
    {
        private readonly Mock<IRepositoryWrapper> _repoWrapper;
        private readonly Mock<UserManager<User>> _userManager;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IEventCategoryManager> _eventCategoryManager;
        private readonly Mock<IEventTypeManager> _eventTypeManager;
        private readonly Mock<IEventStatusManager> _eventStatusManager;
        private readonly Mock<IParticipantStatusManager> _participantStatusManager;
        private readonly Mock<IParticipantManager> _participantManager;
        private readonly Mock<IEventGalleryManager> _eventGalleryManager;

        public ActionManagerTests()
        {
            var userStore = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _eventStatusManager = new Mock<IEventStatusManager>();
            _eventCategoryManager = new Mock<IEventCategoryManager>();
            _eventTypeManager = new Mock<IEventTypeManager>();
            _participantStatusManager = new Mock<IParticipantStatusManager>();
            _participantManager = new Mock<IParticipantManager>();
            _eventGalleryManager = new Mock<IEventGalleryManager>();
        }

        [Fact]
        public async void GetActionCategoriesTest()
        {
            //Arrange
            _eventCategoryManager.Setup(x => x.GetDTOAsync())
                .ReturnsAsync(new List<EventCategoryDTO>());
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object, _eventTypeManager.Object, _eventStatusManager.Object, _participantStatusManager.Object, _participantManager.Object, _eventGalleryManager.Object);
            var methodResult = await actionManager.GetActionCategoriesAsync();
            //Assert
            Assert.NotNull(methodResult);
            Assert.IsType<List<EventCategoryDTO>>(methodResult);
        }

        [Fact]
        public async void GetEventsTest()
        {
            //Arrange
            string expectedID = "abc-1";
            int actionId = 3;
            int fakeId = 3;
            _eventStatusManager.Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(fakeId);
            _participantStatusManager.Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(fakeId);
            _eventTypeManager.Setup(x => x.GetTypeIdAsync(It.IsAny<string>()))
                .ReturnsAsync(fakeId);
            _userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(expectedID);
            _repoWrapper.Setup(x => x.Event.GetAllAsync(It.IsAny<Expression<Func<Event, bool>>>(), It.IsAny<Func<IQueryable<Event>, IIncludableQueryable<Event, object>>>()))
                .ReturnsAsync(GetEvents());
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object, _eventTypeManager.Object, _eventStatusManager.Object, _participantStatusManager.Object, _participantManager.Object, _eventGalleryManager.Object);
            var methodResult = await actionManager.GetEventsAsync(actionId, new ClaimsPrincipal());
            //Assert
            Assert.NotNull(methodResult);
            Assert.IsType<List<GeneralEventDTO>>(methodResult);
            Assert.Equal(GetEvents().Count(), methodResult.Count);
        }
        [Fact]
        public async void GetEventInfoTest()
        {
            //Arrange
            string expectedID = "abc-1";
            int eventId = 3;
            int fakeId = 3;
            _eventStatusManager.Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(fakeId);
            _participantStatusManager.Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(fakeId);
            _userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(expectedID);
            _mapper.Setup(m => m.Map<Event, EventInfoDTO>(It.IsAny<Event>())).Returns(new EventInfoDTO());
            _repoWrapper.Setup(x => x.Event.GetFirstAsync(It.IsAny<Expression<Func<Event, bool>>>(), It.IsAny<Func<IQueryable<Event>, IIncludableQueryable<Event, object>>>()))
                .ReturnsAsync(GetEvents().First());
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object, _eventTypeManager.Object, _eventStatusManager.Object, _participantStatusManager.Object, _participantManager.Object, _eventGalleryManager.Object);
            var methodResult = await actionManager.GetEventInfoAsync(eventId, new ClaimsPrincipal());
            //Assert
            Assert.NotNull(methodResult);
            Assert.IsType<EventDTO>(methodResult);
        }

        [Fact]
        public async void DeleteEventSuccessTest()
        {
            //Arrange
            int testEventId = 2;
            _repoWrapper.Setup(x => x.Event.GetFirstAsync(It.IsAny<Expression<Func<Event, bool>>>(), null))
                .ReturnsAsync(GetEvents().First());
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object, _eventTypeManager.Object, _eventStatusManager.Object, _participantStatusManager.Object, _participantManager.Object, _eventGalleryManager.Object);
            var methodResult = await actionManager.DeleteEventAsync(testEventId);
            //Assert
            _repoWrapper.Verify(r => r.Event.Delete(It.IsAny<Event>()), Times.Once());
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once());
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }

        [Fact]
        public async void DeleteEventFailTest()
        {
            //Arrange
            int testEventId = 2;
            _repoWrapper.Setup(x => x.Event.GetFirstAsync(It.IsAny<Expression<Func<Event, bool>>>(), null))
                .ThrowsAsync(new Exception());
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object, _eventTypeManager.Object, _eventStatusManager.Object, _participantStatusManager.Object, _participantManager.Object, _eventGalleryManager.Object);
            var methodResult = await actionManager.DeleteEventAsync(testEventId);
            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, methodResult);
        }

        [Fact]
        public async void SubscribeOnEventSuccessTest()
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
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object, _eventTypeManager.Object, _eventStatusManager.Object, _participantStatusManager.Object, _participantManager.Object, _eventGalleryManager.Object);
            var methodResult = await actionManager.SubscribeOnEventAsync(testEventId, new ClaimsPrincipal());
            //Assert
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }
        [Fact]
        public async void SubscribeOnEventFailTest()
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
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object, _eventTypeManager.Object, _eventStatusManager.Object, _participantStatusManager.Object, _participantManager.Object, _eventGalleryManager.Object);
            var methodResult = await actionManager.SubscribeOnEventAsync(testEventId, new ClaimsPrincipal());
            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, methodResult);
        }

        [Fact]
        public async void UnSubscribeOnEventSuccessTest()
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
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object, _eventTypeManager.Object, _eventStatusManager.Object, _participantStatusManager.Object, _participantManager.Object, _eventGalleryManager.Object);
            var methodResult = await actionManager.UnSubscribeOnEventAsync(testEventId, new ClaimsPrincipal());
            //Assert
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }
        [Fact]
        public async void UnSubscribeOnEventFailTest()
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
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object, _eventTypeManager.Object, _eventStatusManager.Object, _participantStatusManager.Object, _participantManager.Object, _eventGalleryManager.Object);
            var methodResult = await actionManager.UnSubscribeOnEventAsync(testEventId, new ClaimsPrincipal());
            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, methodResult);
        }

        [Fact]
        public async void ApproveParticipantTest()
        {
            //Arrange
            int testParticipantId = 3;
            _participantManager.Setup(x => x.ChangeStatusToApprovedAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status200OK);
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object, _eventTypeManager.Object, _eventStatusManager.Object, _participantStatusManager.Object, _participantManager.Object, _eventGalleryManager.Object);
            var methodResult = await actionManager.ApproveParticipantAsync(testParticipantId);
            //Assert
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }
        [Fact]
        public async void UnderReviewParticipantTest()
        {
            //Arrange
            int testParticipantId = 3;
            _participantManager.Setup(x => x.ChangeStatusToUnderReviewAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status200OK);
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object, _eventTypeManager.Object, _eventStatusManager.Object, _participantStatusManager.Object, _participantManager.Object, _eventGalleryManager.Object);
            var methodResult = await actionManager.UnderReviewParticipantAsync(testParticipantId);
            //Assert
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }
        [Fact]
        public async void RejectParticipantTest()
        {
            //Arrange
            int testParticipantId = 3;
            _participantManager.Setup(x => x.ChangeStatusToRejectedAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status200OK);
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object, _eventTypeManager.Object, _eventStatusManager.Object, _participantStatusManager.Object, _participantManager.Object, _eventGalleryManager.Object);
            var methodResult = await actionManager.RejectParticipantAsync(testParticipantId);
            //Assert
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }
        [Fact]
        public async void FillEventGalleryTest()
        {
            //Arrange
            int eventId = 3;
            _eventGalleryManager.Setup(x => x.AddPicturesAsync(It.IsAny<int>(), It.IsAny<IList<IFormFile>>()))
                .ReturnsAsync(StatusCodes.Status200OK);
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object, _eventTypeManager.Object, _eventStatusManager.Object, _participantStatusManager.Object, _participantManager.Object, _eventGalleryManager.Object);
            var methodResult = await actionManager.FillEventGalleryAsync(eventId, new List<IFormFile>());
            //Assert
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }
        [Fact]
        public async void DeletePictureTest()
        {
            //Arrange
            int eventId = 3;
            _eventGalleryManager.Setup(x => x.DeletePictureAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status200OK);
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object, _eventTypeManager.Object, _eventStatusManager.Object, _participantStatusManager.Object, _participantManager.Object, _eventGalleryManager.Object);
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
                    EventAdmins=  new List<EventAdmin>
                    {
                        new EventAdmin{EventID=1,UserID="abc-1"}
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
                     EventAdmins=  new List<EventAdmin>
                    {
                        new EventAdmin{EventID=2,UserID="abc-1"}
                    }
                 }
            }.AsQueryable();
            return events;
        }
    }
}
