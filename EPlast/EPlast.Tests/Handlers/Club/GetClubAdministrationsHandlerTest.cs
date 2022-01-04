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
    public class GetClubAdministrationsHandlerTest
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private GetClubAdministrationsHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _handler = new GetClubAdministrationsHandler(_repoWrapper.Object);
        }

        [Test]
        public async Task GetClubAdministrationsHandlerTest_ReturnsClubAdministrationArray()
        {
            // Arrange
            _repoWrapper
                .Setup(r => r.ClubAdministration.GetAllAsync(It.IsAny<Expression<Func<ClubAdministration, bool>>>(), null))
                .ReturnsAsync(new List<ClubAdministration>());

            // Act
            var responce = await _handler.Handle(It.IsAny<GetClubAdministrationsQuery>(), It.IsAny<CancellationToken>());

            //Assert
            Assert.NotNull(responce);
            Assert.IsInstanceOf<ClubAdministration[]>(responce);

        }
    }
}
