using AutoMapper;
using Moq;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using System;
using EPlast.BLL.Services.ActiveMembership;
using NUnit.Framework;
using System.Threading.Tasks;
using EPlast.BLL.DTO.ActiveMembership;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query;
using EPlast.BLL.DTO.UserProfiles;

namespace EPlast.Tests.Services.ActiveMembership
{
    public class UserDatesServiceTests
    {
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IRepositoryWrapper> _repoWrapper;
        private readonly Mock<IUserManagerService> _userManagerService;

        public UserDatesServiceTests()
        {
            _mapper = new Mock<IMapper>();
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _userManagerService = new Mock<IUserManagerService>();
        }

        private UserDatesService _userDatesService =>
        new UserDatesService(_mapper.Object, _repoWrapper.Object, _userManagerService.Object);


        [Test]
        public async Task ChangeUserMembershipDatesAsync_Valid_ReturnsTrue()
        {
            //Arrange
            _userManagerService.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new UserDTO());

            _repoWrapper.Setup(m => m.UserMembershipDates.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserMembershipDates, bool>>>(),
            It.IsAny<Func<IQueryable<UserMembershipDates>, IIncludableQueryable<UserMembershipDates, object>>>())).ReturnsAsync(new UserMembershipDates());

            _repoWrapper.Setup(m => m.SaveAsync());

            UserDatesService userDatesService = _userDatesService;

            //Act
            var result = await userDatesService.ChangeUserEntryAndOathDateAsync(new EntryAndOathDateDTO() {UserId = " " });
            //Assert
            Assert.IsTrue(result);

