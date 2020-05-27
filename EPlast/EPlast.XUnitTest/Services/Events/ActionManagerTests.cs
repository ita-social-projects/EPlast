using AutoMapper;
using EPlast.BussinessLayer.DTO.Events;
using EPlast.BussinessLayer.Interfaces.Events;
using EPlast.BussinessLayer.Services.Events;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
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
        public void GetActionCategoriesTest()
        {
            //Arrange
            _eventCategoryManager.Setup(x => x.GetDTO())
                .Returns(new List<EventCategoryDTO>());
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object, _eventTypeManager.Object, _eventStatusManager.Object, _participantStatusManager.Object, _participantManager.Object, _eventGalleryManager.Object);
            var methodResult = actionManager.GetActionCategories();
            //Assert
            Assert.NotNull(methodResult);
            Assert.IsType<List<EventCategoryDTO>>(methodResult);
        }

        [Fact]
        public void GetEventsTest()
        {
            //Arrange
            string expectedID = "abc-1";
            int actionId = 3;
            int fakeId = 3;
            _eventStatusManager.Setup(x => x.GetStatusId(It.IsAny<string>()))
                .Returns(fakeId);
            _participantStatusManager.Setup(x => x.GetStatusId(It.IsAny<string>()))
                .Returns(fakeId);
            _eventTypeManager.Setup(x => x.GetTypeId(It.IsAny<string>()))
                .Returns(fakeId);
            _userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(expectedID);
            _repoWrapper.Setup(x => x.Event.FindByCondition(It.IsAny<Expression<Func<Event, bool>>>()))
                .Returns(GetEvents());
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object, _eventTypeManager.Object, _eventStatusManager.Object, _participantStatusManager.Object, _participantManager.Object, _eventGalleryManager.Object);
            var methodResult = actionManager.GetEvents(actionId, new ClaimsPrincipal());
            //Assert
            Assert.NotNull(methodResult);
            Assert.IsType<List<GeneralEventDTO>>(methodResult);
            Assert.Equal(GetEvents().Count(), methodResult.Count);
        }
        [Fact]
        public void GetEventInfoTest()
        {
            //Arrange
            string expectedID = "abc-1";
            int eventId = 3;
            int fakeId = 3;
            _eventStatusManager.Setup(x => x.GetStatusId(It.IsAny<string>()))
                .Returns(fakeId);
            _participantStatusManager.Setup(x => x.GetStatusId(It.IsAny<string>()))
                .Returns(fakeId);
            _userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(expectedID);
            _mapper.Setup(m => m.Map<Event, EventInfoDTO>(It.IsAny<Event>())).Returns(new EventInfoDTO());
            _repoWrapper.Setup(x => x.Event.FindByCondition(It.IsAny<Expression<Func<Event, bool>>>()))
                .Returns(GetEvents());
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object, _eventTypeManager.Object, _eventStatusManager.Object, _participantStatusManager.Object, _participantManager.Object, _eventGalleryManager.Object);
            var methodResult = actionManager.GetEventInfo(eventId, new ClaimsPrincipal());
            //Assert
            Assert.NotNull(methodResult);
            Assert.IsType<EventDTO>(methodResult);
        }

        [Fact]
        public void DeleteEventSuccessTest()
        {
            //Arrange
            int testEventId = 2;
            _repoWrapper.Setup(x => x.Event.FindByCondition(It.IsAny<Expression<Func<Event, bool>>>()))
                .Returns(GetEvents());
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object, _eventTypeManager.Object, _eventStatusManager.Object, _participantStatusManager.Object, _participantManager.Object, _eventGalleryManager.Object);
            var methodResult = actionManager.DeleteEvent(testEventId);
            //Assert
            _repoWrapper.Verify(r => r.Event.Delete(It.IsAny<Event>()), Times.Once());
            _repoWrapper.Verify(r => r.Save(), Times.Once());
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }

        [Fact]
        public void DeleteEventFailTest()
        {
            //Arrange
            int testEventId = 2;
            _repoWrapper.Setup(x => x.Event.FindByCondition(It.IsAny<Expression<Func<Event, bool>>>()))
                .Throws(new Exception());
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object, _eventTypeManager.Object, _eventStatusManager.Object, _participantStatusManager.Object, _participantManager.Object, _eventGalleryManager.Object);
            var methodResult = actionManager.DeleteEvent(testEventId);
            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, methodResult);
        }

        [Fact]
        public void SubscribeOnEventSuccessTest()
        {
            //Arrange
            int testEventId = 2;
            string userId = "051fe5ee6ge";
            _repoWrapper.Setup(x => x.Event.FindByCondition(It.IsAny<Expression<Func<Event, bool>>>()))
                .Returns(GetEvents());
            _userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);
            _participantManager.Setup(x => x.SubscribeOnEvent(It.IsAny<Event>(), It.IsAny<string>()))
                .Returns(StatusCodes.Status200OK);
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object, _eventTypeManager.Object, _eventStatusManager.Object, _participantStatusManager.Object, _participantManager.Object, _eventGalleryManager.Object);
            var methodResult = actionManager.SubscribeOnEvent(testEventId, new ClaimsPrincipal());
            //Assert
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }
        [Fact]
        public void SubscribeOnEventFailTest()
        {
            //Arrange
            int testEventId = 2;
            string userId = "051fe5ee6ge";
            _repoWrapper.Setup(x => x.Event.FindByCondition(It.IsAny<Expression<Func<Event, bool>>>()))
                .Returns(GetEvents());
            _userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);
            _participantManager.Setup(x => x.SubscribeOnEvent(It.IsAny<Event>(), It.IsAny<string>()))
                .Throws(new Exception());
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object, _eventTypeManager.Object, _eventStatusManager.Object, _participantStatusManager.Object, _participantManager.Object, _eventGalleryManager.Object);
            var methodResult = actionManager.SubscribeOnEvent(testEventId, new ClaimsPrincipal());
            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, methodResult);
        }

        [Fact]
        public void UnSubscribeOnEventSuccessTest()
        {
            //Arrange
            int testEventId = 2;
            string userId = "051fe5ee6ge";
            _repoWrapper.Setup(x => x.Event.FindByCondition(It.IsAny<Expression<Func<Event, bool>>>()))
                .Returns(GetEvents());
            _userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);
            _participantManager.Setup(x => x.UnSubscribeOnEvent(It.IsAny<Event>(), It.IsAny<string>()))
                .Returns(StatusCodes.Status200OK);
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object, _eventTypeManager.Object, _eventStatusManager.Object, _participantStatusManager.Object, _participantManager.Object, _eventGalleryManager.Object);
            var methodResult = actionManager.UnSubscribeOnEvent(testEventId, new ClaimsPrincipal());
            //Assert
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }
        [Fact]
        public void UnSubscribeOnEventFailTest()
        {
            //Arrange
            int testEventId = 2;
            string userId = "051fe5ee6ge";
            _repoWrapper.Setup(x => x.Event.FindByCondition(It.IsAny<Expression<Func<Event, bool>>>()))
                .Returns(GetEvents());
            _userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);
            _participantManager.Setup(x => x.UnSubscribeOnEvent(It.IsAny<Event>(), It.IsAny<string>()))
                .Throws(new Exception());
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object, _eventTypeManager.Object, _eventStatusManager.Object, _participantStatusManager.Object, _participantManager.Object, _eventGalleryManager.Object);
            var methodResult = actionManager.UnSubscribeOnEvent(testEventId, new ClaimsPrincipal());
            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, methodResult);
        }

        [Fact]
        public void ApproveParticipantTest()
        {
            //Arrange
            int testParticipantId = 3;
            _participantManager.Setup(x => x.ChangeStatusToApproved(It.IsAny<int>()))
                .Returns(StatusCodes.Status200OK);
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object, _eventTypeManager.Object, _eventStatusManager.Object, _participantStatusManager.Object, _participantManager.Object, _eventGalleryManager.Object);
            var methodResult = actionManager.ApproveParticipant(testParticipantId);
            //Assert
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }
        [Fact]
        public void UnderReviewParticipantTest()
        {
            //Arrange
            int testParticipantId = 3;
            _participantManager.Setup(x => x.ChangeStatusToUnderReview(It.IsAny<int>()))
                .Returns(StatusCodes.Status200OK);
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object, _eventTypeManager.Object, _eventStatusManager.Object, _participantStatusManager.Object, _participantManager.Object, _eventGalleryManager.Object);
            var methodResult = actionManager.UnderReviewParticipant(testParticipantId);
            //Assert
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }
        [Fact]
        public void RejectParticipantTest()
        {
            //Arrange
            int testParticipantId = 3;
            _participantManager.Setup(x => x.ChangeStatusToRejected(It.IsAny<int>()))
                .Returns(StatusCodes.Status200OK);
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object, _eventTypeManager.Object, _eventStatusManager.Object, _participantStatusManager.Object, _participantManager.Object, _eventGalleryManager.Object);
            var methodResult = actionManager.RejectParticipant(testParticipantId);
            //Assert
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }
        [Fact]
        public void FillEventGalleryTest()
        {
            //Arrange
            int eventId = 3;
            _eventGalleryManager.Setup(x => x.AddPictures(It.IsAny<int>(), It.IsAny<IList<IFormFile>>()))
                .Returns(StatusCodes.Status200OK);
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object, _eventTypeManager.Object, _eventStatusManager.Object, _participantStatusManager.Object, _participantManager.Object, _eventGalleryManager.Object);
            var methodResult = actionManager.FillEventGallery(eventId, new List<IFormFile>());
            //Assert
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }
        [Fact]
        public void DeletePictureTest()
        {
            //Arrange
            int eventId = 3;
            _eventGalleryManager.Setup(x => x.DeletePicture(It.IsAny<int>()))
                .Returns(StatusCodes.Status200OK);
            //Act
            var actionManager = new ActionManager(_userManager.Object, _repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object, _eventTypeManager.Object, _eventStatusManager.Object, _participantStatusManager.Object, _participantManager.Object, _eventGalleryManager.Object);
            var methodResult = actionManager.DeletePicture(eventId);
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
