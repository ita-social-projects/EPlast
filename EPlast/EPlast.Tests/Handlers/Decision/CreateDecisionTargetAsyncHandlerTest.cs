using AutoMapper;
using EPlast.BLL.Commands.Decision;
using EPlast.BLL.DTO;
using EPlast.BLL.Handlers.DecisionHandlers;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


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
            _mockMapper.Setup(m => m.Map<DecisionTargetDTO>(It.IsAny<IEnumerable<DecesionTarget>>())).Returns(DecisionTargetDTONull);

            //Act
            var decision = _handler.Handle(_query, It.IsAny<CancellationToken>());
            //Assert
            Assert.IsNotNull(decision);
            Assert.IsInstanceOf<Task<DecisionTargetDTO>>(decision);
        }

        [Test]
        public void CreateDecisionTest_ReturnsOldDecision()
        {
            //Arrange
            _mockMapper.Setup(m => m.Map<DecisionTargetDTO>(It.IsAny<IEnumerable<DecesionTarget>>())).Returns(GetTestDecisionTargetsDto);

            //Act
            var decision = _handler.Handle(_query, It.IsAny<CancellationToken>());
            //Assert
            Assert.IsNotNull(decision);
            Assert.IsInstanceOf<Task<DecisionTargetDTO>>(decision);
        }
        private DecisionTargetDTO DecisionTargetDTONull = null;
        private static DecisionTargetDTO GetTestDecisionTargetsDto = new DecisionTargetDTO { ID = 1, TargetName = "First DecesionTarget" };
       
    }
}
