using EPlast.BLL.Interfaces;
using EPlast.BLL.Services;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Models;

namespace EPlast.Tests.Services.EmailSending
{
    public class NewPlastMemberEmailGreetingTests
    {
        private Mock<IEmailSendingService> _mockEmailSendingService;
        private Mock<IEmailContentService> _mockEmailContentService;
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<UserManager<User>> _mockUserManager;
        private INewPlastMemberEmailGreetingService _newPlastMemberEmailGreetingService;

        [Test]
        public async Task NotifyNewPlastMembersAsync_Valid_Test()
        {
            // Arrange
            List<User> users = new List<User>()
            {
                new User()
                {
                    RegistredOn = DateTime.Now.Date.Subtract(new TimeSpan(366, 0, 0, 0))
                }
            };
            List<string> roles = new List<string>()
            {
                "Прихильник",
                "Колишній член пласту"
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
            _mockEmailSendingService
                .Setup(x => x.SendEmailAsync(It.IsAny<string>(),
                                             It.IsAny<string>(),
                                             It.IsAny<string>(),
                                             It.IsAny<string>()))
                .ReturnsAsync(true);
            _mockEmailContentService.Setup(x => x.GetGreetingForNewPlastMemberEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new EmailModel());

            // Act
            await _newPlastMemberEmailGreetingService.NotifyNewPlastMembersAsync();

            // Assert
            _mockRepoWrapper.Verify();
            _mockUserManager.Verify();
            _mockEmailSendingService.Verify();
        }

        [Test]
        public async Task NotifyNewPlastMembersAsync_Valid_Empty_Test()
        {
            // Arrange
            List<User> users = new List<User>()
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
                .ReturnsAsync(new EmailModel());

            // Act
            await _newPlastMemberEmailGreetingService.NotifyNewPlastMembersAsync();

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
            _newPlastMemberEmailGreetingService = new NewPlastMemberEmailGreetingService(_mockRepoWrapper.Object,
                                                                                         _mockUserManager.Object,
                                                                                         _mockEmailSendingService.Object,
                                                                                         _mockEmailContentService.Object);
        }
    }
}
