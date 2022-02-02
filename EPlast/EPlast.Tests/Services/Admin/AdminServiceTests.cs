using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.FormerMember;
using EPlast.BLL.Services;
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

namespace EPlast.Tests.Services
{
    internal class AdminServiceTests
    {
        private readonly List<string> roles = new List<string>()
        {
            Roles.Admin,
            Roles.Supporter,
            Roles.FormerPlastMember
        };
        private Mock<IMapper> _mapper;
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<RoleManager<IdentityRole>> _roleManager;
        private Mock<IRoleStore<IdentityRole>> _store;
        private Mock<IUserStore<User>> _user;
        private Mock<UserManager<User>> _userManager;
        private AdminService service;
        private Mock<IFormerMemberService> _formerMemberService;

        [Test]
        public void ChangeAsync_ReturnsCorrect()
        {
            // Arrange
            _formerMemberService.Setup(x => x.MakeUserFormerMeberAsync(It.IsAny<string>()));        

            // Act
            var result = service.ChangeAsync(It.IsAny<string>());

            // Assert
            _formerMemberService.Verify();
            Assert.NotNull(result);
        }

        [Test]
        public async Task ChangeCurrentRoleAsync_AddInterested_CaseInterFormer_ReturnsCorrectAsync()
        {
            // Arrange
            string plastun = Roles.PlastMember;
            string admin = Roles.Admin;
            string interested = Roles.Interested;
            string formerMember = Roles.FormerPlastMember;

            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _userManager
              .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
              .ReturnsAsync(new List<string>() { formerMember });
            _userManager
                .Setup(x => x.RemoveFromRoleAsync(It.IsAny<User>(), plastun));

            _repoWrapper
                .Setup(x => x.UserMembershipDates.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserMembershipDates, bool>>>(),
               It.IsAny<Func<IQueryable<UserMembershipDates>,
               IIncludableQueryable<UserMembershipDates, object>>>()))
                .ReturnsAsync(new UserMembershipDates() { DateEntry = default });
            _repoWrapper
                .Setup(x => x.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
               It.IsAny<Func<IQueryable<CityMembers>,
               IIncludableQueryable<CityMembers, object>>>())).ReturnsAsync(new CityMembers() { IsApproved = true });
            _userManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<User>(), admin));

            // Act
            await service.ChangeCurrentRoleAsync("id", interested);
            // Assert
            _userManager.Verify();
            _repoWrapper.Verify();
        }

        [Test]
        public async Task ChangeCurrentRoleAsync_AddInterested_CaseInterInter_ReturnsCorrectAsync()
        {
            // Arrange
            string plastun = Roles.PlastMember;
            string admin = Roles.Admin;
            string interested = Roles.Interested;

            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _userManager
              .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
              .ReturnsAsync(new List<string>() { interested });
            _userManager
                .Setup(x => x.RemoveFromRoleAsync(It.IsAny<User>(), plastun));

            _repoWrapper
                .Setup(x => x.UserMembershipDates.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserMembershipDates, bool>>>(),
               It.IsAny<Func<IQueryable<UserMembershipDates>,
               IIncludableQueryable<UserMembershipDates, object>>>()))
                .ReturnsAsync(new UserMembershipDates() { DateEntry = default });
            _repoWrapper
                .Setup(x => x.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
               It.IsAny<Func<IQueryable<CityMembers>,
               IIncludableQueryable<CityMembers, object>>>())).ReturnsAsync(new CityMembers() { IsApproved = true });
            _userManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<User>(), admin));

            // Act
            await service.ChangeCurrentRoleAsync("id", interested);
            // Assert
            _userManager.Verify();
            _repoWrapper.Verify();
        }

        [Test]
        public async Task ChangeCurrentRoleAsync_AddInterested_CaseInterPlastun_ReturnsCorrectAsync()
        {
            // Arrange
            string plastun = Roles.PlastMember;
            string admin = Roles.Admin;
            string interested = Roles.Interested;

            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _userManager
              .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
              .ReturnsAsync(new List<string>() { plastun });
            _userManager
                .Setup(x => x.RemoveFromRoleAsync(It.IsAny<User>(), plastun));

            _repoWrapper
                .Setup(x => x.UserMembershipDates.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserMembershipDates, bool>>>(),
               It.IsAny<Func<IQueryable<UserMembershipDates>,
               IIncludableQueryable<UserMembershipDates, object>>>()))
                .ReturnsAsync(new UserMembershipDates() { DateEntry = default });
            _repoWrapper
                .Setup(x => x.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
               It.IsAny<Func<IQueryable<CityMembers>,
               IIncludableQueryable<CityMembers, object>>>())).ReturnsAsync(new CityMembers() { IsApproved = true });
            _userManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<User>(), admin));

            // Act
            await service.ChangeCurrentRoleAsync("id", interested);
            // Assert
            _userManager.Verify();
            _repoWrapper.Verify();
        }

        [TestCase("userId")]
        public async Task ChangeCurrentRoleAsync_AddInterested_CaseRegistered_ReturnsCorrectAsync(string userId)
        {
            // Arrange
            var registeredUser = Roles.RegisteredUser;
            var admin = Roles.Admin;
            var interested = Roles.Interested;

            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string>() { registeredUser });
            _userManager
                .Setup(x => x.RemoveFromRoleAsync(It.IsAny<User>(), registeredUser));

            _repoWrapper
                .Setup(x => x.UserMembershipDates.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserMembershipDates, bool>>>(),
                    It.IsAny<Func<IQueryable<UserMembershipDates>,
                        IIncludableQueryable<UserMembershipDates, object>>>()))
                .ReturnsAsync(new UserMembershipDates() { DateEntry = default });
            _repoWrapper
                .Setup(x => x.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<CityMembers>,
                        IIncludableQueryable<CityMembers, object>>>())).ReturnsAsync(new CityMembers() { IsApproved = true });
            _userManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<User>(), admin));

            // Act
            await service.ChangeCurrentRoleAsync(userId, interested);

            // Assert
            _userManager.Verify();
            _repoWrapper.Verify();
        }

        [TestCase("userId")]
        public async Task ChangeCurrentRoleAsync_AddRegistered_CaseFormerMember_ReturnsCorrectAsync(string userId)
        {
            // Arrange
            var registeredUser = Roles.RegisteredUser;
            var admin = Roles.Admin;

            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string>() {});
            _userManager
                .Setup(x => x.RemoveFromRoleAsync(It.IsAny<User>(), registeredUser));

            _repoWrapper
                .Setup(x => x.UserMembershipDates.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserMembershipDates, bool>>>(),
                    It.IsAny<Func<IQueryable<UserMembershipDates>,
                        IIncludableQueryable<UserMembershipDates, object>>>()))
                .ReturnsAsync(new UserMembershipDates() { DateEntry = default });
            _repoWrapper
                .Setup(x => x.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<CityMembers>,
                        IIncludableQueryable<CityMembers, object>>>())).ReturnsAsync(new CityMembers() { IsApproved = true });
            _userManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<User>(), admin));

            // Act
            await service.ChangeCurrentRoleAsync(userId, registeredUser);

            // Assert
            _userManager.Verify();
            _repoWrapper.Verify();
        }

        [Test]
        public async Task ChangeCurrentRoleAsync_AddInterested_ReturnsCorrectAsync()
        {
            // Arrange
            string plastun = Roles.PlastMember;
            string admin = Roles.Admin;
            string interested = Roles.Interested;
            const string supporter = Roles.Supporter;

            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _userManager
              .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
              .ReturnsAsync(new List<string>() { supporter });
            _userManager
                .Setup(x => x.RemoveFromRoleAsync(It.IsAny<User>(), plastun));
            _repoWrapper
                .Setup(x => x.UserMembershipDates.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserMembershipDates, bool>>>(),
               It.IsAny<Func<IQueryable<UserMembershipDates>,
               IIncludableQueryable<UserMembershipDates, object>>>()))
                .ReturnsAsync(new UserMembershipDates() { DateEntry = default });
            _repoWrapper
                .Setup(x => x.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
               It.IsAny<Func<IQueryable<CityMembers>,
               IIncludableQueryable<CityMembers, object>>>())).ReturnsAsync(new CityMembers() { IsApproved = true });
            _userManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<User>(), admin));

            // Act
            await service.ChangeCurrentRoleAsync("id", interested);

            // Assert
            _userManager.Verify();
            _repoWrapper.Verify();
        }

        [Test]
        public void ChangeCurrentRoleAsync_AddPlastun_ReturnsCorrect()
        {
            // Arrange
            string plastun = Roles.PlastMember;
            string admin = Roles.Admin;
            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _userManager
              .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
              .ReturnsAsync(new List<string>() { plastun });
            _userManager
                .Setup(x => x.RemoveFromRoleAsync(It.IsAny<User>(), plastun));
            _userManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<User>(), admin));
            // Act
            var result = service.ChangeCurrentRoleAsync(It.IsAny<string>(), admin);
            // Assert
            _userManager.Verify();
            Assert.NotNull(result);
        }

        [Test]
        public void ChangeCurrentRoleAsync_AddSupporter_ReturnsCorrect()
        {
            // Arrange
            string supporter = Roles.Supporter;
            string admin = Roles.Admin;
            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _userManager
              .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
              .ReturnsAsync(new List<string>() { supporter });
            _userManager
                .Setup(x => x.RemoveFromRoleAsync(It.IsAny<User>(), supporter));
            _userManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<User>(), admin));
            // Act
            var result = service.ChangeCurrentRoleAsync(It.IsAny<string>(), admin);
            // Assert
            _userManager.Verify();
            Assert.NotNull(result);
        }

        [Test]
        public void DeleteUserAsync_ReturnsCorrect()
        {
            // Arrange
            _repoWrapper
               .Setup(x => x.User.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(),
               It.IsAny<Func<IQueryable<User>,
               IIncludableQueryable<User, object>>>()))
               .ReturnsAsync(new User());
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>());
            _repoWrapper
                .Setup(x => x.User.Delete(It.IsAny<User>()));
            _repoWrapper
                 .Setup(x => x.SaveAsync());
            // Act
            var result = service.DeleteUserAsync(It.IsAny<string>());
            // Assert
            _userManager.Verify();
            _repoWrapper.Verify();
            Assert.NotNull(result);
        }

        [Test]
        public void DeleteUserAsync_userIsNull_ReturnsCorrect()
        {
            // Arrange
            _repoWrapper
               .Setup(x => x.User.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(),
               It.IsAny<Func<IQueryable<User>,
               IIncludableQueryable<User, object>>>()));
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>());

            // Act
            var result = service.DeleteUserAsync(It.IsAny<string>());

            // Assert
            _userManager.Verify();
            _repoWrapper.Verify();
            Assert.NotNull(result);
        }

        [Test]
        public void EditAsync_countIsZero_ReturnsCorrect()
        {
            // Arrange
            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User() { FirstName = "James", LastName = "Bond" });
            _userManager
                .Setup(x => x.GetRolesAsync(new User() { FirstName = "James", LastName = "Bond" }))
                .ReturnsAsync(new List<string>() { Roles.Supporter });
            _userManager
                .Setup(x => x.AddToRolesAsync(It.IsAny<User>(), It.IsAny<List<string>>()));
            _userManager
                .Setup(x => x.RemoveFromRolesAsync(It.IsAny<User>(), It.IsAny<List<string>>()));
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string>());
            // Act
            var result = service.EditAsync(It.IsAny<string>(), roles);
            // Assert
            _userManager.Verify();
            Assert.NotNull(result);
        }

        [Test]
        public void EditAsync_ReturnsCorrect()
        {
            // Arrange
            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User() { FirstName = "James", LastName = "Bond" });
            _userManager
                .Setup(x => x.GetRolesAsync(new User() { FirstName = "James", LastName = "Bond" }))
                .ReturnsAsync(new List<string>() { Roles.Supporter });
            _userManager
                .Setup(x => x.AddToRolesAsync(It.IsAny<User>(), It.IsAny<List<string>>()));
            _userManager
                .Setup(x => x.RemoveFromRolesAsync(It.IsAny<User>(), It.IsAny<List<string>>()));
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(roles);
            // Act
            var result = service.EditAsync(It.IsAny<string>(), roles);
            // Assert
            _userManager.Verify();
            Assert.NotNull(result);
        }

        [Test]
        public async Task GetCityRegionAdminsOfUserAsync_RoleOkrugaHead_ReturnsCorrect()
        {
            // Arrange
            AdminType adminType = new AdminType() { AdminTypeName = Roles.OkrugaHead };
            RegionAdministration regionAdministration = new RegionAdministration() { AdminType = adminType };
            ICollection<RegionAdministration> regionAdministrations = new List<RegionAdministration>() { regionAdministration };
            Region region = new Region { RegionAdministration = regionAdministrations };
            DataAccess.Entities.City city = new DataAccess.Entities.City() { ID = 1, Region = region };
            List<DataAccess.Entities.City> cities = new List<DataAccess.Entities.City> { city };

            List<RegionAdministrationDTO> regionAdministrationDTOs = new List<RegionAdministrationDTO>();
            RegionDTO regionDTO = new RegionDTO() { Administration = regionAdministrationDTOs };
            CityDTO cityDTO = new CityDTO() { ID = 1, Region = regionDTO };
            List<CityDTO> cityDTOs = new List<CityDTO>() { cityDTO };

            _repoWrapper
                .Setup(x => x.City.GetAllAsync
                (
                    It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.City>,
                    IIncludableQueryable<DataAccess.Entities.City, object>>>())
                )
                .ReturnsAsync(cities);
            _mapper
                .Setup(x => x.Map<IEnumerable<DataAccess.Entities.City>, IEnumerable<CityDTO>>(It.IsAny<List<DataAccess.Entities.City>>()))
                .Returns(cityDTOs);
            _mapper
                .Setup(x => x.Map<IEnumerable<RegionAdministration>, IEnumerable<RegionAdministrationDTO>>(It.IsAny<IEnumerable<RegionAdministration>>()))
                .Returns(new List<RegionAdministrationDTO>());

            // Act
            var result = await service.GetCityRegionAdminsOfUserAsync("string");

            // Assert
            Assert.IsInstanceOf<IEnumerable<CityDTO>>(result);
        }

        [Test]
        public async Task GetCityRegionAdminsOfUserAsync_RoleOkrugaHeadDeputy_ReturnsCorrect()
        {
            // Arrange
            AdminType adminType = new AdminType() { AdminTypeName = Roles.OkrugaHeadDeputy };
            RegionAdministration regionAdministration = new RegionAdministration() { AdminType = adminType };
            ICollection<RegionAdministration> regionAdministrations = new List<RegionAdministration>() { regionAdministration };
            Region region = new Region { RegionAdministration = regionAdministrations };
            DataAccess.Entities.City city = new DataAccess.Entities.City() { ID = 1, Region = region };
            List<DataAccess.Entities.City> cities = new List<DataAccess.Entities.City> { city };

            List<RegionAdministrationDTO> regionAdministrationDTOs = new List<RegionAdministrationDTO>();
            RegionDTO regionDTO = new RegionDTO() { Administration = regionAdministrationDTOs };
            CityDTO cityDTO = new CityDTO() { ID = 1, Region = regionDTO };
            List<CityDTO> cityDTOs = new List<CityDTO>() { cityDTO };

            _repoWrapper
                .Setup(x => x.City.GetAllAsync
                (
                    It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.City>,
                    IIncludableQueryable<DataAccess.Entities.City, object>>>())
                )
                .ReturnsAsync(cities);
            _mapper
                .Setup(x => x.Map<IEnumerable<DataAccess.Entities.City>, IEnumerable<CityDTO>>(It.IsAny<List<DataAccess.Entities.City>>()))
                .Returns(cityDTOs);
            _mapper
                .Setup(x => x.Map<IEnumerable<RegionAdministration>, IEnumerable<RegionAdministrationDTO>>(It.IsAny<IEnumerable<RegionAdministration>>()))
                .Returns(new List<RegionAdministrationDTO>());

            // Act
            var result = await service.GetCityRegionAdminsOfUserAsync("string");

            // Assert
            Assert.IsInstanceOf<IEnumerable<CityDTO>>(result);
        }

        [Test]
        public void GetRolesExceptAdmin_Valid_Test()
        {
            // Arrange
            var identityRoles = new IdentityRole[] { }.AsQueryable<IdentityRole>();
            _roleManager
                .Setup(x => x.Roles)
                .Returns(identityRoles);

            // Act
            var result = service.GetRolesExceptAdmin();

            // Assert
            Assert.IsInstanceOf<IEnumerable<IdentityRole>>(result);
        }

        [SetUp]
        public void SetUp()
        {
            _store = new Mock<IRoleStore<IdentityRole>>();
            _user = new Mock<IUserStore<User>>();
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _userManager = new Mock<UserManager<User>>(_user.Object, It.IsAny<IOptions<IdentityOptions>>(),
               It.IsAny<IPasswordHasher<User>>(), It.IsAny<IEnumerable<IUserValidator<User>>>(), It.IsAny<IEnumerable<IPasswordValidator<User>>>(),
              It.IsAny<ILookupNormalizer>(), It.IsAny<IdentityErrorDescriber>(), It.IsAny<IServiceProvider>(), It.IsAny<ILogger<UserManager<User>>>());
            _roleManager = new Mock<RoleManager<IdentityRole>>(
               _store.Object, It.IsAny<IEnumerable<IRoleValidator<IdentityRole>>>(),
               It.IsAny<ILookupNormalizer>(), It.IsAny<IdentityErrorDescriber>(), It.IsAny<ILogger<RoleManager<IdentityRole>>>());
            _mapper = new Mock<IMapper>();
            _formerMemberService = new Mock<IFormerMemberService>();
            service = new AdminService(
                _repoWrapper.Object,
                _formerMemberService.Object,
                _userManager.Object,
                _mapper.Object,
                _roleManager.Object
              

            );
        }

        [Test]
        public void UpdateUserDatesByChangeRoleAsyncAsync_interested_ReturnsCorrect()
        {
            // Arrange
            var role = Roles.Interested;
            _repoWrapper
                .Setup(x => x.UserMembershipDates.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserMembershipDates, bool>>>(),
               It.IsAny<Func<IQueryable<UserMembershipDates>,
               IIncludableQueryable<UserMembershipDates, object>>>()))
                .ReturnsAsync(new UserMembershipDates() { DateEntry = default });
            _repoWrapper
                .Setup(x => x.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
               It.IsAny<Func<IQueryable<CityMembers>,
               IIncludableQueryable<CityMembers, object>>>())).ReturnsAsync(new CityMembers() { IsApproved = true });

            // Act
            var result = service.UpdateUserDatesByChangeRoleAsyncAsync(It.IsAny<string>(), role);
            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void UpdateUserDatesByChangeRoleAsyncAsync_plastun_ReturnsCorrect()
        {
            // Arrange
            var role = Roles.PlastMember;
            _repoWrapper
                .Setup(x => x.UserMembershipDates.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserMembershipDates, bool>>>(),
               It.IsAny<Func<IQueryable<UserMembershipDates>,
               IIncludableQueryable<UserMembershipDates, object>>>()))
                .ReturnsAsync(new UserMembershipDates() { DateEntry = default });
            _repoWrapper
                .Setup(x => x.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
               It.IsAny<Func<IQueryable<CityMembers>,
               IIncludableQueryable<CityMembers, object>>>())).ReturnsAsync(new CityMembers() { IsApproved = true });

            // Act
            var result = service.UpdateUserDatesByChangeRoleAsyncAsync(It.IsAny<string>(), role);
            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void UpdateUserDatesByChangeRoleAsyncAsync_supporter_ReturnsCorrect()
        {
            // Arrange
            var role = Roles.Supporter;
            _repoWrapper
                .Setup(x => x.UserMembershipDates.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserMembershipDates, bool>>>(),
               It.IsAny<Func<IQueryable<UserMembershipDates>,
               IIncludableQueryable<UserMembershipDates, object>>>()))
                .ReturnsAsync(new UserMembershipDates() { DateEntry = default });
            _repoWrapper
                .Setup(x => x.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
               It.IsAny<Func<IQueryable<CityMembers>,
               IIncludableQueryable<CityMembers, object>>>())).ReturnsAsync(new CityMembers() { IsApproved = true });

            // Act
            var result = service.UpdateUserDatesByChangeRoleAsyncAsync(It.IsAny<string>(), role);
            // Assert
            Assert.IsNotNull(result);
        }

        [TestCase]
        public async Task UsersTableAsync_NullInput_ReturnsIEnumerableUserTableDTO()
        {
            // Arrange
            _repoWrapper
                .Setup(x => x.User.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>(),
               It.IsAny<Func<IQueryable<User>,
               IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(new List<User>());
            _repoWrapper
                .Setup(x => x.City.GetAllAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
               It.IsAny<Func<IQueryable<DataAccess.Entities.City>,
               IIncludableQueryable<DataAccess.Entities.City, object>>>()))
                .ReturnsAsync(new List<DataAccess.Entities.City>());
            _repoWrapper
                 .Setup(x => x.ClubMembers.GetAllAsync(It.IsAny<Expression<Func<ClubMembers, bool>>>(),
                It.IsAny<Func<IQueryable<ClubMembers>,
                IIncludableQueryable<ClubMembers, object>>>()))
                 .ReturnsAsync(new List<ClubMembers>());
            _repoWrapper
                .Setup(x => x.CityMembers.GetAllAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
               It.IsAny<Func<IQueryable<CityMembers>,
               IIncludableQueryable<CityMembers, object>>>()))
                .ReturnsAsync(new List<CityMembers>());
            _repoWrapper
                .Setup(x => x.AdminType.GetUserTableObjects(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(CreateTuple);
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(roles);
            _mapper
                .Setup(x => x.Map<User, ShortUserInformationDTO>(It.IsAny<User>()))
                .Returns(new ShortUserInformationDTO() { ID = Roles.Admin });

            // Act
            var result = await service.GetUsersTableAsync(new TableFilterParameters(){Page = 1, PageSize = 2, Cities = null, Regions = null, Clubs = null, Degrees = null, Tab = null, FilterRoles = null}, It.IsAny<string>());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<Tuple<IEnumerable<UserTableDTO>, int>>(result);
        }

        [TestCase]
        public async Task UsersTableAsync_NotNullInput_ReturnsIEnumerableUserTableDTO()
        {
            // Arrange
            _repoWrapper
                .Setup(x => x.User.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>(),
               It.IsAny<Func<IQueryable<User>,
               IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(new List<User>());
            _repoWrapper
                .Setup(x => x.City.GetAllAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
               It.IsAny<Func<IQueryable<DataAccess.Entities.City>,
               IIncludableQueryable<DataAccess.Entities.City, object>>>()))
                .ReturnsAsync(new List<DataAccess.Entities.City>());
            _repoWrapper
                 .Setup(x => x.ClubMembers.GetAllAsync(It.IsAny<Expression<Func<ClubMembers, bool>>>(),
                It.IsAny<Func<IQueryable<ClubMembers>,
                IIncludableQueryable<ClubMembers, object>>>()))
                 .ReturnsAsync(new List<ClubMembers>());
            _repoWrapper
                .Setup(x => x.CityMembers.GetAllAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
               It.IsAny<Func<IQueryable<CityMembers>,
               IIncludableQueryable<CityMembers, object>>>()))
                .ReturnsAsync(new List<CityMembers>());
            _repoWrapper
                .Setup(x => x.AdminType.GetUserTableObjects(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(CreateTuple);
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(roles);
            _mapper
                .Setup(x => x.Map<User, ShortUserInformationDTO>(It.IsAny<User>()))
                .Returns(new ShortUserInformationDTO() { ID = Roles.Admin });

            // Act
            var result = await service.GetUsersTableAsync(new TableFilterParameters()
            {
                Page = 1, PageSize = 2, Cities = new List<int> {1}, Regions = new List<int> {1},
                Clubs = new List<int> {1}, Degrees = new List<int> {1}, Tab = null, FilterRoles = null
            }, It.IsAny<string>());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<Tuple<IEnumerable<UserTableDTO>, int>>(result);
        }

        [Test]
        public async Task GetUsersAsync_ReturnsShortUserInformationDTO()
        {
            // Arrange
            _repoWrapper
                .Setup(x => x.User.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>,
                        IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(GetTestUsers());
            _repoWrapper
                .Setup(x => x.City.GetAllAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.City>,
                        IIncludableQueryable<DataAccess.Entities.City, object>>>()))
                .ReturnsAsync(GetTestCities());
            _repoWrapper
                .Setup(x => x.ClubMembers.GetAllAsync(It.IsAny<Expression<Func<ClubMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<ClubMembers>,
                        IIncludableQueryable<ClubMembers, object>>>()))
                .ReturnsAsync(GetTestClubMembers());
            _repoWrapper
                .Setup(x => x.CityMembers.GetAllAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<CityMembers>,
                        IIncludableQueryable<CityMembers, object>>>()))
                .ReturnsAsync(GetTestCityMembers());
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(roles);
            _mapper
                .Setup(x => x.Map<User, ShortUserInformationDTO>(It.IsAny<User>()))
                .Returns(new ShortUserInformationDTO() { ID = "Admin" });

            // Act
            var result = await service.GetUsersAsync();
            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<ShortUserInformationDTO>>(result);
        }

        [TestCase("searchString")]
        public async Task GetShortUserInfoAsync_ReturnsShortUsersInfo(string searchString)
        {
            // Arrange
            var users = GetTestUsers();
            _repoWrapper
                .Setup(x => x.User.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>,
                        IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(users);
            _mapper
                .Setup(x => x.Map<User, ShortUserInformationDTO>(It.IsAny<User>()))
                .Returns(new ShortUserInformationDTO());

            // Act
            var result = await service.GetShortUserInfoAsync(searchString);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<ShortUserInformationDTO>>(result);
            Assert.AreEqual(users.Count(), result.Count());
        }

        [Test]
        public async Task GetUsersCountAsync_ReturnsInt()
        {
            // Arrange
            _repoWrapper.Setup(x => x.AdminType.GetUsersCountAsync()).ReturnsAsync(It.IsAny<int>());

            // Act
            var result = await service.GetUsersCountAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<int>(result);
        }

        [Test]
        public async Task GetUsersByAllRolesAsync_ReturnsUsers()
        {
            //Arrange
            string[] userRole = new string[] { "Role1", "Role2" };
            string[] roles = new string[] { "Role2", "Role1" };
            var user = new User() { Id = "1" };
            _repoWrapper.Setup(x => x.User.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>,
                        IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(new List<User>() { user });
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(userRole);
            _mapper.Setup(x => x.Map<User, ShortUserInformationDTO>(It.IsAny<User>()))
                .Returns(new ShortUserInformationDTO() { ID = "1" });

            //Acts
            var res = (await service.GetUsersByRolesAsync(string.Join(",", roles), true, service.FilterByAllRoles)).ToList();

            //Assert
            Assert.AreEqual(res[0].ID, user.Id);
        }

        [Test]
        public async Task GetUsersByAnyRoleAsync_ReturnsUsers()
        {
            //Arrange
            string[] userRole = new string[] { "Role1", "Role2" };
            string[] roles = new string[] { "Role2" };
            var user = new User() { Id = "1" };
            _repoWrapper.Setup(x => x.User.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>,
                        IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(new List<User>() { user });
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(userRole);
            _mapper.Setup(x => x.Map<User, ShortUserInformationDTO>(It.IsAny<User>()))
                .Returns(new ShortUserInformationDTO() { ID = "1" });

            //Acts
            var res = (await service.GetUsersByRolesAsync(string.Join(",", roles), true, service.FilterByAnyRoles)).ToList();

            //Assert
            Assert.AreEqual(res[0].ID, user.Id);
        }

        [Test]
        public async Task GetUsersByExactRoles_ReturnsUsers()
        {
            //Arrange
            string[] userRole = new string[] { "Role1", "Role2" };
            string[] roles = new string[] { "Role2", "Role1" };
            var user = new User() { Id = "1" };
            _repoWrapper.Setup(x => x.User.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>,
                        IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(new List<User>() { user });
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(userRole);
            _mapper.Setup(x => x.Map<User, ShortUserInformationDTO>(It.IsAny<User>()))
                .Returns(new ShortUserInformationDTO() { ID = "1" });

            //Acts
            var res = (await service.GetUsersByRolesAsync(string.Join(",", roles), true, service.FilterByExactRoles)).ToList();

            //Assert
            Assert.AreEqual(res[0].ID, user.Id);
        }
        [Test]
        public async Task TableFilterParameters_byRole_OkrugaHead_ReturnsCorrect()
        {
           
            string[] roles = new string[] { Roles.OkrugaHead};
            AdminType adminType = new AdminType() { AdminTypeName = Roles.OkrugaHead };
            RegionAdministration regionAdministration = new RegionAdministration() { AdminType = adminType, RegionId = It.IsAny<int>() };
            ICollection<RegionAdministration> regionAdministrations = new List<RegionAdministration>() { regionAdministration };
            Region region = new Region { RegionAdministration = regionAdministrations, RegionName = "qwerty"};

            _repoWrapper
                .Setup(x => x.RegionAdministration.GetSingleAsync
                (
                    It.IsAny<Expression<Func<DataAccess.Entities.RegionAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.RegionAdministration>,
                    IIncludableQueryable<DataAccess.Entities.RegionAdministration, object>>>())
                )
                .ReturnsAsync(regionAdministration);
            _repoWrapper
              .Setup(x => x.Region.GetSingleAsync
              (
                  It.IsAny<Expression<Func<DataAccess.Entities.Region, bool>>>(),
                  It.IsAny<Func<IQueryable<DataAccess.Entities.Region>,
                  IIncludableQueryable<DataAccess.Entities.Region, object>>>())
              )
              .ReturnsAsync(region);
           
            // Act
            FilterTableParametersByRole result = await service.TableFilterParameters_byRole(roles, It.IsAny<string>());

            // Assert
            Assert.IsNotNull(result.Regions);
        }

        [Test]
        public async Task TableFilterParameters_byRole_CityHead_ReturnsCorrect()
        {
            string[] roles = new string[] { Roles.CityHead };
            CityMembers cityMembers = new CityMembers() { CityId = It.IsAny<int>() };
            ICollection<CityMembers> cityAdministrations = new List<CityMembers>() { cityMembers };
            DataAccess.Entities.City city = new DataAccess.Entities.City {  Name = "qwerty" };

            _repoWrapper
                .Setup(x => x.CityMembers.GetSingleAsync
                (
                    It.IsAny<Expression<Func<DataAccess.Entities.CityMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.CityMembers>,
                    IIncludableQueryable<DataAccess.Entities.CityMembers, object>>>())
                )
                .ReturnsAsync(cityMembers);
            _repoWrapper
              .Setup(x => x.City.GetSingleAsync
              (
                  It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
                  It.IsAny<Func<IQueryable<DataAccess.Entities.City>,
                  IIncludableQueryable<DataAccess.Entities.City, object>>>())
              )
              .ReturnsAsync(city);

            // Act
            FilterTableParametersByRole result = await service.TableFilterParameters_byRole(roles, It.IsAny<string>());

            // Assert
            Assert.IsNotNull(result.Cities);
        }

        [Test]
        public async Task TableFilterParameters_byRole_KurinHead_ReturnsCorrect()
        {

            string[] roles = new string[] { Roles.KurinHead };
            AdminType adminType = new AdminType() { AdminTypeName = Roles.KurinHead };
            ClubAdministration clubAdministration = new ClubAdministration() { AdminType = adminType, ClubId = It.IsAny<int>() };
            ICollection<ClubAdministration> clubAdministrations = new List<ClubAdministration>() { clubAdministration };
            DataAccess.Entities.Club club = new DataAccess.Entities.Club { ClubAdministration = clubAdministrations, Name = "qwerty" };

            _repoWrapper
                .Setup(x => x.ClubAdministration.GetSingleAsync
                (
                    It.IsAny<Expression<Func<DataAccess.Entities.ClubAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.ClubAdministration>,
                    IIncludableQueryable<DataAccess.Entities.ClubAdministration, object>>>())
                )
                .ReturnsAsync(clubAdministration);
            _repoWrapper
              .Setup(x => x.Club.GetSingleAsync
              (
                  It.IsAny<Expression<Func<DataAccess.Entities.Club, bool>>>(),
                  It.IsAny<Func<IQueryable<DataAccess.Entities.Club>,
                  IIncludableQueryable<DataAccess.Entities.Club, object>>>())
              )
              .ReturnsAsync(club);

            // Act
            FilterTableParametersByRole result = await service.TableFilterParameters_byRole(roles, It.IsAny<string>());

            // Assert
            Assert.IsNotNull(result.AndClubs);
        }
        [Test]
        public async Task TableFilterParameters_byRole_KurinHead_OkrugaHead_ReturnsCorrect()
        {

            string[] roles = new string[] { Roles.KurinHead, Roles.OkrugaHead };
            AdminType adminType = new AdminType() { AdminTypeName = Roles.KurinHead };
            ClubAdministration clubAdministration = new ClubAdministration() { AdminType = adminType, ClubId = It.IsAny<int>() };
            ICollection<ClubAdministration> clubAdministrations = new List<ClubAdministration>() { clubAdministration };
            DataAccess.Entities.Club club = new DataAccess.Entities.Club { ClubAdministration = clubAdministrations, Name = "qwerty" };

            _repoWrapper
                .Setup(x => x.ClubAdministration.GetSingleAsync
                (
                    It.IsAny<Expression<Func<DataAccess.Entities.ClubAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.ClubAdministration>,
                    IIncludableQueryable<DataAccess.Entities.ClubAdministration, object>>>())
                )
                .ReturnsAsync(clubAdministration);
            _repoWrapper
              .Setup(x => x.Club.GetSingleAsync
              (
                  It.IsAny<Expression<Func<DataAccess.Entities.Club, bool>>>(),
                  It.IsAny<Func<IQueryable<DataAccess.Entities.Club>,
                  IIncludableQueryable<DataAccess.Entities.Club, object>>>())
              )
              .ReturnsAsync(club);
            AdminType adminTypeOkrugaHead = new AdminType() { AdminTypeName = Roles.OkrugaHead };
            RegionAdministration regionAdministration = new RegionAdministration() { AdminType = adminTypeOkrugaHead, RegionId = It.IsAny<int>() };
            ICollection<RegionAdministration> regionAdministrations = new List<RegionAdministration>() { regionAdministration };
            Region region = new Region { RegionAdministration = regionAdministrations, RegionName = "qwerty" };

            _repoWrapper
                .Setup(x => x.RegionAdministration.GetSingleAsync
                (
                    It.IsAny<Expression<Func<DataAccess.Entities.RegionAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.RegionAdministration>,
                    IIncludableQueryable<DataAccess.Entities.RegionAdministration, object>>>())
                )
                .ReturnsAsync(regionAdministration);
            _repoWrapper
              .Setup(x => x.Region.GetSingleAsync
              (
                  It.IsAny<Expression<Func<DataAccess.Entities.Region, bool>>>(),
                  It.IsAny<Func<IQueryable<DataAccess.Entities.Region>,
                  IIncludableQueryable<DataAccess.Entities.Region, object>>>())
              )
              .ReturnsAsync(region);
            // Act
            FilterTableParametersByRole result = await service.TableFilterParameters_byRole(roles, It.IsAny<string>());

            // Assert
            Assert.IsNotNull(result.AndClubs);
            Assert.IsNotNull(result.Regions);
        }
        [Test]
        public void IsCityMember_ReturnsCorrect()
        {
            // Arrange
            CityMembers cityMember = new CityMembers() { UserId = "qwerty"};
            List<CityMembers> cityMembers = new List<CityMembers>(){ cityMember };
            _repoWrapper.Setup(x => x.CityMembers.GetAllAsync(
                It.IsAny<Expression<Func<DataAccess.Entities.CityMembers, bool>>>(),
                     It.IsAny<Func<IQueryable<DataAccess.Entities.CityMembers>,
                     IIncludableQueryable<DataAccess.Entities.CityMembers, object>>>())
                ).ReturnsAsync(cityMembers);

            // Act
            var result = service.IsCityMember("qwerty").Result;

            // Assert
            _formerMemberService.Verify();
            Assert.True(result);
        }
        [Test]
        public void IsCityMember_ReturnsFalse()
        {
            // Arrange
            
            List<CityMembers> cityMembers = new List<CityMembers>();
            _repoWrapper.Setup(x => x.CityMembers.GetAllAsync(
                It.IsAny<Expression<Func<DataAccess.Entities.CityMembers, bool>>>(),
                     It.IsAny<Func<IQueryable<DataAccess.Entities.CityMembers>,
                     IIncludableQueryable<DataAccess.Entities.CityMembers, object>>>())
                ).ReturnsAsync(cityMembers);

            // Act
            var result = service.IsCityMember("qwerty").Result;

            // Assert
            _formerMemberService.Verify();
            Assert.False(result);
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

        private IEnumerable<ClubMembers> GetTestClubMembers()
        {
            return new List<ClubMembers>
            {
                new  ClubMembers
                {
                    UserId = "UserId1",
                    IsApproved = true,
                    Club = new DataAccess.Entities.Club
                    {
                        Name = "ClubName"
                    }
                }
            }.AsEnumerable();
        }

        private IEnumerable<CityMembers> GetTestCityMembers()
        {
            return new List<CityMembers>
            {
                new  CityMembers
                {
                    UserId = "UserId1",
                    IsApproved = true,
                    City = new DataAccess.Entities.City
                    {
                        Name = "CityName"
                    }
                }
            }.AsEnumerable();
        }

        private IEnumerable<DataAccess.Entities.City> GetTestCities()
        {
            return new List<DataAccess.Entities.City>
            {
                new  DataAccess.Entities.City
                {
                    Name = "CityName",
                    Region = new Region
                    {
                        RegionName = "RegionName"
                    }
                }
            }.AsEnumerable();
        }

        private Tuple<IEnumerable<UserTableObject>, int> CreateTuple => new Tuple<IEnumerable<UserTableObject>, int>(CreateUserTableObjects, 100);
        private IEnumerable<UserTableObject> CreateUserTableObjects => new List<UserTableObject>()
        {
            new UserTableObject(),
            new UserTableObject()
        };

    }
}
