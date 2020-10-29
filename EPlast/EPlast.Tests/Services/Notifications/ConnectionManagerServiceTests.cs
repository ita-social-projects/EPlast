using EPlast.BLL.Services.Notifications;
using NUnit.Framework;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPlast.Tests.Services.Notifications
{
    [TestFixture]
    public class ConnectionManagerServiceTests
    {
        private ConnectionManagerService _connectionManagerService;

        [SetUp]
        public void SetUp()
        {
            _connectionManagerService = new ConnectionManagerService();
            var connection = GetTestConnection();
            _connectionManagerService.AddConnection(connection.userId, connection.connectionId);
        }

        [Test]
        public void AddConnection_Valid_Void()
        {
            var newConnection = GetNewTestConnection();

            //Act
            _connectionManagerService.AddConnection(newConnection.userId, newConnection.connectionId);

            //Assert
            Assert.IsTrue(_connectionManagerService.OnlineUsers.Contains(newConnection.userId));
        }

        [Test]
        public void RemoveConnection_Valid_Void()
        {
            var connection = GetTestConnection();

            //Act
            _connectionManagerService.RemoveConnection(connection.connectionId);

            //Assert
            Assert.IsFalse(_connectionManagerService.GetConnections(connection.userId).Contains(connection.connectionId));
        }

        [Test]
        public void GetConnections_Valid_ArrayOfString()
        {
            var connection = GetTestConnection();

            //Act
            var result = _connectionManagerService.GetConnections(connection.userId);

            //Assert
            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result.Contains(connection.connectionId));
        }

        private (string userId, string connectionId) GetTestConnection()
        { 
            return ("u111", "c111");
        }

        private (string userId, string connectionId) GetNewTestConnection()
        {
            return ("u222", "c222");
        }
    }
}
