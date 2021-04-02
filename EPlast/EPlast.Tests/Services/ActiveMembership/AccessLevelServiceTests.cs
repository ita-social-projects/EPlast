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
        private Mock<IPlastDegreeService> _plastDegreeService;
        private IUniqueIdService _uniqueId;

        [SetUp]
        public void SetUp()
        {
            _userManagerService = new Mock<IUserManagerService>();
            _plastDegreeService = new Mock<IPlastDegreeService>();
            _accessLevelService = new AccessLevelService(_plastDegreeService.Object, _userManagerService.Object);
            _uniqueId = new UniqueIdService();
        }

        [Test]
        public async Task GetUserAccessLevelsAsync_RegisteredUser_ReturnsIEnumerableOfStringsWithRegisteredUserRolesForActiveMembership()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _userManagerService.Setup(ums => ums.GetRolesAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(GetUserRolesAsRegisteredUser());

            // Act
            var result = await _accessLevelService.GetUserAccessLevelsAsync(UserId);
            var listResult = result.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
            Assert.AreEqual(GetUserRolesAsRegisteredUser().Count(), listResult.Count);
            Assert.AreEqual(AccessLevelTypeDTO.RegisteredUser.GetDescription(), listResult[0]);
        }

        [Test]
        public async Task GetUserAccessLevelsAsync_SupporterDegree_ReturnsIEnumerableOfStringsWithSupporterRolesForActiveMembership()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _userManagerService.Setup(ums => ums.GetRolesAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(GetUserRolesWithNoRoles());
            _plastDegreeService.Setup(pds => pds.GetUserPlastDegreesAsync(It.IsAny<string>()))
                .ReturnsAsync(getUserPlastDegreeDtos());

            // Act
            var result = await _accessLevelService.GetUserAccessLevelsAsync(UserId);
            var listResult = result.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
            Assert.AreEqual(GetUserRolesAsRegisteredUser().Count(), listResult.Count);
            Assert.AreEqual(AccessLevelTypeDTO.Supporter.GetDescription(), listResult[0]);
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
            var result = await _accessLevelService.GetUserAccessLevelsAsync(UserId);
            var listResult = result.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
            Assert.AreEqual(GetUserRolesAsPlastun().ToList().Count, listResult.Count);
            Assert.AreEqual(AccessLevelTypeDTO.PlastMember.GetDescription(), listResult[0]);
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
            var result = await _accessLevelService.GetUserAccessLevelsAsync(UserId);
            var listResult = result.ToList();

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
            var result = await _accessLevelService.GetUserAccessLevelsAsync(UserId);
            var listResult = result.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
            Assert.AreEqual(GetUserRolesAsLeadershipMember().ToList().Count, listResult.Count);
            Assert.AreEqual(AccessLevelTypeDTO.PlastMember.GetDescription(), listResult[0]);
            Assert.AreEqual(AccessLevelTypeDTO.LeadershipMember.GetDescription(), listResult[1]);
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

        private IEnumerable<UserPlastDegreeDTO> getUserPlastDegreeDtos()
        {
            return new List<UserPlastDegreeDTO>
            {
                new UserPlastDegreeDTO
                {
                    PlastDegree = new PlastDegreeDTO { Name = "Пласт прият" }
                }
            };
        }

        private IEnumerable<string> GetUserRolesAsRegisteredUser()
        {
            return new List<string>
            {
                RolesForActiveMembershipTypeDTO.RegisteredUser.GetDescription()
            };
        }

        private IEnumerable<string> GetUserRolesAsPlastun()
        {
            return new List<string>
            {
                RolesForActiveMembershipTypeDTO.PlastMember.GetDescription()
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
                 RolesForActiveMembershipTypeDTO.PlastMember.GetDescription(),
                 Roles.OkrugaHead

            };
        }

        private IEnumerable<string> GetUserRolesWithNoRoles()
        {
            return new List<string>();
        }
    }
}
