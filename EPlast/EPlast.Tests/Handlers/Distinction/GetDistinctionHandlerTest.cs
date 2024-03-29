﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.Handlers.DistinctionHandlers;
using EPlast.BLL.Queries.Distinction;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Handlers.Distinction
{
    public class GetDistinctionHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<IMapper> _mockMapper;
        private GetDistinctionHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetDistinctionHandler(_mockRepoWrapper.Object, _mockMapper.Object);
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
                .Setup(x => x.Map<DistinctionDto>(It.IsAny<DataAccess.Entities.UserEntities.Distinction>())).Returns(new DistinctionDto());

            //Act
            var result = await _handler.Handle(It.IsAny<GetDistinctionQuery>(), It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<DistinctionDto>(result);
        }
    }
}
