using AutoMapper;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Services;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataAccessCity = EPlast.DataAccess.Entities;
using EPlast.Resources;

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
        private Mock<IUniqueIdService> _uniqueId;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _env = new Mock<IWebHostEnvironment>();
            _cityBlobStorage = new Mock<ICityBlobStorageRepository>();
            _cityAccessService = new Mock<ICityAccessService>();
            _user = new Mock<IUserStore<User>>();
            _uniqueId = new Mock<IUniqueIdService>();
            _userManager = new Mock<UserManager<User>>(_user.Object, null, null, null, null, null, null, null, null);
            _cityService = new CityService(_repoWrapper.Object, _mapper.Object, _env.Object, _cityBlobStorage.Object,
                   _cityAccessService.Object, _userManager.Object, _uniqueId.Object);
        }

        [Test]
        public void GetCityHead_ReturnsCityHead_Valid()
        {
            // Arrange
            CityDTO cityDTO = new CityDTO();
            cityDTO.CityAdministration = new List<CityAdministrationDTO>() 
            { 
                new CityAdministrationDTO()
                {
                    AdminType = new AdminTypeDTO()
                    {
                        AdminTypeName = Roles.CityHead
                    }
                }
            };

            // Act
            var result = _cityService.GetCityHead(cityDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CityAdministrationDTO>(result);
        }

        [Test]
        public void GetCityHead_ReturnsCityHead_InValid()
        {
            // Arrange
            CityDTO cityDTO = new CityDTO();
            cityDTO.CityAdministration = new List<CityAdministrationDTO>()
            {
                new CityAdministrationDTO()
                {
                    AdminType = new AdminTypeDTO()
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
            CityDTO cityDTO = new CityDTO();

            // Act
            var result = _cityService.GetCityHead(cityDTO);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetCityHeadDeputy_ReturnsCityHeadDeputy_Valid()
        {
            // Arrange
            CityDTO cityDTO = new CityDTO();
            cityDTO.CityAdministration = new List<CityAdministrationDTO>()
            {
                new CityAdministrationDTO()
                {
                    AdminType = new AdminTypeDTO()
                    {
                        AdminTypeName = Roles.CityHeadDeputy
                    }
                }
            };

            // Act
            var result = _cityService.GetCityHeadDeputy(cityDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CityAdministrationDTO>(result);
        }

        [Test]
        public void GetCityHeadDeputy_ReturnsCityHeadDeputy_InValid()
        {
            // Arrange
            CityDTO cityDTO = new CityDTO();
            cityDTO.CityAdministration = new List<CityAdministrationDTO>()
            {
                new CityAdministrationDTO()
                {
                    AdminType = new AdminTypeDTO()
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
            CityDTO cityDTO = new CityDTO();

            // Act
            var result = _cityService.GetCityHeadDeputy(cityDTO);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllCities()
        {
            // Arrange
            _repoWrapper.Setup(rw => rw.City.GetAllAsync(It.IsAny<Expression<Func<DataAccessCity.City, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccessCity.City>, IIncludableQueryable<DataAccessCity.City, object>>>()))
                .ReturnsAsync(() => new List<DataAccessCity.City> ());

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
        public async Task GetAllDTOAsync_ReturnsAllDTO()
        {
            // Arrange
            _mapper
                .Setup(m => m.Map< IEnumerable < DataAccessCity.City > ,IEnumerable <CityDTO>>(It.IsAny<IEnumerable<DataAccessCity.City>>()))
                .Returns(GetTestCityDTO());
            _repoWrapper
                .Setup(r => r.City.GetAllAsync(It.IsAny<Expression<Func<DataAccessCity.City, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccessCity.City>, IIncludableQueryable<DataAccessCity.City, object>>>()))
                .ReturnsAsync(GetTestCity());

            // Act
            var result = await _cityService.GetAllDTOAsync();


            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<CityDTO>>(result);
        }

        [Test]
        public async Task GetCitiesByRegionAsync_ReturnsCities()
        {
            // Arrange
            _mapper
                .Setup(m => m.Map<IEnumerable<DataAccessCity.City>, IEnumerable<CityDTO>>(It.IsAny<IEnumerable<DataAccessCity.City>>()))
                .Returns(GetTestCityDTO());
            _repoWrapper
                .Setup(r => r.City.GetAllAsync(It.IsAny<Expression<Func<DataAccessCity.City, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccessCity.City>, IIncludableQueryable<DataAccessCity.City, object>>>()))
                .ReturnsAsync(GetTestCity());

            // Act
            var result = await _cityService.GetCitiesByRegionAsync(Id);


            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<CityDTO>>(result);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsCity()
        {
            // Arrange
            _repoWrapper
                .Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessCity.City, bool>>>(), null))
                .ReturnsAsync(new DataAccessCity.City());        
            _mapper
                .Setup(m => m.Map<DataAccessCity.City, CityDTO>(It.IsAny<DataAccessCity.City>()))
                .Returns( new CityDTO());
            
            // Act
            var result = await _cityService.GetByIdAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<CityDTO>(result);
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
            Assert.IsInstanceOf<CityProfileDTO>(result);
        }

        [Test]
        public async Task GetCityProfileAsync_WhereCityIsNull_ReturnNull()
        {
            // Arrange
            CityService cityService = CreateCityService();
            _mapper.Setup(m=>m.Map<DataAccessCity.City, CityDTO>(It.IsAny<DataAccessCity.City>()))
                .Returns((CityDTO)null);

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
            _mapper.Setup(m => m.Map<DataAccessCity.City, CityDTO>(It.IsAny<DataAccessCity.City>()))
                .Returns(CreateFakeCityDtoWithExAdmin(count).FirstOrDefault());

            // Act
            var result = await cityService.GetCityProfileAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Head);
            Assert.AreEqual(result.Admins, new List<CityAdministrationDTO>());
        }

        [Test]
        public async Task GetCityProfileAsync_WhereAdminEndDateIsNull_ReturnCityProfile()
        {
            // Arrange
            CityService cityService = CreateCityService();
            _mapper.Setup(m => m.Map<DataAccessCity.City, CityDTO>(It.IsAny<DataAccessCity.City>()))
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
            var mockList = new Mock<IList<string>>();
            mockList
                .Setup(m => m.Contains(It.IsAny<string>()))
                .Returns(true);
            _cityAccessService
                .Setup(c => c.HasAccessAsync(It.IsAny<DataAccessCity.User>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _repoWrapper
                .Setup(r => r.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<CityMembers>, IIncludableQueryable<CityMembers, object>>>()))
                .ReturnsAsync(new CityMembers());

            // Act
            var result = await cityService.GetCityProfileAsync(Id, It.IsAny<DataAccessCity.User>());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<CityProfileDTO>(result);
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
            Assert.IsInstanceOf<CityProfileDTO>(result);
        }

        [Test]
        public async Task GetCityMembersAsync_WithCityIsNull_ReturnsNull()
        {
            // Arrange
            CityService cityService = CreateCityService();
            _mapper.Setup(m => m.Map<DataAccessCity.City, CityDTO>(It.IsAny<DataAccessCity.City>()))
                .Returns((CityDTO)null);

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
            Assert.IsInstanceOf<CityProfileDTO>(result);
        }

        [Test]
        public async Task GetCityFollowersAsync_WithCityIsNull_ReturnsNull()
        {
            // Arrange
            CityService cityService = CreateCityService();
            _mapper.Setup(m => m.Map<DataAccessCity.City, CityDTO>(It.IsAny<DataAccessCity.City>()))
                .Returns((CityDTO)null);

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
            Assert.IsInstanceOf<CityProfileDTO>(result);
        }

        [Test]
        public async Task GetCityAdminsAsync_WithCityIsNull_ReturnNull()
        {
            // Arrange
            CityService cityService = CreateCityService();
            _mapper.Setup(m => m.Map<DataAccessCity.City, CityDTO>(It.IsAny<DataAccessCity.City>()))
                .Returns((CityDTO)null);

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
            _mapper.Setup(m => m.Map<DataAccessCity.City, CityDTO>(It.IsAny<DataAccessCity.City>()))
                .Returns(CreateFakeCityDtoWithExAdmin(count).FirstOrDefault());

            // Act
            var result = await cityService.GetCityAdminsAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Head);
            Assert.AreEqual(result.Admins, new List<CityAdministrationDTO>());
        }

        [Test]
        public async Task GetCityAdminsAsync_WhereAdminEndDateIsNull_ReturnCityProfile()
        {
            // Arrange
            CityService cityService = CreateCityService();
            _mapper.Setup(m => m.Map<DataAccessCity.City, CityDTO>(It.IsAny<DataAccessCity.City>()))
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
            Assert.IsInstanceOf<CityProfileDTO>(result);
        }

        [Test]
        public async Task GetCityDocumentsAsync_WithCityIsNull_ReturnNull()
        {
            // Arrange
            CityService cityService = CreateCityService();
            _mapper.Setup(m => m.Map<DataAccessCity.City, CityDTO>(It.IsAny<DataAccessCity.City>()))
                .Returns((CityDTO)null);

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
            Assert.IsInstanceOf<CityProfileDTO>(result);
        }

        [Test]
        public async Task EditAsync_ReturnNull()
        {
            // Arrange
            CityService cityService = CreateCityService();
            _mapper.Setup(m => m.Map<DataAccessCity.City, CityDTO>(It.IsAny<DataAccessCity.City>()))
                .Returns(new List<CityDTO>().FirstOrDefault());

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
            _mapper.Setup(m => m.Map<DataAccessCity.City, CityDTO>(It.IsAny<DataAccessCity.City>()))
                .Returns(CreateFakeCityDtoWithoutMembersWithoutAdminEndDate(count).FirstOrDefault());

            // Act
            var result = await cityService.EditAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<CityProfileDTO>(result);
            Assert.AreEqual(new List<CityMembersDTO>(), result.Members);
        }

        [Test]
        public async Task EditAsync_WithModelAndFormFile_ReturnsCityEdited()
        {
            // Arrange
            CityService cityService = CreateCityService();
            CityProfileDTO cityProfileDto = new CityProfileDTO
            {
                City = new CityDTO
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
            CityDTO cityDto = new CityDTO
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
            CityDTO cityDto = new CityDTO
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
            CityDTO cityDto = new CityDTO
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
            CityDTO cityDto = new CityDTO
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
            CityDTO cityDto = new CityDTO
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
            CityDTO cityDto = new CityDTO
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
            CityProfileDTO cityProfileDto = new CityProfileDTO
            {
                City = new CityDTO
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
            CityProfileDTO cityProfileDto = new CityProfileDTO
            {
                City = new CityDTO
                {
                    ID = 0
                },
            };
            _mapper.Setup(m => m.Map<CityDTO, DataAccessCity.City>(It.IsAny<CityDTO>()))
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
            CityProfileDTO cityProfileDto = new CityProfileDTO
            {
                City = new CityDTO
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
            Assert.IsInstanceOf<IEnumerable<CityForAdministrationDTO>>(result);
        }

        private int Id => 1;
        private string stringId => "1";
        private int count => 2;
        private string logoName => "logoName";
        private string cityName => "cityName";

        private IEnumerable<CityDTO> GetTestCityDTO()
        {
            return new List<CityDTO>
            {
                new CityDTO{Name = "Львів"},
                new CityDTO{Name = "Стрий"},
                new CityDTO{Name = "Миколаїв"}
            }.AsEnumerable();
        }

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
                    IEnumerable<CityDTO>>(It.IsAny<IEnumerable<DataAccessCity.City>>()))
                .Returns(CreateFakeCityDto(count));
            _mapper.Setup(m => m.Map<DataAccessCity.City, CityDTO>(It.IsAny<DataAccessCity.City>()))
                .Returns(CreateFakeCityDto(count).FirstOrDefault());
            _mapper.Setup(m => m.Map<CityDTO, DataAccessCity.City>(It.IsAny<CityDTO>()))
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

            return new CityService(_repoWrapper.Object, _mapper.Object, _env.Object, _cityBlobStorage.Object, _cityAccessService.Object, _userManager.Object, _uniqueId.Object);
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

        private IQueryable<CityDTO> CreateFakeCityDto(int count)
        {
            List<CityDTO> cities = new List<CityDTO>();

            for (int i = 0; i < count; i++)
            {
                cities.Add(new CityDTO
                {
                    CityAdministration = GetCityAdministrationDTO(),
                    CityMembers = new List<CityMembersDTO>
                    {
                        new CityMembersDTO
                        {
                            StartDate = new Random().Next(0,1) ==1 ? DateTime.Today : (DateTime?) null,
                            IsApproved = true,
                            UserId = "5"
                        }
                    },
                    CityDocuments = new List<CityDocumentsDTO>
                    {
                        new CityDocumentsDTO(),
                        new CityDocumentsDTO(),
                        new CityDocumentsDTO(),
                        new CityDocumentsDTO(),
                        new CityDocumentsDTO()
                    },
                });
            }
            return cities.AsQueryable();
        }

        private List<CityAdministrationDTO> GetCityAdministrationDTO()
        {
            return new List<CityAdministrationDTO>
            {
                 new CityAdministrationDTO
                 {

                      AdminType = new AdminTypeDTO
                      {
                           AdminTypeName = Roles.CityHead
                      }

                 },
                 new CityAdministrationDTO
                 {
                      AdminType = new AdminTypeDTO
                      {
                           AdminTypeName = "----------"
                      }
                 },
                 new CityAdministrationDTO
                 {
                       AdminType = new AdminTypeDTO
                       {
                            AdminTypeName = Roles.CityHead
                       }
                 },
                 new CityAdministrationDTO
                 {
                       AdminType = new AdminTypeDTO
                       {
                            AdminTypeName = "----------"
                       }
                 }
            };
        }

        private IQueryable<CityDTO> CreateFakeCityDtoWithExAdmin(int count)
        {
            List<CityDTO> cities = new List<CityDTO>();

            for (int i = 0; i < count; i++)
            {
                var cityDto = GetCityDto();
                cityDto.CityAdministration = new List<CityAdministrationDTO>
                {
                    new CityAdministrationDTO
                    {
                        AdminType = new AdminTypeDTO
                        {
                            AdminTypeName = Roles.CityHead
                        },
                        EndDate = DateTime.Now.AddMonths(-3)
                    },
                    new CityAdministrationDTO
                    {
                        AdminType = new AdminTypeDTO
                        {
                            AdminTypeName = "----------",
                        },
                        EndDate = DateTime.Now.AddMonths(-3)
                    }
                };
                cities.Add(cityDto);
            }
            return cities.AsQueryable();
        }

        private IQueryable<CityDTO> CreateFakeCityDtoWithoutMembersWithoutAdminEndDate(int count)
        {
            List<CityDTO> cities = new List<CityDTO>();

            for (int i = 0; i < count; i++)
            {
                var cityDto = GetCityDtoWithoutMembers();
                cityDto.CityMembers = new List<CityMembersDTO>();
                cityDto.CityAdministration = new List<CityAdministrationDTO>
                {
                    new CityAdministrationDTO
                    {
                        AdminType = new AdminTypeDTO
                        {
                            AdminTypeName = Roles.CityHead
                        },
                    },
                    new CityAdministrationDTO
                    {
                        AdminType = new AdminTypeDTO
                        {
                            AdminTypeName = "----------",
                        },
                    }
                };
                cities.Add(cityDto);
            }
            return cities.AsQueryable();
        }

        private CityDTO GetCityDto()
        {
            var city = GetCityDtoWithoutMembers();
            city.CityMembers = new List<CityMembersDTO>
                {
                    new CityMembersDTO
                    {
                        StartDate = new Random().Next(0, 1) == 1 ? DateTime.Today : (DateTime?)null
                    }
                };
            return city;
        }

        private CityDTO GetCityDtoWithoutMembers()
        {
            return new CityDTO
            {
                CityAdministration = GetCityAdministrationDTO(),
                CityDocuments = new List<CityDocumentsDTO>
                    {
                        new CityDocumentsDTO(),
                        new CityDocumentsDTO(),
                        new CityDocumentsDTO(),
                        new CityDocumentsDTO(),
                        new CityDocumentsDTO()
                    },
                Region = new BLL.DTO.Region.RegionDTO()
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
