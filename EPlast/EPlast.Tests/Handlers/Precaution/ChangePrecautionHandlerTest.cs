using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL;
using EPlast.BLL.Commands.Precaution;
using EPlast.BLL.Handlers.PrecautionHandlers;
using EPlast.BLL.Queries.Precaution;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Handlers.Precaution
{
    public class ChangePrecautionHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<IMediator> _mockMediator;
        private ChangePrecautionHandler _handler;
        private ChangePrecautionCommand _query;

        private User _user;
        private PrecautionDto _precautionDTO;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMediator = new Mock<IMediator>();
            _handler = new ChangePrecautionHandler(_mockRepoWrapper.Object, _mockMediator.Object);
            _precautionDTO = new PrecautionDto();
            _user = new User();
            _query = new ChangePrecautionCommand(_precautionDTO, _user);
        }

        [Test]
        public async Task ChangePrecautionHandler_Valid()
        {
            //Arrange
            var precautions = new DataAccess.Entities.UserEntities.Precaution();
            _mockMediator
                .Setup(x => x.Send(It.IsAny<AddPrecautionCommand>(), It.IsAny<CancellationToken>()));
            _mockRepoWrapper
                .Setup(x => x.Precaution.GetFirstAsync(It.IsAny<Expression<Func<DataAccess.Entities.UserEntities.Precaution, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.UserEntities.Precaution>, IIncludableQueryable<DataAccess.Entities.UserEntities.Precaution, object>>>()))
                .ReturnsAsync(precautions);
            _mockRepoWrapper.Setup(x => x.Precaution.Update(It.IsAny<DataAccess.Entities.UserEntities.Precaution>()));
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
