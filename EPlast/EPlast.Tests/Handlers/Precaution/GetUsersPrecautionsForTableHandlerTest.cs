using AutoMapper;
using EPlast.BLL.DTO.PrecautionsDTO;
using EPlast.BLL.Handlers.PrecautionHandlers;
using EPlast.BLL.Queries.Precaution;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
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
    public class GetUsersPrecautionsForTableHandlerTest
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private GetUsersPrecautionsForTableHandler _handler;
        private static int page => 1;
        private static int pageSize => 10;
        private static string searchData => "2022";


        private PrecautionTableSettings _tableSettings;
        private GetUsersPrecautionsForTableQuery query;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _handler = new GetUsersPrecautionsForTableHandler(_repoWrapper.Object, _mapper.Object);
            _tableSettings = new PrecautionTableSettings();
            _tableSettings.SearchedData = searchData;
            _tableSettings.PageSize = pageSize;
            _tableSettings.Page = page;
            query = new GetUsersPrecautionsForTableQuery(_tableSettings);
        }

        [Test]
        public async Task GetUsersPrecautionsForTableByNumberAscend()
        {
            //Arrange
            IEnumerable<string> sortedBy = new List<string> { "number", "ascend" };
            _tableSettings.SortByOrder = sortedBy;

            _repoWrapper
              .Setup(x => x.UserPrecaution.GetRangeAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                It.IsAny<Expression<Func<UserPrecaution, UserPrecaution>>>(), It.IsAny<Func<IQueryable<UserPrecaution>, IQueryable<UserPrecaution>>>(),
                It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>(), It.IsAny<int>(), It.IsAny<int>()))
              .ReturnsAsync(CreateTuple);

            //Act 
            var responce = await _handler.Handle(query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(responce);
            Assert.IsInstanceOf<Tuple<IEnumerable<UserPrecautionsTableObject>, int>>(responce);
        }

        [Test]
        public async Task GetUsersPrecautionsForTableByNumberDescend()
        {
            //Arrange
            IEnumerable<string> sortedBy = new List<string> { "number", "descend" };
            _tableSettings.SortByOrder = sortedBy;

            _repoWrapper
              .Setup(x => x.UserPrecaution.GetRangeAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                It.IsAny<Expression<Func<UserPrecaution, UserPrecaution>>>(), It.IsAny<Func<IQueryable<UserPrecaution>, IQueryable<UserPrecaution>>>(),
                It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>(), It.IsAny<int>(), It.IsAny<int>()))
              .ReturnsAsync(CreateTuple);

            //Act 
            var responce = await _handler.Handle(query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(responce);
            Assert.IsInstanceOf<Tuple<IEnumerable<UserPrecautionsTableObject>, int>>(responce);
        }

        [Test]
        public async Task GetUsersDistinctionsForTableByUserNameAscend()
        {
            //Arrange
            IEnumerable<string> sortedBy = new List<string> { "userName", "ascend" };
            _tableSettings.SortByOrder = sortedBy;

            _repoWrapper
              .Setup(x => x.UserPrecaution.GetRangeAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                It.IsAny<Expression<Func<UserPrecaution, UserPrecaution>>>(), It.IsAny<Func<IQueryable<UserPrecaution>, IQueryable<UserPrecaution>>>(),
                It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>(), It.IsAny<int>(), It.IsAny<int>()))
              .ReturnsAsync(CreateTuple);

            //Act 
            var responce = await _handler.Handle(query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(responce);
            Assert.IsInstanceOf<Tuple<IEnumerable<UserPrecautionsTableObject>, int>>(responce);
        }
        [Test]
        public async Task GetUsersPrecautionsForTableByUserNameDescend()
        {
            //Arrange
            IEnumerable<string> sortedBy = new List<string> { "userName", "descend" };
            _tableSettings.SortByOrder = sortedBy;

            _repoWrapper
              .Setup(x => x.UserPrecaution.GetRangeAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                It.IsAny<Expression<Func<UserPrecaution, UserPrecaution>>>(), It.IsAny<Func<IQueryable<UserPrecaution>, IQueryable<UserPrecaution>>>(),
                It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>(), It.IsAny<int>(), It.IsAny<int>()))
              .ReturnsAsync(CreateTuple);

            //Act 
            var responce = await _handler.Handle(query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(responce);
            Assert.IsInstanceOf<Tuple<IEnumerable<UserPrecautionsTableObject>, int>>(responce);
        }
        [Test]
        public async Task GetUsersDistinctionsForTableByEndDateAscend()
        {
            //Arrange
            IEnumerable<string> sortedBy = new List<string> { "EndName", "ascend" };
            _tableSettings.SortByOrder = sortedBy;

            _repoWrapper
              .Setup(x => x.UserPrecaution.GetRangeAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                It.IsAny<Expression<Func<UserPrecaution, UserPrecaution>>>(), It.IsAny<Func<IQueryable<UserPrecaution>, IQueryable<UserPrecaution>>>(),
                It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>(), It.IsAny<int>(), It.IsAny<int>()))
              .ReturnsAsync(CreateTuple);

            //Act 
            var responce = await _handler.Handle(query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(responce);
            Assert.IsInstanceOf<Tuple<IEnumerable<UserPrecautionsTableObject>, int>>(responce);
        }
        [Test]
        public async Task GetUsersDistinctionsForTableByEndDateDescend()
        {
            //Arrange
            IEnumerable<string> sortedBy = new List<string> { "EndDate", "descend" };
            _tableSettings.SortByOrder = sortedBy;

            _repoWrapper
              .Setup(x => x.UserPrecaution.GetRangeAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                It.IsAny<Expression<Func<UserPrecaution, UserPrecaution>>>(), It.IsAny<Func<IQueryable<UserPrecaution>, IQueryable<UserPrecaution>>>(),
                It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>(), It.IsAny<int>(), It.IsAny<int>()))
              .ReturnsAsync(CreateTuple);

            //Act 
            var responce = await _handler.Handle(query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(responce);
            Assert.IsInstanceOf<Tuple<IEnumerable<UserPrecautionsTableObject>, int>>(responce);
        }
        [Test]
        public async Task GetUsersDistinctionsForTableWithFilters()
        {
            //Arrange                        
            _tableSettings.SortByOrder = new List<string> { "endDate", "ascend" };
            _tableSettings.PrecautionNameFilter = new List<string> { "Догана", "Сувора догана" };
            _tableSettings.SearchedData = "2021";
            _tableSettings.StatusFilter = new List<UserPrecautionStatus> { UserPrecautionStatus.Accepted, UserPrecautionStatus.Confirmed };
            _tableSettings.DateFilter = new List<string> { "2021", "2022" };

            _repoWrapper
              .Setup(x => x.UserPrecaution.GetRangeAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                It.IsAny<Expression<Func<UserPrecaution, UserPrecaution>>>(), It.IsAny<Func<IQueryable<UserPrecaution>, IQueryable<UserPrecaution>>>(),
                It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>(), It.IsAny<int>(), It.IsAny<int>()))
              .ReturnsAsync(CreateTuple);

            //Act 
            var responce = await _handler.Handle(query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(responce);
            Assert.IsInstanceOf<Tuple<IEnumerable<UserPrecautionsTableObject>, int>>(responce);
        }
        
        private List<UserPrecaution> GetClubsByPage()
        {
            return new List<UserPrecaution>()
            {
                new UserPrecaution()
                {
                    Number = 2,
                }
            };
        }

        private Tuple<IEnumerable<UserPrecaution>, int> CreateTuple => new Tuple<IEnumerable<UserPrecaution>, int>(GetClubsByPage(), 100);
    }
}
