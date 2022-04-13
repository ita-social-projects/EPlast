using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.GoverningBody;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Services.GoverningBodies;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities.GoverningBody;

namespace EPlast.Tests.Services.GoverningBody
{
    internal class GoverningBodyAdministrationServiceTests
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IAdminTypeService> _adminTypeService;
        private Mock<UserManager<User>> _userManager;
        private Mock<IUserStore<User>> _user;
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
            _governingBodyAdministrationService = new GoverningBodyAdministrationService(
                _repoWrapper.Object, _userManager.Object,
                _adminTypeService.Object);
        }

        [Test]
        public async Task AddGoverningBodyAdministratorAsync_EndDateToday_ReturnsAdministrator()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.GoverningBodyAdministration.CreateAsync(GoverningBodyAdmin));
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { Roles.PlastMember });
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDTO());

            //Act
            var result = await _governingBodyAdministrationService.AddGoverningBodyAdministratorAsync(GoverningBodyAdministrationDtoEndDateToday);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<GoverningBodyAdministrationDTO>(result);
        }

        [Test]
        public async Task AddGoverningBodyAdministratorAsync_EndDateNull_ReturnsAdministrator()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.GoverningBodyAdministration.CreateAsync(GoverningBodyAdmin));
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { Roles.PlastMember });
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDTO());

            //Act
            var result = await _governingBodyAdministrationService.AddGoverningBodyAdministratorAsync(GoverningBodyAdministrationDtoEndDateNull);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<GoverningBodyAdministrationDTO>(result);
        }

        [Test]
        public void AddGoverningBodyAdministratorAsync_UserHasRestrictedRoles_ThrowsArgumentException()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.GoverningBodyAdministration.CreateAsync(GoverningBodyAdmin));
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { Roles.GoverningBodyHead });
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDTO());

            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _governingBodyAdministrationService.AddGoverningBodyAdministratorAsync(GoverningBodyAdministrationDtoEndDateNull));
        }

        [Test]
        public async Task AddGoverningBodyMainAdminAsync_EndDateToday_ReturnsAdministrator()
        {
            //Arrange
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { Roles.PlastMember });
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDTO());

            //Act
            var result = await _governingBodyAdministrationService.AddGoverningBodyMainAdminAsync(GoverningBodyAdministrationDtoEndDateToday);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<GoverningBodyAdministrationDTO>(result);
        }

        [Test]
        public async Task AddGoverningBodyMainAdminAsync_EndDateNull_ReturnsAdministrator()
        {
            //Arrange
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { Roles.PlastMember });
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDTO());

            //Act
            var result = await _governingBodyAdministrationService.AddGoverningBodyMainAdminAsync(GoverningBodyAdministrationDtoEndDateNull);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<GoverningBodyAdministrationDTO>(result);
        }

        [Test]
        public void AddGoverningBodyMainAdminAsync_UserHasRestrictedRoles_ThrowsArgumentException()
        {
            //Arrange
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { Roles.GoverningBodyHead });
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDTO());

            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _governingBodyAdministrationService.AddGoverningBodyMainAdminAsync(GoverningBodyAdministrationDtoEndDateNull));
        }

        [Test]
        public async Task EditGoverningBodyAdministratorAsync_ReturnsEditedAdministratorWithSameId()
        {
            //Arrange
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDTO());
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
            var result = await _governingBodyAdministrationService.EditGoverningBodyAdministratorAsync(GoverningBodyAdministrationDtoEndDateToday);

            //Assert
            _repoWrapper.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<GoverningBodyAdministrationDTO>(result);
        }

        [Test]
        public async Task EditGoverningBodyAdministratorAsync_WithDifferentAdminTypeId_ReturnsEditedAdministrator()
        {
            //Arrange
            _repoWrapper
                .Setup(r => r.GoverningBodyAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<GoverningBodyAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAdministration>,
                        IIncludableQueryable<GoverningBodyAdministration, object>>>()))
                .ReturnsAsync(new GoverningBodyAdministration());
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { Roles.PlastMember });
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDTO
                {
                    AdminTypeName = Roles.GoverningBodyHead,
                    ID = FakeId
                });
            _adminTypeService
                .Setup(a => a.GetAdminTypeByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new AdminTypeDTO
                {
                    AdminTypeName = Roles.GoverningBodyHead,
                    ID = FakeId
                });

            //Act
            var result = await _governingBodyAdministrationService.EditGoverningBodyAdministratorAsync(GoverningBodyAdministrationDtoEndDateToday);

            //Assert
            _repoWrapper.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<GoverningBodyAdministrationDTO>(result);
        }

        [Test]
        public void RemoveAdministratorAsync_Test()
        {
            //Arrange
            _repoWrapper
                .Setup(r => r.GoverningBodyAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<GoverningBodyAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAdministration>,
                        IIncludableQueryable<GoverningBodyAdministration, object>>>()))
                .ReturnsAsync(GoverningBodyAdmin);
            _adminTypeService
                .Setup(a => a.GetAdminTypeByIdAsync(It.IsAny<int>()))
                .Returns(() => Task<AdminTypeDTO>.Factory.StartNew(() => AdminType));
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
            _adminTypeService
                .Setup(a => a.GetAdminTypeByIdAsync(It.IsAny<int>()))
                .Returns(() => Task<AdminTypeDTO>.Factory.StartNew(() => AdminType));
            _userManager
                .Setup(u => u.FindByIdAsync(It.IsAny<string>()));
            _userManager
                .Setup(u => u.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
            _repoWrapper
                .Setup(r => r.GoverningBodyAdministration.Update(It.IsAny<GoverningBodyAdministration>()));
            _repoWrapper
                .Setup(r => r.SaveAsync());

            //Act
             await _governingBodyAdministrationService.RemoveAdminRolesByUserIdAsync(It.IsAny<string>());

            //Assert
            _repoWrapper.Verify();
            _adminTypeService.Verify();
        }

        [Test]
        public void RemoveMainAdministratorAsync_Test()
        {
            //Arrange
            _userManager
                .Setup(u => u.FindByIdAsync(It.IsAny<string>()));
            _userManager
                .Setup(u => u.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
            _userManager
               .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
               .ReturnsAsync(new List<string> { Roles.PlastMember });
            //Act
            var result = _governingBodyAdministrationService.RemoveMainAdministratorAsync(It.IsAny<string>());

            //Assert
            _repoWrapper.Verify();
            Assert.NotNull(result);
        }

        private const int FakeId = 3;

        private static readonly AdminTypeDTO AdminType = new AdminTypeDTO
        {
            AdminTypeName = Roles.GoverningBodyHead,
            ID = 1
        };

        private static readonly GoverningBodyAdministration GoverningBodyAdmin = new GoverningBodyAdministration
        {
            Id = 1,
            AdminType = new AdminType
            {
                AdminTypeName = Roles.GoverningBodyHead,
                ID = 1
            },
            AdminTypeId = AdminType.ID,
            UserId = Roles.GoverningBodyHead
        };

        private static readonly GoverningBodyAdministrationDTO GoverningBodyAdministrationDtoEndDateToday = new GoverningBodyAdministrationDTO
        {
            ID = 1,
            AdminType = AdminType,
            GoverningBodyId = 1,
            AdminTypeId = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Today,
            User = new GoverningBodyUserDTO(),
            UserId = Roles.GoverningBodyHead
        };

        private readonly GoverningBodyAdministrationDTO GoverningBodyAdministrationDtoEndDateNull = new GoverningBodyAdministrationDTO
        {
            ID = 1,
            AdminType = AdminType,
            GoverningBodyId = 1,
            AdminTypeId = 1,
            StartDate = DateTime.Now,
            EndDate = null,
            Status = true,
            User = new GoverningBodyUserDTO(),
            UserId = Roles.GoverningBodyHead
        };
    }
}
