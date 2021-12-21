using AutoMapper;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Services.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.AzureStorage.Base;
using Xunit;
using Microsoft.AspNetCore.Hosting;
using EPlast.BLL.Interfaces;

namespace EPlast.XUnitTest.Services.ClubTests
{
    public class ClubServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _repoWrapper;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IWebHostEnvironment> _env;
        private readonly Mock<IClubBlobStorageRepository> _ClubBlobStorage;
        private readonly Mock<IClubAccessService> _ClubAccessService;
        private readonly Mock<IUniqueIdService> _uniqueId;
        private readonly Mock<IClubAnnualReportService> _clubAnnualReportService;

        public ClubServiceTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _env = new Mock<IWebHostEnvironment>();
            _ClubBlobStorage = new Mock<IClubBlobStorageRepository>();
            _ClubAccessService = new Mock<IClubAccessService>();
            _uniqueId = new Mock<IUniqueIdService>();
            _clubAnnualReportService = new Mock<IClubAnnualReportService>();
        }

        private ClubService CreateClubService()
        {
            _mapper.Setup(m => m.Map<IEnumerable<DataAccess.Entities.Club>,
                    IEnumerable<ClubDTO>>(It.IsAny<IEnumerable<DataAccess.Entities.Club>>()))
                .Returns(CreateFakeClubDto(10));
            _mapper.Setup(m => m.Map<DataAccess.Entities.Club, ClubDTO>(It.IsAny<DataAccess.Entities.Club>()))
                .Returns(CreateFakeClubDto(10).FirstOrDefault());
            _mapper.Setup(m => m.Map<ClubDTO, DataAccess.Entities.Club>(It.IsAny<ClubDTO>()))
                .Returns(() => new DataAccess.Entities.Club());
            _repoWrapper.Setup(m => m.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(), null))
                .ReturnsAsync(new CityMembers() { });
            _repoWrapper.Setup(r => r.City.GetFirstAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(), null))
                .ReturnsAsync(new DataAccess.Entities.City() { Name = "Львів" });
            _repoWrapper.Setup(r => r.Club.FindAll())
                .Returns(CreateFakeCities(10));
            _repoWrapper.Setup(r => r.Club.FindByCondition(It.IsAny<Expression<Func<DataAccess.Entities.Club, bool>>>()))
                .Returns((Expression<Func<DataAccess.Entities.Club, bool>> condition) =>
                    CreateFakeCities(10).Where(condition));
            _repoWrapper.Setup(r => r.Region.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Region, bool>>>(), null))
               .ReturnsAsync(GetTestRegion());
            _repoWrapper.Setup(r => r.User.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), null))
               .ReturnsAsync(new User() );
            _repoWrapper.Setup(r => r.Club.Update(It.IsAny<DataAccess.Entities.Club>()))
                .Verifiable();
            _repoWrapper.Setup(r => r.Club.Create(It.IsAny<DataAccess.Entities.Club>()))
                .Verifiable();
            _repoWrapper.Setup(r => r.Save())
                .Verifiable();
            _repoWrapper.Setup(r => r.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccess.Entities.Club, bool>>>(), null))
                .ReturnsAsync(GetTestClub());

            return new ClubService(_repoWrapper.Object, _mapper.Object, _env.Object, _ClubBlobStorage.Object, _ClubAccessService.Object, null, _uniqueId.Object);
        }

        [Fact]
        public async Task GetByIdTest()
        {
            ClubService ClubService = CreateClubService();

            var result = await ClubService.GetByIdAsync(GetIdForSearch);

            Assert.NotNull(result);
            Assert.IsType<ClubDTO>(result);
        }

        [Fact]
        public async Task ClubProfileTest()
        {
            ClubService ClubService = CreateClubService();

            var result = await ClubService.GetClubProfileAsync(GetIdForSearch);

            Assert.NotNull(result);
            Assert.IsType<ClubProfileDTO>(result);
        }

        [Fact]
        public async Task ClubMembersTest()
        {
            ClubService ClubService = CreateClubService();

            var result = await ClubService.GetClubMembersAsync(GetIdForSearch);

            Assert.NotNull(result);
            Assert.IsType<ClubProfileDTO>(result);
        }

        [Fact]
        public async Task ClubFollowersTest()
        {
            ClubService ClubService = CreateClubService();

            var result = await ClubService.GetClubFollowersAsync(GetIdForSearch);

            Assert.NotNull(result);
            Assert.IsType<ClubProfileDTO>(result);
        }

        [Fact]
        public async Task ClubAdminsTest()
        {
            ClubService ClubService = CreateClubService();

            var result = await ClubService.GetClubAdminsAsync(GetIdForSearch);

            Assert.NotNull(result);
            Assert.IsType<ClubProfileDTO>(result);
        }

        [Fact]
        public async Task ClubDocumentsTest()
        {
            ClubService ClubService = CreateClubService();

            var result = await ClubService.GetClubDocumentsAsync(GetIdForSearch);

            Assert.NotNull(result);
            Assert.IsType<ClubProfileDTO>(result);
        }

        [Fact]
        public async Task EditGetTest()
        {
            ClubService ClubService = CreateClubService();

            var result = await ClubService.EditAsync(GetIdForSearch);

            Assert.NotNull(result);
            Assert.IsType<ClubProfileDTO>(result);
        }

        [Fact]
        public async Task CreateTest()
        {
            ClubService ClubService = CreateClubService();
            ClubProfileDTO ClubProfileDto = new ClubProfileDTO
            {
                Club = new ClubDTO
                {
                    ID = 0
                }
            };

            var result = await ClubService.CreateAsync(ClubProfileDto, null);

            Assert.Equal(ClubProfileDto.Club.ID, result);
        }

        private static int GetIdForSearch => 1;
        public IQueryable<DataAccess.Entities.Club> CreateFakeCities(int count)
        {
            List<DataAccess.Entities.Club> cities = new List<DataAccess.Entities.Club>();

            for (int i = 0; i < count; i++)
            {
                cities.Add(new DataAccess.Entities.Club());
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

        public DataAccess.Entities.Club GetTestClub()
        {
            var Club = new DataAccess.Entities.Club
            {
                Name = "Club",
                Logo = "710b8b06-6869-45db-894f-7a0b131e6c6b.jpg",
                ClubMembers = new List<ClubMembers> { new ClubMembers() { UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad" } }

            };

            return Club;
        }

        public IQueryable<ClubDTO> CreateFakeClubDto(int count)
        {
            List<ClubDTO> cities = new List<ClubDTO>();

            for (int i = 0; i < count; i++)
            {
                cities.Add(new ClubDTO
                {
                    ClubAdministration = new List<ClubAdministrationDTO>
                    {
                        new ClubAdministrationDTO
                        {
                           UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad",
                           User=new ClubUserDTO(),
                           AdminType = new AdminTypeDTO
                           {
                               AdminTypeName = Roles.CityHead
                           }

                        },
                        new ClubAdministrationDTO
                        {
                            UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad",
                            User=new ClubUserDTO(),
                            AdminType = new AdminTypeDTO
                            {
                                AdminTypeName = "----------"
                            }
                        },
                        new ClubAdministrationDTO
                        {
                            UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad",
                            User=new ClubUserDTO(),
                            AdminType = new AdminTypeDTO
                            {
                                AdminTypeName = Roles.CityHead
                            }
                        },
                        new ClubAdministrationDTO
                        {
                            UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad",
                            User=new ClubUserDTO(),
                            AdminType = new AdminTypeDTO
                            {
                                AdminTypeName = "----------"
                            }
                        }
                    },
                    ClubMembers = new List<ClubMembersDTO>
                    {
                        new ClubMembersDTO
                        {
                            UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad",
                            StartDate = new Random().Next(0,1) ==1 ? DateTime.Today : (DateTime?) null,
                            User=new ClubUserDTO()
                        }
                    },
                    ClubDocuments = new List<ClubDocumentsDTO>
                    {
                        new ClubDocumentsDTO(),
                        new ClubDocumentsDTO(),
                        new ClubDocumentsDTO(),
                        new ClubDocumentsDTO(),
                        new ClubDocumentsDTO()
                    }
                });
            }

            return cities.AsQueryable();
        }

    }
}