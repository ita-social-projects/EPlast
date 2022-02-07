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
    public class GetAllDistinctionHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<IMapper> _mockMapper;
        private GetAllDistinctionHandler _handler;
        private GetAllDistinctionQuery _query;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetAllDistinctionHandler(_mockRepoWrapper.Object, _mockMapper.Object);
            _query = new GetAllDistinctionQuery();
        }

        [Test]
        public async Task GetAllDistinctionHandler_Valid()
        {
            //Arrange
            _mockRepoWrapper.
                Setup(x => x.Distinction.GetAllAsync(It.IsAny<Expression<Func<DataAccess.Entities.UserEntities.Distinction, bool>>>(),
                                It.IsAny<Func<IQueryable<DataAccess.Entities.UserEntities.Distinction>, IIncludableQueryable<DataAccess.Entities.UserEntities.Distinction, object>>>()))
                            .ReturnsAsync(new List<DataAccess.Entities.UserEntities.Distinction>());
            _mockMapper
                .Setup(x => x.Map<IEnumerable<DataAccess.Entities.UserEntities.Distinction>, IEnumerable<DistinctionDTO>>(It.IsAny<IEnumerable<DataAccess.Entities.UserEntities.Distinction>>()));

            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<DistinctionDTO>>(result);            
        }
    }
}
