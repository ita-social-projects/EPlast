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
        private readonly List<string> roles = new List<string>()
        {
            Roles.Supporter,
            Roles.FormerPlastMember
        };
        private EmailReminderService emailReminderService;
        private Mock<IAuthEmailService> mockAuthEmailServices;
        private Mock<IMapper> mockMapper;
        private Mock<IRepositoryWrapper> mockRepoWrapper;
        private Mock<UserManager<User>> mockUserManager;

        [Test]
        public async Task JoinCityReminderAsync_InValid_Execept_Test()
        {
            // Arrange
            List<User> users = new List<User>();
            users.Add(new User());
            mockRepoWrapper
                .Setup(x => x.User.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>(),
               It.IsAny<Func<IQueryable<User>,
               IIncludableQueryable<User, object>>>()))
                .Throws(new Exception());
            mockRepoWrapper
                .Setup(x => x.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                It.IsAny<Func<IQueryable<CityMembers>,
                IIncludableQueryable<CityMembers, object>>>()))
                .ReturnsAsync(new CityMembers() { City = new DataAccess.Entities.City() });
            mockUserManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(roles);

            // Act
            var result = await emailReminderService.JoinCityReminderAsync();

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task JoinCityReminderAsync_Valid_Test()
        {
            // Arrange
            List<User> users = new List<User>();
            users.Add(new User());
            mockRepoWrapper
                .Setup(x => x.User.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>(),
               It.IsAny<Func<IQueryable<User>,
               IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(users);
            mockRepoWrapper
                .Setup(x => x.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                It.IsAny<Func<IQueryable<CityMembers>,
                IIncludableQueryable<CityMembers, object>>>()))
                .ReturnsAsync(new CityMembers() { City = new DataAccess.Entities.City() });
            mockUserManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(roles);

            // Act
            var result = await emailReminderService.JoinCityReminderAsync();

            // Assert
            Assert.IsTrue(result);
        }

        [SetUp]
        public void SetUp()
        {
            mockRepoWrapper = new Mock<IRepositoryWrapper>();
            mockAuthEmailServices = new Mock<IAuthEmailService>();
            mockMapper = new Mock<IMapper>();
            var store = new Mock<IUserStore<User>>();
            mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            emailReminderService = new EmailReminderService(mockRepoWrapper.Object,
                mockAuthEmailServices.Object,
                mockMapper.Object,
                mockUserManager.Object);
        }
    }
}
