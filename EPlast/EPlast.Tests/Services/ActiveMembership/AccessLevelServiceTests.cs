using EPlast.BLL.DTO.ActiveMembership;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.ExtensionMethods;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.BLL.Services;
using EPlast.BLL.Services.ActiveMembership;
using EPlast.BLL.Services.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.Resources;

namespace EPlast.Tests.Services.ActiveMembership
{
    [TestFixture]
    public class AccessLevelServiceTests
    {
        private IAccessLevelService _accessLevelService;
        private Mock<IUserManagerService> _userManagerService;
        private IUniqueIdService _uniqueId;

        [SetUp]
        public void SetUp()
        {
            _userManagerService = new Mock<IUserManagerService>();
            _accessLevelService = new AccessLevelService( _userManagerService.Object);
            _uniqueId = new UniqueIdService();

        }
        [Test]
        public async Task GetUserAccessLevelsAsync_UserIsPlastun_ReturnsIEnumerableOfStringsWithPlastunRolesForActiveMembership()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _userManagerService.Setup(ums => ums.GetRolesAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(GetUserRolesAsPlastun());

            // Act
            IEnumerable<string> result = await _accessLevelService.GetUserAccessLevelsAsync(UserId);
            List<string> listResult = result.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
            Assert.AreEqual(GetUserRolesAsPlastun().ToList().Count, listResult.Count);
            Assert.AreEqual(AccessLevelTypeDTO.Member.GetDescription(), listResult[0]);
        }
        [Test]
        public async Task GetUserAccessLevelsAsync_UserIsSupporter_ReturnsIEnumerableOfStringsWithSupporterRolesForActiveMembership()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _userManagerService.Setup(ums => ums.GetRolesAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(GetUserRolesAsSupporter());

            // Act
            IEnumerable<string> result = await _accessLevelService.GetUserAccessLevelsAsync(UserId);
            List<string> listResult = result.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
            Assert.AreEqual(GetUserRolesAsSupporter().ToList().Count, listResult.Count);
            Assert.AreEqual(AccessLevelTypeDTO.Supporter.GetDescription(), listResult[0]);
        }

        [Test]
        public async Task GetUserAccessLevelsAsync_UserIsLeadershipMember_ReturnsIEnumerableOfStringsWithLeadershipMemberRolesForActiveMembership()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _userManagerService.Setup(ums => ums.GetRolesAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(GetUserRolesAsLeadershipMember());

            // Act
            IEnumerable<string> result = await _accessLevelService.GetUserAccessLevelsAsync(UserId);
            List<string> listResult = result.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
        }

        [Test]
        public async Task GetUserAccessLevelsAsync_UserIsFormerMember_ReturnsIEnumerableOfStringsWithFormerMemberMemberRolesForActiveMembership()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _userManagerService.Setup(ums => ums.GetRolesAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(GetUserRolesAsFormerMember());

            // Act
            IEnumerable<string> result = await _accessLevelService.GetUserAccessLevelsAsync(UserId);
            List<string> listResult = result.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
        }

        private string UserId => _uniqueId.GetUniqueId().ToString();
        private DateTime UserDateOfEntry => DateTime.Today;
        private UserDTO UserDTO => new UserDTO
        {
            Id = UserId,
            RegistredOn = UserDateOfEntry,
            UserPlastDegrees = new List<UserPlastDegreeDTO>
            {
                new UserPlastDegreeDTO()
            }
        };
        private IEnumerable<string> GetUserRolesAsPlastun()
        {
            return new List<string>
            {
                RolesForActiveMembershipTypeDTO.Plastun.GetDescription()
            };
        }
        private IEnumerable<string> GetUserRolesAsSupporter()
        {
            return new List<string>
            {
               RolesForActiveMembershipTypeDTO.Supporter.GetDescription()
            };
        }
        private IEnumerable<string> GetUserRolesAsLeadershipMember()
        {
            return new List<string>
            {
                 RolesForActiveMembershipTypeDTO.Plastun.GetDescription(),
                 Roles.admin
            };
        }
        private IEnumerable<string> GetUserRolesAsFormerMember()
        {
            return new List<string>();
        }
    }
}
