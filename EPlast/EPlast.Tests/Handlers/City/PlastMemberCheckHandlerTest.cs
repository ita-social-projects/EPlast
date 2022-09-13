using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.Handlers.CityHandlers;
using EPlast.BLL.Queries.City;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Handlers.City
{
    public class PlastMemberCheckHandlerTest
    {
        private Mock<UserManager<User>> _mockUserManager;
        private Mock<IUserStore<User>> _mockUser;
        private PlastMemberCheckHandler _handler;
        private PlastMemberCheckQuery _query;

        private const string UserId = "UserId";

        [SetUp]
        public void SetUp()
        {
            _mockUser = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(_mockUser.Object, null, null, null, null, null, null, null, null);
            _handler = new PlastMemberCheckHandler(_mockUserManager.Object);
            _query = new PlastMemberCheckQuery(UserId);
        }

        [Test]
        public async Task PlastMemberCheckHandlerTest_ReturnsBool()
        {
            //Arrange
            _mockUserManager
                .Setup(u => u.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _mockUserManager
                .Setup(u => u.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(new bool());

            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<bool>(result);
        }
    }
}
