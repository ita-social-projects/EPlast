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
        public async Task GetUserAccessLevelsAsync_UserIsFormer_ReturnsIEnumerableOfStringsWithFormerRolesForActiveMembership()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _userManagerService.Setup(ums => ums.GetRolesAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(GetUserRolesAsFormer());

            // Act
            var result = await _accessLevelService.GetUserAccessLevelsAsync(UserId);
            var listResult = result.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
            Assert.AreEqual(GetUserRolesAsFormer().ToList().Count, listResult.Count);
            Assert.AreEqual(AccessLevelTypeDTO.FormerPlastMember.GetDescription(), listResult[0]);
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
        public async Task GetUserAccessLevelsAsync_UserIsGoverningBodyHead_ReturnsIEnumerableOfStringsWithGoverningBodyHeadRolesForActiveMembership()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _userManagerService.Setup(ums => ums.GetRolesAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(GetUserRolesAsGoverningBodyHead());

            // Act
            var result = await _accessLevelService.GetUserAccessLevelsAsync(UserId);
            var listResult = result.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
            Assert.AreEqual(GetUserRolesAsGoverningBodyHead().ToList().Count, listResult.Count);
            Assert.AreEqual(AccessLevelTypeDTO.PlastMember.GetDescription(), listResult[0]);
            Assert.AreEqual(AccessLevelTypeDTO.LeadershipMemberForGoverningBodyHead.GetDescription(), listResult[1]);
        }

        [Test]
        public async Task GetUserAccessLevelsAsync_UserIsGoverningBodySectorHead_ReturnsIEnumerableOfStringsWithGoverningBodySectorHeadDeputyRolesForActiveMembership()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _userManagerService.Setup(ums => ums.GetRolesAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(GetUserRolesAsGoverningBodySectorHead());

            // Act
            var result = await _accessLevelService.GetUserAccessLevelsAsync(UserId);
            var listResult = result.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
            Assert.AreEqual(GetUserRolesAsGoverningBodySectorHead().ToList().Count, listResult.Count);
            Assert.AreEqual(AccessLevelTypeDTO.PlastMember.GetDescription(), listResult[0]);
            Assert.AreEqual(AccessLevelTypeDTO.LeadershipMemberForGoverningBodySectorHead.GetDescription(), listResult[1]);
        }

        [Test]
        public async Task GetUserAccessLevelsAsync_UserIsGoverningBodySecretary_ReturnsIEnumerableOfStringsWithGoverningBodySecretarySecretaryRolesForActiveMembership()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _userManagerService.Setup(ums => ums.GetRolesAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(GetUserRolesAsGoverningBodySecretary());

            // Act
            var result = await _accessLevelService.GetUserAccessLevelsAsync(UserId);
            var listResult = result.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
            Assert.AreEqual(GetUserRolesAsGoverningBodySecretary().ToList().Count, listResult.Count);
            Assert.AreEqual(AccessLevelTypeDTO.PlastMember.GetDescription(), listResult[0]);
            Assert.AreEqual(AccessLevelTypeDTO.LeadershipMemberForGoverningBodySecretary.GetDescription(), listResult[1]);
        }

        [Test]
        public async Task GetUserAccessLevelsAsync_UserIsOkrugaHead_ReturnsIEnumerableOfStringsWithOkrugaHeadRolesForActiveMembership()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _userManagerService.Setup(ums => ums.GetRolesAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(GetUserRolesAsOkrugaHead());

            // Act
            var result = await _accessLevelService.GetUserAccessLevelsAsync(UserId);
            var listResult = result.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
            Assert.AreEqual(GetUserRolesAsOkrugaHead().ToList().Count, listResult.Count);
            Assert.AreEqual(AccessLevelTypeDTO.PlastMember.GetDescription(), listResult[0]);
            Assert.AreEqual(AccessLevelTypeDTO.LeadershipMemberForOkrugaHead.GetDescription(), listResult[1]);
        }

        [Test]
        public async Task GetUserAccessLevelsAsync_UserIsOkrugaHeadDeputy_ReturnsIEnumerableOfStringsWithOkrugaHeadDeputyRolesForActiveMembership()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _userManagerService.Setup(ums => ums.GetRolesAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(GetUserRolesAsOkrugaHeadDeputy());

            // Act
            var result = await _accessLevelService.GetUserAccessLevelsAsync(UserId);
            var listResult = result.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
            Assert.AreEqual(GetUserRolesAsOkrugaHead().ToList().Count, listResult.Count);
            Assert.AreEqual(AccessLevelTypeDTO.PlastMember.GetDescription(), listResult[0]);
            Assert.AreEqual(AccessLevelTypeDTO.LeadershipMemberForOkrugaHeadDeputy.GetDescription(), listResult[1]);
        }

        [Test]
        public async Task GetUserAccessLevelsAsync_UserIsOkrugaSecretary_ReturnsIEnumerableOfStringsWithOkrugaSecretaryRolesForActiveMembership()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _userManagerService.Setup(ums => ums.GetRolesAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(GetUserRolesAsOkrugaSecretary());

            // Act
            var result = await _accessLevelService.GetUserAccessLevelsAsync(UserId);
            var listResult = result.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
            Assert.AreEqual(GetUserRolesAsOkrugaSecretary().ToList().Count, listResult.Count);
            Assert.AreEqual(AccessLevelTypeDTO.PlastMember.GetDescription(), listResult[0]);
            Assert.AreEqual(AccessLevelTypeDTO.LeadershipMemberForOkrugaSecretary.GetDescription(), listResult[1]);
        }

        [Test]
        public async Task GetUserAccessLevelsAsync_UserIsCityHead_ReturnsIEnumerableOfStringsWithCityHeadRolesForActiveMembership()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _userManagerService.Setup(ums => ums.GetRolesAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(GetUserRolesAsCityHead());

            // Act
            var result = await _accessLevelService.GetUserAccessLevelsAsync(UserId);
            var listResult = result.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
            Assert.AreEqual(GetUserRolesAsCityHead().ToList().Count, listResult.Count);
            Assert.AreEqual(AccessLevelTypeDTO.PlastMember.GetDescription(), listResult[0]);
            Assert.AreEqual(AccessLevelTypeDTO.LeadershipMemberForCityHead.GetDescription(), listResult[1]);
        }

        [Test]
        public async Task GetUserAccessLevelsAsync_UserIsCityHeadDeputy_ReturnsIEnumerableOfStringsWithCityHeadDeputyRolesForActiveMembership()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _userManagerService.Setup(ums => ums.GetRolesAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(GetUserRolesAsCityHeadDeputy());

            // Act
            var result = await _accessLevelService.GetUserAccessLevelsAsync(UserId);
            var listResult = result.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
            Assert.AreEqual(GetUserRolesAsCityHeadDeputy().ToList().Count, listResult.Count);
            Assert.AreEqual(AccessLevelTypeDTO.PlastMember.GetDescription(), listResult[0]);
            Assert.AreEqual(AccessLevelTypeDTO.LeadershipMemberForCityHeadDeputy.GetDescription(), listResult[1]);
        }

        [Test]
        public async Task GetUserAccessLevelsAsync_UserIsCitySecretary_ReturnsIEnumerableOfStringsWithCitySecretaryRolesForActiveMembership()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _userManagerService.Setup(ums => ums.GetRolesAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(GetUserRolesAsCitySecretary());

            // Act
            var result = await _accessLevelService.GetUserAccessLevelsAsync(UserId);
            var listResult = result.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
            Assert.AreEqual(GetUserRolesAsCitySecretary().ToList().Count, listResult.Count);
            Assert.AreEqual(AccessLevelTypeDTO.PlastMember.GetDescription(), listResult[0]);
            Assert.AreEqual(AccessLevelTypeDTO.LeadershipMemberForCitySecretary.GetDescription(), listResult[1]);
        }

        [Test]
        public async Task GetUserAccessLevelsAsync_UserIsKurinHead_ReturnsIEnumerableOfStringsWithKurinHeadRolesForActiveMembership()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _userManagerService.Setup(ums => ums.GetRolesAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(GetUserRolesAsKurinHead());

            // Act
            var result = await _accessLevelService.GetUserAccessLevelsAsync(UserId);
            var listResult = result.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
            Assert.AreEqual(GetUserRolesAsKurinHead().ToList().Count, listResult.Count);
            Assert.AreEqual(AccessLevelTypeDTO.PlastMember.GetDescription(), listResult[0]);
            Assert.AreEqual(AccessLevelTypeDTO.LeadershipMemberForKurinHead.GetDescription(), listResult[1]);
        }

        [Test]
        public async Task GetUserAccessLevelsAsync_UserIsKurinHeadDeputy_ReturnsIEnumerableOfStringsWithKurinHeadDeputyRolesForActiveMembership()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _userManagerService.Setup(ums => ums.GetRolesAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(GetUserRolesAsKurinHeadDeputy());

            // Act
            var result = await _accessLevelService.GetUserAccessLevelsAsync(UserId);
            var listResult = result.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
            Assert.AreEqual(GetUserRolesAsKurinHeadDeputy().ToList().Count, listResult.Count);
            Assert.AreEqual(AccessLevelTypeDTO.PlastMember.GetDescription(), listResult[0]);
            Assert.AreEqual(AccessLevelTypeDTO.LeadershipMemberForKurinHeadDeputy.GetDescription(), listResult[1]);
        }

        [Test]
        public async Task GetUserAccessLevelsAsync_UserIsKurinSecretary_ReturnsIEnumerableOfStringsWithKurinSecretaryRolesForActiveMembership()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _userManagerService.Setup(ums => ums.GetRolesAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(GetUserRolesAsKurinSecretary());

            // Act
            var result = await _accessLevelService.GetUserAccessLevelsAsync(UserId);
            var listResult = result.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
            Assert.AreEqual(GetUserRolesAsKurinSecretary().ToList().Count, listResult.Count);
            Assert.AreEqual(AccessLevelTypeDTO.PlastMember.GetDescription(), listResult[0]);
            Assert.AreEqual(AccessLevelTypeDTO.LeadershipMemberForKurinSecretary.GetDescription(), listResult[1]);
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
                    PlastDegree = new PlastDegreeDTO { Name = "Пластприят" }
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

        private IEnumerable<string> GetUserRolesAsFormer()
        {
            return new List<string>
            {
                RolesForActiveMembershipTypeDTO.FormerPlastMember.GetDescription()
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

        private IEnumerable<string> GetUserRolesAsGoverningBodyHead()
        {
            return new List<string>
            {
                 RolesForActiveMembershipTypeDTO.PlastMember.GetDescription(),
                 Roles.GoverningBodyHead

            };
        }

        private IEnumerable<string> GetUserRolesAsGoverningBodySectorHead()
        {
            return new List<string>
            {
                 RolesForActiveMembershipTypeDTO.PlastMember.GetDescription(),
                 Roles.GoverningBodySectorHead

            };
        }

        private IEnumerable<string> GetUserRolesAsGoverningBodySecretary()
        {
            return new List<string>
            {
                 RolesForActiveMembershipTypeDTO.PlastMember.GetDescription(),
                 Roles.GoverningBodySecretary

            };
        }

        private IEnumerable<string> GetUserRolesAsOkrugaHead()
        {
            return new List<string>
            {
                 RolesForActiveMembershipTypeDTO.PlastMember.GetDescription(),
                 Roles.OkrugaHead

            };
        }

        private IEnumerable<string> GetUserRolesAsOkrugaHeadDeputy()
        {
            return new List<string>
            {
                 RolesForActiveMembershipTypeDTO.PlastMember.GetDescription(),
                 Roles.OkrugaHeadDeputy

            };
        }

        private IEnumerable<string> GetUserRolesAsOkrugaSecretary()
        {
            return new List<string>
            {
                 RolesForActiveMembershipTypeDTO.PlastMember.GetDescription(),
                 Roles.OkrugaSecretary

            };
        }

        private IEnumerable<string> GetUserRolesAsCityHead()
        {
            return new List<string>
            {
                 RolesForActiveMembershipTypeDTO.PlastMember.GetDescription(),
                 Roles.CityHead

            };
        }
        private IEnumerable<string> GetUserRolesAsCityHeadDeputy()
        {
            return new List<string>
            {
                 RolesForActiveMembershipTypeDTO.PlastMember.GetDescription(),
                 Roles.CityHeadDeputy

            };
        }
        private IEnumerable<string> GetUserRolesAsCitySecretary()
        {
            return new List<string>
            {
                 RolesForActiveMembershipTypeDTO.PlastMember.GetDescription(),
                 Roles.CitySecretary

            };
        }

        private IEnumerable<string> GetUserRolesAsKurinHead()
        {
            return new List<string>
            {
                 RolesForActiveMembershipTypeDTO.PlastMember.GetDescription(),
                 Roles.KurinHead

            };
        }
        private IEnumerable<string> GetUserRolesAsKurinHeadDeputy()
        {
            return new List<string>
            {
                 RolesForActiveMembershipTypeDTO.PlastMember.GetDescription(),
                 Roles.KurinHeadDeputy

            };
        }
        private IEnumerable<string> GetUserRolesAsKurinSecretary()
        {
            return new List<string>
            {
                 RolesForActiveMembershipTypeDTO.PlastMember.GetDescription(),
                 Roles.KurinSecretary

            };
        }

        private IEnumerable<string> GetUserRolesWithNoRoles()
        {
            return new List<string>();
        }
    }
}
