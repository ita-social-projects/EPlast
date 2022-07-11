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
using Moq;
using Xunit;

namespace EPlast.XUnitTest.Services.City
{
    public class CityServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _repoWrapper;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IWebHostEnvironment> _env;
        private readonly Mock<ICityBlobStorageRepository> _cityBlobStorage;
        private readonly Mock<ICityAccessService> _cityAccessService;

        public CityServiceTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _env = new Mock<IWebHostEnvironment>();
            _cityBlobStorage = new Mock<ICityBlobStorageRepository>();
            _cityAccessService = new Mock<ICityAccessService>();
        }

        private CityService CreateCityService()
        {
            _mapper.Setup(m => m.Map<IEnumerable<DataAccess.Entities.City>,
                    IEnumerable<CityDto>>(It.IsAny<IEnumerable<DataAccess.Entities.City>>()))
                .Returns(CreateFakeCityDto(10));
            _mapper.Setup(m => m.Map<DataAccess.Entities.City, CityDto>(It.IsAny<DataAccess.Entities.City>()))
                .Returns(CreateFakeCityDto(10).FirstOrDefault());
            _mapper.Setup(m => m.Map<CityDto, DataAccess.Entities.City>(It.IsAny<CityDto>()))
                .Returns(() => new DataAccess.Entities.City());
            _repoWrapper.Setup(r => r.City.FindAll())
                .Returns(CreateFakeCities(10));
            _repoWrapper.Setup(r => r.City.FindByCondition(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>()))
                .Returns((Expression<Func<DataAccess.Entities.City, bool>> condition) =>
                    CreateFakeCities(10).Where(condition));
            _repoWrapper.Setup(r => r.Region.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Region, bool>>>(), null))
               .ReturnsAsync(GetTestRegion());
            _repoWrapper.Setup(r => r.City.Update(It.IsAny<DataAccess.Entities.City>()))
                .Verifiable();
            _repoWrapper.Setup(r => r.City.Create(It.IsAny<DataAccess.Entities.City>()))
                .Verifiable();
            _repoWrapper.Setup(r => r.Save())
                .Verifiable();
            _repoWrapper.Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(), null))
                .ReturnsAsync(GetTestCity());

            return new CityService(
                _repoWrapper.Object,
                _mapper.Object,
                _env.Object,
                _cityBlobStorage.Object,
                _cityAccessService.Object,
                null
            );
        }

        [Fact]
        public async Task GetAllTest()
        {
            CityService cityService = CreateCityService();

            var result = await cityService.GetAllAsync();

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAllDtoTest()
        {
            CityService cityService = CreateCityService();

            var result = await cityService.GetAllCitiesAsync(null);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetByIdTest()
        {
            CityService cityService = CreateCityService();

            var result = await cityService.GetByIdAsync(GetIdForSearch);

            Assert.NotNull(result);
            Assert.IsType<CityDto>(result);
        }

        [Fact]
        public async Task CityProfileTest()
        {
            CityService cityService = CreateCityService();

            var result = await cityService.GetCityProfileAsync(GetIdForSearch);

            Assert.NotNull(result);
            Assert.IsType<CityProfileDto>(result);
        }

        [Fact]
        public async Task CityMembersTest()
        {
            CityService cityService = CreateCityService();

            var result = await cityService.GetCityMembersAsync(GetIdForSearch);

            Assert.NotNull(result);
            Assert.IsType<CityProfileDto>(result);
        }

        [Fact]
        public async Task CityFollowersTest()
        {
            CityService cityService = CreateCityService();

            var result = await cityService.GetCityFollowersAsync(GetIdForSearch);

            Assert.NotNull(result);
            Assert.IsType<CityProfileDto>(result);
        }

        [Fact]
        public async Task CityAdminsTest()
        {
            CityService cityService = CreateCityService();

            var result = await cityService.GetCityAdminsAsync(GetIdForSearch);

            Assert.NotNull(result);
            Assert.IsType<CityProfileDto>(result);
        }

        [Fact]
        public async Task CityDocumentsTest()
        {
            CityService cityService = CreateCityService();

            var result = await cityService.GetCityDocumentsAsync(GetIdForSearch);

            Assert.NotNull(result);
            Assert.IsType<CityProfileDto>(result);
        }

        [Fact]
        public async Task EditGetTest()
        {
            CityService cityService = CreateCityService();

            var result = await cityService.EditAsync(GetIdForSearch);

            Assert.NotNull(result);
            Assert.IsType<CityProfileDto>(result);
        }

        [Fact]
        public async Task CreateTest()
        {
            CityService cityService = CreateCityService();
            CityProfileDto cityProfileDto = new CityProfileDto
            {
                City = new CityDto
                {
                    ID = 0
                }
            };

            var result = await cityService.CreateAsync(cityProfileDto, null);

            Assert.Equal(cityProfileDto.City.ID, result);
        }

        private static int GetIdForSearch => 1;
        public IQueryable<DataAccess.Entities.City> CreateFakeCities(int count)
        {
            List<DataAccess.Entities.City> cities = new List<DataAccess.Entities.City>();

            for (int i = 0; i < count; i++)
            {
                cities.Add(new DataAccess.Entities.City());
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
        
        public DataAccess.Entities.City GetTestCity()
        {
            var city = new DataAccess.Entities.City
            {
                Name = "city",
                Logo = "710b8b06-6869-45db-894f-7a0b131e6c6b.jpg"
            };

            return city;
        }

        public IQueryable<CityDto> CreateFakeCityDto(int count)
        {
            List<CityDto> cities = new List<CityDto>();

            for (int i = 0; i < count; i++)
            {
                cities.Add(new CityDto
                {
                    CityAdministration = new List<CityAdministrationDto>
                    {
                        new CityAdministrationDto
                        {

                           AdminType = new AdminTypeDto
                           {
                               AdminTypeName = Roles.CityHead
                           }

                        },
                        new CityAdministrationDto
                        {
                            AdminType = new AdminTypeDto
                            {
                                AdminTypeName = "----------"
                            }
                        },
                        new CityAdministrationDto
                        {
                            AdminType = new AdminTypeDto
                            {
                                AdminTypeName = Roles.CityHead
                            }
                        },
                        new CityAdministrationDto
                        {
                            AdminType = new AdminTypeDto
                            {
                                AdminTypeName = "----------"
                            }
                        }
                    },
                    CityMembers = new List<CityMembersDto>
                    {
                        new CityMembersDto
                        {
                            StartDate = new Random().Next(0,1) ==1 ? DateTime.Today : (DateTime?) null
                        }
                    },
                    CityDocuments = new List<CityDocumentsDto>
                    {
                        new CityDocumentsDto(),
                        new CityDocumentsDto(),
                        new CityDocumentsDto(),
                        new CityDocumentsDto(),
                        new CityDocumentsDto()
                    }
                });
            }

            return cities.AsQueryable();
        }

    }
}