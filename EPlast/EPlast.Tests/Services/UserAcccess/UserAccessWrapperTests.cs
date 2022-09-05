using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.Blank;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.BLL.Interfaces.Region;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Services.Interfaces;
using EPlast.BLL.Services.UserAccess;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services.UserAcccess
{
    [TestFixture]
    public class UserAccessWrapperTests
    {
        private Mock<IClubAccessService> _clubAccessService;
        private Mock<IEventUserAccessService> _eventAccessService;
        private Mock<ISecurityModel> _securityModel;
        private Mock<ICityAccessService> _cityAccessService;
        private Mock<IRegionAccessService> _regionAccessService;
        private Mock<IUserProfileAccessService> _userProfileAccessService;
        private Mock<IAnnualReportAccessService> _annualReportAccessService;
        private Mock<IBlankAccessService> _blankAccessService;

        private UserAccessWrapper _userAcccessWrapper;

        [SetUp]
        public void SetUp()
        {
            _clubAccessService = new Mock<IClubAccessService>();
            _securityModel = new Mock<ISecurityModel>();
            _eventAccessService = new Mock<IEventUserAccessService>();
            _cityAccessService = new Mock<ICityAccessService>();
            _regionAccessService = new Mock<IRegionAccessService>();
            _annualReportAccessService = new Mock<IAnnualReportAccessService>();
            _userProfileAccessService = new Mock<IUserProfileAccessService>();
            _blankAccessService = new Mock<IBlankAccessService>();
            _userAcccessWrapper = new UserAccessWrapper(
                _clubAccessService.Object,
                _cityAccessService.Object,
                _regionAccessService.Object,
                _annualReportAccessService.Object,
                _userProfileAccessService.Object,
                _eventAccessService.Object,
                _blankAccessService.Object);
        }

        [Test]
        public void GetClubAccessService_Valid()
        {
            // Act 
            var result  = _userAcccessWrapper.ClubAccessService;

            // Assert
            Assert.IsInstanceOf<IClubAccessService>(result);
        }

        [Test]
        public void GetCityAccessService_Valid()
        {
            // Act 
            var result = _userAcccessWrapper.CityAccessService;

            // Assert
            Assert.IsInstanceOf<ICityAccessService>(result);
        }

        [Test]
        public void GetAnnualReportAccessService_Valid()
        {
            // Act 
            var result = _userAcccessWrapper.AnnualReportAccessService;

            // Assert
            Assert.IsInstanceOf<IAnnualReportAccessService>(result);
        }

        [Test]
        public void GetUserProfileAccessService_Valid()
        {
            // Act 
            var result = _userAcccessWrapper.UserProfileAccessService;

            // Assert
            Assert.IsInstanceOf<IUserProfileAccessService>(result);
        }

        [Test]
        public void GetEventAccessService_Valid()
        {
            // Act 
            var result = _userAcccessWrapper.EventAccessService;

            // Assert
            Assert.IsInstanceOf<IEventUserAccessService>(result);
        }

        [Test]
        public void GetRegionAccessService_Valid()
        {
            // Act 
            var result = _userAcccessWrapper.RegionAccessService;

            // Assert
            Assert.IsInstanceOf<IRegionAccessService>(result);
        }

        [Test]
        public void GetBlankAccessService_Valid()
        {
            // Act
            var result = _userAcccessWrapper.BlankAccessService;

            // Assert
            Assert.IsInstanceOf<IBlankAccessService>(result);
        }
    }
}
