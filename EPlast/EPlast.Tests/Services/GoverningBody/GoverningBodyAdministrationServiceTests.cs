using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.GoverningBody;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Services.GoverningBodies;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.GoverningBody;
using EPlast.DataAccess.Entities.GoverningBody.Sector;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services.GoverningBody
{
    internal class GoverningBodyAdministrationServiceTests
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IAdminTypeService> _adminTypeService;
        private Mock<UserManager<User>> _userManager;
        private Mock<IUserStore<User>> _user;
        private Mock<IMapper> _mapperMock;
        private GoverningBodyAdministrationService _governingBodyAdministrationService;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _adminTypeService = new Mock<IAdminTypeService>();
            _user = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(_user.Object, It.IsAny<IOptions<IdentityOptions>>(),
                It.IsAny<IPasswordHasher<User>>(), It.IsAny<IEnumerable<IUserValidator<User>>>(), It.IsAny<IEnumerable<IPasswordValidator<User>>>(),
                It.IsAny<ILookupNormalizer>(), It.IsAny<IdentityErrorDescriber>(), It.IsAny<IServiceProvider>(), It.IsAny<ILogger<UserManager<User>>>());
            _mapperMock = new Mock<IMapper>();
            _governingBodyAdministrationService = new GoverningBodyAdministrationService(
                _repoWrapper.Object, _userManager.Object,
                _adminTypeService.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetGoverningBodyAdministratorsByPageAsync_ReturnsTuple()
        {
            //Arrange
            _repoWrapper.Setup(r => r.GoverningBodyAdministration.GetRangeAsync(
                    It.IsAny<Expression<Func<GoverningBodyAdministration, bool>>>(),
                    It.IsAny<Expression<Func<GoverningBodyAdministration, GoverningBodyAdministration>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAdministration>, IQueryable<GoverningBodyAdministration>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAdministration>,
                        IIncludableQueryable<GoverningBodyAdministration, object>>>(), It.IsAny<int>(),
                    It.IsAny<int>()))
                .ReturnsAsync(
                    new Tuple<IEnumerable<GoverningBodyAdministration>, int>(new List<GoverningBodyAdministration>(),
                        It.IsAny<int>()));
            _mapperMock.Setup(m =>
                m.Map<GoverningBodyAdministration, GoverningBodyAdministrationDto>(
                    It.IsAny<GoverningBodyAdministration>()));

            //Act
            var result =
                await _governingBodyAdministrationService.GetGoverningBodyAdministratorsByPageAsync(It.IsAny<int>(),
                    It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<Tuple<IEnumerable<GoverningBodyAdministrationDto>, int>>(result);
        }

        [Test]
        public async Task GetGoverningBodyAdministratorsAsync_ReturnsIEnumerableOfGoverningBodyAdministrationDTO()
        {
            //Arrange
            _adminTypeService.Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<AdminTypeDto>());
            _repoWrapper
                .Setup(x => x.GoverningBodyAdministration.GetAllAsync(It.IsAny<Expression<Func<GoverningBodyAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAdministration>, IIncludableQueryable<GoverningBodyAdministration, object>>>()))
                .ReturnsAsync(new List<GoverningBodyAdministration>() { new GoverningBodyAdministration() { Id = 1 } });
            _mapperMock.Setup(m =>
                m.Map<IEnumerable<GoverningBodyAdministration>, IEnumerable<GoverningBodyAdministrationDto>>(
                    It.IsAny<IEnumerable<GoverningBodyAdministration>>())).Returns(new List<GoverningBodyAdministrationDto>() { new GoverningBodyAdministrationDto() { ID = 1 } });

            //Act
            var result = await _governingBodyAdministrationService.GetGoverningBodyAdministratorsAsync();

            //Assert
            Assert.IsInstanceOf<IEnumerable<GoverningBodyAdministrationDto>>(result);
        }

        [Test]
        public async Task AddGoverningBodyAdministratorAsync_EndDateToday_ReturnsAdministrator()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.GoverningBodyAdministration.CreateAsync(GoverningBodyAdmin));
            _repoWrapper
               .Setup(r => r.GoverningBody.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                   It.IsAny<Func<IQueryable<Organization>,
                       IIncludableQueryable<Organization, object>>>()))
               .ReturnsAsync(GoverningBody);
            _userManager
                .Setup(x => x.IsInRoleAsync(It.IsAny<User>(), It.Is<string>(v => v == Roles.PlastMember)))
                .ReturnsAsync(true);
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDto());

            //Act
            var result = await _governingBodyAdministrationService.AddGoverningBodyAdministratorAsync(GoverningBodyAdministrationDtoEndDateToday);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<GoverningBodyAdministrationDto>(result);
        }

        [Test]
        public async Task AddGoverningBodyAdministratorAsync_EndDateNull_ReturnsAdministrator()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.GoverningBodyAdministration.CreateAsync(GoverningBodyAdmin));
            _repoWrapper
                .Setup(r => r.GoverningBody.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                    It.IsAny<Func<IQueryable<Organization>,
                        IIncludableQueryable<Organization, object>>>()))
                .ReturnsAsync(GoverningBody);
            _userManager
                .Setup(x => x.IsInRoleAsync(It.IsAny<User>(), It.Is<string>(v => v == Roles.PlastMember)))
                .ReturnsAsync(true);
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDto());

            //Act
            var result = await _governingBodyAdministrationService.AddGoverningBodyAdministratorAsync(GoverningBodyAdministrationDtoEndDateNull);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<GoverningBodyAdministrationDto>(result);
        }

        [Test]
        public void AddGoverningBodyAdministratorAsync_UserHasRestrictedRoles_ThrowsArgumentException()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.GoverningBodyAdministration.CreateAsync(GoverningBodyAdmin));
            _repoWrapper
               .Setup(r => r.GoverningBody.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                   It.IsAny<Func<IQueryable<Organization>,
                       IIncludableQueryable<Organization, object>>>()))
               .ReturnsAsync(GoverningBody);
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { Roles.GoverningBodyHead });
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDto());

            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _governingBodyAdministrationService.AddGoverningBodyAdministratorAsync(GoverningBodyAdministrationDtoEndDateNull));
        }

        [Test]
        public async Task AddGoverningBodyMainAdminAsync_EndDateToday_ReturnsAdministrator()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.GoverningBodyAdministration.CreateAsync(GoverningBodyAdmin));
            _userManager.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User());
            _userManager
                .Setup(x => x.IsInRoleAsync(It.IsAny<User>(), It.Is<string>(v => v == Roles.PlastMember)))
                .ReturnsAsync(true);
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDto());

            //Act
            var result = await _governingBodyAdministrationService.AddGoverningBodyMainAdminAsync(GoverningBodyAdministrationDtoEndDateToday);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<GoverningBodyAdministrationDto>(result);
        }

        [Test]
        public async Task AddGoverningBodyMainAdminAsync_EndDateNull_ReturnsAdministrator()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.GoverningBodyAdministration.CreateAsync(GoverningBodyAdmin));
            _userManager.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User());
            _userManager
                .Setup(x => x.IsInRoleAsync(It.IsAny<User>(), It.Is<string>(v => v == Roles.PlastMember)))
                .ReturnsAsync(true);
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDto());


            //Act
            var result = await _governingBodyAdministrationService.AddGoverningBodyMainAdminAsync(GoverningBodyAdministrationDtoEndDateNull);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<GoverningBodyAdministrationDto>(result);
        }

        [Test]
        public void AddGoverningBodyMainAdminAsync_UserHasRestrictedRoles_ThrowsArgumentException()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.GoverningBodyAdministration.CreateAsync(GoverningBodyAdmin));
            _userManager.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User());
            _userManager
                .Setup(x => x.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDto());
            //Act
            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _governingBodyAdministrationService.AddGoverningBodyMainAdminAsync(GoverningBodyAdministrationDtoEndDateNull));
        }

        [Test]
        public void AddGoverningBodyMainAdminAsync_UserDontHaveNeededRoles_ThrowsArgumentException()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.GoverningBodyAdministration.CreateAsync(GoverningBodyAdmin));
            _userManager.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User());
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { Roles.GoverningBodyAdmin });
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDto());

            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _governingBodyAdministrationService.AddGoverningBodyMainAdminAsync(GoverningBodyAdministrationDtoEndDateNull));
        }

        [Test]
        public void AddGoverningBodyMainAdminAsync_RoleNameExists_ThrowsArgumentException()
        {
            //Arrange

            _repoWrapper.Setup(w => w.GoverningBodyAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<GoverningBodyAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAdministration>,
                        IIncludableQueryable<GoverningBodyAdministration, object>>>()))
                .ReturnsAsync(new GoverningBodyAdministration());
            _repoWrapper
                .Setup(s => s.GoverningBodyAdministration.CreateAsync(GoverningBodyAdmin));
            _userManager.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User());
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { Roles.GoverningBodyAdmin });
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDto());

            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _governingBodyAdministrationService.AddGoverningBodyMainAdminAsync(GoverningBodyAdministrationDtoEndDateNull));
        }

        [Test]
        public async Task EditGoverningBodyAdministratorAsync_SameTypeId_ReturnsEditedAdministrator()
        {
            //Arrange
            GoverningBodyAdministration nullAdministration = null;

            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDto());
            _repoWrapper
                .SetupSequence(r => r.GoverningBodyAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<GoverningBodyAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAdministration>,
                        IIncludableQueryable<GoverningBodyAdministration, object>>>()))
                .ReturnsAsync(nullAdministration)
                .ReturnsAsync(new GoverningBodyAdministration());
            _repoWrapper
                .Setup(r => r.GoverningBodyAdministration.Update(It.IsAny<GoverningBodyAdministration>()));
            _repoWrapper
                .Setup(r => r.SaveAsync());

            //Act
            var result = await _governingBodyAdministrationService.EditGoverningBodyAdministratorAsync(GoverningBodyAdministrationDtoEndDateToday);

            //Assert
            _repoWrapper.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<GoverningBodyAdministrationDto>(result);
        }

        [Test]
        public async Task EditGoverningBodyAdministratorAsync_WithDifferentAdminTypeId_ReturnsEditedAdministrator()
        {
            //Arrange
            GoverningBodyAdministration nullAdministration = null;
            _repoWrapper
                .SetupSequence(r => r.GoverningBodyAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<GoverningBodyAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAdministration>,
                        IIncludableQueryable<GoverningBodyAdministration, object>>>()))
                .ReturnsAsync(nullAdministration)
                .ReturnsAsync(new GoverningBodyAdministration())
                .ReturnsAsync(new GoverningBodyAdministration())
                .ReturnsAsync(new GoverningBodyAdministration())
                .ReturnsAsync(new GoverningBodyAdministration());
            _repoWrapper
               .Setup(r => r.GoverningBody.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                   It.IsAny<Func<IQueryable<Organization>,
                       IIncludableQueryable<Organization, object>>>()))
               .ReturnsAsync(GoverningBody);
            _repoWrapper
                .Setup(r => r.GoverningBodySectorAdministration.GetAllAsync(It.IsAny<Expression<Func<SectorAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<SectorAdministration>,
                        IIncludableQueryable<SectorAdministration, object>>>()))
                .ReturnsAsync(new List<SectorAdministration>() { new SectorAdministration() { Id = FakeId } });
            _userManager
                .Setup(x => x.IsInRoleAsync(It.IsAny<User>(), It.Is<string>(v => v == Roles.PlastMember)))
                .ReturnsAsync(true);
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDto
                {
                    AdminTypeName = Roles.GoverningBodyHead,
                    ID = FakeId
                });
            _adminTypeService
                .Setup(a => a.GetAdminTypeByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new AdminTypeDto
                {
                    AdminTypeName = Roles.GoverningBodyHead,
                    ID = FakeId
                });

            //Act
            var result = await _governingBodyAdministrationService.EditGoverningBodyAdministratorAsync(GoverningBodyAdministrationDtoEndDateToday);

            //Assert
            _repoWrapper.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<GoverningBodyAdministrationDto>(result);
        }

        [Test]
        public void EditGoverningBodyAdministratorAsync_RoleNameExists_ThrowsArgumentException()
        {
            //Arrange
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDto());
            _repoWrapper
                .Setup(r => r.GoverningBodyAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<GoverningBodyAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAdministration>,
                        IIncludableQueryable<GoverningBodyAdministration, object>>>()))
                .ReturnsAsync(new GoverningBodyAdministration());
            _repoWrapper
                .Setup(r => r.GoverningBodyAdministration.Update(It.IsAny<GoverningBodyAdministration>()));
            _repoWrapper
                .Setup(r => r.SaveAsync());

            //Act
            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _governingBodyAdministrationService.EditGoverningBodyAdministratorAsync(GoverningBodyAdministrationDtoEndDateNull));
        }

        [Test]
        public void RemoveAdministratorAsync_withoutRolesGovHeadAndGovAdmin_Test()
        {
            //Arrange
            _repoWrapper
                .Setup(r => r.GoverningBodyAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<GoverningBodyAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAdministration>,
                        IIncludableQueryable<GoverningBodyAdministration, object>>>()))
                .ReturnsAsync(GoverningBodyAdmin);
            _adminTypeService
                .Setup(a => a.GetAdminTypeByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new AdminTypeDto { ID = 0, AdminTypeName = "Крайовий Адмін" });
            _userManager
                .Setup(u => u.FindByIdAsync(It.IsAny<string>()));
            _userManager
                .Setup(u => u.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
            _repoWrapper
                .Setup(r => r.GoverningBodyAdministration.Update(It.IsAny<GoverningBodyAdministration>()));
            _repoWrapper
                .Setup(r => r.SaveAsync());

            //Act
            var result = _governingBodyAdministrationService.RemoveAdministratorAsync(It.IsAny<int>());
            //Assert
            _repoWrapper.Verify();
            _userManager.Verify(x => x.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);

            Assert.NotNull(result);
        }

        [Test]
        public void RemoveAdministratorAsync_withRolesGovHeadAndGovAdmin_Test()
        {
            //Arrange
            _repoWrapper
                .Setup(r => r.GoverningBodyAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<GoverningBodyAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAdministration>,
                        IIncludableQueryable<GoverningBodyAdministration, object>>>()))
                .ReturnsAsync(GoverningBodyAdmin);
            _adminTypeService
                .Setup(a => a.GetAdminTypeByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new AdminTypeDto { ID = 0, AdminTypeName = Roles.GoverningBodyHead });
            _userManager
                .Setup(u => u.FindByIdAsync(It.IsAny<string>()));
            _userManager
                .Setup(u => u.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { Roles.GoverningBodyHead, Roles.GoverningBodyAdmin });
            _repoWrapper
                .Setup(r => r.GoverningBodyAdministration.Update(It.IsAny<GoverningBodyAdministration>()));
            _repoWrapper
                .Setup(r => r.SaveAsync());

            //Act
            var result = _governingBodyAdministrationService.RemoveAdministratorAsync(It.IsAny<int>());
            //Assert
            _repoWrapper.Verify();
            _userManager.Verify(x => x.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);

            Assert.NotNull(result);
        }

        [Test]
        public async Task RemoveAdminRolesByUserIdAsync_ValidTest()
        {
            //Arrange
            _repoWrapper
               .Setup(x => x.GoverningBodyAdministration.GetAllAsync(It.IsAny<Expression<Func<GoverningBodyAdministration, bool>>>(),
                   It.IsAny<Func<IQueryable<GoverningBodyAdministration>, IIncludableQueryable<GoverningBodyAdministration, object>>>()))
               .ReturnsAsync(new List<GoverningBodyAdministration>() { new GoverningBodyAdministration() { Id = 1 } });
            _repoWrapper
                .Setup(r => r.GoverningBodyAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<GoverningBodyAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAdministration>,
                        IIncludableQueryable<GoverningBodyAdministration, object>>>()))
                .ReturnsAsync(GoverningBodyAdmin);
            _repoWrapper
                .Setup(r => r.GoverningBodySectorAdministration.GetAllAsync(It.IsAny<Expression<Func<SectorAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<SectorAdministration>,
                        IIncludableQueryable<SectorAdministration, object>>>()))
                .ReturnsAsync(new List<SectorAdministration>() { new SectorAdministration() { Id = FakeId } });
            _adminTypeService
                .Setup(a => a.GetAdminTypeByIdAsync(It.IsAny<int>()))
                .Returns(() => Task<AdminTypeDto>.Factory.StartNew(() => AdminTypeDto));
            _userManager
                .Setup(u => u.FindByIdAsync(It.IsAny<string>()));
            _userManager
                .Setup(u => u.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
            _userManager
                .Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { });
            _repoWrapper
                .Setup(r => r.GoverningBodyAdministration.Update(It.IsAny<GoverningBodyAdministration>()));
            _repoWrapper
                .Setup(r => r.SaveAsync());

            //Act
            await _governingBodyAdministrationService.RemoveGbAdminRoleAsync(It.IsAny<string>());

            //Assert
            _repoWrapper.Verify();
            _adminTypeService.Verify();
        }

        [Test]
        public void RemoveMainAdministratorAsync_withAdminRole_ValidTest()
        {
            //Arrange
            _repoWrapper
                .Setup(
                    r => r.AdminType.GetAllAsync(
                        It.IsAny<Expression<Func<AdminType, bool>>>(),
                        It.IsAny<Func<IQueryable<AdminType>,
                        IIncludableQueryable<AdminType, object>>>()))
                .ReturnsAsync(AdminTypes);
            _repoWrapper
                .Setup(
                    r => r.GoverningBodyAdministration.GetAllAsync(
                        It.IsAny<Expression<Func<GoverningBodyAdministration, bool>>>(),
                        It.IsAny<Func<IQueryable<GoverningBodyAdministration>,
                        IIncludableQueryable<GoverningBodyAdministration, object>>>()))
                .ReturnsAsync(GoverningBodyAdmins);
            _repoWrapper
                .Setup(r => r.GoverningBodyAdministration.Update(It.IsAny<GoverningBodyAdministration>()));
            _repoWrapper
                .Setup(r => r.SaveAsync());
            _userManager
                .Setup(u => u.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(FakeUser);

            //Act
            var result = _governingBodyAdministrationService.RemoveMainAdministratorAsync(It.IsAny<string>());

            //Assert
            _userManager.Verify(x => x.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
            Assert.NotNull(result);
        }

        [Test]
        public async Task GetUsersForGoverningBodyAdminFormAsync_ReturnsIEnumerableShortUserInformationDTO()
        {
            //Arrange
            List<string> roles = new List<string>
            {
                Roles.GoverningBodyAdmin,
                Roles.PlastMember
            };
            _repoWrapper
                .Setup(x => x.User.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>,
                        IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(GetTestUsers);
            _userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(roles);
            _mapperMock.Setup(m => m.Map<User, ShortUserInformationDto>(It.IsAny<User>()))
                .Returns(new ShortUserInformationDto());

            //Act
            var result = await _governingBodyAdministrationService.GetUsersForGoverningBodyAdminFormAsync();

            //Assert
            Assert.IsInstanceOf<IEnumerable<ShortUserInformationDto>>(result);
        }

        [Test]
        public async Task CheckRoleNameExistsAsync_RoleNameExists_ReturnsTrue()
        {
            //Arrange
            _repoWrapper.Setup(w => w.GoverningBodyAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<GoverningBodyAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAdministration>,
                        IIncludableQueryable<GoverningBodyAdministration, object>>>()))
                .ReturnsAsync(new GoverningBodyAdministration());

            //Act
            var result = await _governingBodyAdministrationService.CheckRoleNameExistsAsync("Test role");

            //Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task CheckRoleNameExistsAsync_RoleNameDontExist_ReturnsFalse()
        {
            //Arrange
            GoverningBodyAdministration nullValue = null;

            _repoWrapper.Setup(w => w.GoverningBodyAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<GoverningBodyAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAdministration>,
                        IIncludableQueryable<GoverningBodyAdministration, object>>>()))
                .ReturnsAsync(nullValue);

            //Act
            var result = await _governingBodyAdministrationService.CheckRoleNameExistsAsync(It.IsAny<string>());

            //Assert
            Assert.AreEqual(false, result);
        }

        private IEnumerable<User> GetTestUsers()
        {
            return new List<User>
            {
                new  User
                {
                    Id = "UserId1",
                    UserProfile = new UserProfile
                    {
                        Gender = new Gender{ ID = 1, Name = UserGenders.Male },
                        UpuDegree = new UpuDegree
                        {
                            Name = "UpuDegreeName"
                        }
                    },
                    UserPlastDegrees = new UserPlastDegree()
                },
                new  User
                {
                    Id = "UserId2",
                    UserProfile = new UserProfile
                    {
                        Gender = new Gender{ ID = 2, Name = UserGenders.Female },
                        UpuDegree = new UpuDegree
                        {
                            Name = "UpuDegreeName"
                        }
                    },
                    UserPlastDegrees = new UserPlastDegree()
                },
            }.AsEnumerable();
        }

        private const int FakeId = 3;

        private static readonly AdminTypeDto AdminTypeDto = new AdminTypeDto
        {
            ID = 1,
            AdminTypeName = Roles.GoverningBodyHead
        };

        private static readonly IEnumerable<AdminType> AdminTypes = new List<AdminType>
        {
            new AdminType {
                ID = 1,
                AdminTypeName = Roles.GoverningBodyHead
            },
            new AdminType
            {
                ID = 2,
                AdminTypeName = Roles.GoverningBodyAdmin
            }
        };

        private static readonly GoverningBodyAdministration GoverningBodyAdmin = new GoverningBodyAdministration
        {
            Id = 1,
            AdminType = new AdminType
            {
                AdminTypeName = Roles.GoverningBodyHead,
                ID = 1
            },
            AdminTypeId = AdminTypeDto.ID,
            UserId = Roles.GoverningBodyHead
        };

        private static readonly IEnumerable<GoverningBodyAdministration> GoverningBodyAdmins = new List<GoverningBodyAdministration>
        {
            new GoverningBodyAdministration
            {
                Id = 1,
                AdminType = new AdminType
                {
                    AdminTypeName = Roles.GoverningBodyHead
                },
                AdminTypeId = AdminTypes.ElementAt(0).ID,
                UserId = Roles.GoverningBodyHead
            },
            new GoverningBodyAdministration
            {
                Id = 2,
                AdminType = new AdminType
                {
                    AdminTypeName = Roles.GoverningBodyAdmin
                },
                AdminTypeId = AdminTypes.ElementAt(0).ID,
                UserId = Roles.GoverningBodyHead
            }
        };

        private static readonly Organization GoverningBody = new Organization
        {
            IsMainStatus = true
        };

        private static readonly GoverningBodyAdministrationDto GoverningBodyAdministrationDtoEndDateToday = new GoverningBodyAdministrationDto
        {
            ID = 1,
            AdminType = AdminTypeDto,
            GoverningBodyId = 1,
            AdminTypeId = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Today,
            User = new GoverningBodyUserDto(),
            UserId = Roles.GoverningBodyHead,
            GoverningBodyAdminRole = "Admin"
        };

        private readonly GoverningBodyAdministrationDto GoverningBodyAdministrationDtoEndDateNull = new GoverningBodyAdministrationDto
        {
            ID = 1,
            AdminType = AdminTypeDto,
            GoverningBodyId = 1,
            AdminTypeId = 1,
            StartDate = DateTime.Now,
            EndDate = null,
            Status = true,
            User = new GoverningBodyUserDto(),
            UserId = Roles.GoverningBodyHead,
            GoverningBodyAdminRole = "Admin"
        };

        private readonly User FakeUser = new User
        {
            Id = "testId"
        };

        private readonly List<string> RolesList = new List<string>
        {
            Roles.GoverningBodyAdmin
        };
    }
}
