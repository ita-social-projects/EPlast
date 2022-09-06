using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.EventUser;
using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.BLL.Interfaces.UserAccess;
using EPlast.BLL.Services.EventUser;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using DAEvent = EPlast.DataAccess.Entities.Event.Event;

namespace EPlast.Tests.Services.Event
{
    class EventUserManagerTests
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private Mock<IEventCategoryManager> _eventCategoryManager;
        private Mock<IEventStatusManager> _eventStatusManager;
        private Mock<IEventAdministrationTypeManager> _eventAdministrationTypeManager;
        private Mock<IUserAccessService> _userAccesses;
        private Mock<UserManager<User>> _userManager;
        private EventUserManager _service;

        [SetUp]
        public void Setup()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _eventCategoryManager = new Mock<IEventCategoryManager>();
            _eventStatusManager = new Mock<IEventStatusManager>();
            _eventAdministrationTypeManager = new Mock<IEventAdministrationTypeManager>();
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _userAccesses = new Mock<IUserAccessService>();

            _service = new EventUserManager(_repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object,
                _eventStatusManager.Object, _eventAdministrationTypeManager.Object, _userManager.Object, _userAccesses.Object);
        }

        [Test]
        public async Task EditEventAsyncTest_UserHasNoAccess_ReturnsFalse()
        {
            //Arrange
            EventCreateDto model = new EventCreateDto()
            {
                Event = new EventCreationDto() { ID = 1 }
            };

            _userAccesses.Setup(x => x.GetUserEventAccessAsync(It.IsAny<string>(), It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(new Dictionary<string, bool>() { { "EditEvent", false } });

            //Act
            var result = await _service.EditEventAsync(model, new User() { Id = Guid.Empty.ToString() });

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task EditEventAsyncTest_UserHasAccess_EventIsEditedAndReturnsTrue()
        {
            //Arrange
            var tempAlternate = new EventAdministration() { UserID = "2" };
            List<EventAdministration> tempList = new List<EventAdministration>();
            var initializedEvent = new DAEvent() { EventAdministrations = tempList, ID = 0 };

            EventCreateDto model = new EventCreateDto();
            model.Сommandant = new EventAdministrationDto() { UserId = "4" };
            model.Alternate = new EventAdministrationDto() { UserId = "2" };
            model.Bunchuzhnyi = new EventAdministrationDto() { UserId = "1" };
            model.Pysar = new EventAdministrationDto() { UserId = "3" };
            model.Event = new EventCreationDto() { ID = 1 };

            _userAccesses.Setup(x => x.GetUserEventAccessAsync(It.IsAny<string>(), It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(new Dictionary<string, bool>() { { "EditEvent", true } });

            _mapper
               .Setup(x => x.Map<EventCreationDto, DAEvent>(It.IsAny<EventCreationDto>()))
               .Returns(initializedEvent);
            _repoWrapper
               .Setup(x => x.EventAdministration.GetFirstAsync(
                   It.IsAny<Expression<Func<EventAdministration, bool>>>(),
                   It.IsAny<Func<IQueryable<EventAdministration>, IIncludableQueryable<EventAdministration, object>>>()))
               .ReturnsAsync(new EventAdministration() { ID = 1 });
            _repoWrapper.Setup(x => x.EventAdministration.GetFirstAsync(It.IsAny<Expression<Func<EventAdministration, bool>>>(), null))
                .ReturnsAsync(new EventAdministration() { UserID = "1" });
            _repoWrapper
                .Setup(x => x.EventAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<EventAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<EventAdministration>, IIncludableQueryable<EventAdministration, object>>>()))
                .ReturnsAsync(tempAlternate);
            _repoWrapper
               .Setup(x => x.Event.Update(It.IsAny<DAEvent>()));

            //Act
            var result = await _service.EditEventAsync(model, new User() { Id = Guid.Empty.ToString() });

            //Assert
            _repoWrapper.Verify(t => t.EventAdministration.Delete(tempAlternate));
            Assert.AreEqual(4, initializedEvent.EventAdministrations.Count);
            Assert.IsTrue(result);
        }


        [Test]
        public async Task EditEventAsyncTest_UpdatesAndSavesRepo_WithoutAlternate()
        {
            //Arrange
            _userAccesses.Setup(x => x.GetUserEventAccessAsync(It.IsAny<string>(), It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(new Dictionary<string, bool>() { { "EditEvent", true } });

            _eventStatusManager
                .Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(1);
            _mapper
                .Setup(x => x.Map<EventCreationDto, DAEvent>(It.IsAny<EventCreationDto>()))
                .Returns(new DAEvent() { EventAdministrations = new List<EventAdministration>(), ID = 0 });
            _repoWrapper
                .Setup(x => x.EventAdministration.GetFirstAsync(
                    It.IsAny<Expression<Func<EventAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<EventAdministration>, IIncludableQueryable<EventAdministration, object>>>()))
                .ReturnsAsync(new EventAdministration() { ID = 1 });
            _repoWrapper
                .Setup(x => x.Event.Update(It.IsAny<DAEvent>()));
            var inputModel = new EventCreateDto();
            inputModel.Event = new EventCreationDto() { ID = 1};
            inputModel.Сommandant = new EventAdministrationDto();
            inputModel.Alternate = new EventAdministrationDto();
            inputModel.Bunchuzhnyi = new EventAdministrationDto();
            inputModel.Pysar = new EventAdministrationDto();

            //Act
            var result = await _service.EditEventAsync(inputModel, new User() { Id = Guid.Empty.ToString()});

            //Assert
            _repoWrapper.Verify(x => x.Event.Update(It.IsAny<DAEvent>()));
            _repoWrapper.Verify(x => x.SaveAsync());
            Assert.IsTrue(result);
        }

        [Test]
        public async Task EditEventAsyncTest_UpdatesAndSavesRepo()
        {
            //Arrange
            _userAccesses.Setup(x => x.GetUserEventAccessAsync(It.IsAny<string>(), It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(new Dictionary<string, bool>() { { "EditEvent", true } });

            _eventStatusManager
                .Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(1);
            _mapper
                .Setup(x => x.Map<EventCreationDto, DAEvent>(It.IsAny<EventCreationDto>()))
                .Returns(new DAEvent() { EventAdministrations = new List<EventAdministration>(), ID = 0 });
            _repoWrapper
                .Setup(x => x.EventAdministration.GetFirstAsync(
                    It.IsAny<Expression<Func<EventAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<EventAdministration>, IIncludableQueryable<EventAdministration, object>>>()))
                .ReturnsAsync(new EventAdministration() { ID = 1 });
            _repoWrapper
                .Setup(x => x.EventAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<EventAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<EventAdministration>, IIncludableQueryable<EventAdministration, object>>>()))
                .ReturnsAsync(new EventAdministration() { ID = 1 });
            _repoWrapper
                .Setup(x => x.Event.Update(It.IsAny<DAEvent>()));
            var inputModel = new EventCreateDto();
            inputModel.Event = new EventCreationDto();
            inputModel.Сommandant = new EventAdministrationDto();
            inputModel.Alternate = new EventAdministrationDto() { UserId = "Nazario Nazario" };
            inputModel.Bunchuzhnyi = new EventAdministrationDto();
            inputModel.Pysar = new EventAdministrationDto();

            //Act
            var result = await _service.EditEventAsync(inputModel, new User() { Id = Guid.Empty.ToString() });

            //Assert
            _repoWrapper.Verify(x => x.Event.Update(It.IsAny<DAEvent>()));
            _repoWrapper.Verify(x => x.SaveAsync());
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ApproveEventAsync_Returns200()
        {
            //Arrange
            _repoWrapper
                .Setup(x => x.Event.GetFirstAsync(
                    It.IsAny<Expression<Func<DAEvent, bool>>>(), 
                    It.IsAny<Func<IQueryable<DAEvent>, IIncludableQueryable<DAEvent, object>>>()))
                .ReturnsAsync(new DAEvent());
            _repoWrapper
                .Setup(x => x.Event.Update(It.IsAny<DAEvent>()));
            _eventStatusManager
                .Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(1);

            //Act
            var result = await _service.ApproveEventAsync(1);

            //Assert
            Assert.AreEqual(200, result);
        }

        [Test]
        public async Task ApproveEventAsync_Returns400()
        {
            //Arrange
            _repoWrapper
                .Setup(x => x.Event.GetFirstAsync(
                    It.IsAny<Expression<Func<DAEvent, bool>>>(),
                    It.IsAny<Func<IQueryable<DAEvent>, IIncludableQueryable<DAEvent, object>>>()))
                .ReturnsAsync(new DAEvent());
            _repoWrapper
                .Setup(x => x.Event.Update(It.IsAny<DAEvent>()))
                .Throws<Exception>();

            //Act
            var result = await _service.ApproveEventAsync(1);

            //Assert
            Assert.AreEqual(400, result);
        }
    }
}
