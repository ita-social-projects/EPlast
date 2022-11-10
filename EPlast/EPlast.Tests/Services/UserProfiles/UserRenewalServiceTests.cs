using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Models;
using EPlast.BLL.Queries.City;
using EPlast.BLL.Services.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services.UserProfiles
{
    class UserRenewalServiceTests
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<IMapper> _mockMapper;
        private Mock<IMediator> _mockMediator;
        private Mock<ICityParticipantsService> _mockCityParticipantsService;
        private Mock<IEmailSendingService> _mockEmailSendingService;
        private Mock<IEmailContentService> _mockEmailContentService;
        private Mock<UserManager<User>> _mockUserManager;
        private Mock<IUserStore<User>> _mockUser;
        private UserRenewalService _userRenewalService;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockMediator = new Mock<IMediator>();
            _mockCityParticipantsService = new Mock<ICityParticipantsService>();
            _mockEmailSendingService = new Mock<IEmailSendingService>();
            _mockEmailContentService = new Mock<IEmailContentService>();
            _mockUser = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(_mockUser.Object, null, null, null, null, null, null, null, null);
            _userRenewalService = new UserRenewalService(_mockRepoWrapper.Object, _mockMapper.Object, _mockMediator.Object,
                _mockCityParticipantsService.Object, _mockEmailSendingService.Object, _mockEmailContentService.Object, _mockUserManager.Object);
        }

        [Test]
        public void GetUserRenewalsForTable_ReturnsUserRenewalsTableObject()
        {
            //Arrange
            _mockRepoWrapper
                .Setup(r => r.UserRenewal.GetUserRenewals(It.IsAny<string>(), 
                    It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new List<UserRenewalsTableObject>());

            //Act
            var result = _userRenewalService.GetUserRenewalsTableObject(It.IsAny<string>(),
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>());

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<List<UserRenewalsTableObject>>(result);
        }

        [Test]
        public async Task ChangeUserRenewal_ChangesTargetUserRenewalAsync()
        {
            //Arrange
            _mockRepoWrapper
                .Setup(r => r.UserRenewal.GetFirstAsync(It.IsAny<Expression<Func<UserRenewal, bool>>>(),
                    It.IsAny<Func<IQueryable<UserRenewal>, IIncludableQueryable<UserRenewal, object>>>()))
                .ReturnsAsync(new UserRenewal());

            //Act
            await _userRenewalService.ChangeUserRenewalAsync(userRenewalDTO);

            //Assert
            _mockRepoWrapper.Verify();
        }

        [Test]
        public async Task AddUserRenewal_AddsNewRenewalAsync()
        {
            //Arrange
            _mockRepoWrapper
                .Setup(r => r.UserRenewal.CreateAsync(It.IsAny<UserRenewal>()));

            //Act
            await _userRenewalService.AddUserRenewalAsync(userRenewalDTO);
            
            //Assert
            _mockRepoWrapper.Verify();
        }

        [Test]
        public async Task IsValidUserRenewal_ValidAsync()
        {
            //Arrange
            _mockUserManager
                .Setup(u => u.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _mockUserManager
                .Setup(u => u.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            var renewals = GetAllRenewalsForUserByIdTest(userRenewalDTO.UserId);
            _mockRepoWrapper
                .Setup(r => r.UserRenewal.GetAllAsync(It.IsAny<Expression<Func<UserRenewal, bool>>>(),
                    It.IsAny<Func<IQueryable<UserRenewal>, IIncludableQueryable<UserRenewal, object>>>()))
                .ReturnsAsync(renewals);

            //Act
            var result = await _userRenewalService.IsValidUserRenewalAsync(userRenewalDTO);

            //Assert
            Assert.IsTrue(result);
        }
        
        [Test]
        public async Task IsValidUserRenewal_NotValidRoleAsync()
        {
            //Arrange
            _mockUserManager
                .Setup(u => u.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _mockUserManager
                .Setup(u => u.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(false);
            var renewals = GetAllRenewalsForUserByIdTest(invalidUserRenewalDTO.UserId);
            _mockRepoWrapper
                .Setup(r => r.UserRenewal.GetAllAsync(It.IsAny<Expression<Func<UserRenewal, bool>>>(),
                    It.IsAny<Func<IQueryable<UserRenewal>, IIncludableQueryable<UserRenewal, object>>>()))
                .ReturnsAsync(renewals);

            //Act
            var result = await _userRenewalService.IsValidUserRenewalAsync(userRenewalDTO);

            //Assert
            Assert.IsFalse(result);
        }
        
        [Test]
        public async Task IsValidUserRenewal_NotValidRenewalAsync()
        {
            //Arrange
            _mockUserManager
                .Setup(u => u.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _mockUserManager
                .Setup(u => u.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            var renewals = GetAllRenewalsForUserByIdTest(invalidUserRenewalDTO.UserId);
            _mockRepoWrapper
                .Setup(r => r.UserRenewal.GetAllAsync(It.IsAny<Expression<Func<UserRenewal, bool>>>(),
                    It.IsAny<Func<IQueryable<UserRenewal>, IIncludableQueryable<UserRenewal, object>>>()))
                .ReturnsAsync(renewals);

            //Act
            var result = await _userRenewalService.IsValidUserRenewalAsync(invalidUserRenewalDTO);

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task IsValidAdmin_ValidAsync()
        {
            //Arrange
            _mockUserManager
                .Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(GetRoles());
            _mockMediator.Setup(x => x.Send(It.IsAny<GetCityAdminsIdsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync("Admin1,Admin2");

            //Act
            var result = await _userRenewalService.IsValidAdminAsync(It.IsAny<User>(), It.IsAny<int>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task IsValidAdmin_NotValidAsync()
        {
            //Arrange
            _mockUserManager
                .Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(GetNoAdminRoles);
            _mockMediator.Setup(x => x.Send(It.IsAny<GetCityAdminsIdsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync("Admin1,Admin2");


            //Act
            var result = await _userRenewalService.IsValidAdminAsync(notCityAdmin, It.IsAny<int>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task IsValidAdmin_ValidCityAdminAsync()
        {
            //Arrange
            _mockUserManager
                .Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(GetNoAdminRoles);
            _mockMediator.Setup(x => x.Send(It.IsAny<GetCityAdminsIdsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync("Admin1,Admin2");

            //Act
            var result = await _userRenewalService.IsValidAdminAsync(cityAdmin, It.IsAny<int>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task SendRenewalConfirmationEmail_SendsEmailAsync()
        {
            //Arrange
            _mockUserManager
                .Setup(u => u.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _mockMediator.Setup(x => x.Send(It.IsAny<GetCityByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new CityDto());

            _mockEmailContentService
                .Setup(e => e.GetUserRenewalConfirmationEmail(It.IsAny<string>()))
                .Returns(new EmailModel());
            _mockEmailSendingService
                .Setup(e => e.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>()));
            //Act
            await _userRenewalService.SendRenewalConfirmationEmailAsync(It.IsAny<string>(), It.IsAny<int>());

            //Assert
            _mockUserManager.Verify();
            _mockMediator.Verify();
            _mockEmailContentService.Verify();
            _mockEmailSendingService.Verify();
        }

        [Test]
        public async Task RenewFormerMemberUser_RenewsFormerMemberAsync()
        {
            //Arrange
            _mockRepoWrapper
                .Setup(u => u.UserMembershipDates.GetFirstAsync(It.IsAny<Expression<Func<UserMembershipDates, bool>>>(),
                    It.IsAny<Func<IQueryable<UserMembershipDates>, IIncludableQueryable<UserMembershipDates, object>>>()))
                .ReturnsAsync(new UserMembershipDates());
            _mockRepoWrapper
                .Setup(r => r.UserRenewal.GetFirstAsync(It.IsAny<Expression<Func<UserRenewal, bool>>>(),
                    It.IsAny<Func<IQueryable<UserRenewal>, IIncludableQueryable<UserRenewal, object>>>()))
                .ReturnsAsync(new UserRenewal());
            _mockRepoWrapper
                .Setup(u => u.UserFormerMembershipDates.GetFirstAsync(It.IsAny<Expression<Func<UserFormerMembershipDates, bool>>>(),
                        It.IsAny<Func<IQueryable<UserFormerMembershipDates>, IIncludableQueryable<UserFormerMembershipDates, object>>>()))
                    .ReturnsAsync(new UserFormerMembershipDates());
            _mockUserManager
                .Setup(u => u.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _mockUserManager
                .Setup(u => u.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            _mockUserManager
                .Setup(u => u.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
            _mockUserManager
                .Setup(u => u.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
            _mockCityParticipantsService
                .Setup(c => c.AddFollowerAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(new CityMembersDto());

            //Act
            await _userRenewalService.ResolveUserMembershipDatesAsync(It.IsAny<string>());
            var result = await _userRenewalService.RenewFormerMemberUserAsync(userRenewalDTO);
            await _userRenewalService.ChangeUserRenewalAsync(userRenewalDTO);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CityMembersDto>(result);
        }

        [Test]
        public void RenewFormerMemberUser_ThrowsArgumentException()
        {
            //Arrange
            _mockUserManager
                .Setup(u => u.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _mockUserManager
                .Setup(u => u.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            //Assert
            Assert.ThrowsAsync<ArgumentException>(
                async ()=>{await _userRenewalService.RenewFormerMemberUserAsync(userRenewalDTO);});
        }

        [Test]
        public async Task ResolveMembershipDates_ResolvesDatesAsync()
        {
            //Arrange
            _mockRepoWrapper
                .Setup(u => u.UserMembershipDates.GetFirstAsync(It.IsAny<Expression<Func<UserMembershipDates, bool>>>(),
                    It.IsAny<Func<IQueryable<UserMembershipDates>, IIncludableQueryable<UserMembershipDates, object>>>()))
                .ReturnsAsync(new UserMembershipDates());
            
            //Act
            await _userRenewalService.ResolveUserMembershipDatesAsync(It.IsAny<string>());
            
            //Assert
            _mockRepoWrapper.Verify();
            
        }

        private readonly UserRenewalDto invalidUserRenewalDTO = new UserRenewalDto
        {
            Id = 5,
            CityId = 13,
            Approved = true,
            UserId = "470a790f-3d8d-42ac-b763-5e6eac673d69",
            RequestDate = DateTime.Now
        };

        private readonly UserRenewalDto userRenewalDTO = new UserRenewalDto
        {
            Id = 1,
            CityId = 13,
            Approved = true,
            UserId = "660c4f26-760d-46f9-b780-5b5c7c153b25",
            RequestDate = new DateTime()
        };

        private static IEnumerable<UserRenewal> GetAllRenewalsForUserByIdTest(string id)
        {
            return new List<UserRenewal>
            {
                new UserRenewal
                {
                    Id = 2, 
                    CityId = 13, 
                    Approved = true, 
                    UserId = "470a790f-3d8d-42ac-b763-5e6eac673d69", 
                    RequestDate = new DateTime()
                },
                new UserRenewal
                {
                    Id = 3,
                    CityId = 13,
                    Approved = true,
                    UserId = "470a790f-3d8d-42ac-b763-5e6eac673d69",
                    RequestDate = DateTime.Now
                },
                new UserRenewal
                {
                    Id = 4, 
                    CityId = 13, 
                    Approved = false, 
                    UserId = "660c4f26-760d-46f9-b780-5b5c7c153b25", 
                    RequestDate = new DateTime()
                }
            }.Where(u=>u.UserId==id).ToList().AsEnumerable();
        }

        private readonly User cityAdmin = new User()
        {
            Id = "Admin1"
        };

        private readonly User notCityAdmin = new User()
        {
            Id = "NoAdmin"
        };

        private List<string> GetRoles()
        {
            return new List<string>
            {
                Roles.Admin,
                "Role1",
                "Role2"
            };
        }

        private List<string> GetNoAdminRoles()
        {
            return new List<string>
            {
                "Role1",
                "Role2"
            };
        }
    }
}