using EPlast.BussinessLayer.Services;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using Xunit;

namespace EPlast.XUnitTest.Services.UserArea
{
    public class ConfirmedUsersServiceTests
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IUserStore<User>> _userStoreMock;
        private Mock<UserManager<User>> _userManager;

        public ConfirmedUsersServiceTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _userStoreMock = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(_userStoreMock.Object, null, null, null, null, null, null, null, null);
        }

        [Fact]
        public void CreateTest()
        {
            _userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("1");
            _repoWrapper.Setup(x => x.ConfirmedUser.FindAll()).Returns(new List<ConfirmedUser>().AsQueryable());
            var service = new ConfirmedUsersService(_repoWrapper.Object, _userManager.Object);
            service.Create(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>());
            _repoWrapper.Verify(r => r.ConfirmedUser.Create(It.IsAny<ConfirmedUser>()), Times.Once());
            _repoWrapper.Verify(r => r.Save(), Times.Once());
        }
        [Fact]
        public void DeleteTest()
        {
            _repoWrapper.Setup(x => x.ConfirmedUser.FindByCondition(It.IsAny<Expression<Func<ConfirmedUser, bool>>>())).Returns(new List<ConfirmedUser> { new ConfirmedUser() }.AsQueryable());
            var service = new ConfirmedUsersService(_repoWrapper.Object, _userManager.Object);
            service.Delete(It.IsAny<int>());
            _repoWrapper.Verify(r => r.ConfirmedUser.Delete(It.IsAny<ConfirmedUser>()), Times.Once());
            _repoWrapper.Verify(r => r.Save(), Times.Once());
        }
    }
}