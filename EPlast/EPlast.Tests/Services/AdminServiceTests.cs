using AutoMapper;
using EPlast.BLL.DTO;
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
using System.Threading.Tasks;

namespace EPlast.Tests.Services
{
    internal class AdminServiceTests
    {
        private readonly List<string> roles = new List<string>()
        {
            "Admin",
            "Прихильник",
            "Колишній член пласту"
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
        public void ChangeCurrentRoleAsync_AddPlastun_ReturnsCorrect()
        {
            // Arrange
            string plastun = "Пластун";
            string admin = "Admin";
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
            string supporter = "Прихильник";
            string admin = "Admin";
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
        public void EditAsync_ReturnsCorrect()
        {
            // Arrange
            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User() { FirstName = "James", LastName = "Bond" });
            _userManager
                .Setup(x => x.GetRolesAsync(new User() { FirstName = "James", LastName = "Bond" }))
                .ReturnsAsync(new List<string>() { "Прихильник" });
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
        public async Task GetCityRegionAdminsOfUser_ReturnsCorrect()
        {
            // Arrange
            _repoWrapper
                .Setup(x => x.City.GetAllAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
               It.IsAny<Func<IQueryable<DataAccess.Entities.City>,
               IIncludableQueryable<DataAccess.Entities.City, object>>>()))
                .ReturnsAsync(new List<DataAccess.Entities.City>());
            _mapper
                .Setup(x => x.Map<IEnumerable<DataAccess.Entities.City>, IEnumerable<BLL.DTO.City.CityDTO>>(new List<DataAccess.Entities.City>()));
            _mapper
                .Setup(x => x.Map<IEnumerable<RegionAdministration>, IEnumerable<RegionAdministrationDTO>>(new List<RegionAdministration>()));
            // Act
            var result = await service.GetCityRegionAdminsOfUser(It.IsAny<string>());
            // Assert
            Assert.IsInstanceOf<IEnumerable<BLL.DTO.City.CityDTO>>(result);
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
            var role = "Зацікавлений";
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
            var role = "Пластун";
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
            var role = "Прихильник";
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
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(roles);
            _mapper
                .Setup(x => x.Map<User, ShortUserInformationDTO>(It.IsAny<User>()))
                .Returns(new ShortUserInformationDTO() { ID = "Admin" });

            // Act
            var result = await service.UsersTableAsync();
            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<UserTableDTO>>(result);
        }
    }
}
