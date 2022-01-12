using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Handlers.CityHandlers;
using EPlast.BLL.Queries.City;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Handlers.City
{
    public class GetCityUsersHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<IMapper> _mockMapper;
        private GetCityUsersHandler _handler;
        private GetCityUsersQuery _query;

        private const int CityId = 1;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetCityUsersHandler(_mockRepoWrapper.Object, _mockMapper.Object);
            _query = new GetCityUsersQuery(CityId);
        }

        [Test]
        public async Task GetCityUsersHandlerTest_ReturnsCityUsers()
        {
            //Arrange
            _mockRepoWrapper
                .Setup(r => r.CityMembers.GetAllAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<CityMembers>, IIncludableQueryable<CityMembers, object>>>()))
                .ReturnsAsync(GetMembers());
            var users = GetMembers().Select(u => u.User);
            _mockMapper
                .Setup(m => m.Map<IEnumerable<User>, IEnumerable<CityUserDTO>>(users))
                .Returns(GetExpectedUsers());

            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<CityUserDTO>>(result);
        }

        private static IEnumerable<CityUserDTO> GetExpectedUsers()
        {
            return new List<CityUserDTO>
            {
                new CityUserDTO
                {
                    ID = "UserId"
                },
                new CityUserDTO
                {
                    ID = "UserId2"
                }
            };
        }

        private static IEnumerable<CityMembers> GetMembers()
        {
            return new List<CityMembers>
            {
                new CityMembers
                {
                    User = new User
                    {
                        Id = "UserId"
                    }
                },
                new CityMembers
                {
                    User = new User
                    {
                        Id = "UserId2"
                    }
                }
            };
        }
    }
}
