using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Services;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using DataAccessCity = EPlast.DataAccess.Entities;

namespace EPlast.Tests.Services.City
{
    [TestFixture]
    public class CityServiceTests
    {
        private CityService _cityService;
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private Mock<IWebHostEnvironment> _env;
        private Mock<ICityBlobStorageRepository> _cityBlobStorage;
        private Mock<ICityAccessService> _cityAccessService;
        private Mock<UserManager<User>> _userManager;
        private Mock<IUserStore<User>> _user;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _env = new Mock<IWebHostEnvironment>();
            _cityBlobStorage = new Mock<ICityBlobStorageRepository>();
            _cityAccessService = new Mock<ICityAccessService>();
            _user = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(_user.Object, null, null, null, null, null, null, null, null);
            _cityService = new CityService(
                _repoWrapper.Object,
                _mapper.Object,
                _env.Object,
                _cityBlobStorage.Object,
                _cityAccessService.Object,
                _userManager.Object
            );
        }

        [Test]
        public async Task ArchiveAsync_WithMode_ReturnsArchived()
        {
            // Arrange
            CityService cityService = CreateCityService();
            _repoWrapper.Setup(r => r.City.Update(It.IsAny<DataAccessCity.City>()));
            _repoWrapper.Setup(r => r.SaveAsync());

            // Act
            await cityService.ArchiveAsync(Id);

            // Assert
            _repoWrapper.Verify(r => r.City.Update(It.IsAny<DataAccessCity.City>()), Times.Once);
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Test]
        public void ArchiveAsync_CityIsNotEmpty_ThrowInvalidOperationException()
        {
            // Arrange
            _repoWrapper.Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessCity.City, bool>>>(), null))
               .ReturnsAsync(new DataAccessCity.City()
               {
                   CityAdministration = new List<CityAdministration>(),
                   CityMembers = new List<CityMembers>()
               });
            _repoWrapper.Setup(r => r.City.Update(It.IsAny<DataAccessCity.City>()));
            _repoWrapper.Setup(r => r.SaveAsync());

            // Act // Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _cityService.ArchiveAsync(Id));
        }

        [Test]
        public void GetCityHead_ReturnsCityHead_Valid()
        {
            // Arrange
            CityDto cityDTO = new CityDto();
            cityDTO.CityAdministration = new List<CityAdministrationDto>()
            {
                new CityAdministrationDto()
                {
                    AdminType = new AdminTypeDto()
                    {
                        AdminTypeName = Roles.CityHead
                    },
                    Status = true
                }
            };

            // Act
            var result = _cityService.GetCityHead(cityDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CityAdministrationDto>(result);
        }

        [Test]
        public void GetCityHead_ReturnsCityHead_InValid()
        {
            // Arrange
            CityDto cityDTO = new CityDto();
            cityDTO.CityAdministration = new List<CityAdministrationDto>()
            {
                new CityAdministrationDto()
                {
                    AdminType = new AdminTypeDto()
                    {
                        AdminTypeName = Roles.CityHead
                    },
                    EndDate = new DateTime(2000, 10, 5)
                }
            };

            // Act
            var result = _cityService.GetCityHead(cityDTO);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetCityHead_WithoutCityAdministration()
        {
            // Arrange
            CityDto cityDTO = new CityDto();

            // Act
            var result = _cityService.GetCityHead(cityDTO);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetCityHeadDeputy_ReturnsCityHeadDeputy_Valid()
        {
            // Arrange
            CityDto cityDTO = new CityDto();
            cityDTO.CityAdministration = new List<CityAdministrationDto>()
            {
                new CityAdministrationDto()
                {
                    AdminType = new AdminTypeDto()
                    {
                        AdminTypeName = Roles.CityHeadDeputy
                    },
                    Status = true
                }
            };

            // Act
            var result = _cityService.GetCityHeadDeputy(cityDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CityAdministrationDto>(result);
        }

        [Test]
        public void GetCityHeadDeputy_ReturnsCityHeadDeputy_InValid()
        {
            // Arrange
            CityDto cityDTO = new CityDto();
            cityDTO.CityAdministration = new List<CityAdministrationDto>()
            {
                new CityAdministrationDto()
                {
                    AdminType = new AdminTypeDto()
                    {
                        AdminTypeName = Roles.CityHeadDeputy
                    },
                    EndDate = new DateTime(2000, 10, 5)
                }
            };

            // Act
            var result = _cityService.GetCityHeadDeputy(cityDTO);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetCityHeadDeputy_WithoutCityHeadDeputy()
        {
            // Arrange
            CityDto cityDTO = new CityDto();

            // Act
            var result = _cityService.GetCityHeadDeputy(cityDTO);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllActiveAsync_ReturnsAllActiveCities()
        {
            // Arrange
            _repoWrapper.Setup(rw => rw.City.GetAllAsync(It.IsAny<Expression<Func<DataAccessCity.City, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccessCity.City>, IIncludableQueryable<DataAccessCity.City, object>>>()))
                .ReturnsAsync(() => new List<DataAccessCity.City>());

            // Act
            var result = await _cityService.GetAllActiveAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<DataAccessCity.City>>(result);
        }

        [Test]
        public async Task GetAllNotActiveAsync_ReturnsAllNotActiveCities()
        {
            // Arrange
            _repoWrapper.Setup(rw => rw.City.GetAllAsync(It.IsAny<Expression<Func<DataAccessCity.City, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccessCity.City>, IIncludableQueryable<DataAccessCity.City, object>>>()))
                .ReturnsAsync(() => new List<DataAccessCity.City>());

            // Act
            var result = await _cityService.GetAllNotActiveAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<DataAccessCity.City>>(result);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllCities()
        {
            // Arrange
            _repoWrapper.Setup(rw => rw.City.GetAllAsync(It.IsAny<Expression<Func<DataAccessCity.City, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccessCity.City>, IIncludableQueryable<DataAccessCity.City, object>>>()))
                .ReturnsAsync(() => new List<DataAccessCity.City>());

            // Act
            var result = await _cityService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<List<DataAccessCity.City>>(result);
        }

        [Test]
        public async Task GetAllAsync_ReturnCitiesByName()
        {
            // Arrange
            _repoWrapper.Setup(rw => rw.City.GetAllAsync(It.IsAny<Expression<Func<DataAccessCity.City, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccessCity.City>, IIncludableQueryable<DataAccessCity.City, object>>>()))
                .ReturnsAsync(() => new List<DataAccessCity.City>());

            // Act
            var result = await _cityService.GetAllAsync(cityName);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<DataAccessCity.City>>(result);
        }

        [Test]
        public async Task GetAllActiveCitiesAsync_ReturnsAllActiveCities()
        {
            // Arrange
            _mapper
                .Setup(m => m.Map<IEnumerable<DataAccessCity.City>, IEnumerable<CityDto>>(It.IsAny<IEnumerable<DataAccessCity.City>>()))
                .Returns(GetTestCityDTO());
            _repoWrapper
                .Setup(r => r.City.GetAllAsync(It.IsAny<Expression<Func<DataAccessCity.City, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccessCity.City>, IIncludableQueryable<DataAccessCity.City, object>>>()))
                .ReturnsAsync(GetTestCity());

            // Act
            var result = await _cityService.GetAllActiveCitiesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<CityDto>>(result);
        }

        [TestCase]
        public async Task GetAllCitiesByPageAndIsArchiveAsync_NullInput_ReturnsIEnumerableCityObjectDTO()
        {
            // Arrange
            _repoWrapper
                .Setup(x => x.City.GetCitiesObjects(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<UkraineOblasts>()))
                .ReturnsAsync(CreateTuple);
            _cityBlobStorage.Setup(x => x.GetBlobBase64Async(It.IsAny<string>())).Throws(new ArgumentException("Can not get image"));

            // Act
            var result = await _cityService.GetAllCitiesByPageAndIsArchiveAsync(1, 2, null, false);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<Tuple<IEnumerable<CityObjectDto>, int>>(result);
        }

        [Test]
        public async Task GetAllCitiesAsync_ReturnsAllCities()
        {
            // Arrange
            _mapper
                .Setup(m => m.Map<IEnumerable<DataAccessCity.City>, IEnumerable<CityDto>>(It.IsAny<IEnumerable<DataAccessCity.City>>()))
                .Returns(GetTestCityDTO());
            _repoWrapper
                .Setup(r => r.City.GetAllAsync(It.IsAny<Expression<Func<DataAccessCity.City, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccessCity.City>, IIncludableQueryable<DataAccessCity.City, object>>>()))
                .ReturnsAsync(GetTestCity());

            // Act
            var result = await _cityService.GetAllCitiesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<CityDto>>(result);
        }

        [Test]
        public async Task GetAllNotActiveCitiesAsync_ReturnsAllNotActiveCities()
        {
            // Arrange
            _mapper
                .Setup(m => m.Map<IEnumerable<DataAccessCity.City>, IEnumerable<CityDto>>(It.IsAny<IEnumerable<DataAccessCity.City>>()))
                .Returns(GetTestCityDTO());
            _repoWrapper
                .Setup(r => r.City.GetAllAsync(It.IsAny<Expression<Func<DataAccessCity.City, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccessCity.City>, IIncludableQueryable<DataAccessCity.City, object>>>()))
                .ReturnsAsync(GetTestCity());

            // Act
            var result = await _cityService.GetAllNotActiveCitiesAsync();


            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<CityDto>>(result);
        }

        [Test]
        public async Task GetCitiesByRegionAsync_ReturnsCities()
        {
            // Arrange
            _mapper
                .Setup(m => m.Map<IEnumerable<DataAccessCity.City>, IEnumerable<CityDto>>(It.IsAny<IEnumerable<DataAccessCity.City>>()))
                .Returns(GetTestCityDTO());
            _repoWrapper
                .Setup(r => r.City.GetAllAsync(It.IsAny<Expression<Func<DataAccessCity.City, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccessCity.City>, IIncludableQueryable<DataAccessCity.City, object>>>()))
                .ReturnsAsync(GetTestCity());
            _repoWrapper
                .Setup(r => r.CityMembers.GetAllAsync(It.IsAny<Expression<Func<DataAccessCity.CityMembers, bool>>>(),
                 It.IsAny<Func<IQueryable<DataAccessCity.CityMembers>, IIncludableQueryable<DataAccessCity.CityMembers, object>>>()))
                .ReturnsAsync(new List<CityMembers>());

            // Act
            var result = await _cityService.GetCitiesByRegionAsync(Id);


            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<CityDto>>(result);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsCity()
        {
            // Arrange
            _repoWrapper
                .Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessCity.City, bool>>>(), null))
                .ReturnsAsync(new DataAccessCity.City());
            _mapper
                .Setup(m => m.Map<DataAccessCity.City, CityDto>(It.IsAny<DataAccessCity.City>()))
                .Returns(new CityDto());

            _repoWrapper
                .Setup(r => r.CityAdministration.GetAllAsync(It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<CityAdministration>, IIncludableQueryable<CityAdministration, object>>>()))
                .ReturnsAsync(new List<CityAdministration>());
            _repoWrapper
                 .Setup(r => r.CityMembers.GetAllAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                     It.IsAny<Func<IQueryable<CityMembers>, IIncludableQueryable<CityMembers, object>>>()))
                 .ReturnsAsync(new List<CityMembers>());

            // Act
            var result = await _cityService.GetByIdAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<CityDto>(result);
        }

        [Test]
        public async Task GetCityByIdAsync_ReturnsCity()
        {
            _repoWrapper
                .Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessCity.City, bool>>>(), null))
                .ReturnsAsync(new DataAccessCity.City());
            _mapper
                .Setup(m => m.Map<DataAccessCity.City, CityDto>(It.IsAny<DataAccessCity.City>()))
                .Returns(new CityDto());

            _repoWrapper
                .Setup(r => r.CityAdministration.GetAllAsync(It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<CityAdministration>, IIncludableQueryable<CityAdministration, object>>>()))
                .ReturnsAsync(new List<CityAdministration>());

            // Act
            var result = await _cityService.GetCityByIdAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<CityDto>(result);
        }

        [Test]
        public async Task GetCityProfileAsync_ReturnsCityProfile()
        {
            // Arrange
            CityService cityService = CreateCityService();

            // Act
            var result = await cityService.GetCityProfileAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<CityProfileDto>(result);
        }

        [Test]
        public async Task GetCityUsersAsync_CityId_ReturnsCityUsers()
        {
            // Arrange
            _repoWrapper
                .Setup(u => u.CityMembers.GetAllAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(), null));

            // Act
            var result = await _cityService.GetCityUsersAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<CityUserDto[]>(result);
        }

        [Test]
        public async Task GetCityProfileAsync_WhereCityIsNull_ReturnNull()
        {
            // Arrange
            CityService cityService = CreateCityService();
            _mapper.Setup(m => m.Map<DataAccessCity.City, CityDto>(It.IsAny<DataAccessCity.City>()))
                .Returns((CityDto)null);

            // Act
            var result = await cityService.GetCityProfileAsync(Id);

            // Assert
            Assert.Null(result);
        }

        [Test]
        public async Task GetCityProfileAsync_WhereHeadIsNull_ReturnCityProfile()
        {
            // Arrange
            CityService cityService = CreateCityService();
            _mapper.Setup(m => m.Map<DataAccessCity.City, CityDto>(It.IsAny<DataAccessCity.City>()))
                .Returns(CreateFakeCityDtoWithExAdmin(count).FirstOrDefault());

            // Act
            var result = await cityService.GetCityProfileAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Head);
            Assert.AreEqual(result.Admins, new List<CityAdministrationDto>());
        }

        [Test]
        public async Task GetCityProfileAsync_WhereAdminEndDateIsNull_ReturnCityProfile()
        {
            // Arrange
            CityService cityService = CreateCityService();
            _mapper.Setup(m => m.Map<DataAccessCity.City, CityDto>(It.IsAny<DataAccessCity.City>()))
                .Returns(CreateFakeCityDtoWithoutMembersWithoutAdminEndDate(count).FirstOrDefault());

            // Act
            var result = await cityService.GetCityProfileAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Head);
            Assert.NotNull(result.Admins);
            Assert.AreNotEqual(0, result.City.AdministrationCount);
        }

        [Test]
        public async Task GetCityProfileAsync_WithUser_ReturnsCityProfile()
        {
            // Arrange
            CityService cityService = CreateCityService();
            _userManager
                .Setup(u => u.GetUserIdAsync(It.IsAny<DataAccessCity.User>()))
                .ReturnsAsync(stringId);
            _userManager
                .Setup(u => u.GetRolesAsync(It.IsAny<DataAccessCity.User>()))
                .ReturnsAsync(new List<string>());
            _cityAccessService
                .Setup(c => c.HasAccessAsync(It.IsAny<DataAccessCity.User>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _repoWrapper
                .Setup(r => r.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<CityMembers>, IIncludableQueryable<CityMembers, object>>>()))
                .ReturnsAsync(new CityMembers());

            _repoWrapper
                  .Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessCity.City, bool>>>(),
                   It.IsAny<Func<IQueryable<DataAccessCity.City>, IIncludableQueryable<DataAccessCity.City, object>>>()))
                  .ReturnsAsync(new DataAccessCity.City());

            // Act
            var result = await cityService.GetCityProfileAsync(Id, It.IsAny<DataAccessCity.User>());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<CityProfileDto>(result);
        }

        [Test]
        public async Task GetCityMembersAsync_ReturnsCityMembers()
        {
            // Arrange
            CityService cityService = CreateCityService();

            // Act
            var result = await cityService.GetCityMembersAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<CityProfileDto>(result);
        }

        [Test]
        public async Task PlastMemberCheck_UserId_ReturnTrue()
        {
            // Arrange
            User user = new User() { Id = "a" };
            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _userManager
                .Setup(x => x.IsInRoleAsync(user, It.IsAny<string>()))
                .ReturnsAsync(true);
            const bool expected = true;

            // Act
            var result = await _cityService.PlastMemberCheck(It.IsAny<string>());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<bool>(result);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public async Task PlastMemberCheck_UserId_ReturnFalse()
        {
            // Arrange
            User user = new User() { Id = "a" };
            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _userManager
                .Setup(x => x.IsInRoleAsync(user, It.IsAny<string>()))
                .ReturnsAsync(false);
            const bool expected = false;

            // Act
            var result = await _cityService.PlastMemberCheck(It.IsAny<string>());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<bool>(result);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public async Task GetCityMembersAsync_WithCityIsNull_ReturnsNull()
        {
            // Arrange
            CityService cityService = CreateCityService();
            _mapper.Setup(m => m.Map<DataAccessCity.City, CityDto>(It.IsAny<DataAccessCity.City>()))
                .Returns((CityDto)null);

            // Act
            var result = await cityService.GetCityMembersAsync(Id);

            // Assert
            Assert.Null(result);
        }

        [Test]
        public async Task GetCityFollowersAsync_ReturnsCityFollowers()
        {
            // Arrange
            CityService cityService = CreateCityService();

            // Act
            var result = await cityService.GetCityFollowersAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<CityProfileDto>(result);
        }

        [Test]
        public async Task GetCityFollowersAsync_WithCityIsNull_ReturnsNull()
        {
            // Arrange
            CityService cityService = CreateCityService();
            _mapper.Setup(m => m.Map<DataAccessCity.City, CityDto>(It.IsAny<DataAccessCity.City>()))
                .Returns((CityDto)null);

            // Act
            var result = await cityService.GetCityFollowersAsync(Id);

            // Assert
            Assert.Null(result);
        }

        [Test]
        public async Task GetCityAdminsAsync_ReturnsCityAdmins()
        {
            // Arrange
            CityService cityService = CreateCityService();

            // Act
            var result = await cityService.GetCityAdminsAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<CityProfileDto>(result);
        }

        [Test]
        public async Task GetCityAdminsIdsAsync_ReturnsCityAdminsIds()
        {
            // Arrange
            CityService cityService = CreateCityService();

            // Act
            var result = await cityService.GetCityAdminsIdsAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<string>(result);
        }

        [Test]
        public async Task GetCityAdminsIdsAsync_ReturnsNull()
        {
            // Arrange
            CityService cityService = CreateCityService();
            _mapper.Setup(m => m.Map<DataAccessCity.City, CityDto>(It.IsAny<DataAccessCity.City>()))
                .Returns((CityDto)null);

            // Act
            var result = await cityService.GetCityAdminsIdsAsync(Id);

            // Assert
            Assert.Null(result);
        }

        [Test]
        public async Task GetAdministrationAsync_CityId_ReturnClubAdministrtionGetDTO()
        {
            // Arrange
            _repoWrapper.Setup(x => x.CityAdministration.GetAllAsync(It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<CityAdministration>, IIncludableQueryable<CityAdministration, object>>>()))
                .ReturnsAsync(new List<CityAdministration>());

            _mapper.Setup(x => x.Map<IEnumerable<CityAdministration>, IEnumerable<CityAdministrationGetDto>>(It.IsAny<IEnumerable<CityAdministration>>()))
                .Returns(GetFakeAdminDTO());

            //Act
            var result = await _cityService.GetAdministrationAsync(It.IsAny<int>());

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<CityAdministrationGetDto>>(result);
            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public async Task GetCityAdminsAsync_WithCityIsNull_ReturnNull()
        {
            // Arrange
            CityService cityService = CreateCityService();
            _mapper.Setup(m => m.Map<DataAccessCity.City, CityDto>(It.IsAny<DataAccessCity.City>()))
                .Returns((CityDto)null);

            // Act
            var result = await cityService.GetCityAdminsAsync(Id);

            // Assert
            Assert.Null(result);
        }

        [Test]
        public async Task GetCityAdminsAsync_WhereHeadIsNull_ReturnCityProfile()
        {
            // Arrange
            CityService cityService = CreateCityService();
            _mapper.Setup(m => m.Map<DataAccessCity.City, CityDto>(It.IsAny<DataAccessCity.City>()))
                .Returns(CreateFakeCityDtoWithExAdmin(count).FirstOrDefault());

            // Act
            var result = await cityService.GetCityAdminsAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Head);
            Assert.AreEqual(result.Admins, new List<CityAdministrationDto>());
        }

        [Test]
        public async Task GetCityAdminsAsync_WhereAdminEndDateIsNull_ReturnCityProfile()
        {
            // Arrange
            CityService cityService = CreateCityService();
            _mapper.Setup(m => m.Map<DataAccessCity.City, CityDto>(It.IsAny<DataAccessCity.City>()))
                .Returns(CreateFakeCityDtoWithoutMembersWithoutAdminEndDate(count).FirstOrDefault());

            // Act
            var result = await cityService.GetCityAdminsAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Head);
            Assert.NotNull(result.Admins);
            Assert.AreEqual(0, result.City.AdministrationCount);
        }

        [Test]
        public async Task GetCityDocumentsAsync_ReturnsCityDocuments()
        {
            // Arrange
            CityService cityService = CreateCityService();

            // Act
            var result = await cityService.GetCityDocumentsAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<CityProfileDto>(result);
        }

        [Test]
        public async Task GetCityDocumentsAsync_WithCityIsNull_ReturnNull()
        {
            // Arrange
            CityService cityService = CreateCityService();
            _mapper.Setup(m => m.Map<DataAccessCity.City, CityDto>(It.IsAny<DataAccessCity.City>()))
                .Returns((CityDto)null);

            // Act
            var result = await cityService.GetCityDocumentsAsync(Id);

            // Assert
            Assert.Null(result);
        }

        [Test]
        public async Task GetLogoBase64_ReturnLogoBase64()
        {
            // Arrange
            CityService cityService = CreateCityService();
            _cityBlobStorage.Setup(c => c.GetBlobBase64Async(It.IsAny<string>())).ReturnsAsync(logoName);

            // Act
            var result = await cityService.GetLogoBase64(logoName);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(logoName, result);
        }

        [Test]
        public async Task RemoveAsync()
        {
            // Arrange
            CityService cityService = CreateCityService();
            _cityBlobStorage.Setup(c => c.DeleteBlobAsync(It.IsAny<string>()));
            _repoWrapper.Setup(r => r.City.Delete(It.IsAny<DataAccessCity.City>()));
            _repoWrapper.Setup(r => r.SaveAsync());

            // Act
            await cityService.RemoveAsync(It.IsAny<int>());

            // Assert
            _cityBlobStorage.Verify(c => c.DeleteBlobAsync(It.IsAny<string>()), Times.Once);
            _repoWrapper.Verify(r => r.City.Delete(It.IsAny<DataAccessCity.City>()), Times.Once);
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task RemoveAsync_WithoutLogo()
        {
            // Arrange
            CityService cityService = CreateCityService();
            _repoWrapper.Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessCity.City, bool>>>(), null))
                .ReturnsAsync(GetTestCityWithoutLogo());
            _cityBlobStorage.Setup(c => c.DeleteBlobAsync(It.IsAny<string>()));
            _repoWrapper.Setup(r => r.City.Delete(It.IsAny<DataAccessCity.City>()));
            _repoWrapper.Setup(r => r.SaveAsync());

            // Act
            await cityService.RemoveAsync(It.IsAny<int>());

            // Assert
            _cityBlobStorage.Verify(c => c.DeleteBlobAsync(It.IsAny<string>()), Times.Never);
            _repoWrapper.Verify(r => r.City.Delete(It.IsAny<DataAccessCity.City>()), Times.Once);
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task EditAsync_ReturnsCityEdited()
        {
            // Arrange
            CityService cityService = CreateCityService();

            // Act
            var result = await cityService.EditAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<CityProfileDto>(result);
        }

        [Test]
        public async Task EditAsync_ReturnNull()
        {
            // Arrange
            CityService cityService = CreateCityService();
            _mapper.Setup(m => m.Map<DataAccessCity.City, CityDto>(It.IsAny<DataAccessCity.City>()))
                .Returns(new List<CityDto>().FirstOrDefault());

            // Act
            var result = await cityService.EditAsync(Id);

            // Assert
            Assert.Null(result);
        }

        [Test]
        public async Task EditAsync_WhereMembersIsNull_ReturnsCityEdited()
        {
            // Arrange
            CityService cityService = CreateCityService();
            _mapper.Setup(m => m.Map<DataAccessCity.City, CityDto>(It.IsAny<DataAccessCity.City>()))
                .Returns(CreateFakeCityDtoWithoutMembersWithoutAdminEndDate(count).FirstOrDefault());

            // Act
            var result = await cityService.EditAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<CityProfileDto>(result);
            Assert.AreEqual(new List<CityMembersDto>(), result.Members);
        }

        [Test]
        public async Task EditAsync_WithModelAndFormFile_ReturnsCityEdited()
        {
            // Arrange
            CityService cityService = CreateCityService();
            CityProfileDto cityProfileDto = new CityProfileDto
            {
                City = new CityDto
                {
                    ID = 0
                }
            };
            _repoWrapper.Setup(r => r.City.Attach(It.IsAny<DataAccessCity.City>()));
            _repoWrapper.Setup(r => r.City.Update(It.IsAny<DataAccessCity.City>()));
            _repoWrapper.Setup(r => r.SaveAsync());

            // Act
            await cityService.EditAsync(cityProfileDto, It.IsAny<IFormFile>());

            // Assert
            _repoWrapper.Verify(r => r.City.Attach(It.IsAny<DataAccessCity.City>()), Times.Once);
            _repoWrapper.Verify(r => r.City.Update(It.IsAny<DataAccessCity.City>()), Times.Once);
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task EditAsync_WithModel_ReturnsCityEdited()
        {
            // Arrange
            CityService cityService = CreateCityService();
            CityDto cityDto = new CityDto
            {
                ID = 0
            };
            _repoWrapper.Setup(r => r.City.Attach(It.IsAny<DataAccessCity.City>()));
            _repoWrapper.Setup(r => r.City.Update(It.IsAny<DataAccessCity.City>()));
            _repoWrapper.Setup(r => r.SaveAsync());

            // Act
            await cityService.EditAsync(cityDto);

            // Assert
            _repoWrapper.Verify(r => r.City.Attach(It.IsAny<DataAccessCity.City>()), Times.Once);
            _repoWrapper.Verify(r => r.City.Update(It.IsAny<DataAccessCity.City>()), Times.Once);
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task CreateAsync_ReturnCityDtoID()
        {
            // Arrange
            CityService cityService = CreateCityService();
            CityDto cityDto = new CityDto
            {
                ID = 0
            };
            _cityBlobStorage.Setup(b => b.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()));
            _cityBlobStorage.Setup(b => b.DeleteBlobAsync(It.IsAny<string>()));

            // Act
            var result = await cityService.CreateAsync(cityDto);

            // Assert
            Assert.AreEqual(cityDto.ID, result);
            _cityBlobStorage.Verify(b => b.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _cityBlobStorage.Verify(b => b.DeleteBlobAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_WithCityDtoLogo_ReturnCityDtoID()
        {
            // Arrange
            CityService cityService = CreateCityService();
            CityDto cityDto = new CityDto
            {
                ID = 0,
                Logo = "data:application/.jpeg;base64,/9j/"
            };
            _cityBlobStorage.Setup(b => b.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()));
            _cityBlobStorage.Setup(b => b.DeleteBlobAsync(It.IsAny<string>()));

            // Act
            var result = await cityService.CreateAsync(cityDto);

            // Assert
            Assert.AreEqual(cityDto.ID, result);
            _cityBlobStorage.Verify(b => b.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _cityBlobStorage.Verify(b => b.DeleteBlobAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_WithCityDtoLogo_CityIsNull_ReturnCityDtoID()
        {
            // Arrange
            CityService cityService = CreateCityService();
            CityDto cityDto = new CityDto
            {
                ID = 0,
                Logo = "data:application/.jpeg;base64,/9j/"
            };
            _repoWrapper.Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessCity.City, bool>>>(), null))
                .ReturnsAsync((DataAccessCity.City)null);
            _cityBlobStorage.Setup(b => b.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()));


            // Act
            var result = await cityService.CreateAsync(cityDto);

            // Assert
            Assert.AreEqual(cityDto.ID, result);
            _cityBlobStorage.Verify(b => b.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _cityBlobStorage.Verify(b => b.DeleteBlobAsync(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task CreateAsync_WithCityDtoLogo_ExtensionIsEmpty_ReturnCityDtoID()
        {
            // Arrange
            CityService cityService = CreateCityService();
            CityDto cityDto = new CityDto
            {
                ID = 0,
                Logo = "data:application/,/9j/"
            };
            _cityBlobStorage.Setup(b => b.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()));

            // Act
            var result = await cityService.CreateAsync(cityDto);

            // Assert
            Assert.AreEqual(cityDto.ID, result);
            _cityBlobStorage.Verify(b => b.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_WithCityDtoLogo_ExtensionWithoutPoint_ReturnCityDtoID()
        {
            // Arrange
            CityService cityService = CreateCityService();
            CityDto cityDto = new CityDto
            {
                ID = 0,
                Logo = "data:application/base64,/9j/"
            };
            _cityBlobStorage.Setup(b => b.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()));

            // Act
            var result = await cityService.CreateAsync(cityDto);

            // Assert
            Assert.AreEqual(cityDto.ID, result);
            _cityBlobStorage.Verify(b => b.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_WhereFormFileIsNull_ReturnCityDtoID()
        {
            // Arrange
            CityService cityService = CreateCityService();
            CityProfileDto cityProfileDto = new CityProfileDto
            {
                City = new CityDto
                {
                    ID = 0
                }
            };

            // Act
            var result = await cityService.CreateAsync(cityProfileDto, null);

            // Assert
            Assert.AreEqual(cityProfileDto.City.ID, result);
        }

        [Test]
        public async Task CreateAsync_WithOldImageName_WhereRegionIsNullFormFileIsNull_ReturnCityDtoID()
        {
            // Arrange
            CityService cityService = CreateCityService();
            CityProfileDto cityProfileDto = new CityProfileDto
            {
                City = new CityDto
                {
                    ID = 0
                },
            };
            _mapper.Setup(m => m.Map<CityDto, DataAccessCity.City>(It.IsAny<CityDto>()))
                .Returns(GetTestNewCity());
            _repoWrapper.Setup(r => r.Region.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Region, bool>>>(), null))
                .ReturnsAsync((Region)null);
            _repoWrapper.Setup(r => r.Region.CreateAsync(It.IsAny<Region>()));

            // Act
            var result = await cityService.CreateAsync(cityProfileDto, null);

            // Assert
            Assert.AreEqual(cityProfileDto.City.ID, result);
            _repoWrapper.Verify(r => r.Region.CreateAsync(It.IsAny<Region>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_WithOldImageName_WhereFormFileIsNull_ReturnCityDtoID()
        {
            // Arrange
            CityService cityService = CreateCityService();
            CityProfileDto cityProfileDto = new CityProfileDto
            {
                City = new CityDto
                {
                    ID = 0
                }
            };
            _repoWrapper.Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessCity.City, bool>>>(), null))
                .ReturnsAsync((DataAccessCity.City)null);

            // Act
            var result = await cityService.CreateAsync(cityProfileDto, null);

            // Assert
            Assert.AreEqual(cityProfileDto.City.ID, result);
        }

        [Test]
        public async Task GetCities_ReturnCityForAdministrationDTOs()
        {
            // Arrange
            CityService cityService = CreateCityService();

            // Act
            var result = await cityService.GetCities();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<CityForAdministrationDto>>(result);
        }

        [Test]
        public async Task UnArchiveAsync_WithMode_ReturnsArchived()
        {
            // Arrange
            CityService cityService = CreateCityService();
            _repoWrapper.Setup(r => r.City.Update(It.IsAny<DataAccessCity.City>()));
            _repoWrapper.Setup(r => r.SaveAsync());

            // Act
            await cityService.UnArchiveAsync(Id);

            // Assert
            _repoWrapper.Verify(r => r.City.Update(It.IsAny<DataAccessCity.City>()), Times.Once);
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task GetCityIdByUserIdAsync_ReturnNotNull()
        {
            // Arrange
            _repoWrapper.Setup(x => x.CityMembers.GetFirstAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(), null))
                .ReturnsAsync(new CityMembers());

            // Act
            var result = await _cityService.GetCityIdByUserIdAsync(It.IsAny<string>());

            // Assert
            Assert.NotNull(result);
        }

        private int Id => 1;
        private string stringId => "1";
        private int count => 2;
        private string logoName => "logoName";
        private string cityName => "cityName";

        private IEnumerable<CityDto> GetTestCityDTO()
        {
            return new List<CityDto>
            {
                new CityDto{Name = "Львів"},
                new CityDto{Name = "Стрий"},
                new CityDto{Name = "Миколаїв"}
            }.AsEnumerable();
        }

        private Tuple<IEnumerable<CityObject>, int> CreateTuple => new Tuple<IEnumerable<CityObject>, int>(CreateCityObjects, 100);

        private IEnumerable<CityObject> CreateCityObjects => new List<CityObject>()
        {
            new CityObject(){ Logo = "logo.png"},
            new CityObject()
        };

        private IEnumerable<DataAccessCity.City> GetTestCity()
        {
            return new List<DataAccessCity.City>
            {
                new DataAccessCity.City{
                    Name = "Львів",
                    CityAdministration = new List<CityAdministration>(),
                    CityMembers = new List<CityMembers>(),
                    CityDocuments = new List<CityDocuments>(),
                    Region = new Region(),
                    },
                new DataAccessCity.City{
                    Name = "Стрий",
                    CityAdministration = new List<CityAdministration>(),
                    CityMembers = new List<CityMembers>(),
                    CityDocuments = new List<CityDocuments>(),
                    Region = new Region(),
                }
            }.AsEnumerable();
        }

        private CityService CreateCityService()
        {
            _mapper.Setup(m => m.Map<IEnumerable<DataAccessCity.City>,
                    IEnumerable<CityDto>>(It.IsAny<IEnumerable<DataAccessCity.City>>()))
                .Returns(CreateFakeCityDto(count));
            _mapper.Setup(m => m.Map<DataAccessCity.City, CityDto>(It.IsAny<DataAccessCity.City>()))
                .Returns(CreateFakeCityDto(count).FirstOrDefault());
            _mapper.Setup(m => m.Map<CityDto, DataAccessCity.City>(It.IsAny<CityDto>()))
                .Returns(() => new DataAccessCity.City());
            _repoWrapper.Setup(r => r.City.FindAll())
                .Returns(CreateFakeCities(count));
            _repoWrapper.Setup(r => r.City.FindByCondition(It.IsAny<Expression<Func<DataAccessCity.City, bool>>>()))
                .Returns((Expression<Func<DataAccessCity.City, bool>> condition) =>
                    CreateFakeCities(count).Where(condition));
            _repoWrapper.Setup(r => r.Region.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Region, bool>>>(), null))
               .ReturnsAsync(GetTestRegion());
            _repoWrapper.Setup(r => r.City.Update(It.IsAny<DataAccessCity.City>()))
                .Verifiable();
            _repoWrapper.Setup(r => r.City.Create(It.IsAny<DataAccessCity.City>()))
                .Verifiable();
            _repoWrapper.Setup(r => r.Save())
                .Verifiable();
            _repoWrapper.Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessCity.City, bool>>>(), null))
                .ReturnsAsync(GetTestNewCity());
            _repoWrapper
                 .Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessCity.City, bool>>>(),
                  It.IsAny<Func<IQueryable<DataAccessCity.City>, IIncludableQueryable<DataAccessCity.City, object>>>()))
                 .ReturnsAsync(GetTestNewCity());

            _repoWrapper
                  .Setup(r => r.CityAdministration.GetAllAsync(It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                      It.IsAny<Func<IQueryable<CityAdministration>, IIncludableQueryable<CityAdministration, object>>>()))
                  .ReturnsAsync(new List<CityAdministration>());
            _repoWrapper
                 .Setup(r => r.CityMembers.GetAllAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                     It.IsAny<Func<IQueryable<CityMembers>, IIncludableQueryable<CityMembers, object>>>()))
                 .ReturnsAsync(new List<CityMembers>());

            return new CityService(
                _repoWrapper.Object,
                _mapper.Object,
                _env.Object,
                _cityBlobStorage.Object,
                _cityAccessService.Object,
                _userManager.Object
            );
        }

        private IQueryable<DataAccessCity.City> CreateFakeCities(int count)
        {
            List<DataAccessCity.City> cities = new List<DataAccessCity.City>();
            for (int i = 0; i < count; i++)
            {
                cities.Add(new DataAccessCity.City());
            }
            return cities.AsQueryable();
        }

        private IQueryable<CityDto> CreateFakeCityDto(int count)
        {
            List<CityDto> cities = new List<CityDto>();

            for (int i = 0; i < count; i++)
            {
                cities.Add(new CityDto
                {
                    CityAdministration = GetCityAdministrationDTO(),
                    CityMembers = new List<CityMembersDto>
                    {
                        new CityMembersDto
                        {
                            StartDate = new Random().Next(0,1) ==1 ? DateTime.Today : (DateTime?) null,
                            IsApproved = true,
                            UserId = "5"
                        }
                    },
                    CityDocuments = new List<CityDocumentsDto>
                    {
                        new CityDocumentsDto(),
                        new CityDocumentsDto(),
                        new CityDocumentsDto(),
                        new CityDocumentsDto(),
                        new CityDocumentsDto()
                    },
                });
            }
            return cities.AsQueryable();
        }

        private List<CityAdministrationDto> GetCityAdministrationDTO()
        {
            return new List<CityAdministrationDto>
            {
                 new CityAdministrationDto
                 {

                      AdminType = new AdminTypeDto
                      {
                           AdminTypeName = Roles.CityHead
                      },
                      Status = true
                 },
                 new CityAdministrationDto
                 {
                      AdminType = new AdminTypeDto
                      {
                           AdminTypeName = "----------"
                      },
                      Status = true
                 },
                 new CityAdministrationDto
                 {
                       AdminType = new AdminTypeDto
                       {
                            AdminTypeName = Roles.CityHead
                       },
                       Status = true
                 },
                 new CityAdministrationDto
                 {
                       AdminType = new AdminTypeDto
                       {
                            AdminTypeName = "----------"
                       },
                       Status = true
                 }
            };
        }

        private IQueryable<CityDto> CreateFakeCityDtoWithExAdmin(int count)
        {
            List<CityDto> cities = new List<CityDto>();

            for (int i = 0; i < count; i++)
            {
                var cityDto = GetCityDto();
                cityDto.CityAdministration = new List<CityAdministrationDto>
                {
                    new CityAdministrationDto
                    {
                        AdminType = new AdminTypeDto
                        {
                            AdminTypeName = Roles.CityHead
                        },
                        EndDate = DateTime.Now.AddMonths(-3),
                        Status = false
                    },
                    new CityAdministrationDto
                    {
                        AdminType = new AdminTypeDto
                        {
                            AdminTypeName = "----------",
                        },
                        EndDate = DateTime.Now.AddMonths(-3),
                        Status = false
                    }
                };
                cities.Add(cityDto);
            }
            return cities.AsQueryable();
        }

        private IQueryable<CityDto> CreateFakeCityDtoWithoutMembersWithoutAdminEndDate(int count)
        {
            List<CityDto> cities = new List<CityDto>();

            for (int i = 0; i < count; i++)
            {
                var cityDto = GetCityDtoWithoutMembers();
                cityDto.CityMembers = new List<CityMembersDto>();
                cityDto.CityAdministration = new List<CityAdministrationDto>
                {
                    new CityAdministrationDto
                    {
                        AdminType = new AdminTypeDto
                        {
                            AdminTypeName = Roles.CityHead
                        },
                        Status = true
                    },
                    new CityAdministrationDto
                    {
                        AdminType = new AdminTypeDto
                        {
                            AdminTypeName = "----------",
                        },
                        Status = true
                    }
                };
                cities.Add(cityDto);
            }
            return cities.AsQueryable();
        }

        private CityDto GetCityDto()
        {
            var city = GetCityDtoWithoutMembers();
            city.CityMembers = new List<CityMembersDto>
                {
                    new CityMembersDto
                    {
                        StartDate = new Random().Next(0, 1) == 1 ? DateTime.Today : (DateTime?)null
                    }
                };
            return city;
        }

        private CityDto GetCityDtoWithoutMembers()
        {
            return new CityDto
            {
                CityAdministration = GetCityAdministrationDTO(),
                CityDocuments = new List<CityDocumentsDto>
                    {
                        new CityDocumentsDto(),
                        new CityDocumentsDto(),
                        new CityDocumentsDto(),
                        new CityDocumentsDto(),
                        new CityDocumentsDto()
                    },
                Region = new BLL.DTO.Region.RegionDto()
            };
        }

        private Region GetTestRegion()
        {
            return new Region()
            {
                ID = 1,
                RegionName = "Lviv",
                Description = "Lviv region"
            };
        }

        private DataAccessCity.City GetTestNewCity()
        {
            var city = GetTestCityWithoutLogo();
            city.Logo = "710b8b06-6869-45db-894f-7a0b131e6c6b.jpg";
            city.Region = GetTestRegion();

            return city;
        }

        private IEnumerable<CityAdministrationGetDto> GetFakeAdminDTO()
        {
            return new List<CityAdministrationGetDto>() {
                new CityAdministrationGetDto(){ Id = 2, AdminTypeId = 2, CityId = 2 },
                new CityAdministrationGetDto(){ Id = 3, AdminTypeId = 3, CityId = 3 },
                new CityAdministrationGetDto(){ Id = 4, AdminTypeId = 4, CityId = 4 }
            };
        }

        private DataAccessCity.City GetTestCityWithoutLogo()
        {
            var city = new DataAccessCity.City
            {
                Name = "city"
            };
            return city;
        }
    }
}
