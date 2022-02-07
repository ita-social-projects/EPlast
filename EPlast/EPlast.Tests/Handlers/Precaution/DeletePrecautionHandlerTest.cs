using EPlast.BLL.Handlers.PrecautionHandlers;
using EPlast.BLL.Queries.Precaution;
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

namespace EPlast.Tests.Handlers.Precaution
{
    public class DeletePrecautionHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<IMediator> _mockMediator;
        private DeletePrecautionHandler _handler;
        private DeletePrecautionQuery _query;

        private User _user;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMediator = new Mock<IMediator>();
            _handler = new DeletePrecautionHandler(_mockRepoWrapper.Object, _mockMediator.Object);
            _user = new User();
            _query = new DeletePrecautionQuery(It.IsAny<int>(), _user);
        }

        [Test]
        public async Task DeletePrecautionHandler_Valid()
        {
            //Arrange
            var distinctions = new DataAccess.Entities.UserEntities.Precaution();
            _mockMediator
                .Setup(x => x.Send(It.IsAny<DeletePrecautionQuery>(), It.IsAny<CancellationToken>()));
            _mockRepoWrapper
                .Setup(x => x.Precaution.GetFirstAsync(It.IsAny<Expression<Func<DataAccess.Entities.UserEntities.Precaution, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.UserEntities.Precaution>, IIncludableQueryable<DataAccess.Entities.UserEntities.Precaution, object>>>()))
                .ReturnsAsync(distinctions);
            _mockRepoWrapper.Setup(x => x.Precaution.Delete(It.IsAny<DataAccess.Entities.UserEntities.Precaution>()));
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
