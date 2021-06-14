using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.EventUser;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.Events;
using Microsoft.AspNetCore.Identity;
using EPlast.DataAccess.Repositories;
using Moq;
using NUnit.Framework;
using EPlast.BLL.Services.EventUser;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DBUser = EPlast.DataAccess.Entities.User;

namespace EPlast.Tests.Services.Event
{
    class EventUserServiceTests
    {
        private EventUserService _service;
        private Mock<IRepositoryWrapper> _repositoryWrapper;
        private Mock<UserManager<DBUser>> _userManager;
        private Mock<IMapper> _mapper;
        private Mock<IParticipantStatusManager> _participantStatusManager;
        private Mock<IParticipantManager> _participantManager;
        private Mock<IEventAdmininistrationManager> _eventAdministrationManager;

        [SetUp]
        public void Setup()
        {
            _repositoryWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _participantStatusManager = new Mock<IParticipantStatusManager>();
            _participantManager = new Mock<IParticipantManager>();
            _eventAdministrationManager = new Mock<IEventAdmininistrationManager>();

            var userStoreMock = new Mock<IUserStore<DBUser>>();
            _userManager = new Mock<UserManager<DBUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            _service = new EventUserService(_repositoryWrapper.Object, _userManager.Object,
                _participantStatusManager.Object, _mapper.Object, _participantManager.Object,
                _eventAdministrationManager.Object);
        }

        [Test]
        public async Task EventUserAsyncTest_ParticipantEventDateEndIsMax()
        {
            //Arrange
            _userManager
                .Setup(x => x.GetUserIdAsync(It.IsAny<DBUser>()))
                .ReturnsAsync("some id");
            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new DBUser());
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<DBUser>()))
                .ReturnsAsync(new List<string>() { "role 1", "role 2"});
            _mapper
                .Setup(x => x.Map<DBUser, UserDTO>(It.IsAny<DBUser>()))
                .Returns(new UserDTO());
            _mapper
                .Setup(x =>
                    x.Map<DataAccess.Entities.Event.Event, EventGeneralInfoDTO>(
                        It.IsAny<DataAccess.Entities.Event.Event>()))
                .Returns(new EventGeneralInfoDTO() { EventDateEnd = DateTime.MaxValue });
            _eventAdministrationManager
                .Setup(x => x.GetEventAdmininistrationByUserIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<EventAdministration>() { new EventAdministration() });
            _participantManager
                .Setup(x => x.GetParticipantsByUserIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<Participant>() { new Participant()
                {
                    Event = new DataAccess.Entities.Event.Event() { EventDateEnd = DateTime.MaxValue }
                } });
            _repositoryWrapper
                .Setup(x => x.User.GetFirstAsync(null, null))
                .ReturnsAsync(new DBUser());

            //Act
            var res = await _service.EventUserAsync("", new User());

            //Assert
            Assert.IsInstanceOf<EventUserDTO>(res);
        }

        [Test]
        public async Task EventUserAsyncTest_ParticipantEventDateEndIsMin()
        {
            //Arrange
            _userManager
                .Setup(x => x.GetUserIdAsync(It.IsAny<DBUser>()))
                .ReturnsAsync("some id");
            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new DBUser());
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<DBUser>()))
                .ReturnsAsync(new List<string>() { "role 1", "role 2" });
            _mapper
                .Setup(x => x.Map<DBUser, UserDTO>(It.IsAny<DBUser>()))
                .Returns(new UserDTO());
            _mapper
                .Setup(x =>
                    x.Map<DataAccess.Entities.Event.Event, EventGeneralInfoDTO>(
                        It.IsAny<DataAccess.Entities.Event.Event>()))
                .Returns(new EventGeneralInfoDTO() { EventDateEnd = DateTime.MaxValue });
            _eventAdministrationManager
                .Setup(x => x.GetEventAdmininistrationByUserIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<EventAdministration>() { new EventAdministration() });
            _participantManager
                .Setup(x => x.GetParticipantsByUserIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<Participant>() { new Participant()
                {
                    Event = new DataAccess.Entities.Event.Event() { EventDateEnd = DateTime.MinValue },
                } });
            _participantStatusManager
                .Setup(x => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(0);
            _repositoryWrapper
                .Setup(x => x.User.GetFirstAsync(null, null))
                .ReturnsAsync(new DBUser());

            //Act
            var res = await _service.EventUserAsync("", new User());

            //Assert
            Assert.IsInstanceOf<EventUserDTO>(res);
        }
    }
}
