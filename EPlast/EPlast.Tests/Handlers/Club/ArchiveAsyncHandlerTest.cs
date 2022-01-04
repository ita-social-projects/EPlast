using EPlast.BLL.Commands.Club;
using EPlast.BLL.Handlers.ClubHandlers;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DataAccessClub = EPlast.DataAccess.Entities;

namespace EPlast.Tests.Handlers.Club
{
    public class ArchiveAsyncHandlerTest
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private ArchiveHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _handler = new ArchiveHandler(_repoWrapper.Object);
        }

        [Test]
        public void ArchiveAsync_ClubIsNotEmpty_ThrowsInvalidOperationException()
        {
            // Arrange
            _repoWrapper.Setup(r => r.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(), null))
               .ReturnsAsync(new DataAccessClub.Club()
               {
                   ClubAdministration = new List<ClubAdministration>(),
                   ClubMembers = new List<ClubMembers>()
               });
            _repoWrapper.Setup(r => r.Club.Update(It.IsAny<DataAccessClub.Club>()));
            _repoWrapper.Setup(r => r.SaveAsync());

            // Act // Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _handler.Handle(It.IsAny<ArchiveCommand>(), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ArchiveAsyncHandlerTest_ValidTest()
        {
            // Arrange
            _repoWrapper.Setup(r => r.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(), null))
               .ReturnsAsync(new DataAccessClub.Club());
            _repoWrapper.Setup(r => r.Club.Update(It.IsAny<DataAccessClub.Club>()));
            _repoWrapper.Setup(r => r.SaveAsync());

            // Act
            await _handler.Handle(It.IsAny<ArchiveCommand>(), It.IsAny<CancellationToken>());

            // Assert
            _repoWrapper.Verify(r => r.Club.Update(It.IsAny<DataAccessClub.Club>()), Times.Once);
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once);
        }
    }
}
