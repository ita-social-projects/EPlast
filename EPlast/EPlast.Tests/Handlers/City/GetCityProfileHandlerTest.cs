using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Handlers.CityHandlers;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Queries.City;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Handlers.City
{
    public class GetCityProfileHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<IMediator> _mockMediator;
        private Mock<UserManager<User>> _mockUserManager;
        private Mock<IUserStore<User>> _mockUser;
        private Mock<ICityAccessService> _mockCityAccessService;
        private GetCityProfileHandler _handler;
        private GetCityProfileQuery _query;

        private User _testUser;
        private const int CityId = 1;
        private const string UserId = "UserId";
        
        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMediator = new Mock<IMediator>();
            _mockUser = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(_mockUser.Object, null, null, null, null, null, null, null, null);
            _mockCityAccessService = new Mock<ICityAccessService>();
            _handler = new GetCityProfileHandler(_mockMediator.Object, 
                _mockRepoWrapper.Object, _mockUserManager.Object, _mockCityAccessService.Object);
            _testUser = new User();
            _query = new GetCityProfileQuery(CityId, _testUser);
        }

        [Test]
        public async Task GetCityProfileHandlerTest_ReturnsCityProfile()
        {
            //Arrange
            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetCityProfileBasicQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetCityProfile());
            _mockUserManager
                .Setup(u => u.GetUserIdAsync(It.IsAny<User>()))
                .ReturnsAsync(UserId);
            _mockUserManager
                .Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(GetRoles());
            _mockCityAccessService
                .Setup(a => a.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _mockRepoWrapper
                .Setup(r => r.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<CityMembers>, IIncludableQueryable<CityMembers, object>>>()))
                .ReturnsAsync(new CityMembers());
            
            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CityProfileDto>(result);
        }

        private static List<string> GetRoles()
        {
            return new List<string>
            {
                Roles.Admin,
                "Role1",
                "Role2"
            };
        }

        private static CityProfileDto GetCityProfile()
        {
            return new CityProfileDto
            {
                City = new CityDto
                {
                    CanCreate = true,
                    CanEdit = true,
                    CanJoin = true
                }
            };
        }
    }
}
