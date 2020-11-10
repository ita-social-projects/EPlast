using AutoMapper;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Services;
using EPlast.BLL.Services.Club;
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
using DataAccessClub = EPlast.DataAccess.Entities;

namespace EPlast.Tests.Services.Club
{
    [TestFixture]
    public class ClubServiceTests
    {
        private ClubService _clubService;
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private Mock<IWebHostEnvironment> _env;
        private Mock<IClubBlobStorageRepository> _clubBlobStorage;
        private Mock<IClubAccessService> _clubAccessService;
        private Mock<UserManager<User>> _userManager;
        private Mock<IUserStore<User>> _user;
        private Mock<IUniqueIdService> _uniqueId;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _env = new Mock<IWebHostEnvironment>();
            _clubBlobStorage = new Mock<IClubBlobStorageRepository>();
            _clubAccessService = new Mock<IClubAccessService>();
            _user = new Mock<IUserStore<User>>();
            _uniqueId = new Mock<IUniqueIdService>();
            _userManager = new Mock<UserManager<User>>(_user.Object, null, null, null, null, null, null, null, null);
            _clubService = new ClubService(_repoWrapper.Object, _mapper.Object, _env.Object, _clubBlobStorage.Object,
                   _clubAccessService.Object, _userManager.Object, _uniqueId.Object);
        }


        [Test]
        public async Task GetAllAsync_ReturnsAllCities()
        {
            // Arrange
            _repoWrapper.Setup(rw => rw.Club.GetAllAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccessClub.Club>, IIncludableQueryable<DataAccessClub.Club, object>>>()))
                .ReturnsAsync(() => new List<DataAccessClub.Club>());

