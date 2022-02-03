using EPlast.BLL;
using EPlast.BLL.Handlers.DistinctionHandlers;
using EPlast.BLL.Queries.Distinction;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.Tests.Handlers.Distinction
{
    public class DeleteDistinctionHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<IMediator> _mockMediator;
        private DeleteDistinctionHandler _handler;
        private DeleteDistinctionQuery _query;

        private User _user;
        private int _id;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMediator = new Mock<IMediator>();
            _handler = new DeleteDistinctionHandler(_mockRepoWrapper.Object, _mockMediator.Object);            
            _user = new User();
            _query = new DeleteDistinctionQuery(_id, _user);
        }

        [Test]
        public async Task DeleteDistinctionHandler_Valid()
        {
            //Arrange
            var distinctions = new DataAccess.Entities.UserEntities.Distinction();
            _mockMediator
                .Setup(x => x.Send(It.IsAny<DeleteDistinctionQuery>(), It.IsAny<CancellationToken>()));
            _mockRepoWrapper
                .Setup(x => x.Distinction.GetFirstAsync(It.IsAny<Expression<Func<DataAccess.Entities.UserEntities.Distinction, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.UserEntities.Distinction>, IIncludableQueryable<DataAccess.Entities.UserEntities.Distinction, object>>>()))
                .ReturnsAsync(distinctions);
            _mockRepoWrapper.Setup(x => x.Distinction.Delete(It.IsAny<DataAccess.Entities.UserEntities.Distinction>()));
            _mockRepoWrapper.Setup(x => x.SaveAsync());

            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Unit>(result);
            Assert.DoesNotThrowAsync(async () => { await _handler.Handle(_query, It.IsAny<CancellationToken>()); });
        }
    }
}
