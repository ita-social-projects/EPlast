using EPlast.BLL.Handlers.ClubHandlers;
using EPlast.BLL.Queries.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.Tests.Handlers.Club
{
    public class GetCountDeletedUsersPerYearHandlerTest
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private GetCountDeletedUsersPerYearHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _handler = new GetCountDeletedUsersPerYearHandler(_repoWrapper.Object);
        }

        [Test]
        public async Task GetCountDeletedUsersPerYearHandlerTest_ReturnsDefaultInteger()
        {
            // Arrange
            _repoWrapper
                .Setup(r => r.ClubMemberHistory.GetAllAsync(It.IsAny<Expression<Func<ClubMemberHistory, bool>>>(), null))
                .ReturnsAsync(new List<ClubMemberHistory>());

            // Act
            var responce = await _handler.Handle(It.IsAny<GetCountDeletedUsersPerYearQuery>(), It.IsAny<CancellationToken>());

            // Assert
            Assert.NotNull(responce);
            Assert.IsInstanceOf<int>(responce);
            Assert.AreEqual(responce, 0);
        }
    }
}
