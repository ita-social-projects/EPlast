using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.GoverningBody.Sector;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Services.GoverningBodies.Sector;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.GoverningBody;
using EPlast.DataAccess.Entities.GoverningBody.Sector;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services.GoverningBody.Sector
{
    internal class SectorAdministrationServiceTests
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<UserManager<User>> _userManager;
        private Mock<IAdminTypeService> _adminTypeService;
        private Mock<IMapper> _mapper;
        private SectorAdministrationService _service;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _adminTypeService = new Mock<IAdminTypeService>();
            _mapper = new Mock<IMapper>();

            var store = new Mock<Microsoft.AspNetCore.Identity.IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            _service = new SectorAdministrationService(
                _repoWrapper.Object,
                _userManager.Object,
                _adminTypeService.Object,
                _mapper.Object);
        }

        [Test]
        public async Task AddSectorAdministratorAsync_CreatesAdministrator()
        {
            //Arrange
            _adminTypeService
                .Setup(x => x.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDto());
            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _repoWrapper
                .Setup(x => x.GoverningBodySectorAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<SectorAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<SectorAdministration>,
                        IIncludableQueryable<SectorAdministration, object>>>()))
                .ReturnsAsync(new SectorAdministration());
            _adminTypeService
                .Setup(x => x.GetAdminTypeByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new AdminTypeDto());
            _userManager
                .Setup(x => x.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { Roles.PlastMember });
            _repoWrapper
                .Setup(x => x.GoverningBodyAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<GoverningBodyAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAdministration>,
                        IIncludableQueryable<GoverningBodyAdministration, object>>>()))
                .ReturnsAsync(new GoverningBodyAdministration());
            var testSectorAdmin = new SectorAdministrationDto()
            {
                AdminType = new AdminTypeDto() { AdminTypeName = "test" }
            };

            //Act
            var result = await _service.AddSectorAdministratorAsync(testSectorAdmin);

            //Assert
            _repoWrapper.Verify(x => x.GoverningBodySectorAdministration.CreateAsync(
                It.IsAny<SectorAdministration>()), Times.Once);
            _repoWrapper.Verify(x => x.SaveAsync(), Times.Exactly(2));
            Assert.AreEqual(testSectorAdmin, result);
        }

        [Test]
        public void AddSectorAdministratorAsync_UserHasRestrictedRoles_ThrowsArgumentException()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.GoverningBodySectorAdministration.CreateAsync(It.IsAny<SectorAdministration>()));
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { Roles.GoverningBodySectorHead });
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDto());

            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddSectorAdministratorAsync(new SectorAdministrationDto() { AdminType = new AdminTypeDto() }));
        }

        [Test]
        public async Task RemoveAdminRolesByUserIdAsync_ValidTest()
        {
            //Arrange
            _repoWrapper
                .Setup(x => x.GoverningBodySectorAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<SectorAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<SectorAdministration>,
                        IIncludableQueryable<SectorAdministration, object>>>()))
                .ReturnsAsync(new SectorAdministration());
            _adminTypeService
               .Setup(x => x.GetAdminTypeByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(new AdminTypeDto());
            _repoWrapper
                .Setup(x => x.GoverningBodySectorAdministration.GetAllAsync(It.IsAny<Expression<Func<SectorAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<SectorAdministration>, IIncludableQueryable<SectorAdministration, object>>>()))
                .ReturnsAsync(new List<SectorAdministration>() { new SectorAdministration() { Id = 1 } });
            _repoWrapper
                .Setup(x => x.GoverningBodyAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<GoverningBodyAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAdministration>,
                        IIncludableQueryable<GoverningBodyAdministration, object>>>()))
                .ReturnsAsync(new GoverningBodyAdministration());

            //Act
            await _service.RemoveAdminRolesByUserIdAsync(It.IsAny<string>());

            //Assert
            _repoWrapper.Verify();
            _adminTypeService.Verify();
        }

        [Test]
        public async Task EditSectorAdministratorAsync_AdminTypeIdEquals_UpdatesRepo()
        {
            //Arrange
            _repoWrapper
                .Setup(x => x.GoverningBodySectorAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<SectorAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<SectorAdministration>,
                        IIncludableQueryable<SectorAdministration, object>>>()))
                .ReturnsAsync(new SectorAdministration() { AdminTypeId = 1 });
            _adminTypeService
                .Setup(x => x.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDto() { ID = 1 });
            var testSectorAdmin = new SectorAdministrationDto()
            {
                AdminType = new AdminTypeDto() { AdminTypeName = "test" }
            };

            //Act
            var result = await _service.EditSectorAdministratorAsync(testSectorAdmin);

            //Assert
            _repoWrapper.Verify(x => x.GoverningBodySectorAdministration.Update(
                It.IsAny<SectorAdministration>()), Times.Once);
            _repoWrapper.Verify(x => x.SaveAsync(), Times.Once);
            Assert.AreEqual(testSectorAdmin, result);
        }

        [Test]
        public async Task EditSectorAdministratorAsync_AdminTypeIdNotEqual_RemovesAndAddsAdmin()
        {
            //Arrange
            _repoWrapper
                .Setup(x => x.GoverningBodySectorAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<SectorAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<SectorAdministration>,
                        IIncludableQueryable<SectorAdministration, object>>>()))
                .ReturnsAsync(new SectorAdministration() { AdminTypeId = 1 });
            _adminTypeService
                .Setup(x => x.GetAdminTypeByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new AdminTypeDto());
            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _userManager
                .Setup(x => x.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
            _adminTypeService
                .Setup(x => x.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDto() { ID = 2 });
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { Roles.PlastMember });
            _repoWrapper
                .Setup(x => x.GoverningBodyAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<GoverningBodyAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAdministration>,
                        IIncludableQueryable<GoverningBodyAdministration, object>>>()))
                .ReturnsAsync(new GoverningBodyAdministration());
            var testSectorAdmin = new SectorAdministrationDto()
            {
                AdminType = new AdminTypeDto() { AdminTypeName = "test" }
            };

            //Act
            var result = await _service.EditSectorAdministratorAsync(testSectorAdmin);

            //Assert
            _repoWrapper.Verify(x => x.GoverningBodySectorAdministration.Update(
                It.IsAny<SectorAdministration>()));
            _repoWrapper.Verify(x => x.GoverningBodySectorAdministration.CreateAsync(
                It.IsAny<SectorAdministration>()), Times.Once);
            _repoWrapper.Verify(x => x.SaveAsync(), Times.Exactly(3));
            Assert.AreEqual(testSectorAdmin, result);
        }
    }
}
