﻿using AutoMapper;
using EPlast.BLL.Commands.Decision;
using EPlast.BLL.DTO;
using EPlast.BLL.Handlers.DecisionHandlers;
using EPlast.BLL.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EPlast.XUnitTest.Handlers.Decision
{
    public class SaveDecisionAsyncHandlerTest
    {
        private Mock<IMediator> _mockMediator;
        private Mock<IRepositoryWrapper> _repository;
        private Mock<IMapper> _mockMapper;
        private SaveDecisionAsyncHandler _handler;
        private SaveDecisionAsyncCommand _query;
        private Mock<IUniqueIdService> _uniqueId;

        public SaveDecisionAsyncHandlerTest()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockMediator = new Mock<IMediator>();
            _uniqueId = new Mock<IUniqueIdService>();
            _handler = new SaveDecisionAsyncHandler(_repository.Object, _mockMapper.Object, _uniqueId.Object, _mockMediator.Object);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(1)]
        [InlineData(3)]
        public async Task SaveDecisionTest(int decisionId)
        {

            var decision = new DecisionWrapperDTO
            {
                Decision = new DecisionDTO
                {
                    ID = decisionId,
                    DecisionTarget = new DecisionTargetDTO
                    {
                        ID = new Random().Next(),
                        TargetName = Guid.NewGuid().ToString()
                    }
                },
            };
            _mockMapper.Setup(m => m.Map<Decesion>(It.IsAny<DecisionDTO>())).Returns(new Decesion());
            _repository.Setup(x => x.Decesion.Attach(It.IsAny<Decesion>()));
            _repository.Setup(x => x.Decesion.Create(It.IsAny<Decesion>()));

            _query = new SaveDecisionAsyncCommand(decision);
            var actualReturn = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            Assert.Equal(decisionId, actualReturn);
        }
    }
}