            // Act
            var result = await _clubService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<List<DataAccessClub.Club>>(result);
        }

        [Test]
        public async Task GetAllDTOAsync_ReturnsAllDTO()
        {
            // Arrange
            _mapper
                .Setup(m => m.Map<IEnumerable<DataAccessClub.Club>, IEnumerable<ClubDTO>>(It.IsAny<IEnumerable<DataAccessClub.Club>>()))
                .Returns(GetTestClubDTO());
            _repoWrapper
                .Setup(r => r.Club.GetAllAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccessClub.Club>, IIncludableQueryable<DataAccessClub.Club, object>>>()))
                .ReturnsAsync(GetTestClub());

            // Act
            var result = await _clubService.GetAllDTOAsync();


            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<ClubDTO>>(result);
        }



        [Test]
        public async Task GetByIdAsync_ReturnsClub()
        {
            // Arrange
            _repoWrapper
                .Setup(r => r.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(), null))
                .ReturnsAsync(new DataAccessClub.Club());
            _mapper
                .Setup(m => m.Map<DataAccessClub.Club, ClubDTO>(It.IsAny<DataAccessClub.Club>()))
                .Returns(new ClubDTO());

            // Act
            var result = await _clubService.GetByIdAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ClubDTO>(result);
        }


        [Test]
        public async Task GetClubProfileAsync_ReturnsClubProfile()
        {
            //// Arrange
            ClubService clubService = CreateClubService();

            //// Act
            var result = await clubService.GetClubProfileAsync(Id);

            //// Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ClubProfileDTO>(result);
        }

        [Test]
        public async Task GetClubMembersAsync_ReturnsClubMembers()
        {
            //// Arrange
            ClubService clubService = CreateClubService();

            //// Act
            var result = await clubService.GetClubMembersAsync(Id);

            //// Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ClubProfileDTO>(result);
        }

        [Test]
        public async Task GetClubFollowersAsync_ReturnsClubFollowers()
        {
            //// Arrange
            ClubService clubService = CreateClubService();

            //// Act
            var result = await clubService.GetClubFollowersAsync(Id);

            //// Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ClubProfileDTO>(result);
        }

        [Test]
        public async Task GetClubAdminsAsync_ReturnsClubAdmins()
        {
            //// Arrange
            ClubService clubService = CreateClubService();

            //// Act
            var result = await clubService.GetClubAdminsAsync(Id);

            //// Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ClubProfileDTO>(result);
        }

        [Test]
        public async Task GetClubDocumentsAsync_ReturnsClubDocuments()
        {
            //// Arrange
            ClubService clubService = CreateClubService();

            //// Act
            var result = await clubService.GetClubDocumentsAsync(Id);

            //// Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ClubProfileDTO>(result);
        }

        [Test]
        public async Task EditAsync_ReturnsClubEdited()
        {
            //// Arrange
            ClubService clubService = CreateClubService();

            //// Act
            var result = await clubService.EditAsync(Id);

            //// Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ClubProfileDTO>(result);
        }

        private int Id => 1;

        private IEnumerable<ClubDTO> GetTestClubDTO()
        {
            return new List<ClubDTO>
            {
                new ClubDTO{Name = "Львів"},
                new ClubDTO{Name = "Стрий"},
                new ClubDTO{Name = "Миколаїв"}
            }.AsEnumerable();
        }

        private IEnumerable<DataAccessClub.Club> GetTestClub()
        {
            return new List<DataAccessClub.Club>
            {
                new DataAccessClub.Club{
                    Name = "Львів",
                    ClubAdministration = new List<ClubAdministration>(),
                    ClubMembers = new List<ClubMembers>(),
                    ClubDocuments = new List<ClubDocuments>(),
                    },
                new DataAccessClub.Club{
                    Name = "Стрий",
                    ClubAdministration = new List<ClubAdministration>(),
                    ClubMembers = new List<ClubMembers>(),
                    ClubDocuments = new List<ClubDocuments>(),
                }
            }.AsEnumerable();
        }

        private ClubService CreateClubService()
        {
            _mapper.Setup(m => m.Map<IEnumerable<DataAccessClub.Club>,
                    IEnumerable<ClubDTO>>(It.IsAny<IEnumerable<DataAccess.Entities.Club>>()))
                .Returns(CreateFakeClubDto(10));
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDTO>(It.IsAny<DataAccessClub.Club>()))
                .Returns(CreateFakeClubDto(10).FirstOrDefault());
            _mapper.Setup(m => m.Map<ClubDTO, DataAccessClub.Club>(It.IsAny<ClubDTO>()))
                .Returns(() => new DataAccessClub.Club());
            _repoWrapper.Setup(r => r.Club.FindAll())
                .Returns(CreateFakeCities(10));
            _repoWrapper.Setup(r => r.Club.FindByCondition(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>()))
                .Returns((Expression<Func<DataAccessClub.Club, bool>> condition) =>
                    CreateFakeCities(10).Where(condition));
            _repoWrapper.Setup(r => r.Region.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Region, bool>>>(), null))
               .ReturnsAsync(GetTestRegion());
            _repoWrapper.Setup(r => r.Club.Update(It.IsAny<DataAccessClub.Club>()))
                .Verifiable();
            _repoWrapper.Setup(r => r.Club.Create(It.IsAny<DataAccessClub.Club>()))
                .Verifiable();
            _repoWrapper.Setup(r => r.Save())
                .Verifiable();
            _repoWrapper.Setup(r => r.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(), null))
                .ReturnsAsync(GetTestNewClub());

            return new ClubService(_repoWrapper.Object, _mapper.Object, _env.Object, _clubBlobStorage.Object, _clubAccessService.Object, null,_uniqueId.Object);
        }


        private IQueryable<DataAccessClub.Club> CreateFakeCities(int count)
        {
            List<DataAccessClub.Club> cities = new List<DataAccessClub.Club>();
            for (int i = 0; i < count; i++)
            {
                cities.Add(new DataAccessClub.Club());
            }
            return cities.AsQueryable();
        }

        private IQueryable<ClubDTO> CreateFakeClubDto(int count)
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
                           AdminType = new AdminTypeDTO
                           {
                               AdminTypeName = "Голова Станиці"
                           }
                        },
                        new ClubAdministrationDTO
                        {
                            AdminType = new AdminTypeDTO
                            {
                                AdminTypeName = "----------"
                            }
                        },
                        new ClubAdministrationDTO
                        {
                            AdminType = new AdminTypeDTO
                            {
                                AdminTypeName = "Голова Станиці"
                            }
                        },
                        new ClubAdministrationDTO
                        {
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
                            StartDate = new Random().Next(0,1) ==1 ? DateTime.Today : (DateTime?) null
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

        private Region GetTestRegion()
        {
            return new Region()
            {
                ID = 1,
                RegionName = "Lviv",
                Description = "Lviv region"
            };
        }

        private DataAccessClub.Club GetTestNewClub()
        {
            return new DataAccessClub.Club
            {
                Name = "club",
                Logo = "710b8b06-6869-45db-894f-7a0b131e6c6b.jpg"
            };
        }
    }
}
