using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Events;
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
using NUnit.Framework;
using NUnit.Framework.Internal;
using Microsoft.EntityFrameworkCore;
using EPlast.Resources;

namespace EPlast.Tests.Services.Events
{
    internal class ActionManagerTests
    {
        private IActionManager _actionManager;
        private Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private Mock<UserManager<User>> _mockUserManager;
        private Mock<IMapper> _mockMapper;
        private Mock<IParticipantStatusManager> _mockParticipantStatusManager;
        private Mock<IParticipantManager> _mockParticipantManager;
        private Mock<IEventWrapper> _mockEventWrapper;
        private Mock<INotificationService> _mockNotificationService;

        private int testEventId = 1;
        private int testParticipantId = 1;
        private int testFeedbackId = 1;
        private int testEstimate = 1;
        private readonly string fakeIdString = "1";
        private int testCategoryId = 1;
        private int testTypeId = 1;

        [SetUp]
        public void SetUp()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockParticipantStatusManager = new Mock<IParticipantStatusManager>();
            _mockParticipantManager = new Mock<IParticipantManager>();
            _mockEventWrapper = new Mock<IEventWrapper>();
            _mockNotificationService = new Mock<INotificationService>();
            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _actionManager = new ActionManager(
                _mockUserManager.Object,
                _mockRepositoryWrapper.Object,
                _mockMapper.Object,
                _mockParticipantStatusManager.Object,
                _mockParticipantManager.Object,
                _mockEventWrapper.Object,
                _mockNotificationService.Object
            );
        }

        [Test]
        public void CheckEventsStatusesAsync_Valid()
        {
            //Arrange
            var eventToCheck =  _mockRepositoryWrapper.Setup(x => x.Event.GetAllAsync(
                    It.IsAny<Expression<Func<DataAccess.Entities.Event.Event, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.Event.Event>,
                        IIncludableQueryable<DataAccess.Entities.Event.Event, object>>>()))
                .ReturnsAsync(new List<DataAccess.Entities.Event.Event>());
            _mockRepositoryWrapper.Setup(x => x.Event.Update(It.IsAny<DataAccess.Entities.Event.Event>()));
            
            //Act
            var result =  _actionManager.CheckEventsStatusesAsync();

            //Assert
            Assert.IsNotNull(result);
        }
        
        [TestCase(1)]
        [TestCase(2)]
        public async Task GetEventsByStatusAsync_Valid(int testStatus)
        {
            //Arrange
            _mockRepositoryWrapper.Setup(x => x.Event.GetAllAsync(
                            It.IsAny<Expression<Func<DataAccess.Entities.Event.Event, bool>>>(),
                            It.IsAny<Func<IQueryable<DataAccess.Entities.Event.Event>,
                                IIncludableQueryable<DataAccess.Entities.Event.Event, object>>>()))
                        .ReturnsAsync(new List<DataAccess.Entities.Event.Event>());
            _mockEventWrapper.Setup(x => x.EventStatusManager.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(testStatus);
            _mockRepositoryWrapper.Setup(x => x.EventAdministration.GetAllAsync(
                    It.IsAny<Expression<Func<EventAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<EventAdministration>,
                        IIncludableQueryable<EventAdministration, object>>>()))
                .ReturnsAsync(new List<EventAdministration>() { new EventAdministration() { ID = 2 } });

            //Act
            var result = await _actionManager.GetEventsByStatusAsync(testCategoryId, testTypeId, testStatus, new User());

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetEventSectionsTestAsync()
        {
            //Arrange
            _mockEventWrapper.Setup(x => x.EventSectionManager.GetEventSectionsDTOAsync())
                .ReturnsAsync(new List<EventSectionDto>());
            //Act
            var methodResult = await _actionManager.GetEventSectionsAsync();

            //Assert
            Assert.NotNull(methodResult);
            Assert.IsAssignableFrom<List<EventSectionDto>>(methodResult);
        }

        [Test]
        public async Task LeaveFeedback_EventDoesntExist_ReturnsNotFound()
        {
            //Arrange
            _mockRepositoryWrapper.Setup(x => x.Event.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<DataAccess.Entities.Event.Event, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccess.Entities.Event.Event>,
                    IIncludableQueryable<DataAccess.Entities.Event.Event, object>>>()
                    )).ReturnsAsync((DataAccess.Entities.Event.Event)null);

            //Act
            var result = await _actionManager.LeaveFeedbackAsync(testEventId, new EventFeedbackDto(), new User());

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<int>(result);
            Assert.AreEqual(StatusCodes.Status404NotFound, result);
        }

        [Test]
        public async Task LeaveFeedback_ParticipantDoesntExist_ReturnsForbidden()
        {
            //Arrange
            _mockRepositoryWrapper.Setup(x => x.Event.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<DataAccess.Entities.Event.Event, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccess.Entities.Event.Event>,
                    IIncludableQueryable<DataAccess.Entities.Event.Event, object>>>()
                    )).ReturnsAsync(new DataAccess.Entities.Event.Event() { ID = testEventId });

            _mockRepositoryWrapper.Setup(x => x.Participant.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Participant, bool>>>(),
                It.IsAny<Func<IQueryable<Participant>,
                    IIncludableQueryable<Participant, object>>>()
                    )).ReturnsAsync((Participant)null);

