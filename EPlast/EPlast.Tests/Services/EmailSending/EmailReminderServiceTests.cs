using AutoMapper;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Services.EmailSending;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EPlast.Tests.Services.EmailSending
{
    public class EmailReminderServiceTests
    {
        private readonly List<string> _roles = new List<string>()
        {
            Roles.Supporter,
            Roles.FormerPlastMember
        };
        private EmailReminderService _emailReminderService;
        private Mock<IEmailContentService> _mockEmailContentService;
        private Mock<IEmailSendingService> _mockEmailSendingService;
        private Mock<IAuthEmailService> _mockAuthEmailServices;
        private Mock<IMapper> _mockMapper;
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<UserManager<User>> _mockUserManager;

        [Test]
        public async Task JoinCityReminderAsync_InValid_Execept_Test()
        {
            // Arrange
            List<User> users = new List<User>();
            users.Add(new User());
            _mockRepoWrapper
                .Setup(x => x.User.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>(),
               It.IsAny<Func<IQueryable<User>,
               IIncludableQueryable<User, object>>>()))
                .Throws(new Exception());
            _mockRepoWrapper
                .Setup(x => x.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                It.IsAny<Func<IQueryable<CityMembers>,
                IIncludableQueryable<CityMembers, object>>>()))
                .ReturnsAsync(new CityMembers() { City = new DataAccess.Entities.City() });
            _mockUserManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(_roles);

            // Act
            var result = await _emailReminderService.JoinCityReminderAsync();

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task JoinCityReminderAsync_Valid_Test()
        {
            // Arrange
            List<User> users = new List<User>();
            users.Add(new User());
            _mockRepoWrapper
                .Setup(x => x.User.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>(),
               It.IsAny<Func<IQueryable<User>,
               IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(users);
            _mockRepoWrapper
                .Setup(x => x.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                It.IsAny<Func<IQueryable<CityMembers>,
                IIncludableQueryable<CityMembers, object>>>()))
                .ReturnsAsync(new CityMembers() { City = new DataAccess.Entities.City() });
            _mockUserManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(_roles);

            // Act
            var result = await _emailReminderService.JoinCityReminderAsync();

            // Assert
            Assert.IsTrue(result);
        }

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockAuthEmailServices = new Mock<IAuthEmailService>();
            _mockEmailContentService = new Mock<IEmailContentService>();
            _mockEmailSendingService = new Mock<IEmailSendingService>();
            _mockMapper = new Mock<IMapper>();
            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            _emailReminderService = new EmailReminderService(_mockRepoWrapper.Object,
                _mockAuthEmailServices.Object,
                _mockEmailContentService.Object,
                _mockEmailSendingService.Object,
                _mockMapper.Object,
                _mockUserManager.Object);
        }
    }
}
