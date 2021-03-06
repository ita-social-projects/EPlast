﻿using System;
using AutoMapper;
using EPlast.BLL.DTO.EventUser;
using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EPlast.BLL.Services.EventUser;
using Microsoft.EntityFrameworkCore.Query;
using NUnit.Framework;
using Moq;
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
        private EventUserManager _service;

        [SetUp]
        public void Setup()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _eventCategoryManager = new Mock<IEventCategoryManager>();
            _eventStatusManager = new Mock<IEventStatusManager>();
            _eventAdministrationTypeManager = new Mock<IEventAdministrationTypeManager>();

            _service = new EventUserManager(_repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object,
                _eventStatusManager.Object, _eventAdministrationTypeManager.Object);
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
