using AutoMapper;
using EPlast.BLL.DTO.AnnualReport;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.BLL.Interfaces.Region;
using EPlast.BLL.Mapping.AnnualReport;
using EPlast.BLL.Services;
using EPlast.BLL.Services.EventUser.EventUserAccess;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EPlast.Tests.Services.Event
{
    class EventUserAccessServiceTests
    {
        private IEventUserAccessService _eventUserAccessService;
        private Mock<IEventAdmininistrationManager> _eventAdministrationManager;

        [SetUp]
        public void SetUp()
        {
            _eventAdministrationManager = new Mock<IEventAdmininistrationManager>();
            _eventUserAccessService = new EventUserAccessService(_eventAdministrationManager.Object);
        }

        [Test]
        public async Task HasAccessAsync_ReturnsBool()
        {
            //Arrange
            _eventAdministrationManager
                .Setup(x => x.GetEventAdmininistrationByUserIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<EventAdministration>() { new EventAdministration { ID = 1 } });
            //Act
            var result = await _eventUserAccessService.HasAccessAsync(new User { Id = "1" }, It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<bool>(result);
        }
    }
}

