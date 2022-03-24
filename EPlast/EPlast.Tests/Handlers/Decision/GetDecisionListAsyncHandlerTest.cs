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
    public class GetDecisionListAsyncHandlerTest
    {
        private Mock<IRepositoryWrapper>  _repository;
        private Mock<IMapper> _mockMapper;
        private GetDecisionListAsyncHandler _handler;
        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetDecisionListAsyncHandler(_repository.Object, _mockMapper.Object);
        }
        [Test]
        public async Task GetDecisionListTest_ReturnDecisionList()
        {
            //Arrange
            _repository.Setup(rep => rep.Decesion.GetAllAsync(It.IsAny<Expression<Func<Decesion, bool>>>(),
                    It.IsAny<Func<IQueryable<Decesion>, IIncludableQueryable<Decesion, object>>>()))
                .ReturnsAsync(GetTestDecesionQueryable().AsEnumerable);
            

            //Act
            var decision = (await _handler.Handle(It.IsAny<GetDecisionListAsyncQuery>(), It.IsAny<CancellationToken>())).ToList();

            //Assert
            Assert.IsInstanceOf<List<DecisionWrapperDTO>>(decision);
        }

        [Test]
        public async Task GetDecisionListCount_Valid_Test()
        {
            //Arrange
            _repository.Setup(rep => rep.Decesion.GetAllAsync(It.IsAny<Expression<Func<Decesion, bool>>>(),
                    It.IsAny<Func<IQueryable<Decesion>, IIncludableQueryable<Decesion, object>>>()))
                .ReturnsAsync(GetTestDecesionQueryable());
            
                  
            _mockMapper.Setup(m => m.Map<IEnumerable<DecisionDTO>>(It.IsAny<IEnumerable<Decesion>>())).Returns(GetTestDecisionsDtoList());

            //Act
            var decision = (await _handler.Handle(It.IsAny<GetDecisionListAsyncQuery>(), It.IsAny<CancellationToken>())).ToList();

            //Assert
            Assert.AreEqual(GetTestDecisionsDtoList().Count, decision.Count);
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
        private static List<DecisionDTO> GetTestDecisionsDtoList()
        {
            return new List<DecisionDTO>
            {
                new DecisionDTO {ID = 1,Description = "old", GoverningBody = new GoverningBodyDTO(), DecisionTarget = new DecisionTargetDTO()},
                new DecisionDTO {ID = 2,Description = "old", GoverningBody = new GoverningBodyDTO(), DecisionTarget = new DecisionTargetDTO()},
                new DecisionDTO {ID = 3,Description = "old", GoverningBody = new GoverningBodyDTO(), DecisionTarget = new DecisionTargetDTO()},
                new DecisionDTO {ID = 4,Description = "old", GoverningBody = new GoverningBodyDTO(), DecisionTarget = new DecisionTargetDTO()}
            };
        }
    }
}
