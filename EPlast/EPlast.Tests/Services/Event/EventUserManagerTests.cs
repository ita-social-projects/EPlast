using AutoMapper;
using EPlast.BLL.DTO.EventUser;
using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.BLL.Services.EventUser;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

            _service = new EventUserManager(_repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object,
                _eventStatusManager.Object, _eventAdministrationTypeManager.Object, _userManager.Object);
        }

        [Test]
        public async Task EditEventAsyncTest_UpdatesAndSavesRepo()
        {
            //Arrange
            _eventStatusManager
                .Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(1);
            _mapper
                .Setup(x => x.Map<EventCreationDTO, DAEvent>(It.IsAny<EventCreationDTO>()))
                .Returns(new DAEvent() { EventAdministrations = new List<EventAdministration>(), ID = 0 });
            _repoWrapper
                .Setup(x => x.EventAdministration.GetFirstAsync(
                    It.IsAny<Expression<Func<EventAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<EventAdministration>, IIncludableQueryable<EventAdministration, object>>>()))
                .ReturnsAsync(new EventAdministration() { ID = 1 });
            _repoWrapper
                .Setup(x => x.Event.Update(It.IsAny<DAEvent>()));
            var inputModel = new EventCreateDTO();
            inputModel.Event = new EventCreationDTO();
            inputModel.Сommandant = new EventAdministrationDTO();
            inputModel.Alternate = new EventAdministrationDTO();
            inputModel.Bunchuzhnyi = new EventAdministrationDTO();
            inputModel.Pysar = new EventAdministrationDTO();

            //Act
            await _service.EditEventAsync(inputModel);

            //Assert
            _repoWrapper.Verify(x => x.Event.Update(It.IsAny<DAEvent>()));
            _repoWrapper.Verify(x => x.SaveAsync());
        }

        [Test]  
        public async Task EditEventAsyncTest_UpdatesAndSavesRepo_WithoutAlternate()
        {
            //Arrange
            _eventStatusManager
                .Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(1);
            _mapper
                .Setup(x => x.Map<EventCreationDTO, DAEvent>(It.IsAny<EventCreationDTO>()))
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
            var inputModel = new EventCreateDTO();
            inputModel.Event = new EventCreationDTO();
            inputModel.Сommandant = new EventAdministrationDTO();
            inputModel.Alternate = new EventAdministrationDTO() { UserId = "Nazario Nazario" };
            inputModel.Bunchuzhnyi = new EventAdministrationDTO();
            inputModel.Pysar = new EventAdministrationDTO();

            //Act
            await _service.EditEventAsync(inputModel);

            //Assert
            _repoWrapper.Verify(x => x.Event.Update(It.IsAny<DAEvent>()));
            _repoWrapper.Verify(x => x.SaveAsync());
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
