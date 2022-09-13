using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.Handlers.DecisionHandlers;
using EPlast.BLL.Queries.Decision;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Xunit;

namespace EPlast.XUnitTest.Handlers.Decision
{
    public class GetDecisionListAsyncHandlerTest
    {
        private Mock<IRepositoryWrapper> _repository;
        private Mock<IMapper> _mockMapper;
        private GetDecisionListAsyncHandler _handler;

        public GetDecisionListAsyncHandlerTest()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetDecisionListAsyncHandler(_repository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetDecisionListTest()
        {
            _repository.Setup(rep => rep.Decesion.GetAllAsync(It.IsAny<Expression<Func<Decesion, bool>>>(),
                    It.IsAny<Func<IQueryable<Decesion>, IIncludableQueryable<Decesion, object>>>()))
                .ReturnsAsync(GetTestDecesionQueryable().AsEnumerable);

            var decision = (await _handler.Handle(It.IsAny<GetDecisionListAsyncQuery>(), It.IsAny<CancellationToken>())).ToList();

            Assert.IsType<List<DecisionWrapperDto>>(decision);
        }

        [Fact]
        public async Task GetDecisionListCountTest()
        {
            _repository.Setup(rep => rep.Decesion.GetAllAsync(It.IsAny<Expression<Func<Decesion, bool>>>(),
                    It.IsAny<Func<IQueryable<Decesion>, IIncludableQueryable<Decesion, object>>>()))
                .ReturnsAsync(GetTestDecesionQueryable());
            _mockMapper.Setup(m => m.Map<IEnumerable<DecisionDto>>(It.IsAny<IEnumerable<Decesion>>())).Returns(GetTestDecisionsDtoList());

            var decision = (await _handler.Handle(It.IsAny<GetDecisionListAsyncQuery>(), It.IsAny<CancellationToken>())).ToList();

            Assert.Equal(GetTestDecisionsDtoList().Count, decision.Count);
        }

        private static IQueryable<Decesion> GetTestDecesionQueryable()
        {
            return new List<Decesion>
            {
                new Decesion  {ID = 1,Description = "old"},
                new Decesion  {ID = 2,Description = "old"},
                new Decesion  {ID = 3,Description = "old"},
                new Decesion  {ID = 4,Description = "old"}
            }.AsQueryable();
        }
        private static List<DecisionDto> GetTestDecisionsDtoList()
        {
            return new List<DecisionDto>
            {
                new DecisionDto {ID = 1,Description = "old", GoverningBody = new GoverningBodyDto(), DecisionTarget = new DecisionTargetDto()},
                new DecisionDto {ID = 2,Description = "old", GoverningBody = new GoverningBodyDto(), DecisionTarget = new DecisionTargetDto()},
                new DecisionDto {ID = 3,Description = "old", GoverningBody = new GoverningBodyDto(), DecisionTarget = new DecisionTargetDto()},
                new DecisionDto {ID = 4,Description = "old", GoverningBody = new GoverningBodyDto(), DecisionTarget = new DecisionTargetDto()}
            };
        }
    }
}
