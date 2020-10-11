using AutoMapper;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Services;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
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
            _cityService = new CityService(_repoWrapper.Object, _mapper.Object, _env.Object, _cityBlobStorage.Object,
                   _cityAccessService.Object, _userManager.Object);
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
            //// Arrange
            CityService cityService = CreateCityService();

            //// Act
            var result = await cityService.GetCityProfileAsync(Id);

            //// Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<CityProfileDTO>(result);
        }

        [Test]
        public async Task GetCityMembersAsync_ReturnsCityMembers()
        {
            //// Arrange
            CityService cityService = CreateCityService();

            //// Act
            var result = await cityService.GetCityMembersAsync(Id);

            //// Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<CityProfileDTO>(result);
        }

        [Test]
        public async Task GetCityFollowersAsync_ReturnsCityFollowers()
        {
            //// Arrange
            CityService cityService = CreateCityService();

            //// Act
            var result = await cityService.GetCityFollowersAsync(Id);

            //// Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<CityProfileDTO>(result);
        }

        [Test]
        public async Task GetCityAdminsAsync_ReturnsCityAdmins()
        {
            //// Arrange
            CityService cityService = CreateCityService();

            //// Act
            var result = await cityService.GetCityAdminsAsync(Id);

            //// Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<CityProfileDTO>(result);
        }

        [Test]
        public async Task GetCityDocumentsAsync_ReturnsCityDocuments()
        {
            //// Arrange
            CityService cityService = CreateCityService();

            //// Act
            var result = await cityService.GetCityDocumentsAsync(Id);

            //// Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<CityProfileDTO>(result);
        }

        [Test]
        public async Task EditAsync_ReturnsCityEdited()
        {
            //// Arrange
            CityService cityService = CreateCityService();

            //// Act
            var result = await cityService.EditAsync(Id);

            //// Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<CityProfileDTO>(result);
        }

        private int Id => 1;

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
                    IEnumerable<CityDTO>>(It.IsAny<IEnumerable<DataAccess.Entities.City>>()))
                .Returns(CreateFakeCityDto(10));
            _mapper.Setup(m => m.Map<DataAccessCity.City, CityDTO>(It.IsAny<DataAccessCity.City>()))
                .Returns(CreateFakeCityDto(10).FirstOrDefault());
            _mapper.Setup(m => m.Map<CityDTO, DataAccessCity.City>(It.IsAny<CityDTO>()))
                .Returns(() => new DataAccessCity.City());
            _repoWrapper.Setup(r => r.City.FindAll())
                .Returns(CreateFakeCities(10));
            _repoWrapper.Setup(r => r.City.FindByCondition(It.IsAny<Expression<Func<DataAccessCity.City, bool>>>()))
                .Returns((Expression<Func<DataAccessCity.City, bool>> condition) =>
                    CreateFakeCities(10).Where(condition));
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

            return new CityService(_repoWrapper.Object, _mapper.Object, _env.Object, _cityBlobStorage.Object, _cityAccessService.Object, null);
        }


        public IQueryable<DataAccessCity.City> CreateFakeCities(int count)
        {
            List<DataAccessCity.City> cities = new List<DataAccessCity.City>();

            for (int i = 0; i < count; i++)
            {
                cities.Add(new DataAccessCity.City());
            }

            return cities.AsQueryable();
        }

        public IQueryable<CityDTO> CreateFakeCityDto(int count)
        {
            List<CityDTO> cities = new List<CityDTO>();

            for (int i = 0; i < count; i++)
            {
                cities.Add(new CityDTO
                {
                    CityAdministration = new List<CityAdministrationDTO>
                    {
                        new CityAdministrationDTO
                        {

                           AdminType = new AdminTypeDTO
                           {
                               AdminTypeName = "Голова Станиці"
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
                                AdminTypeName = "Голова Станиці"
                            }
                        },
                        new CityAdministrationDTO
                        {
                            AdminType = new AdminTypeDTO
                            {
                                AdminTypeName = "----------"
                            }
                        }
                    },
                    CityMembers = new List<CityMembersDTO>
                    {
                        new CityMembersDTO
                        {
                            StartDate = new Random().Next(0,1) ==1 ? DateTime.Today : (DateTime?) null
                        }
                    },
                    CityDocuments = new List<CityDocumentsDTO>
                    {
                        new CityDocumentsDTO(),
                        new CityDocumentsDTO(),
                        new CityDocumentsDTO(),
                        new CityDocumentsDTO(),
                        new CityDocumentsDTO()
                    }
                });
            }

            return cities.AsQueryable();
        }

        public Region GetTestRegion()
        {
            var region = new Region()
            {
                ID = 1,
                RegionName = "Lviv",
                Description = "Lviv region"
            };

            return region;
        }

        public DataAccessCity.City GetTestNewCity()
        {
            var city = new DataAccessCity.City
            {
                Name = "city",
                Logo = "710b8b06-6869-45db-894f-7a0b131e6c6b.jpg"
            };

            return city;
        }
    }
}
