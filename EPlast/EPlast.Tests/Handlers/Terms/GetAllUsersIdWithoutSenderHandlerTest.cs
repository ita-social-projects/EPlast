using EPlast.BLL.Handlers.TermsOfUse;
using EPlast.BLL.Queries.TermsOfUse;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.Tests.Handlers.Terms
{
    public class GetAllUsersIdWithoutSenderHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<IMediator> _mockMediator;
        private Mock<UserManager<User>> _mockUserManager;
        private GetAllUsersIdWithoutSenderQuery _query;
        private GetAllUsersIdWithoutSenderHandler _handler;

        private User _user;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMediator = new Mock<IMediator>();
            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _user = new User();
            _query = new GetAllUsersIdWithoutSenderQuery(_user);
            _handler = new GetAllUsersIdWithoutSenderHandler(_mockRepoWrapper.Object, _mockMediator.Object, _mockUserManager.Object);
        }

        [Test]
        public async Task ReturnAllUsersIdWithoutSenderHandler_Valid()
        {
            //Arrange
            var userId = new User() {Id = "963b1137-d8b5-4de7-b83f-66791b7ca4d8" };
            _mockMediator
                .Setup(x => x.Send(It.IsAny<CheckIfAdminForTermsQuery>(), It.IsAny<CancellationToken>()));
            _mockUserManager.Setup(x => x.GetUserIdAsync(userId));
            _mockRepoWrapper
                .Setup(x => x.UserProfile.GetAllAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(),
                    It.IsAny<Func<IQueryable<UserProfile>, IIncludableQueryable<UserProfile, object>>>()))
                .ReturnsAsync(GetTestUsersId());

            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
            Assert.AreEqual(GetTestUsersId().Select(x => x.UserID), result);
        }

        private IEnumerable<UserProfile> GetTestUsersId()
        {
            return new List<UserProfile>
            {
                new UserProfile{ UserID = "963b1137-d8b5-4de7-b83f-66791b7ca4d8", GenderID = 1},
                new UserProfile{ UserID = "99dbe3c2-6108-43cc-bac2-e8efe7e08481", GenderID = 1}
            }.AsEnumerable();
        }
    }
}