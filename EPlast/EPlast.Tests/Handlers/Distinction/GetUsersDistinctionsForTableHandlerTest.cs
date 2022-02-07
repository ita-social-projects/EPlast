using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.DTO.Distinction;
using EPlast.BLL.Handlers.DistinctionHandlers;
using EPlast.BLL.Queries.Distinction;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
using MediatR;
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
    public class GetUsersDistinctionsForTableHandlerTest
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private GetUsersDistinctionsForTableHandler _handler;
        private static int page => 1;
        private static int pageSize => 10;
        private static string searchData => "2022";
        

        private DistictionTableSettings _tableSettings;
        private GetUsersDistinctionsForTableQuery query;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _handler = new GetUsersDistinctionsForTableHandler(_repoWrapper.Object, _mapper.Object);
            _tableSettings = new DistictionTableSettings();            
            _tableSettings.SearchedData = searchData;
            _tableSettings.PageSize = pageSize;
            _tableSettings.Page = page;
            query = new GetUsersDistinctionsForTableQuery(_tableSettings);
        }

        [Test]
        public async Task GetUsersDistinctionsForTableByNumberAscend()
        {
            //Arrange
            IEnumerable<string> sortedBy = new List<string> { "number", "ascend" };
            _tableSettings.SortByOrder = sortedBy;

            _repoWrapper
              .Setup(x => x.UserDistinction.GetRangeAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                It.IsAny<Expression<Func<UserDistinction, UserDistinction>>>(), It.IsAny<Func<IQueryable<UserDistinction>, IQueryable<UserDistinction>>>(),
                It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>(), It.IsAny<int>(), It.IsAny<int>()))
              .ReturnsAsync(CreateTuple);

            //Act 
            var responce = await _handler.Handle(query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(responce);
            Assert.IsInstanceOf<Tuple<IEnumerable<UserDistinctionsTableObject>, int>>(responce);
        }

        [Test]
        public async Task GetUsersDistinctionsForTableByNumberDescend()
        {
            //Arrange
            IEnumerable<string> sortedBy = new List<string> { "number", "descend" };
            _tableSettings.SortByOrder = sortedBy;

            _repoWrapper
              .Setup(x => x.UserDistinction.GetRangeAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                It.IsAny<Expression<Func<UserDistinction, UserDistinction>>>(), It.IsAny<Func<IQueryable<UserDistinction>, IQueryable<UserDistinction>>>(),
                It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>(), It.IsAny<int>(), It.IsAny<int>()))
              .ReturnsAsync(CreateTuple);

            //Act 
            var responce = await _handler.Handle(query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(responce);
            Assert.IsInstanceOf<Tuple<IEnumerable<UserDistinctionsTableObject>, int>>(responce);
        }

        [Test]
        public async Task GetUsersDistinctionsForTableByDistinctionNameAscend()
        {
            //Arrange
            IEnumerable<string> sortedBy = new List<string> { "distinctionName", "ascend" };
            _tableSettings.SortByOrder = sortedBy;

            _repoWrapper
              .Setup(x => x.UserDistinction.GetRangeAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                It.IsAny<Expression<Func<UserDistinction, UserDistinction>>>(), It.IsAny<Func<IQueryable<UserDistinction>, IQueryable<UserDistinction>>>(),
                It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>(), It.IsAny<int>(), It.IsAny<int>()))
              .ReturnsAsync(CreateTuple);

            //Act 
            var responce = await _handler.Handle(query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(responce);
            Assert.IsInstanceOf<Tuple<IEnumerable<UserDistinctionsTableObject>, int>>(responce);
        }
        [Test]
        public async Task GetUsersDistinctionsForTableByDistinctionNameDescend()
        {
            //Arrange
            IEnumerable<string> sortedBy = new List<string> { "distinctionName", "descend" };
            _tableSettings.SortByOrder = sortedBy;

            _repoWrapper
              .Setup(x => x.UserDistinction.GetRangeAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                It.IsAny<Expression<Func<UserDistinction, UserDistinction>>>(), It.IsAny<Func<IQueryable<UserDistinction>, IQueryable<UserDistinction>>>(),
                It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>(), It.IsAny<int>(), It.IsAny<int>()))
              .ReturnsAsync(CreateTuple);

            //Act 
            var responce = await _handler.Handle(query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(responce);
            Assert.IsInstanceOf<Tuple<IEnumerable<UserDistinctionsTableObject>, int>>(responce);
        }
        [Test]
        public async Task GetUsersDistinctionsForTableByUserNameAscend()
        {
            //Arrange
            IEnumerable<string> sortedBy = new List<string> { "userName", "ascend" };
            _tableSettings.SortByOrder = sortedBy;

            _repoWrapper
              .Setup(x => x.UserDistinction.GetRangeAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                It.IsAny<Expression<Func<UserDistinction, UserDistinction>>>(), It.IsAny<Func<IQueryable<UserDistinction>, IQueryable<UserDistinction>>>(),
                It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>(), It.IsAny<int>(), It.IsAny<int>()))
              .ReturnsAsync(CreateTuple);

            //Act 
            var responce = await _handler.Handle(query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(responce);
            Assert.IsInstanceOf<Tuple<IEnumerable<UserDistinctionsTableObject>, int>>(responce);
        }
        [Test]
        public async Task GetUsersDistinctionsForTableByUserNameDescend()
        {
            //Arrange
            IEnumerable<string> sortedBy = new List<string> { "userName", "descend" };
            _tableSettings.SortByOrder = sortedBy;

            _repoWrapper
              .Setup(x => x.UserDistinction.GetRangeAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                It.IsAny<Expression<Func<UserDistinction, UserDistinction>>>(), It.IsAny<Func<IQueryable<UserDistinction>, IQueryable<UserDistinction>>>(),
                It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>(), It.IsAny<int>(), It.IsAny<int>()))
              .ReturnsAsync(CreateTuple);

            //Act 
            var responce = await _handler.Handle(query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(responce);
            Assert.IsInstanceOf<Tuple<IEnumerable<UserDistinctionsTableObject>, int>>(responce);
        }
        [Test]
        public async Task GetUsersDistinctionsForTableByDateAscend()
        {
            //Arrange
            IEnumerable<string> sortedBy = new List<string> { "date", "ascend" };
            _tableSettings.SortByOrder = sortedBy;

            _repoWrapper
              .Setup(x => x.UserDistinction.GetRangeAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                It.IsAny<Expression<Func<UserDistinction, UserDistinction>>>(), It.IsAny<Func<IQueryable<UserDistinction>, IQueryable<UserDistinction>>>(),
                It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>(), It.IsAny<int>(), It.IsAny<int>()))
              .ReturnsAsync(CreateTuple);

            //Act 
            var responce = await _handler.Handle(query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(responce);
            Assert.IsInstanceOf<Tuple<IEnumerable<UserDistinctionsTableObject>, int>>(responce);
        }
        [Test]
        public async Task GetUsersDistinctionsForTableByDateDescend()
        {
            //Arrange
            IEnumerable<string> sortedBy = new List<string> { "date", "descend" };
            _tableSettings.SortByOrder = sortedBy;

            _repoWrapper
              .Setup(x => x.UserDistinction.GetRangeAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                It.IsAny<Expression<Func<UserDistinction, UserDistinction>>>(), It.IsAny<Func<IQueryable<UserDistinction>, IQueryable<UserDistinction>>>(),
                It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>(), It.IsAny<int>(), It.IsAny<int>()))
              .ReturnsAsync(CreateTuple);

            //Act 
            var responce = await _handler.Handle(query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(responce);
            Assert.IsInstanceOf<Tuple<IEnumerable<UserDistinctionsTableObject>, int>>(responce);
        }
        private List<UserDistinction> GetClubsByPage()
        {
            return new List<UserDistinction>()
            {
                new UserDistinction()
                {
                    Number = 2,
                }
            };
        }

        private Tuple<IEnumerable<UserDistinction>, int> CreateTuple => new Tuple<IEnumerable<UserDistinction>, int>(GetClubsByPage(), 100);
    }
}
