using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.Handlers.DecisionHandlers;
using EPlast.BLL.Queries.Decision;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.Tests.Handlers.Decision
{
    public class GetDecisionTargetSearchListAsyncHandlerTest
    {
        private Mock<IRepositoryWrapper> _repository;
        private Mock<IMapper> _mockMapper;
        private GetDecisionTargetSearchListAsyncHandler _handler;
        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetDecisionTargetSearchListAsyncHandler(_repository.Object, _mockMapper.Object);
        }
       

        [Test]
        public async Task GetDecisionTargetListSearchAsyncTest()
        {
            //Arrange
            List<DecisionTargetDTO> decisionTargets = GetTestDecisionTargetsDtoList();
            _repository.Setup(rep => rep.DecesionTarget.GetAllAsync(It.IsAny<Expression<Func<DecesionTarget, bool>>>(),
                It.IsAny<Func<IQueryable<DecesionTarget>, IIncludableQueryable<DecesionTarget, object>>>())).ReturnsAsync(new List<DecesionTarget>());
            _mockMapper.Setup(m => m.Map<IEnumerable<DecisionTargetDTO>>(It.IsAny<IEnumerable<DecesionTarget>>())).Returns(GetTestDecisionTargetsDtoList());
            //Act
            var query = new GetDecisionTargetSearchListAsyncQuery(It.IsAny<string>());
            var actualReturn = await _handler.Handle(query, It.IsAny<CancellationToken>());

            //Assert
            Assert.AreEqual(decisionTargets.Aggregate("", (x, y) => y.TargetName), actualReturn.Aggregate("", (x, y) => y.TargetName));
        }
        private static List<DecisionTargetDTO> GetTestDecisionTargetsDtoList()
        {
            return new List<DecisionTargetDTO>
            {
                new DecisionTargetDTO {ID = 1, TargetName = "First DecesionTarget"},
                new DecisionTargetDTO {ID = 2, TargetName = "Second DecesionTarget"},
                new DecisionTargetDTO {ID = 3, TargetName = "Third DecesionTarget"}
            };
        }
    }
}
