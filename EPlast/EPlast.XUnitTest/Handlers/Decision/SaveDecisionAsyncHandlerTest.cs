using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.Commands.Decision;
using EPlast.BLL.DTO;
using EPlast.BLL.Handlers.DecisionHandlers;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using MediatR;
using Moq;
using Xunit;

namespace EPlast.XUnitTest.Handlers.Decision
{
    public class SaveDecisionAsyncHandlerTest
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly SaveDecisionAsyncHandler _handler;
        private SaveDecisionAsyncCommand _query;

        public SaveDecisionAsyncHandlerTest()
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

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(1)]
        [InlineData(3)]
        public async Task SaveDecisionTest(int decisionId)
        {

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

            _query = new SaveDecisionAsyncCommand(decision);
            var actualReturn = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            Assert.Equal(decisionId, actualReturn);
        }
    }
}
