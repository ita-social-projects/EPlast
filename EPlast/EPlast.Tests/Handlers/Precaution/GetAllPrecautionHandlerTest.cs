using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.Handlers.PrecautionHandlers;
using EPlast.BLL.Queries.Precaution;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.Tests.Handlers.Precaution
{
    class GetAllPrecautionHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<IMapper> _mockMapper;
        private GetAllPrecautionHandler _handler;
        private GetAllPrecautionQuery _query;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetAllPrecautionHandler(_mockRepoWrapper.Object, _mockMapper.Object);
            _query = new GetAllPrecautionQuery();
        }

        [Test]
        public async Task GetAllPrecautionHandler_Valid()
        {
            //Arrange
            _mockRepoWrapper.
                Setup(x => x.Precaution.GetAllAsync(It.IsAny<Expression<Func<DataAccess.Entities.UserEntities.Precaution, bool>>>(),
                                It.IsAny<Func<IQueryable<DataAccess.Entities.UserEntities.Precaution>, IIncludableQueryable<DataAccess.Entities.UserEntities.Precaution, object>>>()))
                            .ReturnsAsync(new List<DataAccess.Entities.UserEntities.Precaution>());
            _mockMapper
                .Setup(x => x.Map<IEnumerable<DataAccess.Entities.UserEntities.Precaution>, IEnumerable<PrecautionDTO>>(It.IsAny<IEnumerable<DataAccess.Entities.UserEntities.Precaution>>()));

            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<PrecautionDTO>>(result);
        }
    }
}
