using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Services.Events;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using Participant = EPlast.DataAccess.Entities.Participant;

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
        public async Task SubscribeOnEventSuccessTest()
        {
            //Arrange
            var targetEvent = new Event() { ID = 1, EventStatusID = 3 };
            string userId = "abc-1";
            int underReviewStatus = 1;
            int finishedEventStatus = 2;
            _repoWrapper.Setup(x => x.Participant.CreateAsync((It.IsAny<Participant>())));
            _participantStatusManager.Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(underReviewStatus);
            _eventStatusManager.Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(finishedEventStatus);
            //Act
            var participantManager = new ParticipantManager(_repoWrapper.Object, _eventStatusManager.Object, _participantStatusManager.Object);
            var methodResult = await participantManager.SubscribeOnEventAsync(targetEvent, userId);
            //Assert
            _repoWrapper.Verify(r => r.Participant.CreateAsync(It.IsAny<Participant>()), Times.Once());
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once());
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }

        [Fact]
        public async Task SubscribeOnEventConflictTest()
        {
            //Arrange
            var targetEvent = new Event() { ID = 1, EventStatusID = 2 };
            string userId = "abc-1";
            int underReviewStatus = 1;
            int finishedEventStatus = 2;
            _participantStatusManager.Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(underReviewStatus);
            _eventStatusManager.Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(finishedEventStatus);
            //Act
            var participantManager = new ParticipantManager(_repoWrapper.Object, _eventStatusManager.Object, _participantStatusManager.Object);
            var methodResult = await participantManager.SubscribeOnEventAsync(targetEvent, userId);
            //Assert
            Assert.Equal(StatusCodes.Status409Conflict, methodResult);
        }

        [Fact]
        public async Task SubscribeOnEventFailTest()
        {
            //Arrange
            var targetEvent = new Event() { ID = 1, EventStatusID = 2 };
            string userId = "abc-1";
            int finishedEventStatus = 2;
            _participantStatusManager.Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception());
            _eventStatusManager.Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(finishedEventStatus);
            //Act
            var participantManager = new ParticipantManager(_repoWrapper.Object, _eventStatusManager.Object, _participantStatusManager.Object);
            var methodResult = await participantManager.SubscribeOnEventAsync(targetEvent, userId);
            //Assert
            Assert.Equal(StatusCodes.Status400BadRequest, methodResult);
        }

        [Fact]
        public async Task UnSubscribeOnEventSuccessTest()
        {
            //Arrange
            var targetEvent = new Event() { ID = 1, EventStatusID = 3 };
            string userId = "abc-1";
            int rejectedStatus = 1;
            int finishedEventStatus = 2;
            _repoWrapper.Setup(x => x.Participant.GetFirstAsync(It.IsAny<Expression<Func<Participant, bool>>>(), null))
                .ReturnsAsync(
                    new Participant { ID = 1, ParticipantStatusId = 3, EventId = 1, UserId = "abc-1" }
                );
            _participantStatusManager.Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(rejectedStatus);
            _eventStatusManager.Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(finishedEventStatus);
            //Act
            var participantManager = new ParticipantManager(_repoWrapper.Object, _eventStatusManager.Object, _participantStatusManager.Object);
            var methodResult = await participantManager.UnSubscribeOnEventAsync(targetEvent, userId);
            //Assert
            _repoWrapper.Verify(r => r.Participant.Delete(It.IsAny<Participant>()), Times.Once());
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once());
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }
        [Fact]
        public async Task UnSubscribeOnEventConflictTest()
        {
            //Arrange
            var targetEvent = new Event() { ID = 1, EventStatusID = 3 };
            string userId = "abc-1";
            int rejectedStatus = 3;
            int finishedEventStatus = 2;
            _repoWrapper.Setup(x => x.Participant.GetFirstAsync(It.IsAny<Expression<Func<Participant, bool>>>(), null))
                .ReturnsAsync(
                    new Participant { ID = 1, ParticipantStatusId = 3, EventId = 1, UserId = "abc-1" }
                );
            _participantStatusManager.Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(rejectedStatus);
            _eventStatusManager.Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(finishedEventStatus);
            //Act
            var participantManager = new ParticipantManager(_repoWrapper.Object, _eventStatusManager.Object, _participantStatusManager.Object);
            var methodResult = await participantManager.UnSubscribeOnEventAsync(targetEvent, userId);
            //Assert
            Assert.Equal(StatusCodes.Status409Conflict, methodResult);
        }

        [Fact]
        public async Task UnSubscribeOnEventFailTest()
        {
            //Arrange
            var targetEvent = new Event() { ID = 1, EventStatusID = 3 };
            string userId = "abc-1";
            int rejectedStatus = 3;
            int finishedEventStatus = 2;
            _repoWrapper.Setup(x => x.Participant.GetFirstAsync(It.IsAny<Expression<Func<Participant, bool>>>(), null))
                .ThrowsAsync(new Exception());
            _participantStatusManager.Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(rejectedStatus);
            _eventStatusManager.Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(finishedEventStatus);
            //Act
            var participantManager = new ParticipantManager(_repoWrapper.Object, _eventStatusManager.Object, _participantStatusManager.Object);
            var methodResult = await participantManager.UnSubscribeOnEventAsync(targetEvent, userId);
            //Assert
            Assert.Equal(StatusCodes.Status400BadRequest, methodResult);
        }

        [Fact]
        public async Task ChangeStatusToApprovedSuccessTest()
        {
            //Arrange
            int participantId = 1;
            int participantStatus = 3;
            _repoWrapper.Setup(x => x.Participant.GetFirstAsync(It.IsAny<Expression<Func<Participant, bool>>>(), null))
                .ReturnsAsync(
                    new Participant { ID = 1, ParticipantStatusId = 3, EventId = 1, UserId = "abc-1" }
                    );
            _participantStatusManager.Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(participantStatus);
            //Act
            var participantManager = new ParticipantManager(_repoWrapper.Object, _eventStatusManager.Object, _participantStatusManager.Object);
            var methodResult = await participantManager.ChangeStatusToApprovedAsync(participantId);
            //Assert
            _repoWrapper.Verify(r => r.Participant.Update(It.IsAny<Participant>()), Times.Once());
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once());
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }

        [Fact]
        public async Task ChangeStatusToApprovedFailTest()
        {
            //Arrange
            int participantId = 1;
            int participantStatus = 3;
            _repoWrapper.Setup(x => x.Participant.GetFirstAsync(It.IsAny<Expression<Func<Participant, bool>>>(), null))
                .ThrowsAsync(new Exception());
            _participantStatusManager.Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(participantStatus);
            //Act
            var participantManager = new ParticipantManager(_repoWrapper.Object, _eventStatusManager.Object, _participantStatusManager.Object);
            var methodResult = await participantManager.ChangeStatusToApprovedAsync(participantId);
            //Assert
            Assert.Equal(StatusCodes.Status400BadRequest, methodResult);
        }

        [Fact]
        public async Task ChangeStatusToUnderReviewSuccessTest()
        {
            //Arrange
            int participantId = 1;
            int participantStatus = 3;
            _repoWrapper.Setup(x => x.Participant.GetFirstAsync(It.IsAny<Expression<Func<Participant, bool>>>(), null))
                .ReturnsAsync(
                    new Participant { ID = 1, ParticipantStatusId = 3, EventId = 1, UserId = "abc-1" }
                );
            _participantStatusManager.Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(participantStatus);
            //Act
            var participantManager = new ParticipantManager(_repoWrapper.Object, _eventStatusManager.Object, _participantStatusManager.Object);
            var methodResult = await participantManager.ChangeStatusToUnderReviewAsync(participantId);
            //Assert
            _repoWrapper.Verify(r => r.Participant.Update(It.IsAny<Participant>()), Times.Once());
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once());
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }

        [Fact]
        public async Task ChangeStatusToUnderReviewFailTest()
        {
            //Arrange
            int participantId = 1;
            int participantStatus = 3;
            _repoWrapper.Setup(x => x.Participant.GetFirstAsync(It.IsAny<Expression<Func<Participant, bool>>>(), null))
                .ThrowsAsync(new Exception());
            _participantStatusManager.Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(participantStatus);
            //Act
            var participantManager = new ParticipantManager(_repoWrapper.Object, _eventStatusManager.Object, _participantStatusManager.Object);
            var methodResult = await participantManager.ChangeStatusToUnderReviewAsync(participantId);
            //Assert
            Assert.Equal(StatusCodes.Status400BadRequest, methodResult);
        }

        [Fact]
        public async Task ChangeStatusToRejectedSuccessTest()
        {
            //Arrange
            int participantId = 1;
            int participantStatus = 3;
            var participant = new Participant { ID = 1, ParticipantStatusId = 2, EventId = 1, UserId = "abc-1" };
            var testEvent = new Event { ID = 1 ,EventStatusID=1};
        _repoWrapper.Setup(x => x.Participant.GetFirstAsync(It.IsAny<Expression<Func<Participant, bool>>>(), null))
                .ReturnsAsync(participant);
            _repoWrapper.Setup(x => x.Event.GetFirstAsync(It.IsAny<Expression<Func<Event, bool>>>(), null)).ReturnsAsync(testEvent);
            _participantStatusManager.Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(participantStatus);
            //Act
            var participantManager = new ParticipantManager(_repoWrapper.Object, _eventStatusManager.Object, _participantStatusManager.Object);
            var methodResult = await participantManager.ChangeStatusToRejectedAsync(participantId);
            //Assert
            _repoWrapper.Verify(r => r.Participant.Update(It.IsAny<Participant>()), Times.Once());
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once());
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }

        [Fact]
        public async Task ChangeStatusToRejectedFailTest()
        {
            //Arrange
            int participantId = 1;
            int participantStatus = 3;
            _repoWrapper.Setup(x => x.Participant.GetFirstAsync(It.IsAny<Expression<Func<Participant, bool>>>(), null))
                .ThrowsAsync(new Exception());
            _participantStatusManager.Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(participantStatus);
            //Act
            var participantManager = new ParticipantManager(_repoWrapper.Object, _eventStatusManager.Object, _participantStatusManager.Object);
            var methodResult = await participantManager.ChangeStatusToRejectedAsync(participantId);
            //Assert
            Assert.Equal(StatusCodes.Status400BadRequest, methodResult);
        }

        [Fact]
        public async Task ChangeStatusToRejectedFailStatusidTest()
        {
            //Arrange
            int participantId = 1;
            int participantStatus = 3;
            var participant = new Participant { ID = 1, ParticipantStatusId = 3, EventId = 1, UserId = "abc-1" };
            var testEvent = new Event { ID = 1, EventStatusID = 1 };
            _repoWrapper.Setup(x => x.Participant.GetFirstAsync(It.IsAny<Expression<Func<Participant, bool>>>(), null))
                .ReturnsAsync(participant);
            _repoWrapper.Setup(x => x.Event.GetFirstAsync(It.IsAny<Expression<Func<Event, bool>>>(), null)).ReturnsAsync(testEvent);
            _participantStatusManager.Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(participantStatus);
            //Act
            var participantManager = new ParticipantManager(_repoWrapper.Object, _eventStatusManager.Object, _participantStatusManager.Object);
            var methodResult = await participantManager.ChangeStatusToRejectedAsync(participantId);
            //Assert
            Assert.Equal(StatusCodes.Status409Conflict, methodResult);
        }

        [Fact]
        public async Task GetParticipantsByUserIdAsyncSuccessTest()
        {
            //Arrange
            var collection = new List<Participant>();
            var participant = new Participant {UserId = "1",Event = new Event()};
            _repoWrapper.Setup(x => x.Participant.GetAllAsync(a=>a.UserId==participant.UserId, null))
                .ReturnsAsync(collection);

            //Act
            var participantManager = new ParticipantManager(_repoWrapper.Object, _eventStatusManager.Object, _participantStatusManager.Object);
            var methodResult = await participantManager.GetParticipantsByUserIdAsync(participant.UserId);
            //Assert
            Assert.Equal(collection, methodResult);
        }

        [Fact]
        public async Task EstimateEventByParticipantAsyncSuccessTest()
        {
            //Arrange
            var estimate = 1.0;
            var collection = new List<Participant>();
            var participant = new Participant { UserId = "1", EventId = 1};
            _repoWrapper.Setup(x => x.Participant.GetFirstAsync(It.IsAny<Expression<Func<Participant, bool>>>(),null))
                .ReturnsAsync(participant);
            participant.Estimate = estimate;
            _repoWrapper.Setup(x => x.Participant.Update(participant));
            _repoWrapper.Setup(x => x.SaveAsync());
            _repoWrapper.Setup(x => x.Participant.GetAllAsync(a => a.UserId == participant.UserId, null))
                .ReturnsAsync(collection);
            var eventRating = Math.Round(collection.Sum(p => p.Estimate) / collection.Count(), 2, MidpointRounding.AwayFromZero);

            //Act
            var participantManager = new ParticipantManager(_repoWrapper.Object, _eventStatusManager.Object, _participantStatusManager.Object);
            var methodResult = await participantManager.EstimateEventByParticipantAsync(participant.EventId,participant.UserId,estimate);
            //Assert
            Assert.Equal(eventRating, methodResult);
        }

        [Fact]
        public async Task ChangeParticipantPresentStatustAsyncSuccessTest()
        {
            //Arrange
            var newStatus = true;
            var collection = new List<Participant>();
            var participant = new Participant { UserId = "1", EventId = 1, WasPresent = false };
            _repoWrapper.Setup(x => x.Participant.GetFirstAsync(It.IsAny<Expression<Func<Participant, bool>>>(), null))
                .ReturnsAsync(participant);
            participant.WasPresent = newStatus;
            _repoWrapper.Setup(x => x.Participant.Update(participant));
            _repoWrapper.Setup(x => x.SaveAsync());
            _repoWrapper.Setup(x => x.Participant.GetAllAsync(a => a.UserId == participant.UserId, null))
                .ReturnsAsync(collection);

            //Act
            var participantManager = new ParticipantManager(_repoWrapper.Object, _eventStatusManager.Object, _participantStatusManager.Object);
            var methodResult = await participantManager.ChangeUserPresentStatus(participant.EventId);
            //Assert
            Assert.Equal(204, methodResult);
        }

        [Fact]
        public async Task ChangeParticipantPresentStatustAsyncNotFoundTest()
        {
            //Arrange
            var collection = new List<Participant>();
            var participant = new Participant { UserId = "1", EventId = 1, WasPresent = false };
            _repoWrapper.Setup(x => x.Participant.GetFirstAsync(It.IsAny<Expression<Func<Participant, bool>>>(), null))
               .ThrowsAsync(new InvalidOperationException());

            //Act
            var participantManager = new ParticipantManager(_repoWrapper.Object, _eventStatusManager.Object, _participantStatusManager.Object);
            var methodResult = await participantManager.ChangeUserPresentStatus(2);
            //Assert
            Assert.Equal(404, methodResult);
        }
    }
}
