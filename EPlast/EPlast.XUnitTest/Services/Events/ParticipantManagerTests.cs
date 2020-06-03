using EPlast.BussinessLayer.Interfaces.Events;
using EPlast.BussinessLayer.Services.Events;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EPlast.DataAccess.Entities.Event;
using Xunit;

namespace EPlast.XUnitTest.Services.Events
{
    public class ParticipantManagerTests
    {
        private readonly Mock<IRepositoryWrapper> _repoWrapper;
        private readonly Mock<IEventStatusManager> _eventStatusManager;
        private readonly Mock<IParticipantStatusManager> _participantStatusManager;


        public ParticipantManagerTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _eventStatusManager = new Mock<IEventStatusManager>();
            _participantStatusManager = new Mock<IParticipantStatusManager>();
        }

        [Fact]
        public void SubscribeOnEventSuccessTest()
        {
            //Arrange
            var targetEvent = new Event() { ID = 1, EventStatusID = 3};
            string userId = "abc-1";
            int underReviewStatus = 1;
            int finishedEventStatus = 2;
            _repoWrapper.Setup(x => x.Participant.Create((It.IsAny<Participant>())));
            _participantStatusManager.Setup(x => x.GetStatusId(It.IsAny<string>()))
                .Returns(underReviewStatus);
            _eventStatusManager.Setup(x => x.GetStatusId(It.IsAny<string>()))
                .Returns(finishedEventStatus);
            //Act
            var participantManager = new ParticipantManager(_repoWrapper.Object,_eventStatusManager.Object,_participantStatusManager.Object);
            var methodResult = participantManager.SubscribeOnEvent(targetEvent,userId);
            //Assert
            _repoWrapper.Verify(r => r.Participant.Create(It.IsAny<Participant>()), Times.Once());
            _repoWrapper.Verify(r => r.Save(), Times.Once());
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }

        [Fact]
        public void SubscribeOnEventConflictTest()
        {
            //Arrange
            var targetEvent = new Event() { ID = 1, EventStatusID = 2 };
            string userId = "abc-1";
            int underReviewStatus = 1;
            int finishedEventStatus = 2;
            _participantStatusManager.Setup(x => x.GetStatusId(It.IsAny<string>()))
                .Returns(underReviewStatus);
            _eventStatusManager.Setup(x => x.GetStatusId(It.IsAny<string>()))
                .Returns(finishedEventStatus);
            //Act
            var participantManager = new ParticipantManager(_repoWrapper.Object, _eventStatusManager.Object, _participantStatusManager.Object);
            var methodResult = participantManager.SubscribeOnEvent(targetEvent, userId);
            //Assert
            Assert.Equal(StatusCodes.Status409Conflict, methodResult);
        }