            //Act
            var result = await _actionManager.LeaveFeedbackAsync(testEventId, new EventFeedbackDto(), new User());

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<int>(result);
            Assert.AreEqual(StatusCodes.Status403Forbidden, result);
        }

        [Test]
        public async Task LeaveFeedback_FeedbackExists_FeedbackIsUpdatedAndReturnsOk()
        {
            //Arrange
            _mockRepositoryWrapper.Setup(x => x.Event.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<DataAccess.Entities.Event.Event, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccess.Entities.Event.Event>,
                    IIncludableQueryable<DataAccess.Entities.Event.Event, object>>>()
                    )).ReturnsAsync(new DataAccess.Entities.Event.Event() { ID = testEventId });

            _mockRepositoryWrapper.Setup(x => x.Participant.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Participant, bool>>>(),
                It.IsAny<Func<IQueryable<Participant>,
                    IIncludableQueryable<Participant, object>>>()
                    )).ReturnsAsync(new Participant() { ID = testParticipantId });

            _mockRepositoryWrapper.Setup(x => x.EventFeedback.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<EventFeedback, bool>>>(),
                It.IsAny<Func<IQueryable<EventFeedback>,
                    IIncludableQueryable<EventFeedback, object>>>()
                    )).ReturnsAsync(new EventFeedback());

            //Act
            var result = await _actionManager.LeaveFeedbackAsync(testEventId, new EventFeedbackDto(), new User());

            //Assert
            _mockRepositoryWrapper.Verify(x => x.EventFeedback.Update(It.IsAny<EventFeedback>()), Times.Once);
            Assert.NotNull(result);
            Assert.IsInstanceOf<int>(result);
            Assert.AreEqual(StatusCodes.Status200OK, result);
        }

        [Test]
        public async Task LeaveFeedback_FeedbackDoesntExist_FeedbackIsCreatedAndReturnsOk()
        {
            //Arrange
            _mockRepositoryWrapper.Setup(x => x.Event.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<DataAccess.Entities.Event.Event, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccess.Entities.Event.Event>,
                    IIncludableQueryable<DataAccess.Entities.Event.Event, object>>>()
                    )).ReturnsAsync(new DataAccess.Entities.Event.Event() { ID = testEventId });

            _mockRepositoryWrapper.Setup(x => x.Participant.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Participant, bool>>>(),
                It.IsAny<Func<IQueryable<Participant>,
                    IIncludableQueryable<Participant, object>>>()
                    )).ReturnsAsync(new Participant() { ID = testParticipantId });

            _mockRepositoryWrapper.Setup(x => x.EventFeedback.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<EventFeedback, bool>>>(),
                It.IsAny<Func<IQueryable<EventFeedback>,
                    IIncludableQueryable<EventFeedback, object>>>()
                    )).ReturnsAsync((EventFeedback)null);

            _mockMapper.Setup(x => x.Map<EventFeedbackDto, EventFeedback>(It.IsAny<EventFeedbackDto>())).Returns(new EventFeedback());

            //Act
            var result = await _actionManager.LeaveFeedbackAsync(testEventId, new EventFeedbackDto(), new User());

            //Assert
            _mockRepositoryWrapper.Verify(x => x.EventFeedback.CreateAsync(It.IsAny<EventFeedback>()), Times.Once);
            Assert.NotNull(result);
            Assert.IsInstanceOf<int>(result);
            Assert.AreEqual(StatusCodes.Status200OK, result);
        }

        [Test]
        public async Task DeleteFeedback_EventDoesntExist_ReturnsNotFound()
        {
            //Arrange
            _mockRepositoryWrapper.Setup(x => x.Event.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<DataAccess.Entities.Event.Event, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccess.Entities.Event.Event>,
                    IIncludableQueryable<DataAccess.Entities.Event.Event, object>>>()
                    )).ReturnsAsync((DataAccess.Entities.Event.Event)null);

            //Act
            var result = await _actionManager.DeleteFeedbackAsync(testEventId, testFeedbackId, new User());

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<int>(result);
            Assert.AreEqual(StatusCodes.Status404NotFound, result);
        }

        [Test]
        public async Task DeleteFeedback_FeedbackDoesntExist_ReturnsNotFound()
        {
            //Arrange
            _mockRepositoryWrapper.Setup(x => x.Event.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<DataAccess.Entities.Event.Event, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccess.Entities.Event.Event>,
                    IIncludableQueryable<DataAccess.Entities.Event.Event, object>>>()
                    )).ReturnsAsync(new DataAccess.Entities.Event.Event());

            _mockRepositoryWrapper.Setup(x => x.EventFeedback.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<EventFeedback, bool>>>(),
                It.IsAny<Func<IQueryable<EventFeedback>,
                    IIncludableQueryable<EventFeedback, object>>>()
                    )).ReturnsAsync((EventFeedback)null);

            _mockUserManager.Setup(x => x.GetUserIdAsync(It.IsAny<User>()))
                .ReturnsAsync(fakeIdString);

            _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new string[] { Roles.Supporter });


            //Act
            var result = await _actionManager.DeleteFeedbackAsync(testEventId, testFeedbackId, new User());

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<int>(result);
            Assert.AreEqual(StatusCodes.Status404NotFound, result);
        }

        [Test]
        public async Task DeleteFeedback_UserIsAdminAndUserIdDoesntMatch_ReturnsOk()
        {
            //Arrange
            string currentFakeUserId = Guid.Empty.ToString();
            string otherFakeUserId = Guid.NewGuid().ToString();

            _mockRepositoryWrapper.Setup(x => x.Event.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<DataAccess.Entities.Event.Event, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccess.Entities.Event.Event>,
                    IIncludableQueryable<DataAccess.Entities.Event.Event, object>>>()
                    )).ReturnsAsync(new DataAccess.Entities.Event.Event());

            _mockRepositoryWrapper.Setup(x => x.EventFeedback.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<EventFeedback, bool>>>(),
                It.IsAny<Func<IQueryable<EventFeedback>,
                    IIncludableQueryable<EventFeedback, object>>>()
                    )).ReturnsAsync(new EventFeedback() { Participant = new Participant() { UserId = otherFakeUserId }});

            _mockUserManager.Setup(x => x.GetUserIdAsync(It.IsAny<User>()))
                .ReturnsAsync(currentFakeUserId);

            _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new string[] { Roles.Admin });

            //Act
            var result = await _actionManager.DeleteFeedbackAsync(testEventId, testFeedbackId, new User());

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<int>(result);
            Assert.AreEqual(StatusCodes.Status200OK, result);
            _mockRepositoryWrapper.Verify(x => x.EventFeedback.Delete(It.IsAny<EventFeedback>()), Times.Once);
        }

        [Test]
        public async Task DeleteFeedback_UserIsNotAdminAndUserIdMatches_ReturnsForbidden()
        {
            //Arrange
            string currentFakeUserId = Guid.Empty.ToString();

            _mockRepositoryWrapper.Setup(x => x.Event.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<DataAccess.Entities.Event.Event, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccess.Entities.Event.Event>,
                    IIncludableQueryable<DataAccess.Entities.Event.Event, object>>>()
                    )).ReturnsAsync(new DataAccess.Entities.Event.Event());

            _mockRepositoryWrapper.Setup(x => x.EventFeedback.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<EventFeedback, bool>>>(),
                It.IsAny<Func<IQueryable<EventFeedback>,
                    IIncludableQueryable<EventFeedback, object>>>()
                    )).ReturnsAsync(new EventFeedback() { Participant = new Participant() { UserId = currentFakeUserId } });

            _mockUserManager.Setup(x => x.GetUserIdAsync(It.IsAny<User>()))
                .ReturnsAsync(currentFakeUserId);

            _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new string[] { Roles.Supporter });

            //Act
            var result = await _actionManager.DeleteFeedbackAsync(testEventId, testFeedbackId, new User());

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<int>(result);
            Assert.AreEqual(StatusCodes.Status200OK, result);
            _mockRepositoryWrapper.Verify(x => x.EventFeedback.Delete(It.IsAny<EventFeedback>()), Times.Once);
        }

        [Test]
        public async Task DeleteFeedback_UserIsNotAdminAndUserIdDoesntMatch_ReturnsForbidden()
        {
            //Arrange
            string currentFakeUserId = Guid.Empty.ToString();
            string otherFakeUserId = Guid.NewGuid().ToString();

            _mockRepositoryWrapper.Setup(x => x.Event.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<DataAccess.Entities.Event.Event, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccess.Entities.Event.Event>,
                    IIncludableQueryable<DataAccess.Entities.Event.Event, object>>>()
                    )).ReturnsAsync(new DataAccess.Entities.Event.Event());

            _mockRepositoryWrapper.Setup(x => x.EventFeedback.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<EventFeedback, bool>>>(),
                It.IsAny<Func<IQueryable<EventFeedback>,
                    IIncludableQueryable<EventFeedback, object>>>()
                    )).ReturnsAsync(new EventFeedback() { Participant = new Participant() { UserId = otherFakeUserId } });

            _mockUserManager.Setup(x => x.GetUserIdAsync(It.IsAny<User>()))
                .ReturnsAsync(currentFakeUserId);

            _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new string[] { Roles.Supporter });

            //Act
            var result = await _actionManager.DeleteFeedbackAsync(testEventId, testFeedbackId, new User());

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<int>(result);
            Assert.AreEqual(StatusCodes.Status403Forbidden, result);
        }

        [Test]
        public async Task GetCategoryById_CategoryExists_ReturnsCategory()
        {
            //Arrange
            int categoryId = 1;
            _mockRepositoryWrapper.Setup(x => x.EventCategory.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<EventCategory, bool>>>(),
                    It.IsAny<Func<IQueryable<EventCategory>,
                        IIncludableQueryable<EventCategory, object>>>()))
                .ReturnsAsync(new EventCategory() { ID = categoryId });

            _mockMapper.Setup(x => x.Map<EventCategory, EventCategoryDto>(It.IsAny<EventCategory>()))
                .Returns(new EventCategoryDto() { EventCategoryId = categoryId });

            //Act
            var result = await _actionManager.GetCategoryByIdAsync(categoryId);

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<EventCategoryDto>(result);
        }

        [Test]
        public async Task GetCategoryById_CategoryDoesntExist_ReturnsNull()
        {
            //Arrange
            int categoryId = 1;
            var eventToCheck = _mockRepositoryWrapper.Setup(x => x.EventCategory.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<EventCategory, bool>>>(),
                    It.IsAny<Func<IQueryable<EventCategory>,
                        IIncludableQueryable<EventCategory, object>>>()))
                .ReturnsAsync((EventCategory)null);

            //Act
            var result = await _actionManager.GetCategoryByIdAsync(categoryId);

            //Assert
            Assert.Null(result);
        }

    }
}
