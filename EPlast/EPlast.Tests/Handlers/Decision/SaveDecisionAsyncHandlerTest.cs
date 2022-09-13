using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.Commands.Decision;
using EPlast.BLL.DTO;
using EPlast.BLL.Handlers.DecisionHandlers;
using EPlast.BLL.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using MediatR;
using Moq;
using NUnit.Framework;
namespace EPlast.Tests.Handlers.Decision
{
    public class SaveDecisionAsyncHandlerTest
    {
        private Mock<IMediator> _mockMediator;
        private Mock<IRepositoryWrapper> _repository;
        private Mock<IMapper> _mockMapper;
        private SaveDecisionAsyncHandler _handler;
        private SaveDecisionAsyncCommand _query;
        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockMediator = new Mock<IMediator>();
            _handler = new SaveDecisionAsyncHandler(
                _repository.Object,
                _mockMapper.Object,
                _mockMediator.Object
            );
        }
        [TestCase(2)]
        [TestCase(4)]
        [TestCase(1)]
        [TestCase(3)]
        public async Task SaveDecisionTest(int decisionId)
        {
            //Arrange
            var decision = new DecisionWrapperDto
            {
                Decision = new DecisionDto
                {
                    ID = decisionId,
                    DecisionTarget = new DecisionTargetDto
                    {
                        ID = new Random().Next(),
                        TargetName = Guid.NewGuid().ToString()
                    }
                },
            };

            _mockMapper.Setup(m => m.Map<Decesion>(It.IsAny<DecisionDto>())).Returns(new Decesion());
            _repository.Setup(x => x.Decesion.Attach(It.IsAny<Decesion>()));
            _repository.Setup(x => x.Decesion.Create(It.IsAny<Decesion>()));
            //Act
            _query = new SaveDecisionAsyncCommand(decision);
            var actualReturn = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.AreEqual(decisionId, actualReturn);
        }
    }
}
