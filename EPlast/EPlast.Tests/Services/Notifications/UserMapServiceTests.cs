using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Notification;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.Notifications;
using EPlast.BLL.Services.Notifications;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services.Notifications
{

    [TestFixture]
    class UserMapServiceTests
    {
        private Mock<IUserMapService> _userMap;

        private ConcurrentDictionary<string, HashSet<ConnectionDto>> userMapDictionary;
        private UserMapService userMapService;
        #region  Setup
        [SetUp]
        public void SetUp()

        {
            _userMap = new Mock<IUserMapService>();
            userMapService = new UserMapService();
            userMapDictionary = new ConcurrentDictionary<string, HashSet<ConnectionDto>>();
        }
        #endregion
        #region CreateAnObject
        [Test]
        public void CreateNotificationConnectionManager_IsNotNull()
        {
            //Arrange

            //Act
            var result = new UserMapService();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<UserMapService>(result);
        }
        #endregion
        #region  ReturnsNotNull
        [Test]
        public void UserConnections_IsNotNull()
        {
            //Arrange
            _userMap.Setup(a => a.UserConnections).Returns(userMapDictionary);
            //Act
            var result = userMapService.UserConnections;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ConcurrentDictionary<string, HashSet<ConnectionDto>>>(result);
        }
        #endregion
    }
}
