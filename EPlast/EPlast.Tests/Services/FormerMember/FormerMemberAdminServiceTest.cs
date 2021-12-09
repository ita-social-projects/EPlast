using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.GoverningBodies;
using EPlast.BLL.Interfaces.GoverningBodies.Sector;
using EPlast.BLL.Interfaces.Region;
using EPlast.BLL.Services.FormerMember;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.Tests.Services.FormerMember
{
    internal class FormerMemberAdminServiceTest
    {
        private Mock<ICityParticipantsService> _cityParticipanteService;
        private Mock<IClubParticipantsService> _clubParticipantsService;
        private Mock<IGoverningBodyAdministrationService> _governingBodyAdministrationService;
        private Mock<IRegionAdministrationService> _regionAdministrationService;
        private Mock<ISectorAdministrationService> _sectorAdministrationService;
        private protected Mock<UserManager<User>> _userManager;
        private FormerMemberAdminService _formerMemberAdminService;
        private readonly string userFakeId = "Id";

        [SetUp]
        public void SetUp()
        {
            _cityParticipanteService = new Mock<ICityParticipantsService>();
            _clubParticipantsService = new Mock<IClubParticipantsService>();
            _governingBodyAdministrationService = new Mock<IGoverningBodyAdministrationService>();
            _regionAdministrationService = new Mock<IRegionAdministrationService>();
            _sectorAdministrationService = new Mock<ISectorAdministrationService>();
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            _formerMemberAdminService = new FormerMemberAdminService(
                _clubParticipantsService.Object,
                _cityParticipanteService.Object,
                _governingBodyAdministrationService.Object,
                _regionAdministrationService.Object,
                _sectorAdministrationService.Object,
                _userManager.Object);
        }

        [Test]
        public async Task RemoveFromAdminRole_ValidTest()
        {
            //Arrange
            _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<User>());
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string> { "Role" });
            _userManager.Setup(x => x.RemoveFromRolesAsync(It.IsAny<User>(), It.IsAny<List<string>>()));
            _clubParticipantsService
                .Setup(x => x.RemoveAdminRolesByUserIdAsync(It.IsAny<string>()));
            _cityParticipanteService
                .Setup(x => x.RemoveAdminRolesByUserIdAsync(It.IsAny<string>()));
            _regionAdministrationService
                .Setup(x => x.RemoveAdminRolesByUserIdAsync(It.IsAny<string>()));
            _governingBodyAdministrationService
              .Setup(x => x.RemoveAdminRolesByUserIdAsync(It.IsAny<string>()));
            _sectorAdministrationService
                .Setup(x => x.RemoveAdminRolesByUserIdAsync(It.IsAny<string>()));
            //Act
            await _formerMemberAdminService.RemoveFromAdminRolesAsync(userFakeId);

            //Assert
            _userManager.Verify();
            _clubParticipantsService.Verify();
            _cityParticipanteService.Verify();
            _regionAdministrationService.Verify();
            _governingBodyAdministrationService.Verify();
            _sectorAdministrationService.Verify();
        }
    }
}
