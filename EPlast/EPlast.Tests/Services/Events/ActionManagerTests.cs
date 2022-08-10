﻿using System;
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

        private int testEventId = 1;
        private int testEstimate = 1;
        private readonly string fakeIdString = "1";
        private int testCategoryId = 1;
        private int testTypeId = 1;
    }
}
