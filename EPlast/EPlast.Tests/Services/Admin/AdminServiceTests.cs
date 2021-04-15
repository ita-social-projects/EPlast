using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.Region;
using EPlast.BLL.Services;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
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
using EPlast.Resources;
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
        private Mock<ICityParticipantsService> _cityParticipantsService;
        private Mock<IClubParticipantsService> _clubParticipants;
        private Mock<IMapper> _mapper;
        private Mock<IRegionAdministrationService> _regionService;
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<RoleManager<IdentityRole>> _roleManager;
        private Mock<IRoleStore<IdentityRole>> _store;
        private Mock<IUserStore<User>> _user;
        private Mock<UserManager<User>> _userManager;
        private AdminService service;

        [Test]
        public void ChangeAsync_ReturnsCorrect()
        {
            // Arrange
            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User() { FirstName = "James", LastName = "Bond" });
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>());
            _repoWrapper
                .Setup(x => x.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                It.IsAny<Func<IQueryable<CityMembers>,
                IIncludableQueryable<CityMembers, object>>>()))
                .ReturnsAsync(new CityMembers());
            _cityParticipantsService
                .Setup(x => x.RemoveMemberAsync(It.IsAny<CityMembers>()));
            _repoWrapper
               .Setup(x => x.ClubMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubMembers, bool>>>(),
               It.IsAny<Func<IQueryable<ClubMembers>,
               IIncludableQueryable<ClubMembers, object>>>()))
               .ReturnsAsync(new ClubMembers());
            _clubParticipants
                .Setup(x => x.RemoveMemberAsync(It.IsAny<ClubMembers>()));
            _repoWrapper
               .Setup(x => x.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
               It.IsAny<Func<IQueryable<RegionAdministration>,
               IIncludableQueryable<RegionAdministration, object>>>()))
               .ReturnsAsync(new RegionAdministration());
            _regionService
                .Setup(x => x.DeleteAdminByIdAsync(It.IsAny<int>()));
            _userManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<User>(), Roles.FormerPlastMember));

            // Act
            var result = service.ChangeAsync(It.IsAny<string>());

            // Assert
            _userManager.Verify();
            _repoWrapper.Verify();
            _cityParticipantsService.Verify();
            _clubParticipants.Verify();
            Assert.NotNull(result);
        }

        [Test]
        public void ChangeAsync_Valid_FewRoles_Test()
        {
            // Arrange
            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User() { FirstName = "James", LastName = "Bond" });
            roles.Add("First role");
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(roles);
            _userManager
                .Setup(x => x.RemoveFromRolesAsync(It.IsAny<User>(),
                                                   It.IsAny<IEnumerable<string>>()));
            _repoWrapper
                .Setup(x => x.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                It.IsAny<Func<IQueryable<CityMembers>,
                IIncludableQueryable<CityMembers, object>>>()))
                .ReturnsAsync(new CityMembers());
            _cityParticipantsService
                .Setup(x => x.RemoveMemberAsync(It.IsAny<CityMembers>()));
            _repoWrapper
               .Setup(x => x.ClubMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubMembers, bool>>>(),
               It.IsAny<Func<IQueryable<ClubMembers>,
               IIncludableQueryable<ClubMembers, object>>>()))
               .ReturnsAsync(new ClubMembers());
            _clubParticipants
                .Setup(x => x.RemoveMemberAsync(It.IsAny<ClubMembers>()));
            _repoWrapper
               .Setup(x => x.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
               It.IsAny<Func<IQueryable<RegionAdministration>,
               IIncludableQueryable<RegionAdministration, object>>>()))
               .ReturnsAsync(new RegionAdministration());
            _regionService
                .Setup(x => x.DeleteAdminByIdAsync(It.IsAny<int>()));
            _userManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<User>(), "Колишній член пласту"));

            // Act
            var result = service.ChangeAsync(It.IsAny<string>());

            // Assert
            _userManager.Verify();
            _repoWrapper.Verify();
            _cityParticipantsService.Verify();
            _clubParticipants.Verify();
            Assert.NotNull(result);
        }

        [Test]
        public async Task ChangeCurrentRoleAsync_AddInterested_CaseFormer_ReturnsCorrectAsync()
        {
            // Arrange
            string plastun = Roles.PlastMember;
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
            _repoWrapper
               .Setup(x => x.ClubMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubMembers, bool>>>(),
               It.IsAny<Func<IQueryable<ClubMembers>,
               IIncludableQueryable<ClubMembers, object>>>()))
               .ReturnsAsync(new ClubMembers());
            _repoWrapper
               .Setup(x => x.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
               It.IsAny<Func<IQueryable<RegionAdministration>,
               IIncludableQueryable<RegionAdministration, object>>>()))
               .ReturnsAsync(new RegionAdministration());
            _regionService
                .Setup(x => x.DeleteAdminByIdAsync(It.IsAny<int>()));
            _userManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()));

            // Act
            await service.ChangeCurrentRoleAsync("id", formerMember);
            // Assert
            _userManager.Verify();
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
        public async Task GetCityRegionAdminsOfUserAsync_ReturnsCorrect()
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
            _cityParticipantsService = new Mock<ICityParticipantsService>();
            _clubParticipants = new Mock<IClubParticipantsService>();
            _regionService = new Mock<IRegionAdministrationService>();

            service = new AdminService(
                _repoWrapper.Object,
                _userManager.Object,
                _mapper.Object,
                _roleManager.Object,
                _clubParticipants.Object,
                _regionService.Object,
                _cityParticipantsService.Object
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
        public async Task UsersTableAsync_ReturnsIEnumerableUserTableDTO()
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
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),It.IsAny<string>())).ReturnsAsync(CreateTuple);
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(roles);
            _mapper
                .Setup(x => x.Map<User, ShortUserInformationDTO>(It.IsAny<User>()))
                .Returns(new ShortUserInformationDTO() { ID = Roles.Admin });

            // Act
            var result = await service.GetUsersTableAsync(1, 2, null, null, null, null, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<Tuple<IEnumerable<UserTableDTO>, int>>(result);
        }

        [Test]
        public async Task GetUsersAsync_ReturnsIEnumerableUserTableDTO()
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
            Assert.IsInstanceOf<IEnumerable<UserTableDTO>>(result);
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
                    UserPlastDegrees = new List<UserPlastDegree>
                    {
                        new UserPlastDegree
                        {
                            PlastDegree = new PlastDegree()
                        }
                    }

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
                    UserPlastDegrees = new List<UserPlastDegree>
                    {
                        new UserPlastDegree
                        {
                            PlastDegree = new PlastDegree()
                        }
                    }

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
