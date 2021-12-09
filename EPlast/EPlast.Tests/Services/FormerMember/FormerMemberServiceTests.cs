using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.FormerMember;
using EPlast.BLL.Services.FormerMember;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.Tests.Services.FormerMember
{
    internal class FormerMemberServiceTests
    {
        private Mock<ICityParticipantsService> _cityParticipanteService;
        private Mock<IClubParticipantsService> _clubParticipantsService;
        private Mock<IFormerMemberAdminService> _formerMemberAdminService;
        private Mock<IUserDatesService> _userDatesService;
        private FormerMemberService _formerMemberService;
        private protected Mock<UserManager<User>> _userManager;
        private readonly string userFakeId = "Id";

        [SetUp]
        public void SetUp()
        {
            _cityParticipanteService = new Mock<ICityParticipantsService>();
            _clubParticipantsService = new Mock<IClubParticipantsService>();
            _formerMemberAdminService = new Mock<IFormerMemberAdminService>();
            _userDatesService = new Mock<IUserDatesService>();
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            _formerMemberService = new FormerMemberService(
                _cityParticipanteService.Object,
                _clubParticipantsService.Object,
                _formerMemberAdminService.Object,
                _userDatesService.Object,
                _userManager.Object);
        }

        [Test]
        public async Task RemoveFromPlast_ValidTest()
        {
            //Arrange
            _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<User>());
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string> { "Role" });
            _userManager.Setup(x => x.RemoveFromRolesAsync(It.IsAny<User>(), It.IsAny<List<string>>()));
            _clubParticipantsService.Setup(x => x.RemoveMemberAsync(It.IsAny<string>()));
            _cityParticipanteService.Setup(x => x.RemoveMemberAsync(It.IsAny<string>()));
            _formerMemberAdminService.Setup(x => x.RemoveFromAdminRolesAsync(It.IsAny<string>()));
            _userDatesService.Setup(x => x.EndUserMembership(It.IsAny<string>()));
            _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()));

            //Act
            await _formerMemberService.MakeUserFormerMeberAsync(userFakeId);

            //Assert
            _userManager.Verify();
            _clubParticipantsService.Verify();
            _cityParticipanteService.Verify();
            _formerMemberAdminService.Verify();
            _userDatesService.Verify();

        }
    }
}
