using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.Handlers.DecisionHandlers;
using EPlast.BLL.Queries.Decision;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.Tests.Handlers.Decision
{
    public class GetDecisionAsyncHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<IMapper> _mockMapper;
        private GetDecisionAsyncHandler _handler;
        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetDecisionAsyncHandler(_mockRepoWrapper.Object, _mockMapper.Object);
        }
        [Test]
        public async Task GetDecisionTest_ReturnsObj()
        {
            //Arrange
            _mockRepoWrapper.
                Setup(x => x.Decesion.GetFirstAsync(It.IsAny<Expression<Func<DataAccess.Entities.Decesion, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccess.Entities.Decesion>, IIncludableQueryable<DataAccess.Entities.Decesion, object>>>())).ReturnsAsync(new DataAccess.Entities.Decesion());
            _mockMapper
                .Setup(x => x.Map<DecisionDTO>(It.IsAny<DataAccess.Entities.Decesion>())).Returns(new DecisionDTO());

            //Act
            var result = await _handler.Handle(It.IsAny<GetDecisionAsyncQuery>(), It.IsAny<CancellationToken>());

            //Assert
            Assert.IsInstanceOf<DecisionDTO>(result);
        }

    }
}
