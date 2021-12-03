using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.BLL.Interfaces.Region;
using EPlast.BLL.Services.Interfaces;
using EPlast.BLL.Services.UserAccess;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
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
        private Mock<ICityAccessService> _cityAccessService;
        private Mock<IRegionAccessService> _regionAccessService;
        private Mock<IAnnualReportAccessService> _annualReportAccessService;

        private UserAccessService _userAccessService;

        [SetUp]
        public void SetUp()
        {
            _clubAccessService = new Mock<IClubAccessService>();
            _securityModel = new Mock<ISecurityModel>();
            _eventAccessService = new Mock<IEventUserAccessService>();
            _cityAccessService = new Mock<ICityAccessService>();
            _regionAccessService = new Mock<IRegionAccessService>();
            _annualReportAccessService = new Mock<IAnnualReportAccessService>();

            _userAccessService = new UserAccessService(_clubAccessService.Object, _eventAccessService.Object, _cityAccessService.Object, _regionAccessService.Object, _annualReportAccessService.Object, _securityModel.Object);
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
        public async Task GetUserEventAccesses_EventIdNotNull_ReturnsListOfEventAccesses()
        {
            //Arrange
            int? eventId = 1;

            Dictionary<string, bool> dict = new Dictionary<string, bool>();
            dict.Add("action", It.IsAny<bool>());

            _securityModel.Setup(x => x.GetUserAccessAsync(It.IsAny<string>(), It.IsAny<IEnumerable<string>>())).ReturnsAsync(dict);
            _eventAccessService.Setup(x => x.RedefineAccessesAsync(dict, It.IsAny<User>(), eventId)).ReturnsAsync(dict);

            //Act
            var result = await _userAccessService.GetUserEventAccessAsync(It.IsAny<string>(), It.IsAny<User>(), eventId);

            //Assert
            Assert.IsNotEmpty(result);
            Assert.IsInstanceOf<Dictionary<string, bool>>(result);
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
            dict.Add("CanEditReport", It.IsAny<bool>());
            _securityModel.Setup(x => x.GetUserAccessAsync(It.IsAny<string>(), It.IsAny<IEnumerable<string>>())).ReturnsAsync(dict);

            //Act
            var result = await _userAccessService.GetUserAnnualReportAccessAsync(It.IsAny<string>(), It.IsAny<User>(), It.IsAny<ReportType?>(), It.IsAny<int?>());

            //Assert
            Assert.IsNotEmpty(result);
            Assert.IsInstanceOf<Dictionary<string, bool>>(result);
        }

        [Test]
        public async Task GetUserStatisticsAccesses_ReturnsListOfStatisticsAccesses()
        {
            //Arrange
            Dictionary<string, bool> dict = new Dictionary<string, bool>();
            dict.Add("action", It.IsAny<bool>());
            _securityModel.Setup(x => x.GetUserAccessAsync(It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(dict);

            //Act
            var result = await _userAccessService.GetUserStatisticsAccessAsync(It.IsAny<string>());

            //Assert
            Assert.IsNotEmpty(result);
            Assert.IsInstanceOf<Dictionary<string, bool>>(result);
        }
    }
}
