using AutoMapper;
using EPlast.BLL.Handlers.DecisionHandlers;
using EPlast.DataAccess.Repositories;
using Moq;
using EPlast.BLL.Queries.Decision;
using System.Threading.Tasks;
using Xunit;
using System.Threading;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq.Expressions;
using EPlast.BLL.DTO;

namespace EPlast.XUnitTest.Handlers.Decision
{
    public class GetDecisionAsyncHandlerTest
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mockMapper;
        private GetDecisionAsyncHandler _handler;
        
        public  GetDecisionAsyncHandlerTest()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetDecisionAsyncHandler(_repoWrapper.Object, _mockMapper.Object);
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetDecisionTest(int decisionId)
        {
            _repoWrapper.
              Setup(x => x.Decesion.GetFirstAsync(It.IsAny<Expression<Func<DataAccess.Entities.Decesion, bool>>>(),
              It.IsAny<Func<IQueryable<DataAccess.Entities.Decesion>, IIncludableQueryable<DataAccess.Entities.Decesion, object>>>())).ReturnsAsync(new DataAccess.Entities.Decesion());
            _mockMapper
                .Setup(x => x.Map<DecisionDTO>(It.IsAny<DataAccess.Entities.Decesion>())).Returns(new DecisionDTO() { ID = decisionId});
            var query = new GetDecisionAsyncQuery(decisionId);
            var decision = await _handler.Handle( query, It.IsAny<CancellationToken>());

            Assert.Equal(decisionId, decision.ID);
        }

    }
}
