using AutoMapper;
using EPlast.BLL.DTO.GoverningBody.Sector;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Services.GoverningBodies.Sector;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.Interfaces.Admin;
using EPlast.DataAccess.Entities.GoverningBody.Sector;

namespace EPlast.Tests.Services.GoverningBody.Sector
{
    internal class SectorAdministrationServiceTests
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<UserManager<User>> _userManager;
        private Mock<IAdminTypeService> _adminTypeService;
        private SectorAdministrationService _service;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _adminTypeService = new Mock<IAdminTypeService>();

            var store = new Mock<Microsoft.AspNetCore.Identity.IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            _service = new SectorAdministrationService(
                _repoWrapper.Object,
                _userManager.Object,
                _adminTypeService.Object);
        }

        [Test]
        public async Task AddSectorAdministratorAsync_CreatesAdministrator()
        {
            //Arrange
            _adminTypeService
                .Setup(x => x.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDTO());
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
                .ReturnsAsync(new AdminTypeDTO());
            _userManager
                .Setup(x => x.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
            var testSectorAdmin = new SectorAdministrationDTO()
            {
                AdminType = new AdminTypeDTO() { AdminTypeName = "test" }
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
                .ReturnsAsync(new AdminTypeDTO() { ID = 1 });
            var testSectorAdmin = new SectorAdministrationDTO()
            {
                AdminType = new AdminTypeDTO() { AdminTypeName = "test" }
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
                .ReturnsAsync(new AdminTypeDTO());
            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _userManager
                .Setup(x => x.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
            _adminTypeService
                .Setup(x => x.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDTO() { ID = 2 });
            var testSectorAdmin = new SectorAdministrationDTO()
            {
                AdminType = new AdminTypeDTO() { AdminTypeName = "test" }
            };

            //Act
            var result = await _service.EditSectorAdministratorAsync(testSectorAdmin);

            //Assert
            _repoWrapper.Verify(x => x.GoverningBodySectorAdministration.Delete(
                It.IsAny<SectorAdministration>()));
            _repoWrapper.Verify(x => x.GoverningBodySectorAdministration.CreateAsync(
                It.IsAny<SectorAdministration>()), Times.Once);
            _repoWrapper.Verify(x => x.SaveAsync(), Times.Exactly(3));
            Assert.AreEqual(testSectorAdmin, result);
        }
    }
}
