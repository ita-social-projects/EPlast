using AutoMapper;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Services.Region;
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

namespace EPlast.Tests.Services.Regions
{
    class RegionAdministrationServiceTests
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private Mock<IAdminTypeService> _adminTypeService;
        private Mock<UserManager<User>> _userManager;
        private Mock<IUserStore<User>> _user;

        private RegionAdministrationService _servise;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _adminTypeService = new Mock<IAdminTypeService>();
            _user = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(_user.Object, It.IsAny<IOptions<IdentityOptions>>(),
               It.IsAny<IPasswordHasher<User>>(), It.IsAny<IEnumerable<IUserValidator<User>>>(), It.IsAny<IEnumerable<IPasswordValidator<User>>>(),
              It.IsAny<ILookupNormalizer>(), It.IsAny<IdentityErrorDescriber>(), It.IsAny<IServiceProvider>(), It.IsAny<ILogger<UserManager<User>>>());
            _servise = new RegionAdministrationService(
                _repoWrapper.Object, _mapper.Object, _adminTypeService.Object,
                _userManager.Object);
        }


        [Test]
        public async Task GetUsersAdministrations_ReturnsIEnumerableRegionAdministrationDTO()
        {
            // Arrange
            _repoWrapper.Setup(x => x.RegionAdministration.GetAllAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
                It.IsAny<Func<IQueryable<RegionAdministration>, IIncludableQueryable<RegionAdministration, object>>>()))
            .ReturnsAsync(new List<RegionAdministration>());

            _mapper.Setup(x => x.Map<IEnumerable<RegionAdministration>, IEnumerable<RegionAdministrationDTO>>(It.IsAny<IEnumerable<RegionAdministration>>()))
                .Returns(GetFakeAdminDTO());

            //Act
            var result = await _servise.GetUsersAdministrations(It.IsAny<string>());
            // Assert
            Assert.IsInstanceOf<IEnumerable<RegionAdministrationDTO>>(result);
            Assert.IsNotNull(result);
        }


        [Test]
        public async Task GetUsersPreviousAdministrations_ReturnsIEnumerableRegionAdministrationDTO()
        {
            // Arrange
            _repoWrapper.Setup(x => x.RegionAdministration.GetAllAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
                It.IsAny<Func<IQueryable<RegionAdministration>, IIncludableQueryable<RegionAdministration, object>>>()))
            .ReturnsAsync(new List<RegionAdministration>());

            _mapper.Setup(x => x.Map<IEnumerable<RegionAdministration>, IEnumerable<RegionAdministrationDTO>>(It.IsAny<IEnumerable<RegionAdministration>>()))
                .Returns(GetFakeAdminDTO());

            //Act
            var result = await _servise.GetUsersPreviousAdministrations(It.IsAny<string>());
            // Assert
            Assert.IsInstanceOf<IEnumerable<RegionAdministrationDTO>>(result);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetHead_ReturnsRegionAdministrationDTO()
        {
            // Arrange
            _repoWrapper.Setup(x => x.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
                It.IsAny<Func<IQueryable<RegionAdministration>, IIncludableQueryable<RegionAdministration, object>>>()))
            .ReturnsAsync(new RegionAdministration());

            _mapper.Setup(x => x.Map<RegionAdministration, RegionAdministrationDTO>(It.IsAny<RegionAdministration>()))
                .Returns(new RegionAdministrationDTO() { ID=2 });

            //Act
            var result = await _servise.GetHead(It.IsAny<int>());
            // Assert
            Assert.IsInstanceOf<RegionAdministrationDTO>(result);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetHeadDeputy_ReturnsRegionAdministrationDTO()
        {
            // Arrange
            _repoWrapper.Setup(x => x.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
                It.IsAny<Func<IQueryable<RegionAdministration>, IIncludableQueryable<RegionAdministration, object>>>()))
            .ReturnsAsync(new RegionAdministration());

            _mapper.Setup(x => x.Map<RegionAdministration, RegionAdministrationDTO>(It.IsAny<RegionAdministration>()))
                .Returns(new RegionAdministrationDTO() { ID = 2 });

            //Act
            var result = await _servise.GetHeadDeputy(It.IsAny<int>());
            // Assert
            Assert.IsInstanceOf<RegionAdministrationDTO>(result);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAdminType_ReturnsAdminTypeId()
        {
            // Arrange
            _repoWrapper.Setup(x => x.AdminType.GetFirstAsync(It.IsAny<Expression<Func<AdminType, bool>>>(),
                It.IsAny<Func<IQueryable<AdminType>, IIncludableQueryable<AdminType, object>>>()))
            .ReturnsAsync(new AdminType() { ID=2});
          
            //Act
            var result = await _servise.GetAdminType(It.IsAny<string>());
            // Assert
            Assert.IsInstanceOf<int>(result);
            Assert.AreEqual(2, result);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAdministrationAsyn_ReturnsIEnumerableRegionAdministrationDTO()
        {
            // Arrange
            _repoWrapper.Setup(x => x.RegionAdministration.GetAllAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
                It.IsAny<Func<IQueryable<RegionAdministration>, IIncludableQueryable<RegionAdministration, object>>>()))
            .ReturnsAsync(new List<RegionAdministration>());

            _mapper.Setup(x => x.Map<IEnumerable<RegionAdministration>, IEnumerable<RegionAdministrationDTO>>(It.IsAny<IEnumerable<RegionAdministration>>()))
                .Returns(GetFakeAdminDTO());

            //Act
            var result = await _servise.GetAdministrationAsync(It.IsAny<int>());
            // Assert
            Assert.IsInstanceOf<IEnumerable<RegionAdministrationDTO>>(result);
            Assert.AreEqual(3, result.Count());
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllAdminTypes_ReturnsIEnumerableRegionAdministrationDTO()
        {
            // Arrange
            _repoWrapper.Setup(x => x.AdminType.GetAllAsync(It.IsAny<Expression<Func<AdminType, bool>>>(),
                It.IsAny<Func<IQueryable<AdminType>, IIncludableQueryable<AdminType, object>>>()))
            .ReturnsAsync(new List<AdminType>());

            _mapper.Setup(x => x.Map<IEnumerable<AdminType>, IEnumerable<AdminTypeDTO>>(It.IsAny<IEnumerable<AdminType>>()))
                .Returns(GetFakeAdminTypes());

            //Act
            var result = await _servise.GetAllAdminTypes();
            // Assert
            Assert.IsInstanceOf<IEnumerable<AdminTypeDTO>>(result);
            Assert.AreEqual(3, result.Count());
            Assert.IsNotNull(result);
        }


        [Test]
        public void DeleteAdminByIdAsync_ReturnsCorrect()
        {
            //Arrange
            _repoWrapper
                .Setup(r => r.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
                It.IsAny<Func<IQueryable<RegionAdministration>,
                IIncludableQueryable<RegionAdministration, object>>>()))
                .ReturnsAsync(regionAdmHead);
            _adminTypeService
                .Setup(a => a.GetAdminTypeByIdAsync(It.IsAny<int>()))
                .Returns(() => Task<AdminTypeDTO>.Factory.StartNew(() => AdminTypeHead));
            _userManager
                .Setup(u => u.FindByIdAsync(It.IsAny<string>()));
            _userManager
                .Setup(u => u.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
            _repoWrapper
                .Setup(r => r.RegionAdministration.Delete(It.IsAny<RegionAdministration>()));
            _repoWrapper
                .Setup(r => r.SaveAsync());

            //Act
            var result = _servise.DeleteAdminByIdAsync(It.IsAny<int>());

            //Assert
            _repoWrapper.Verify();
            Assert.NotNull(result);
        }

        [Test]
        public void DeleteAdminByIdAsync_HeadDeputy_ReturnsCorrect()
        {
            //Arrange
            _repoWrapper
                .Setup(r => r.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
                It.IsAny<Func<IQueryable<RegionAdministration>,
                IIncludableQueryable<RegionAdministration, object>>>()))
                .ReturnsAsync(regionAdmHead);
            _adminTypeService
                .Setup(a => a.GetAdminTypeByIdAsync(It.IsAny<int>()))
                .Returns(() => Task<AdminTypeDTO>.Factory.StartNew(() => AdminTypeHeadDeputy));
            _userManager
                .Setup(u => u.FindByIdAsync(It.IsAny<string>()));
            _userManager
                .Setup(u => u.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
            _repoWrapper
                .Setup(r => r.RegionAdministration.Delete(It.IsAny<RegionAdministration>()));
            _repoWrapper
                .Setup(r => r.SaveAsync());

            //Act
            var result = _servise.DeleteAdminByIdAsync(It.IsAny<int>());

            //Assert
            _repoWrapper.Verify();
            Assert.NotNull(result);
        }

        [Test]
        public void DeleteAdminByIdAsync_Secretary_ReturnsCorrect()
        {
            //Arrange
            _repoWrapper
                .Setup(r => r.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
                It.IsAny<Func<IQueryable<RegionAdministration>,
                IIncludableQueryable<RegionAdministration, object>>>()))
                .ReturnsAsync(regionAdmHead);
            _adminTypeService
                .Setup(a => a.GetAdminTypeByIdAsync(It.IsAny<int>()))
                .Returns(() => Task<AdminTypeDTO>.Factory.StartNew(() => AdminTypeSecretary));
            _userManager
                .Setup(u => u.FindByIdAsync(It.IsAny<string>()));
            _userManager
                .Setup(u => u.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
            _repoWrapper
                .Setup(r => r.RegionAdministration.Delete(It.IsAny<RegionAdministration>()));
            _repoWrapper
                .Setup(r => r.SaveAsync());

            //Act
            var result = _servise.DeleteAdminByIdAsync(It.IsAny<int>());

            //Assert
            _repoWrapper.Verify();
            Assert.NotNull(result);
        }

        [Test]
        public async Task EditRegionAdministrator_SameAdminTypeID_ReturnsCorrect()
        {
            //Arrange
            RegionAdministrationDTO regionAdministrationDTO = regionAdmDTOEndDateNull;
            regionAdministrationDTO.AdminTypeId = 1;
            _repoWrapper
                .Setup(r => r.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
                It.IsAny<Func<IQueryable<RegionAdministration>,
                IIncludableQueryable<RegionAdministration, object>>>()))
                .ReturnsAsync(regionAdmHead);
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .Returns(() => Task<AdminTypeDTO>.Factory.StartNew(() => AdminTypeHead));
            _repoWrapper
                .Setup(r => r.RegionAdministration.Update(It.IsAny<RegionAdministration>()));
            _repoWrapper
                .Setup(r => r.SaveAsync());
            //Act
            var result = await _servise.EditRegionAdministrator(regionAdministrationDTO);
            //Assert
            _repoWrapper.Verify();
            Assert.IsInstanceOf<RegionAdministrationDTO>(result);
        }

        [Test]
        public async Task EditRegionAdministrator_OtherAdminTypeID_ReturnsCorrect()
        {
            //Arrange
            RegionAdministrationDTO regionAdministrationDTO = regionAdmDTOEndDateNull;
            _repoWrapper
                .Setup(r => r.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
                It.IsAny<Func<IQueryable<RegionAdministration>,
                IIncludableQueryable<RegionAdministration, object>>>()))
                .ReturnsAsync(regionAdmHead);
            _adminTypeService
                 .SetupSequence(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                 .ReturnsAsync(AdminTypeSecretary)
                 .ReturnsAsync(AdminTypeHead)
                 .ReturnsAsync(AdminTypeHeadDeputy)
                 .ReturnsAsync(AdminTypeSecretary)
                 .ReturnsAsync(AdminTypeHead)
                .ReturnsAsync(AdminTypeHeadDeputy);
            _adminTypeService
                .Setup(a => a.GetAdminTypeByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(AdminTypeHeadDeputy);
            //Act
            var result = await _servise.EditRegionAdministrator(regionAdministrationDTO);
            //Assert
            _repoWrapper.Verify();
            Assert.IsInstanceOf<RegionAdministrationDTO>(result);
        }

        [Test]
        public async Task EditRegionAdministrator_EditHeadAndRemoveHeadDeputy_ReturnsCorrect()
        {
            //Arrange
            RegionAdministrationDTO regionAdministrationDTO = regionAdmDTOEndDateNull;
            _repoWrapper
                .Setup(r => r.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
                It.IsAny<Func<IQueryable<RegionAdministration>,
                IIncludableQueryable<RegionAdministration, object>>>()))
                .ReturnsAsync(regionAdmSecretary);
            _adminTypeService
                .SetupSequence(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(AdminTypeHead)
                .ReturnsAsync(AdminTypeHead)
                .ReturnsAsync(AdminTypeHeadDeputy)
                .ReturnsAsync(AdminTypeSecretary)
                .ReturnsAsync(AdminTypeHead)
                .ReturnsAsync(AdminTypeHeadDeputy);
            _adminTypeService
                .Setup(a => a.GetAdminTypeByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(AdminTypeHead);
            _repoWrapper
                .Setup(r => r.RegionAdministration.Update(It.IsAny<RegionAdministration>()));
            _repoWrapper
                .Setup(r => r.SaveAsync());
            //Act
            var result = await _servise.EditRegionAdministrator(regionAdministrationDTO);
            //Assert
            _repoWrapper.Verify();
            Assert.IsInstanceOf<RegionAdministrationDTO>(result);
        }

        [Test]
        public async Task EditRegionAdministrator_EditHeadNotHeadDeputy_ReturnsCorrect()
        {
            //Arrange
            _repoWrapper
                .SetupSequence(r => r.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
                It.IsAny<Func<IQueryable<RegionAdministration>,
                IIncludableQueryable<RegionAdministration, object>>>()))
                .ReturnsAsync(regionAdmSecretary)
                .ReturnsAsync(new RegionAdministration() { UserId = Roles.CityHeadDeputy })
                .ReturnsAsync(regionAdmHead)
                .ReturnsAsync(regionAdmSecretary);
            _adminTypeService
                .SetupSequence(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(AdminTypeHead)
                .ReturnsAsync(AdminTypeHead)
                .ReturnsAsync(AdminTypeHeadDeputy)
                .ReturnsAsync(AdminTypeSecretary)
                .ReturnsAsync(AdminTypeHead)
                .ReturnsAsync(AdminTypeHeadDeputy);
            _adminTypeService
                .Setup(a => a.GetAdminTypeByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(AdminTypeHead);
            _repoWrapper
                .Setup(r => r.RegionAdministration.Update(It.IsAny<RegionAdministration>()));
            _repoWrapper
                .Setup(r => r.SaveAsync());
            //Act
            var result = await _servise.EditRegionAdministrator(regionAdmDTOEndDateNull);
            //Assert
            _repoWrapper.Verify();
            Assert.IsInstanceOf<RegionAdministrationDTO>(result);
        }

        [Test]
        public void AddRegionAdministrator_NullOldAdminWithEndDateToday_ReturnsCorrect()
        {
            //Arrange
            RegionAdministration adm = null;
            _adminTypeService
              .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
              .Returns(() => Task<AdminTypeDTO>.Factory.StartNew(() => AdminTypeHead));
            _repoWrapper
               .Setup(r => r.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
               It.IsAny<Func<IQueryable<RegionAdministration>,
               IIncludableQueryable<RegionAdministration, object>>>()))
               .ReturnsAsync(adm);
            _userManager
                .Setup(x=>x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User() { Id= "Some" });
            _userManager
                .Setup(x=>x.AddToRoleAsync(new User() { Id = "Some" }, Roles.OkrugaHead));
            _repoWrapper
                .Setup(x=>x.RegionAdministration.CreateAsync(regionAdmHead));
            //Act
            var result = _servise.AddRegionAdministrator(regionAdmDTOEndDateToday);
            //Assert
            _adminTypeService.Verify();
            _userManager.Verify();          
            Assert.NotNull(result);
        }

        [Test]
        public void AddRegionAdministrator_HeadDeputy_ReturnsCorrect()
        {
            //Arrange
            RegionAdministration adm = null;
            _adminTypeService
              .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
              .Returns(() => Task<AdminTypeDTO>.Factory.StartNew(() => AdminTypeHeadDeputy));
            _repoWrapper
               .Setup(r => r.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
               It.IsAny<Func<IQueryable<RegionAdministration>,
               IIncludableQueryable<RegionAdministration, object>>>()))
               .ReturnsAsync(adm);
            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User() { Id = "Some" });
            _userManager
                .Setup(x => x.AddToRoleAsync(new User() { Id = "Some" }, Roles.OkrugaHead));
            _repoWrapper
                .Setup(x => x.RegionAdministration.CreateAsync(regionAdmHead));
            //Act
            var result = _servise.AddRegionAdministrator(regionAdmDTOEndDateToday);
            //Assert
            _adminTypeService.Verify();
            _userManager.Verify();
            Assert.NotNull(result);
        }

        [Test]
        public async Task AddRegionAdministrator_Head_RemovesHeadDeputy_ReturnsCorrect()
        {
            //Arrange
            _repoWrapper
                .SetupSequence(r => r.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
                It.IsAny<Func<IQueryable<RegionAdministration>,
                IIncludableQueryable<RegionAdministration, object>>>()))
                .ReturnsAsync(regionAdmSecretary)
                .ReturnsAsync(new RegionAdministration() { UserId = Roles.OkrugaHead })
                .ReturnsAsync(regionAdmHead)
                .ReturnsAsync(regionAdmSecretary);
            _adminTypeService
                .SetupSequence(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(AdminTypeHead)
                .ReturnsAsync(AdminTypeHead)
                .ReturnsAsync(AdminTypeHeadDeputy)
                .ReturnsAsync(AdminTypeSecretary);
            _adminTypeService
               .Setup(a => a.GetAdminTypeByIdAsync(It.IsAny<int>()))
              .ReturnsAsync(AdminTypeHead);
            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User() { Id = "Some" });
            _userManager
                .Setup(x => x.AddToRoleAsync(new User() { Id = "Some" }, Roles.OkrugaHead));
            _repoWrapper
                .Setup(x => x.RegionAdministration.CreateAsync(regionAdmHead));

            //Act
            var result = await _servise.AddRegionAdministrator(regionAdmDTOEndDateToday);

            //Assert    
            _adminTypeService.Verify();
            _userManager.Verify();
            Assert.IsInstanceOf<RegionAdministrationDTO>(result);
        }

        [Test]
        public void AddRegionAdministrator_Secretary_ReturnsCorrect()
        {
            //Arrange
            RegionAdministration adm = null;
            _adminTypeService
              .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
              .Returns(() => Task<AdminTypeDTO>.Factory.StartNew(() => AdminTypeSecretary));
            _repoWrapper
               .Setup(r => r.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
               It.IsAny<Func<IQueryable<RegionAdministration>,
               IIncludableQueryable<RegionAdministration, object>>>()))
               .ReturnsAsync(adm);
            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User() { Id = "Some" });
            _userManager
                .Setup(x => x.AddToRoleAsync(new User() { Id = "Some" }, Roles.OkrugaHead));
            _repoWrapper
                .Setup(x => x.RegionAdministration.CreateAsync(regionAdmHead));
            //Act
            var result = _servise.AddRegionAdministrator(regionAdmDTOEndDateToday);
            //Assert
            _adminTypeService.Verify();
            _userManager.Verify();
            Assert.NotNull(result);
        }

        [Test]
        public void AddRegionAdministrator_NullOldAdminWithEndDateNull_ReturnsCorrect()
        {
            //Arrange
            RegionAdministration adm = null;
            _adminTypeService
              .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
              .Returns(() => Task<AdminTypeDTO>.Factory.StartNew(() => AdminTypeHead));
            _repoWrapper
               .Setup(r => r.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
               It.IsAny<Func<IQueryable<RegionAdministration>,
               IIncludableQueryable<RegionAdministration, object>>>()))
               .ReturnsAsync(adm);
            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User() { Id = "Some" });
            _userManager
                .Setup(x => x.AddToRoleAsync(new User() { Id = "Some" }, Roles.OkrugaHead));
            _repoWrapper
                .Setup(x => x.RegionAdministration.CreateAsync(regionAdmHead));
            //Act
            var result = _servise.AddRegionAdministrator(regionAdmDTOEndDateNull);
            //Assert
            _adminTypeService.Verify();
            _userManager.Verify();
            Assert.NotNull(result);
        }

        [Test]
        public void AddRegionAdministrator_EndDateToday_ReturnsCorrect()
        {
            //Arrange
            _adminTypeService
              .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
              .Returns(() => Task<AdminTypeDTO>.Factory.StartNew(() => AdminTypeHead));
            _repoWrapper
               .Setup(r => r.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
               It.IsAny<Func<IQueryable<RegionAdministration>,
               IIncludableQueryable<RegionAdministration, object>>>()))
               .ReturnsAsync(regionAdmHead);
            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User() { Id = "Some" })
                .Callback(()=> _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User() { Id = "SomeNew" }));
            _userManager
                .Setup(x => x.AddToRoleAsync(new User() { Id = "Some" }, Roles.OkrugaHead));
            _repoWrapper
                .Setup(x => x.RegionAdministration.CreateAsync(regionAdmHead));
            //Act
            var result = _servise.AddRegionAdministrator(regionAdmDTOEndDateToday);
            //Assert
            _adminTypeService.Verify();
            _userManager.Verify();
            Assert.NotNull(result);
        }

        [Test]
        public void AddRegionAdministrator_EndDateNull_ReturnsCorrect()
        {
            //Arrange
            _adminTypeService
              .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
              .Returns(() => Task<AdminTypeDTO>.Factory.StartNew(() => AdminTypeHead));
            _repoWrapper
               .Setup(r => r.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
               It.IsAny<Func<IQueryable<RegionAdministration>,
               IIncludableQueryable<RegionAdministration, object>>>()))
               .ReturnsAsync(regionAdmHead);
            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User() { Id = "Some" })
                .Callback(() => _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User() { Id = "SomeNew" }));
            _userManager
                .Setup(x => x.AddToRoleAsync(new User() { Id = "Some" }, Roles.OkrugaHead));
            _repoWrapper
                .Setup(x => x.RegionAdministration.CreateAsync(regionAdmHead));
            //Act
            var result = _servise.AddRegionAdministrator(regionAdmDTOEndDateNull);
            //Assert
            _adminTypeService.Verify();
            _userManager.Verify();
            Assert.NotNull(result);
        }

        private static AdminTypeDTO AdminTypeHead = new AdminTypeDTO
        {
            AdminTypeName = Roles.OkrugaHead,
            ID = 1
        };

        private static AdminTypeDTO AdminTypeHeadDeputy = new AdminTypeDTO
        {
            AdminTypeName = Roles.OkrugaHeadDeputy,
            ID = 1
        };

        private static AdminTypeDTO AdminTypeSecretary = new AdminTypeDTO
        {
            AdminTypeName = Roles.OkrugaSecretary,
            ID = 2
        };

        private readonly RegionAdministration regionAdmHead = new RegionAdministration
        {
            ID = 1,
            AdminType = new AdminType()
            {
                AdminTypeName = Roles.OkrugaHead,
                ID = 1
            },
            Status=true,
            AdminTypeId = AdminTypeHead.ID,
            UserId = Roles.OkrugaHead
        };

        private readonly RegionAdministration regionAdmSecretary = new RegionAdministration
        {
            ID = 2,
            AdminType = new AdminType()
            {
                AdminTypeName = Roles.OkrugaSecretary,
                ID = 2
            },
            Status = true,
            AdminTypeId = AdminTypeSecretary.ID,
            UserId = Roles.OkrugaHead
        };

        private readonly RegionAdministrationDTO regionAdmDTOEndDateToday = new RegionAdministrationDTO
        {
            ID = 1,
            AdminType = new AdminTypeDTO()
            {
                AdminTypeName = Roles.OkrugaHead,
                ID = 1
            },
            EndDate = DateTime.Today,
            Status = true,
            AdminTypeId = AdminTypeHead.ID,
            UserId = Roles.OkrugaHead,
            RegionId=2
        };

        private readonly RegionAdministrationDTO regionAdmDTOEndDateNull = new RegionAdministrationDTO
        {
            ID = 1,
            AdminType = new AdminTypeDTO()
            {
                AdminTypeName = Roles.OkrugaHead,
                ID = 1
            },
            Status = true,
            AdminTypeId = AdminTypeHead.ID,
            UserId = Roles.OkrugaHead,
            RegionId = 2
        };

        private IEnumerable<RegionAdministrationDTO> GetFakeAdminDTO() {
            return new List<RegionAdministrationDTO>() {
                    new RegionAdministrationDTO(){ID=2, AdminTypeId=2, CityId=2 },
                    new RegionAdministrationDTO(){ID=3, AdminTypeId=3, CityId=3 },
                    new RegionAdministrationDTO(){ID=4, AdminTypeId=4, CityId=4 }
            };
        }

        private IEnumerable<AdminTypeDTO> GetFakeAdminTypes()
        {
            return new List<AdminTypeDTO>() {
                    new AdminTypeDTO(){ID=2 },
                    new AdminTypeDTO(){ID=3 },
                    new AdminTypeDTO(){ID=4 }
            };
        }
    }
}
