using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.Handlers.PrecautionHandlers;
using EPlast.BLL.Queries.Precaution;
using EPlast.DataAccess.Repositories;
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
    public class GetPrecautionHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<IMapper> _mockMapper;
        private GetPrecautionHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetPrecautionHandler(_mockRepoWrapper.Object, _mockMapper.Object);
        }

        [Test]
        public async Task GetPrecautionHandler_Valid()
        {
            //Arrange
            _mockRepoWrapper.
                Setup(x => x.Precaution.GetFirstAsync(It.IsAny<Expression<Func<DataAccess.Entities.UserEntities.Precaution, bool>>>(),
                                It.IsAny<Func<IQueryable<DataAccess.Entities.UserEntities.Precaution>, IIncludableQueryable<DataAccess.Entities.UserEntities.Precaution, object>>>()))
                            .ReturnsAsync(new DataAccess.Entities.UserEntities.Precaution());
            _mockMapper
                .Setup(x => x.Map<PrecautionDTO>(It.IsAny<DataAccess.Entities.UserEntities.Precaution>())).Returns(new PrecautionDTO());

            //Act
            var result = await _handler.Handle(It.IsAny<GetPrecautionQuery>(), It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<PrecautionDTO>(result);
        }
    }
}
