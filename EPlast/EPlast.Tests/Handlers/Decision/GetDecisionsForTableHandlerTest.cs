using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.Handlers.DecisionHandlers;
using EPlast.BLL.Queries.Decision;
using EPlast.DataAccess.Entities.Decision;
using EPlast.DataAccess.Repositories;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Handlers.Decision
{
    public class GetDecisionsForTableHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private GetDecisionsForTableHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _handler = new GetDecisionsForTableHandler(_mockRepoWrapper.Object);
        }

        [Test]
        public void GetDecisionsForTable_ReturnsUserDistinctionsTableObjectAsync()
        {
            //Arrange
            _mockRepoWrapper
                .Setup(x => x.Decesion.GetDecisions(It.IsAny<string>(),
                    It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<DecisionTableObject>().AsEnumerable());

            //Act
            var result = _handler.Handle(It.IsAny<GetDecisionsForTableQuery>(), It.IsAny<CancellationToken>());

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<Task<IEnumerable<DecisionTableObject>>>(result);
        }
    }
}
