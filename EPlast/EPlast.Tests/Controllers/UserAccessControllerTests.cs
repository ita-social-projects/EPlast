using EPlast.BLL.Interfaces.UserAccess;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.Tests.Controllers
{
    [TestFixture]
    class UserAcessControllerTests
    {
        private Mock<IUserAccessService> _userAccessService;
        private Mock<UserManager<User>> _userManager;

        private UserAccessController _userAccessController;

        [SetUp]
        public void SetUp()
        {
            _userAccessService = new Mock<IUserAccessService>();
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            _userAccessController = new UserAccessController(_userAccessService.Object, _userManager.Object);
        }

        [Test]
        public async Task GetUserClubAccesses_ReturnsAccessList()
        {
            //Arrange
            Dictionary<string, bool> dict = new Dictionary<string, bool>();
            dict.Add("action", It.IsAny<bool>());
            _userAccessService
                .Setup(x => x.GetUserClubAccessAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<User>()))
                .ReturnsAsync(dict);

            //Act
            var result = await _userAccessController.GetUserClubAccess(It.IsAny<int>(), It.IsAny<string>());
            var resultValue = (result as ObjectResult)?.Value;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotEmpty(resultValue as Dictionary<string, bool>);
            Assert.IsInstanceOf<Dictionary<string, bool>>(resultValue);
        }

        [Test]
        public async Task GetEventUserAccess_ReturnsAccessList()
        {
            //Arrange
            Dictionary<string, bool> dict = new Dictionary<string, bool>();
            dict.Add("action", It.IsAny<bool>());
            _userAccessService
                .Setup(x => x.GetUserEventAccessAsync(It.IsAny<string>(), It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(dict);

            //Act
            var result = await _userAccessController.GetEventUserAccess(It.IsAny<string>(), It.IsAny<int>());
            var resultValue = (result as ObjectResult)?.Value;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotEmpty(resultValue as Dictionary<string, bool>);
            Assert.IsInstanceOf<Dictionary<string, bool>>(resultValue);
        }

        [Test]
        public async Task GetUserCityAccesses_ReturnsAccessList()
        {
            //Arrange
            Dictionary<string, bool> dict = new Dictionary<string, bool>();
            dict.Add("action", It.IsAny<bool>());
            _userAccessService
                .Setup(x => x.GetUserCityAccessAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<User>()))
                .ReturnsAsync(dict);

            //Act
            var result = await _userAccessController.GetUserCityAccess(It.IsAny<int>(), It.IsAny<string>());
            var resultValue = (result as ObjectResult)?.Value;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotEmpty(resultValue as Dictionary<string, bool>);
            Assert.IsInstanceOf<Dictionary<string, bool>>(resultValue);
        }

        [Test]
        public async Task GetUserRegionAccesses_ReturnsAccessList()
        {
            //Arrange
            Dictionary<string, bool> dict = new Dictionary<string, bool>();
            dict.Add("action", It.IsAny<bool>());
            _userAccessService
                .Setup(x => x.GetUserRegionAccessAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<User>()))
                .ReturnsAsync(dict);

            //Act
            var result = await _userAccessController.GetUserRegionAccess(It.IsAny<int>(), It.IsAny<string>());
            var resultValue = (result as ObjectResult)?.Value;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotEmpty(resultValue as Dictionary<string, bool>);
            Assert.IsInstanceOf<Dictionary<string, bool>>(resultValue);
        }

        [Test]
        public async Task GetUserDistinctionAccesses_ReturnsAccessList()
        {
            //Arrange
            Dictionary<string, bool> dict = new Dictionary<string, bool>();
            dict.Add("action", It.IsAny<bool>());
            _userAccessService
                .Setup(x => x.GetUserDistinctionAccessAsync(It.IsAny<string>()))
                .ReturnsAsync(dict);

            //Act
            var result = await _userAccessController.GetUserDistinctionAccess(It.IsAny<string>());
            var resultValue = (result as ObjectResult)?.Value;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotEmpty(resultValue as Dictionary<string, bool>);
            Assert.IsInstanceOf<Dictionary<string, bool>>(resultValue);
        }

        [Test]
        public async Task GetUserAnnualReportAccesses_ReturnsAccessList()
        {
            //Arrange
            Dictionary<string, bool> dict = new Dictionary<string, bool>();
            dict.Add("action", It.IsAny<bool>());
            _userAccessService
                .Setup(x => x.GetUserAnnualReportAccessAsync(It.IsAny<string>(), It.IsAny<User>(), It.IsAny<ReportType?>(),It.IsAny<int?>()))
                .ReturnsAsync(dict);

            //Act
            var result = await _userAccessController.GetUserAnnualReportAccess(It.IsAny<string>(), It.IsAny<ReportType?>(), It.IsAny<int?>());
            var resultValue = (result as ObjectResult)?.Value;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotEmpty(resultValue as Dictionary<string, bool>);
            Assert.IsInstanceOf<Dictionary<string, bool>>(resultValue);
        }

        [Test]
        public async Task GetUserStatisticsAccesses_ReturnsAccessList()
        {
            //Arrange
            Dictionary<string, bool> dict = new Dictionary<string, bool>();
            dict.Add("action", It.IsAny<bool>());
            _userAccessService
                .Setup(x => x.GetUserStatisticsAccessAsync(It.IsAny<string>()))
                .ReturnsAsync(dict);

            //Act
            var result = await _userAccessController.GetUserStatisticsAccess(It.IsAny<string>());
            var resultValue = (result as ObjectResult)?.Value;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotEmpty(resultValue as Dictionary<string, bool>);
            Assert.IsInstanceOf<Dictionary<string, bool>>(resultValue);
        }

        [Test]
        public async Task GetUserProfileAccess_ReturnsAccessList()
        {
            //Arrange
            Dictionary<string, bool> dict = new Dictionary<string, bool>();
            dict.Add("action", It.IsAny<bool>());
            _userAccessService
                .Setup(x => x.GetUserProfileAccessAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<User>()))
                .ReturnsAsync(dict);

            //Act
            var result = await _userAccessController.GetUserProfileAccess(It.IsAny<string>(), It.IsAny<string>());
            var resultValue = (result as ObjectResult)?.Value;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotEmpty(resultValue as Dictionary<string, bool>);
            Assert.IsInstanceOf<Dictionary<string, bool>>(resultValue);
        }
    }
}
