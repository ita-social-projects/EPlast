using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Services.Events;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
            _repoWrapper.Setup(x => x.Participant.GetFirstAsync(It.IsAny<Expression<Func<Participant, bool>>>(), null))
                .ReturnsAsync(
                    new Participant { ID = 1, ParticipantStatusId = 3, EventId = 1, UserId = "abc-1" }
                );
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
    }
}
