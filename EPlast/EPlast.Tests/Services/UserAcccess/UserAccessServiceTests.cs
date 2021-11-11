using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.BLL.Interfaces.Region;
using EPlast.BLL.Services.Interfaces;
using EPlast.BLL.Services.UserAccess;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.Tests.Services.UserAccess
{
    [TestFixture]
    class UserAccessServiceTests
    {
        private Mock<IClubAccessService> _clubAccessService;
        private Mock<IEventUserAccessService> _eventAccessService;
        private Mock<ISecurityModel> _securityModel;
        private Mock<UserManager<User>> _userManager;
        private Mock<ICityAccessService> _cityAccessService;
        private Mock<IRegionAccessService> _regionAccessService;
        private Mock<IAnnualReportAccessService> _annualReportAccessService;

        private UserAccessService _userAccessService;

        [SetUp]
        public void SetUp()
        {
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _clubAccessService = new Mock<IClubAccessService>();
            _securityModel = new Mock<ISecurityModel>();
            _eventAccessService = new Mock<IEventUserAccessService>();
            _cityAccessService = new Mock<ICityAccessService>();
            _regionAccessService = new Mock<IRegionAccessService>();
            _annualReportAccessService = new Mock<IAnnualReportAccessService>();

            _userAccessService = new UserAccessService(_clubAccessService.Object, _eventAccessService.Object, _userManager.Object, _cityAccessService.Object, _regionAccessService.Object, _annualReportAccessService.Object, _securityModel.Object);
        }

        [Test]
        public async Task GetUserClubAccesses_ReturnsListOfClubAccesses()
        {
            //Arrange
            Dictionary<string, bool> dict = new Dictionary<string, bool>();
            dict.Add("action", It.IsAny<bool>());
            _securityModel.Setup(x => x.GetUserAccessAsync(It.IsAny<string>(), It.IsAny<IEnumerable<string>>())).ReturnsAsync(dict);

            //Act
            var result = await _userAccessService.GetUserClubAccessAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<User>());

            //Assert
            Assert.IsNotEmpty(result);
            Assert.IsInstanceOf<Dictionary<string, bool>>(result);
        }

        [Test] 
        public async Task GetUserEventAccesses_EventIdNotNullAndRolePlastMember_FunctionHasAccessAsyncCalled_And_ReturnsListOfEventAccesses()
        {
            //Arrange
            int? eventId = 1;

            Dictionary<string, bool> dict = new Dictionary<string, bool>();
            dict.Add("action", It.IsAny<bool>());
            
            _securityModel.Setup(x => x.GetUserAccessAsync(It.IsAny<string>(), It.IsAny<IEnumerable<string>>())).ReturnsAsync(dict);
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.PlastMember });
            _eventAccessService.Setup(x => x.HasAccessAsync(It.IsAny<User>(), (int)eventId))
                .ReturnsAsync(It.IsAny<bool>());

            //Act
            var result = await _userAccessService.GetUserEventAccessAsync(It.IsAny<string>(), It.IsAny<User>(), eventId);

            //Assert
            Assert.IsNotEmpty(result);
            Assert.IsInstanceOf<Dictionary<string, bool>>(result);
            _eventAccessService.Verify(v => v.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>()), Times.Once());
        }

        [Test]
        public async Task GetUserEventAccesses_EventIdNull_FunctionHasAccessAsyncNotCalled_And_ReturnsListOfEventAccesses()
        {
            //Arrange
            Dictionary<string, bool> dict = new Dictionary<string, bool>();
            dict.Add("action", It.IsAny<bool>());
            
            _securityModel.Setup(x => x.GetUserAccessAsync(It.IsAny<string>(), It.IsAny<IEnumerable<string>>())).ReturnsAsync(dict);
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.PlastMember });
            _eventAccessService.Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(It.IsAny<bool>());
            
            //Act
            var result = await _userAccessService.GetUserEventAccessAsync(It.IsAny<string>(), It.IsAny<User>());

            //Assert
            Assert.IsNotEmpty(result);
            Assert.IsInstanceOf<Dictionary<string, bool>>(result);
            _eventAccessService.Verify(v => v.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>()), Times.Never());
        }

        [Test]
        public async Task GetUserCityAccesses_ReturnsListOfCityAccesses()
        {
            //Arrange
            Dictionary<string, bool> dict = new Dictionary<string, bool>();
            dict.Add("action", It.IsAny<bool>());
            _securityModel.Setup(x => x.GetUserAccessAsync(It.IsAny<string>(), It.IsAny<IEnumerable<string>>())).ReturnsAsync(dict);

            //Act
            var result = await _userAccessService.GetUserCityAccessAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<User>());

            //Assert
            Assert.IsNotEmpty(result);
            Assert.IsInstanceOf<Dictionary<string, bool>>(result);
        }

        [Test]
        public async Task GetUserRegionAccesses_ReturnsListOfCityAccesses()
        {
            //Arrange
            Dictionary<string, bool> dict = new Dictionary<string, bool>();
            dict.Add("action", It.IsAny<bool>());
            _securityModel.Setup(x => x.GetUserAccessAsync(It.IsAny<string>(), It.IsAny<IEnumerable<string>>())).ReturnsAsync(dict);

            //Act
            var result = await _userAccessService.GetUserRegionAccessAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<User>());

            //Assert
            Assert.IsNotEmpty(result);
            Assert.IsInstanceOf<Dictionary<string, bool>>(result);
        }

        [Test]
        public async Task GetUserDistinctionAccesses_ReturnsListOfDistinctionAccesses()
        {
            //Arrange
            Dictionary<string, bool> dict = new Dictionary<string, bool>();
            dict.Add("action", It.IsAny<bool>());
            _securityModel.Setup(x => x.GetUserAccessAsync(It.IsAny<string>(), It.IsAny<IEnumerable<string>>())).ReturnsAsync(dict);

            //Act
            var result = await _userAccessService.GetUserDistinctionAccessAsync(It.IsAny<string>());

            //Assert
            Assert.IsNotEmpty(result);
            Assert.IsInstanceOf<Dictionary<string, bool>>(result);
        }

        [Test]
        public async Task GetUserAnnualReportAccesses_ReturnsListOfAnnualReportAccesses()
        {
            //Arrange
            Dictionary<string, bool> dict = new Dictionary<string, bool>();
            dict.Add("action", It.IsAny<bool>());
            _securityModel.Setup(x => x.GetUserAccessAsync(It.IsAny<string>(), It.IsAny<IEnumerable<string>>())).ReturnsAsync(dict);

            //Act
            var result = await _userAccessService.GetUserAnnualReportAccessAsync(It.IsAny<string>(), It.IsAny<int>());

            //Assert
            Assert.IsNotEmpty(result);
            Assert.IsInstanceOf<Dictionary<string, bool>>(result);
        }
    }
}
