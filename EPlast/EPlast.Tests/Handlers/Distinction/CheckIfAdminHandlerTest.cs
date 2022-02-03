using EPlast.BLL.Handlers.DistinctionHandlers;
using EPlast.BLL.Handlers.TermsOfUse;
using EPlast.BLL.Queries.Distinction;
using EPlast.BLL.Queries.TermsOfUse;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.Tests.Handlers.Distinction
{
    public class CheckIfAdminHandlerTest
    {
        private Mock<UserManager<User>> _userManager;
        private CheckIfAdminHandler _handler;
        private CheckIfAdminQuery _query;

        private User _user;

        [SetUp]
        public void SetUp()
        {
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _handler = new CheckIfAdminHandler(_userManager.Object);
            _user = new User();
            _query = new CheckIfAdminQuery(_user);
        }

        [Test]
        public void CheckIfAdminHandler_True()
        {
            //Arrange
            _userManager.
                Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRolesWithoutAdmin());

            //Assert
            Exception exception = Assert.ThrowsAsync(typeof(UnauthorizedAccessException),
                async () => await _handler.Handle(_query, It.IsAny<CancellationToken>()));
            Assert.AreEqual("Attempted to perform an unauthorized operation.", exception.Message);
        }

        [Test]
        public async Task CheckIfAdminHandler_False()
        {
            //Arrange
            _userManager.
                Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRolesWithAdmin());

            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Unit>(result);
        }

        private IList<string> GetRolesWithoutAdmin()
        {
            return new List<string>
            {
                "Htos",
                "Nixto"
            };
        }

        private IList<string> GetRolesWithAdmin()
        {
            return new List<string>
            {
                Roles.Admin,
                "Htos",
                "Nixto"
            };
        }
    }
}
