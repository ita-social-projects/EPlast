using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Services.City;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DataAccessCity = EPlast.DataAccess.Entities;


namespace EPlast.Tests.Services.City
{
    [TestFixture]
    public class CityMembersServiceTests
    {
        private CityMembersService _cityMembersService;
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private Mock<UserManager<User>> _userManager;
        private Mock<IUserStore<User>> _user;
        private Mock<ICityAdministrationService> _cityAdministrationService;
        private Mock<ICityService> _cityService;
        private Mock<IUserManagerService> _userManagerService;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _cityAdministrationService = new Mock<ICityAdministrationService>();
            _user = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(_user.Object, null, null, null, null, null, null, null, null);
            _userManagerService = new Mock<IUserManagerService>();
            _cityService = new Mock<ICityService>();
            _cityMembersService = new CityMembersService(_repoWrapper.Object, _mapper.Object, _userManager.Object,
                _cityAdministrationService.Object);
        }


        [Test]
        public async Task GetMembersByCityIdAsync_ReturnsMembers()
        {
            // Arrange
            _repoWrapper.Setup(r => r.CityMembers.GetAllAsync(It.IsAny<Expression<Func<DataAccessCity.CityMembers, bool>>>(),
            It.IsAny<Func<IQueryable<DataAccessCity.CityMembers>, IIncludableQueryable<DataAccessCity.CityMembers, object>>>()))
            .ReturnsAsync(new List<DataAccessCity.CityMembers> { new DataAccessCity.CityMembers() }); ;

            // Act
            var result = await _cityMembersService.GetMembersByCityIdAsync(It.IsAny<int>());


            // Assert
            Assert.NotNull(result);
            _mapper.Verify(m => m.Map<IEnumerable<DataAccessCity.CityMembers>, IEnumerable<CityMembersDTO>>(It.IsAny<IEnumerable<DataAccessCity.CityMembers>>()));
        }

        [Test]
        public async Task ToggleApproveStatusAsyncTest()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessCity.CityMembers, bool>>>(),
            It.IsAny<Func<IQueryable<DataAccessCity.CityMembers>, IIncludableQueryable<DataAccessCity.CityMembers, object>>>()))
                .ReturnsAsync(new CityMembers());

            //Act
            await _cityMembersService.ToggleApproveStatusAsync(It.IsAny<int>());

            //Assert
            _repoWrapper.Verify(i => i.CityMembers.Update(It.IsAny<CityMembers>()), Times.Once());
        }
    }
}