            _userManagerService.Verify(f => f.FindByIdAsync(It.IsAny<string>()));
            _repoWrapper.Verify(f => f.UserMembershipDates.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserMembershipDates, bool>>>(),
            It.IsAny<Func<IQueryable<UserMembershipDates>, IIncludableQueryable<UserMembershipDates, object>>>()));
            _repoWrapper.Verify(f => f.SaveAsync());
        }

        [Test]
        public async Task ChangeUserMembershipDatesAsync_UserNotFind_ReturnsFalse()
        {
            //Arrange
            _userManagerService.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((UserDTO)null);

            UserDatesService userDatesService = _userDatesService;

            //Act
            var result = await userDatesService.ChangeUserEntryAndOathDateAsync(new EntryAndOathDateDTO() { UserId = " " });
            //Assert
            Assert.IsFalse(result);
            _userManagerService.Verify(f => f.FindByIdAsync(It.IsAny<string>()));
        }

        [Test]
        public async Task ChangeUserMembershipDatesAsync_UserMembershipDatesNotFind_ReturnsFalse()
        {
            //Arrange
            _userManagerService.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new UserDTO());

            _repoWrapper.Setup(m => m.UserMembershipDates.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserMembershipDates, bool>>>(),
            It.IsAny<Func<IQueryable<UserMembershipDates>, IIncludableQueryable<UserMembershipDates, object>>>())).ReturnsAsync((UserMembershipDates)null);

            UserDatesService userDatesService = _userDatesService;

            //Act
            var result = await userDatesService.ChangeUserEntryAndOathDateAsync(new EntryAndOathDateDTO() { UserId = " " });
            //Assert
            Assert.IsFalse(result);
            _userManagerService.Verify(f => f.FindByIdAsync(It.IsAny<string>()));
            _repoWrapper.Verify(f => f.UserMembershipDates.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserMembershipDates, bool>>>(),
            It.IsAny<Func<IQueryable<UserMembershipDates>, IIncludableQueryable<UserMembershipDates, object>>>()));
        }

        [Test]
        public async Task GetUserMembershipDatesAsync_Valid_ReturnsUserMembershipDatesDTO()
        {
            //Arrange
            _userManagerService.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new UserDTO());

            _repoWrapper.Setup(m => m.UserMembershipDates.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserMembershipDates, bool>>>(),
            It.IsAny<Func<IQueryable<UserMembershipDates>, IIncludableQueryable<UserMembershipDates, object>>>())).ReturnsAsync(new UserMembershipDates());

            _mapper.Setup(m => m.Map<UserMembershipDatesDTO>(It.IsAny<UserMembershipDates>())).Returns(new UserMembershipDatesDTO());

            UserDatesService userDatesService = _userDatesService;

            //Act
            var result = await userDatesService.GetUserMembershipDatesAsync(" ");
            //Assert
            Assert.IsInstanceOf<UserMembershipDatesDTO>(result);

            _userManagerService.Verify(f => f.FindByIdAsync(It.IsAny<string>()));
            _repoWrapper.Verify(f => f.UserMembershipDates.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserMembershipDates, bool>>>(),
                    It.IsAny<Func<IQueryable<UserMembershipDates>, IIncludableQueryable<UserMembershipDates, object>>>()));
            _mapper.Verify(f => f.Map<UserMembershipDatesDTO>(It.IsAny<UserMembershipDates>()));
        }

        [Test]
        public void GetUserMembershipDatesAsync_UserNotFind_ThrowException()
        {
            //Arrange
            _userManagerService.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((UserDTO)null);

            UserDatesService userDatesService = _userDatesService;

            //Assert & Act 
            Assert.ThrowsAsync<InvalidOperationException>(() => userDatesService.GetUserMembershipDatesAsync(" "));

            _userManagerService.Verify(f => f.FindByIdAsync(It.IsAny<string>()));
        }

        [Test]
        public void GetUserMembershipDatesAsync_UserMembershipDatesNotFind_ThrowException()
        {
            //Arrange
            _userManagerService.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new UserDTO());

            _repoWrapper.Setup(m => m.UserMembershipDates.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserMembershipDates, bool>>>(),
            It.IsAny<Func<IQueryable<UserMembershipDates>, IIncludableQueryable<UserMembershipDates, object>>>())).ReturnsAsync((UserMembershipDates)null);

            UserDatesService userDatesService = _userDatesService;

            //Assert & Act 
            Assert.ThrowsAsync<InvalidOperationException>(() => userDatesService.GetUserMembershipDatesAsync(" "));

            _userManagerService.Verify(f => f.FindByIdAsync(It.IsAny<string>()));
            _repoWrapper.Verify(f => f.UserMembershipDates.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserMembershipDates, bool>>>(),
            It.IsAny<Func<IQueryable<UserMembershipDates>, IIncludableQueryable<UserMembershipDates, object>>>()));
        }

        [Test]
        public async Task AddDateEntryAsync_Valid_ReturnsTrue()
        {
            //Arrange
            _userManagerService.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new UserDTO());
            _repoWrapper.Setup(m => m.UserMembershipDates.CreateAsync(It.IsAny<UserMembershipDates>()));

            _repoWrapper.Setup(m => m.SaveAsync());

            UserDatesService userDatesService = _userDatesService;

            //Act
            var result = await userDatesService.AddDateEntryAsync(" ");
            //Assert
            Assert.IsTrue(result);

            _userManagerService.Verify(f => f.FindByIdAsync(It.IsAny<string>()));
            _repoWrapper.Verify(f => f.UserMembershipDates.CreateAsync(It.IsAny<UserMembershipDates>()));
            _repoWrapper.Verify(f => f.SaveAsync());

        }

        [Test]
        public async Task AddDateEntryAsync_UserNotFind_ReturnsFalse()
        {
            //Arrange
            _userManagerService.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((UserDTO)null);
           
            UserDatesService userDatesService = _userDatesService;

            //Act
            var result = await userDatesService.AddDateEntryAsync(" ");
            //Assert
            Assert.IsFalse(result);

            _userManagerService.Verify(f => f.FindByIdAsync(It.IsAny<string>()));
        }

    }
}
