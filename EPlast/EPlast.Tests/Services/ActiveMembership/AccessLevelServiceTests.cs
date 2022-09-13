using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BLL.DTO.ActiveMembership;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.ExtensionMethods;
using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.BLL.Services.ActiveMembership;
using EPlast.BLL.Services.Interfaces;
using EPlast.Resources;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services.ActiveMembership
{
    [TestFixture]
    public class AccessLevelServiceTests
    {
        private IAccessLevelService _accessLevelService;
        private Mock<IUserManagerService> _userManagerService;
        private Mock<IPlastDegreeService> _plastDegreeService;

        [SetUp]
        public void SetUp()
        {
            _userManagerService = new Mock<IUserManagerService>();
            _plastDegreeService = new Mock<IPlastDegreeService>();
            _accessLevelService = new AccessLevelService(_plastDegreeService.Object, _userManagerService.Object);
        }

        [Test]
        public async Task GetUserAccessLevelsAsync_RegisteredUser_ReturnsIEnumerableOfStringsWithRegisteredUserRolesForActiveMembership()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _userManagerService.Setup(ums => ums.GetRolesAsync(It.IsAny<UserDto>()))
                .ReturnsAsync(GetUserRolesAsRegisteredUser());

            // Act
            var result = await _accessLevelService.GetUserAccessLevelsAsync(UserId);
            var listResult = result.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
            Assert.AreEqual(GetUserRolesAsRegisteredUser().Count(), listResult.Count);
            Assert.AreEqual(AccessLevelTypeDto.RegisteredUser.GetDescription(), listResult[0]);
        }

        [Test]
        public async Task GetUserAccessLevelsAsync_UserIsFormer_ReturnsIEnumerableOfStringsWithFormerRolesForActiveMembership()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _userManagerService.Setup(ums => ums.GetRolesAsync(It.IsAny<UserDto>()))
                .ReturnsAsync(GetUserRolesAsFormer());

            // Act
            var result = await _accessLevelService.GetUserAccessLevelsAsync(UserId);
            var listResult = result.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
            Assert.AreEqual(GetUserRolesAsFormer().ToList().Count, listResult.Count);
            Assert.AreEqual(AccessLevelTypeDto.FormerPlastMember.GetDescription(), listResult[0]);
        }

        [Test]
        public async Task GetUserAccessLevelsAsync_UserIsPlastun_ReturnsIEnumerableOfStringsWithPlastunRolesForActiveMembership()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _userManagerService.Setup(ums => ums.GetRolesAsync(It.IsAny<UserDto>()))
                .ReturnsAsync(GetUserRolesAsPlastun());

            // Act
            var result = await _accessLevelService.GetUserAccessLevelsAsync(UserId);
            var listResult = result.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
            Assert.AreEqual(GetUserRolesAsPlastun().ToList().Count, listResult.Count);
            Assert.AreEqual(AccessLevelTypeDto.PlastMember.GetDescription(), listResult[0]);
        }

        [Test]
        public async Task GetUserAccessLevelsAsync_UserIsSupporter_ReturnsIEnumerableOfStringsWithSupporterRolesForActiveMembership()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _userManagerService.Setup(ums => ums.GetRolesAsync(It.IsAny<UserDto>()))
                .ReturnsAsync(GetUserRolesAsSupporter());

            // Act
            var result = await _accessLevelService.GetUserAccessLevelsAsync(UserId);
            var listResult = result.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
            Assert.AreEqual(GetUserRolesAsSupporter().ToList().Count, listResult.Count);
            Assert.AreEqual(AccessLevelTypeDto.Supporter.GetDescription(), listResult[0]);
        }

        [Test]
        public async Task GetUserAccessLevelsAsync_UserIsGoverningBodyHead_ReturnsIEnumerableOfStringsWithGoverningBodyHeadRolesForActiveMembership()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _userManagerService.Setup(ums => ums.GetRolesAsync(It.IsAny<UserDto>()))
                .ReturnsAsync(GetUserRolesAsLeadershipMember());

            // Act
            var result = await _accessLevelService.GetUserAccessLevelsAsync(UserId);
            var listResult = result.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
            Assert.AreEqual(GetUserRolesAsLeadershipMember().ToList().Count, listResult.Count);
            Assert.AreEqual(AccessLevelTypeDto.PlastMember.GetDescription(), listResult[0]);
            Assert.AreEqual(AccessLevelTypeDto.LeadershipMemberForOkrugaHead.GetDescription(), listResult[1]);
        }

        [Test]
        public async Task GetUserAccessLevelsAsync_UserHasAllRoles_ReturnsIEnumerableOfStringsWithAllRoles()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _userManagerService.Setup(ums => ums.GetRolesAsync(It.IsAny<UserDto>()))
                .ReturnsAsync(Roles.ListOfRoles);

            // Act
            var result = await _accessLevelService.GetUserAccessLevelsAsync(UserId);
            var listResult = result.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
            Assert.AreEqual(Roles.ListOfRoles.Count, listResult.Count);
            Assert.AreEqual(AccessLevelTypeDto.Supporter.GetDescription(), listResult[1]);
            Assert.AreEqual(AccessLevelTypeDto.PlastMember.GetDescription(), listResult[2]);
        }

        private string UserId => Guid.NewGuid().ToString();
        private DateTime UserDateOfEntry => DateTime.Today;

        private UserDto UserDTO => new UserDto
        {
            Id = UserId,
            RegistredOn = UserDateOfEntry,
            UserPlastDegrees = new UserPlastDegreeDto()
        };

        private UserPlastDegreeDto GetUserPlastDegreeDtos()
        {

            return new UserPlastDegreeDto
            {
                PlastDegree = new PlastDegreeDto { Name = "Пластприят" }
            };
            
        }

        private IEnumerable<string> GetUserRolesAsRegisteredUser()
        {
            return new List<string>
            {
                RolesForActiveMembershipTypeDto.RegisteredUser.GetDescription()
            };
        }

        private IEnumerable<string> GetUserRolesAsFormer()
        {
            return new List<string>
            {
                RolesForActiveMembershipTypeDto.FormerPlastMember.GetDescription()
            };
        }

        private IEnumerable<string> GetUserRolesAsPlastun()
        {
            return new List<string>
            {
                RolesForActiveMembershipTypeDto.PlastMember.GetDescription()
            };
        }

        private IEnumerable<string> GetUserRolesAsSupporter()
        {
            return new List<string>
            {
               RolesForActiveMembershipTypeDto.Supporter.GetDescription()
            };
        }

        private IEnumerable<string> GetUserRolesAsLeadershipMember()
        {
            return new List<string>
            {
                 RolesForActiveMembershipTypeDto.PlastMember.GetDescription(),
                 Roles.OkrugaHead

            };
        }

        private IEnumerable<string> GetUserRolesWithNoRoles()
        {
            return new List<string>();
        }
    }
}
