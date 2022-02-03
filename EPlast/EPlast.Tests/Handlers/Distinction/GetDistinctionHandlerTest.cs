using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.Handlers.DistinctionHandlers;
using EPlast.BLL.Queries.Distinction;
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

namespace EPlast.Tests.Handlers.Distinction
{
    public class GetDistinctionHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<IMapper> _mockMapper;
        private GetDistinctionHandler _handler;
        private GetDistinctionQuery _query;

        private int _id;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetDistinctionHandler(_mockRepoWrapper.Object, _mockMapper.Object);
            _query = new GetDistinctionQuery(_id);
        }

        [Test]
        public async Task GetDistinctionHandler_Valid()
        {
            //Arrange
            _mockRepoWrapper.
                Setup(x => x.Distinction.GetFirstAsync(It.IsAny<Expression<Func<DataAccess.Entities.UserEntities.Distinction, bool>>>(),
                                It.IsAny<Func<IQueryable<DataAccess.Entities.UserEntities.Distinction>, IIncludableQueryable<DataAccess.Entities.UserEntities.Distinction, object>>>()))
                            .ReturnsAsync(new DataAccess.Entities.UserEntities.Distinction());
            _mockMapper
                .Setup(x => x.Map<DistinctionDTO>(It.IsAny<DataAccess.Entities.UserEntities.Distinction>())).Returns(new DistinctionDTO());

            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<DistinctionDTO>(result);
        }
    }
}
