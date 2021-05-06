using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.Notifications;
using System;
using System.Collections.Concurrent;
using EPlast.BLL.Services.Notifications;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Notification;

namespace EPlast.Tests.Services.Notifications
{
    [TestFixture]
    class NotificationConnectionManagerTests
    {
        #region  Setup
        private Mock<INotificationConnectionManager> _notificationConnectionManager;
        private Mock<IUniqueIdService> _uniqueId;
        private Mock<IUserMapService> _userMap;
        private Mock<WebSocket> _socket;
        private NotificationConnectionManager notificationConnectionManager;
        private ConcurrentDictionary<string, HashSet<ConnectionDTO>> userMap;
        private Guid uniqueID;

        [SetUp]
        public void SetUp()
        {
            _uniqueId = new Mock<IUniqueIdService>();
            _userMap = new Mock<IUserMapService>();
            _notificationConnectionManager = new Mock<INotificationConnectionManager>();
            _socket = new Mock<WebSocket>();
            uniqueID = Guid.NewGuid();
            notificationConnectionManager = new NotificationConnectionManager(_uniqueId.Object, _userMap.Object);
            userMap = new ConcurrentDictionary<string, HashSet<ConnectionDTO>>();
            var hash = new HashSet<ConnectionDTO>();
            var connectionDTO = new ConnectionDTO()
            {
                WebSocket = _socket.Object,
                ConnectionId = "1"
            };
            hash.Add(connectionDTO);
            userMap.TryAdd("1",hash);
        }
        #endregion
        #region CreateAnObject
        [Test]
        public void CreateNotificationConnectionManager_IsNotNull()
        {
            //Arrange

            //Act
            var result = new NotificationConnectionManager(_uniqueId.Object, _userMap.Object);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotificationConnectionManager>(result);
        }
        #endregion
        #region  ReturnsNotNull
        [Test]
        public void OnlineUsers_ReturnsNotNull()
        {
            //Arrange
            _userMap.Setup(a => a.UserConnections).Returns(userMap);
            _notificationConnectionManager.Setup(a => a.OnlineUsers).Returns(userMap.Keys);

            //Act
            var result = notificationConnectionManager.OnlineUsers;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
            Assert.AreEqual(userMap.Keys, result);
        }

        [Test]
        public void GetSocketByConnectionId_ReturnsNotNull()
        {
            //Arrange
            var id = "1";
            _userMap.Setup(a => a.UserConnections).Returns(userMap);
            _notificationConnectionManager.Setup(
                a => a.GetSocketByConnectionId(id)).Returns(_socket.Object);

            //Act
            var result = notificationConnectionManager.GetSocketByConnectionId(id);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<WebSocket>(result);
            Assert.AreEqual(_socket.Object, result);
        }

        [Test]
        public void GetUserId_ReturnsNotNull()
        {
            //Arrange
            var id = "1";
            _userMap.Setup(a => a.UserConnections).Returns(userMap);
            _notificationConnectionManager.Setup(
                a => a.GetUserId(_socket.Object)).Returns(id);

            //Act
            var result = notificationConnectionManager.GetUserId(_socket.Object);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<string>(result);
            Assert.AreEqual(id, result);
        }

        [Test]
        public void AddSocket_ReturnsNotNull_Add()
        {
            //Arrange
            var id = "1";
            _userMap.Setup(a => a.UserConnections).Returns(userMap);
            _uniqueId.Setup(a => a.GetUniqueId()).Returns(uniqueID);
            _notificationConnectionManager.Setup(
                a => a.AddSocket(id, _socket.Object)).Returns(id);

            //Act
            var result = notificationConnectionManager.AddSocket(id, _socket.Object);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<string>(result);
            Assert.AreEqual(uniqueID.ToString(), result);
        }

        [Test]
        public void AddSocket_ReturnsNotNull_TryAdd()
        {
            //Arrange
            var id = "2";
            _userMap.Setup(a => a.UserConnections).Returns(userMap);
            _uniqueId.Setup(a => a.GetUniqueId()).Returns(uniqueID);
            _notificationConnectionManager.Setup(
                a => a.AddSocket(id, _socket.Object)).Returns(id);

            //Act
            var result = notificationConnectionManager.AddSocket(id, _socket.Object);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<string>(result);
            Assert.AreEqual(uniqueID.ToString(), result);
        }

        [Test]
        public void RemoveSocketAsync_ReturnsNotNull()
        {
            //Arrange
            var id = "1";
            _userMap.Setup(a => a.UserConnections).Returns(userMap);
            _uniqueId.Setup(a => a.GetUniqueId()).Returns(uniqueID);
            _notificationConnectionManager.Setup(b => b.GetSocketByConnectionId(id)).Returns(_socket.Object);
            var task = notificationConnectionManager.RemoveSocketAsync(id, uniqueID.ToString());
            _notificationConnectionManager.Setup(a => a.RemoveSocketAsync(
                id, uniqueID.ToString())).Returns(task);

            //Act
            var result = notificationConnectionManager.RemoveSocketAsync(id, id);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Task<bool>>(result);
        }

        [Test]
        public void SendMessageAsync_ReturnsNotNull()
        {
            //Arrange
            var id = "1";
            string message = "test";
            _userMap.Setup(a => a.UserConnections).Returns(userMap);
            var task = _notificationConnectionManager.Object.SendMessageAsync(id, message);
            _notificationConnectionManager.Setup(
                a => a.SendMessageAsync(id, message)).Returns(task);

            //Act
            var result = notificationConnectionManager.SendMessageAsync(id, message);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Task>(result);
        }
        #endregion
        #region ThrowsException
        [Test]
        public void GetSocketByConnectionId_ThrowsException()
        {
            //Arrange
            var id = "1";
            _notificationConnectionManager.Setup(a => a.GetSocketByConnectionId(id)).Throws(new InvalidOperationException());
            _userMap.Setup(a => a.UserConnections).Returns(userMap);

            //Act

            //Assert
            Assert.Throws<InvalidOperationException>(() => _notificationConnectionManager.Object.GetSocketByConnectionId(id));
        }

        [Test]
        public void GetUserId_ThrowsException()
        {
            //Arrange
            _userMap.Setup(a => a.UserConnections).Returns(new ConcurrentDictionary<string, HashSet<ConnectionDTO>>());
            _notificationConnectionManager.Setup(
                a => a.GetUserId(_socket.Object));

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => notificationConnectionManager.GetUserId(_socket.Object));
        }
        #endregion
    }
}
