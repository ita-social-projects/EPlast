using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.Commands.Decision;
using EPlast.BLL.DTO;
using EPlast.BLL.Handlers.DecisionHandlers;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Moq;
using NUnit.Framework;


namespace EPlast.Tests.Handlers.Decision
{
    public class CreateDecisionTargetAsyncHandlerTest
    {
        private Mock<IRepositoryWrapper> _repository;
        private Mock<IMapper> _mockMapper;
        private CreateDecisionTargetAsyncHandler _handler;
        private CreateDecisionTargetAsyncCommand _query;
        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _query = new CreateDecisionTargetAsyncCommand(It.IsAny<string>());
            _handler = new CreateDecisionTargetAsyncHandler(_repository.Object, _mockMapper.Object);
        }
        [Test]
        public void CreateDecisionTest_ReturnsNewDecision()
        {
            //Arrange
            _mockMapper.Setup(m => m.Map<DecisionTargetDto>(It.IsAny<IEnumerable<DecesionTarget>>())).Returns(DecisionTargetDTONull);

            //Act
            var decision = _handler.Handle(_query, It.IsAny<CancellationToken>());
            //Assert
            Assert.IsNotNull(decision);
            Assert.IsInstanceOf<Task<DecisionTargetDto>>(decision);
        }

        [Test]
        public void CreateDecisionTest_ReturnsOldDecision()
        {
            //Arrange
            _mockMapper.Setup(m => m.Map<DecisionTargetDto>(It.IsAny<IEnumerable<DecesionTarget>>())).Returns(GetTestDecisionTargetsDto);

            //Act
            var decision = _handler.Handle(_query, It.IsAny<CancellationToken>());
            //Assert
            Assert.IsNotNull(decision);
            Assert.IsInstanceOf<Task<DecisionTargetDto>>(decision);
        }
        private DecisionTargetDto DecisionTargetDTONull = null;
        private static DecisionTargetDto GetTestDecisionTargetsDto = new DecisionTargetDto { ID = 1, TargetName = "First DecesionTarget" };
       
    }
}