        [Fact]
        public void SubscribeOnEventFailTest()
        {
            //Arrange
            var targetEvent = new Event() { ID = 1, EventStatusID = 2 };
            string userId = "abc-1";
            int finishedEventStatus = 2;
            _participantStatusManager.Setup(x => x.GetStatusId(It.IsAny<string>()))
                .Throws(new Exception());
            _eventStatusManager.Setup(x => x.GetStatusId(It.IsAny<string>()))
                .Returns(finishedEventStatus);
            //Act
            var participantManager = new ParticipantManager(_repoWrapper.Object, _eventStatusManager.Object, _participantStatusManager.Object);
            var methodResult = participantManager.SubscribeOnEvent(targetEvent, userId);
            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, methodResult);
        }

        [Fact]
        public void UnSubscribeOnEventSuccessTest()
        {
            //Arrange
            var targetEvent = new Event() { ID = 1, EventStatusID = 3 };
            string userId = "abc-1";
            int rejectedStatus = 1;
            int finishedEventStatus = 2;
            _repoWrapper.Setup(x => x.Participant.FindByCondition(It.IsAny<Expression<Func<Participant, bool>>>()))
                .Returns(new List<Participant>
                {
                    new Participant{ID=1,ParticipantStatusId=3,EventId=1,UserId="abc-1"},
                    new Participant{ID=2,ParticipantStatusId=3,EventId=1,UserId="abc-2"},
                    new Participant{ID=3,ParticipantStatusId=1,EventId=1,UserId="abc-3"}
                }.AsQueryable());
            _participantStatusManager.Setup(x => x.GetStatusId(It.IsAny<string>()))
                .Returns(rejectedStatus);
            _eventStatusManager.Setup(x => x.GetStatusId(It.IsAny<string>()))
                .Returns(finishedEventStatus);
            //Act
            var participantManager = new ParticipantManager(_repoWrapper.Object, _eventStatusManager.Object, _participantStatusManager.Object);
            var methodResult = participantManager.UnSubscribeOnEvent(targetEvent, userId);
            //Assert
            _repoWrapper.Verify(r => r.Participant.Delete(It.IsAny<Participant>()), Times.Once());
            _repoWrapper.Verify(r => r.Save(), Times.Once());
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }
        [Fact]
        public void UnSubscribeOnEventConflictTest()
        {
            //Arrange
            var targetEvent = new Event() { ID = 1, EventStatusID = 3 };
            string userId = "abc-1";
            int rejectedStatus = 3;
            int finishedEventStatus = 2;
            _repoWrapper.Setup(x => x.Participant.FindByCondition(It.IsAny<Expression<Func<Participant, bool>>>()))
                .Returns(new List<Participant>
                {
                    new Participant{ID=1,ParticipantStatusId=3,EventId=1,UserId="abc-1"},
                    new Participant{ID=2,ParticipantStatusId=3,EventId=1,UserId="abc-2"},
                    new Participant{ID=3,ParticipantStatusId=1,EventId=1,UserId="abc-3"}
                }.AsQueryable());
            _participantStatusManager.Setup(x => x.GetStatusId(It.IsAny<string>()))
                .Returns(rejectedStatus);
            _eventStatusManager.Setup(x => x.GetStatusId(It.IsAny<string>()))
                .Returns(finishedEventStatus);
            //Act
            var participantManager = new ParticipantManager(_repoWrapper.Object, _eventStatusManager.Object, _participantStatusManager.Object);
            var methodResult = participantManager.UnSubscribeOnEvent(targetEvent, userId);
            //Assert
            Assert.Equal(StatusCodes.Status409Conflict, methodResult);
        }

        [Fact]
        public void UnSubscribeOnEventFailTest()
        {
            //Arrange
            var targetEvent = new Event() { ID = 1, EventStatusID = 3 };
            string userId = "abc-1";
            int rejectedStatus = 3;
            int finishedEventStatus = 2;
            _repoWrapper.Setup(x => x.Participant.FindByCondition(It.IsAny<Expression<Func<Participant, bool>>>()))
                .Throws(new Exception());
            _participantStatusManager.Setup(x => x.GetStatusId(It.IsAny<string>()))
                .Returns(rejectedStatus);
            _eventStatusManager.Setup(x => x.GetStatusId(It.IsAny<string>()))
                .Returns(finishedEventStatus);
            //Act
            var participantManager = new ParticipantManager(_repoWrapper.Object, _eventStatusManager.Object, _participantStatusManager.Object);
            var methodResult = participantManager.UnSubscribeOnEvent(targetEvent, userId);
            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, methodResult);
        }

        [Fact]
        public void ChangeStatusToApprovedSuccessTest()
        {
            //Arrange
            int participantId = 1;
            int participantStatus = 3;
            _repoWrapper.Setup(x => x.Participant.FindByCondition(It.IsAny<Expression<Func<Participant, bool>>>()))
                .Returns(new List<Participant>
                {
                    new Participant{ID=1,ParticipantStatusId=3,EventId=1,UserId="abc-1"},
                    new Participant{ID=2,ParticipantStatusId=3,EventId=1,UserId="abc-2"},
                    new Participant{ID=3,ParticipantStatusId=1,EventId=1,UserId="abc-3"}
                }.AsQueryable());
            _participantStatusManager.Setup(x => x.GetStatusId(It.IsAny<string>()))
                .Returns(participantStatus);
            //Act
            var participantManager = new ParticipantManager(_repoWrapper.Object, _eventStatusManager.Object, _participantStatusManager.Object);
            var methodResult = participantManager.ChangeStatusToApproved(participantId);
            //Assert
            _repoWrapper.Verify(r => r.Participant.Update(It.IsAny<Participant>()), Times.Once());
            _repoWrapper.Verify(r => r.Save(), Times.Once());
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }

        [Fact]
        public void ChangeStatusToApprovedFailTest()
        {
            //Arrange
            int participantId = 1;
            int participantStatus = 3;
            _repoWrapper.Setup(x => x.Participant.FindByCondition(It.IsAny<Expression<Func<Participant, bool>>>()))
                .Throws(new Exception());
            _participantStatusManager.Setup(x => x.GetStatusId(It.IsAny<string>()))
                .Returns(participantStatus);
            //Act
            var participantManager = new ParticipantManager(_repoWrapper.Object, _eventStatusManager.Object, _participantStatusManager.Object);
            var methodResult = participantManager.ChangeStatusToApproved(participantId);
            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, methodResult);
        }

        [Fact]
        public void ChangeStatusToUnderReviewSuccessTest()
        {
            //Arrange
            int participantId = 1;
            int participantStatus = 3;
            _repoWrapper.Setup(x => x.Participant.FindByCondition(It.IsAny<Expression<Func<Participant, bool>>>()))
                .Returns(new List<Participant>
                {
                    new Participant{ID=1,ParticipantStatusId=3,EventId=1,UserId="abc-1"},
                    new Participant{ID=2,ParticipantStatusId=3,EventId=1,UserId="abc-2"},
                    new Participant{ID=3,ParticipantStatusId=1,EventId=1,UserId="abc-3"}
                }.AsQueryable());
            _participantStatusManager.Setup(x => x.GetStatusId(It.IsAny<string>()))
                .Returns(participantStatus);
            //Act
            var participantManager = new ParticipantManager(_repoWrapper.Object, _eventStatusManager.Object, _participantStatusManager.Object);
            var methodResult = participantManager.ChangeStatusToUnderReview(participantId);
            //Assert
            _repoWrapper.Verify(r => r.Participant.Update(It.IsAny<Participant>()), Times.Once());
            _repoWrapper.Verify(r => r.Save(), Times.Once());
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }

        [Fact]
        public void ChangeStatusToUnderReviewFailTest()
        {
            //Arrange
            int participantId = 1;
            int participantStatus = 3;
            _repoWrapper.Setup(x => x.Participant.FindByCondition(It.IsAny<Expression<Func<Participant, bool>>>()))
                .Throws(new Exception());
            _participantStatusManager.Setup(x => x.GetStatusId(It.IsAny<string>()))
                .Returns(participantStatus);
            //Act
            var participantManager = new ParticipantManager(_repoWrapper.Object, _eventStatusManager.Object, _participantStatusManager.Object);
            var methodResult = participantManager.ChangeStatusToUnderReview(participantId);
            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, methodResult);
        }

        [Fact]
        public void ChangeStatusToRejectedSuccessTest()
        {
            //Arrange
            int participantId = 1;
            int participantStatus = 3;
            _repoWrapper.Setup(x => x.Participant.FindByCondition(It.IsAny<Expression<Func<Participant, bool>>>()))
                .Returns(new List<Participant>
                {
                    new Participant{ID=1,ParticipantStatusId=3,EventId=1,UserId="abc-1"},
                    new Participant{ID=2,ParticipantStatusId=3,EventId=1,UserId="abc-2"},
                    new Participant{ID=3,ParticipantStatusId=1,EventId=1,UserId="abc-3"}
                }.AsQueryable());
            _participantStatusManager.Setup(x => x.GetStatusId(It.IsAny<string>()))
                .Returns(participantStatus);
            //Act
            var participantManager = new ParticipantManager(_repoWrapper.Object, _eventStatusManager.Object, _participantStatusManager.Object);
            var methodResult = participantManager.ChangeStatusToRejected(participantId);
            //Assert
            _repoWrapper.Verify(r => r.Participant.Update(It.IsAny<Participant>()), Times.Once());
            _repoWrapper.Verify(r => r.Save(), Times.Once());
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }

        [Fact]
        public void ChangeStatusToRejectedFailTest()
        {
            //Arrange
            int participantId = 1;
            int participantStatus = 3;
            _repoWrapper.Setup(x => x.Participant.FindByCondition(It.IsAny<Expression<Func<Participant, bool>>>()))
                .Throws(new Exception());
            _participantStatusManager.Setup(x => x.GetStatusId(It.IsAny<string>()))
                .Returns(participantStatus);
            //Act
            var participantManager = new ParticipantManager(_repoWrapper.Object, _eventStatusManager.Object, _participantStatusManager.Object);
            var methodResult = participantManager.ChangeStatusToRejected(participantId);
            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, methodResult);
        }
    }
}
