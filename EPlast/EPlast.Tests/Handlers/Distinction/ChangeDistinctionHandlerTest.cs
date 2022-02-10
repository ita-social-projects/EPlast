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
    public class ChangeDistinctionHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<IMediator> _mockMediator;
        private ChangeDistinctionHandler _handler;
        private ChangeDistinctionCommand _query;

        private User _user;
        private DistinctionDTO _distinctionDTO;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMediator = new Mock<IMediator>();
            _handler = new ChangeDistinctionHandler(_mockRepoWrapper.Object, _mockMediator.Object);
            _distinctionDTO = new DistinctionDTO();
            _user = new User();
            _query = new ChangeDistinctionCommand(_distinctionDTO, _user);
        }

        [Test]
        public async Task ChangeDistinctionHandler_Valid()
        {
            //Arrange
            var distinctions = new DataAccess.Entities.UserEntities.Distinction();
            _mockMediator
                .Setup(x => x.Send(It.IsAny<AddDistinctionCommand>(), It.IsAny<CancellationToken>()));
            _mockRepoWrapper
                .Setup(x => x.Distinction.GetFirstAsync(It.IsAny<Expression<Func<DataAccess.Entities.UserEntities.Distinction, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.UserEntities.Distinction>, IIncludableQueryable<DataAccess.Entities.UserEntities.Distinction, object>>>()))
                .ReturnsAsync(distinctions);
            _mockRepoWrapper.Setup(x => x.Distinction.Update(It.IsAny<DataAccess.Entities.UserEntities.Distinction>()));
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
