using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.Notification;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Notifications;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Models;
using EPlast.BLL.Queries.City;
using EPlast.BLL.Services;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services.EmailSending
{
    public class NewPlastMemberEmailGreetingTests
    {
        private Mock<IEmailSendingService> _mockEmailSendingService;
        private Mock<IEmailContentService> _mockEmailContentService;
        private Mock<INotificationService> _mockNotificationService;
        private Mock<IMediator> _mockMediator;
        private Mock<IUserService> _mockUserService;
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<UserManager<User>> _mockUserManager;
        private INewPlastMemberEmailGreetingService _newPlastMemberEmailGreetingService;

        [Test]
        public async Task NotifyNewPlastMembersAndCityAdminsAsync_Valid_Test()
        {
            // Arrange
            var users = new List<User>()
            {
                new User()
                {
                    RegistredOn = DateTime.Now.Date.Subtract(new TimeSpan(366, 0, 0, 0))
                }
            };

            var user = new UserDto
            {
                UserProfile = new UserProfileDto
                {
                    Birthday = DateTime.Now
                },

                CityMembers = new[]
                {
                    new CityMembers
                    {
                        City = new DataAccess.Entities.City
                        {
                            Name = "CityName"
                        }
                    }
                }
            };

            var cityAdministration = new CityAdministrationViewModelDto
            {
                Administration = new List<CityAdministrationDto>(),
                Head = new CityAdministrationDto
                {
                    User = new CityUserDto()
                },
                HeadDeputy = new CityAdministrationDto
                {
                    User = new CityUserDto()
                }
            };
               
            var roles = new List<string>()
            {
                Roles.Supporter,
                Roles.FormerPlastMember
            };

            var confirmedUsers = new ConfirmedUser[]
            {
                new ConfirmedUser()
                {
                    isClubAdmin = true
                }
            }.AsQueryable<ConfirmedUser>();
            _mockRepoWrapper
                    .Setup(x => x.User.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>(),
                                                   It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                    .ReturnsAsync(users);
            _mockUserManager
                .Setup((x) => x.IsInRoleAsync(It.IsAny<User>(),
                                              It.IsAny<string>()))
                .ReturnsAsync(false);
            _mockUserManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(roles);
            _mockRepoWrapper
                .Setup(r => r.ConfirmedUser.FindByCondition(It.IsAny<Expression<Func<ConfirmedUser, bool>>>()))
                .Returns(confirmedUsers);
            _mockUserManager
                .Setup(u => u.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _mockUserManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<User>(),
                                             It.IsAny<string>()));
            _mockUserService.Setup(x => x.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _mockMediator.Setup(x => x.Send(It.IsAny<GetCityAdminsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(cityAdministration);
            
            _mockEmailSendingService
                .Setup(x => x.SendEmailAsync(It.IsAny<string>(),
                                             It.IsAny<string>(),
                                             It.IsAny<string>(),
                                             It.IsAny<string>()))
                .ReturnsAsync(true);
            _mockEmailContentService.Setup(x => x.GetGreetingForNewPlastMemberEmailAsync(It.IsAny<string>()))
                .Returns(new EmailModel());
            _mockEmailContentService
                .Setup(x => x.GetCityAdminAboutNewPlastMemberEmail(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<DateTime>())).Returns(new EmailModel());
            _mockEmailContentService.Setup(x => x.GetGreetingForNewPlastMemberMessageAsync(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new UserNotification());
            _mockMediator.Setup(x => x.Send(It.IsAny<GetCityIdByUserIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(240);

            _mockNotificationService.Setup(x => x.GetAllNotificationTypesAsync()).ReturnsAsync(FakeTypeId());
            _mockEmailContentService.Setup(x => x.GetGreetingForNewPlastMemberMessageAsync(
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new UserNotification());
            _mockRepoWrapper.Setup(x => x.UserNotifications.CreateAsync(It.IsAny<UserNotification>()));

            // Act
            await _newPlastMemberEmailGreetingService.NotifyNewPlastMembersAndCityAdminsAsync();

            // Assert
            _mockRepoWrapper.Verify();
            _mockUserManager.Verify();
            _mockEmailSendingService.Verify();
        }

        [Test]
        public async Task NotifyNewPlastMembersAndCityAdminsAsync_Valid_Empty_Test()
        {
            // Arrange
            var users = new List<User>()
            {
                new User()
                {
                    RegistredOn = DateTime.Now.Date.Subtract(new TimeSpan(366, 0, 0, 0))
                }
            };
            _mockRepoWrapper
                    .Setup(x => x.User.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>(),
                                                   It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                    .ReturnsAsync(users);
            _mockUserManager
                .Setup((x) => x.IsInRoleAsync(It.IsAny<User>(),
                                              It.IsAny<string>()))
                .ReturnsAsync(true);
            _mockEmailSendingService
                .Setup(x => x.SendEmailAsync(It.IsAny<string>(),
                                             It.IsAny<string>(),
                                             It.IsAny<string>(),
                                             It.IsAny<string>()))
                .ReturnsAsync(true);
            _mockEmailContentService.Setup(x => x.GetGreetingForNewPlastMemberEmailAsync(It.IsAny<string>()))
                .Returns(new EmailModel());

            _mockEmailContentService.Setup(x => x.GetGreetingForNewPlastMemberMessageAsync(It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(), It.IsAny<int>())).Returns(new UserNotification());

            // Act
            await _newPlastMemberEmailGreetingService.NotifyNewPlastMembersAndCityAdminsAsync();

            // Assert
            _mockRepoWrapper.Verify();
            _mockUserManager.Verify();
            _mockEmailSendingService.Verify();
        }

        [SetUp]
        public void SetUp()
        {
            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockEmailSendingService = new Mock<IEmailSendingService>();
            _mockEmailContentService = new Mock<IEmailContentService>();
            _mockNotificationService = new Mock<INotificationService>();
            _mockMediator = new Mock<IMediator>();
            _mockUserService = new Mock<IUserService>();
            _newPlastMemberEmailGreetingService = new NewPlastMemberEmailGreetingService(_mockRepoWrapper.Object,
                                                                                         _mockUserManager.Object,
                                                                                         _mockEmailSendingService.Object,
                                                                                         _mockEmailContentService.Object,
                                                                                         _mockNotificationService.Object,
                                                                                         _mockMediator.Object,
                                                                                         _mockUserService.Object);
        }

        private static List<NotificationTypeDto> FakeTypeId()
        {
            return new List<NotificationTypeDto>
            {
                new NotificationTypeDto()
                {
                    Id = 0
                },
                new NotificationTypeDto()
                {
                    Id = 1
                },
                new NotificationTypeDto()
                {
                    Id = 2
                }
            };
        }
    }
}
