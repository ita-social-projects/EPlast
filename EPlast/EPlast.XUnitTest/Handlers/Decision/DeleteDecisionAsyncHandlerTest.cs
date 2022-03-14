using AutoMapper;
using EPlast.BLL.Commands.Decision;
using EPlast.BLL.Handlers.DecisionHandlers;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EPlast.XUnitTest.Handlers.Decision
{
    public class DeleteDecisionAsyncHandlerTest
    {
        private Mock<IRepositoryWrapper> _repository;
        private Mock<IDecisionBlobStorageRepository> _blob;
        private Mock<IMapper> _mockMapper;
        private DeleteDecisionAsyncHandler _handler;
        private DeleteDecisionAsyncCommand _query;

        public DeleteDecisionAsyncHandlerTest()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _blob = new Mock<IDecisionBlobStorageRepository>();
            _query = new DeleteDecisionAsyncCommand(It.IsAny<int>());
            _handler = new DeleteDecisionAsyncHandler(_repository.Object, _mockMapper.Object, _blob.Object);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task DeleteDecisionTest(int decisionId)
        {
         
            _repository.Setup(rep => rep.Decesion.GetFirstAsync(It.IsAny<Expression<Func<Decesion, bool>>>(),
                    It.IsAny<Func<IQueryable<Decesion>, IIncludableQueryable<Decesion, object>>>()))
                .ReturnsAsync(GetTestDecesionQueryable().FirstOrDefault(d => d.ID == decisionId));

            _query = new DeleteDecisionAsyncCommand(decisionId);
            await _handler.Handle(_query, It.IsAny<CancellationToken>());

            _repository.Verify(rep => rep.Decesion.GetFirstAsync(It.IsAny<Expression<Func<Decesion, bool>>>(),
                    It.IsAny<Func<IQueryable<Decesion>, IIncludableQueryable<Decesion, object>>>()), Times.Once);
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
    }
}
