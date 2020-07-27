using AutoMapper;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Services.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
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

namespace EPlast.XUnitTest.Services.ClubTests
{
    public class ClubServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _repoWrapper;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IClubBlobStorageRepository> _blob;
        private readonly IClubService _clubService;

        public ClubServiceTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _blob = new Mock<IClubBlobStorageRepository>();
            _clubService = new ClubService(_repoWrapper.Object, _mapper.Object, _blob.Object);
        }

        [Fact]
        public async Task GetAllClubsAsync_ReturnsListOfClubDto()
        {
            //arrange
            _repoWrapper.Setup(r => r.Club.GetAllAsync(null,
                    It.IsAny<Func<IQueryable<Club>, IIncludableQueryable<Club, object>>>()))
                .ReturnsAsync(new List<Club> { new Club() });

            //act
            await _clubService.GetAllClubsAsync();

            //assert
            _mapper.Verify(m => m.Map<IEnumerable<Club>, IEnumerable<ClubDTO>>(It.IsAny<IEnumerable<Club>>()));
        }

        [Fact]
        public async Task GetClubProfileAsync_ReturnsClubProfileDto()
        {
            //arrange
            _repoWrapper.Setup(r => r.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Club, bool>>>(),
                    It.IsAny<Func<IQueryable<Club>, IIncludableQueryable<Club, object>>>()))
                .ReturnsAsync(new Club());
            _mapper.Setup(m => m.Map<Club, ClubDTO>(It.IsAny<Club>()))
                .Returns(() => new ClubDTO()
                {
                    ID = It.IsAny<int>(),
                    ClubMembers = new List<ClubMembersDTO>()
                    {
                        new ClubMembersDTO
                        {
                            User = new UserDTO(),
                            IsApproved = It.IsAny<bool>(),
                        }
                    },
                    ClubAdministration = new List<ClubAdministrationDTO>()
                    {
                        new ClubAdministrationDTO
                        {
                            AdminType = new AdminTypeDTO(),
                            StartDate = It.IsAny<DateTime>(),
                            ClubMembers = new ClubMembersDTO
                            {
                                User = new UserDTO(),
                                IsApproved = It.IsAny<bool>(),
                            }
                        }
                    }
                });

            //act
            var result = await _clubService.GetClubProfileAsync(It.IsAny<int>());

            //assert
            Assert.NotNull(result);
            Assert.IsType<ClubProfileDTO>(result);
        }

        [Fact]
        public async Task GetClubInfoByIdAsync_ReturnsClubDto()
        {
            //arrange
            _repoWrapper.Setup(r => r.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Club, bool>>>(),
                    It.IsAny<Func<IQueryable<Club>, IIncludableQueryable<Club, object>>>()))
                .ReturnsAsync(new Club());

            //act
            await _clubService.GetClubInfoByIdAsync(It.IsAny<int>());

            //assert
            _mapper.Verify(m => m.Map<Club, ClubDTO>(It.IsAny<Club>()));
        }

        [Fact]
        public async Task GetClubInfoByIdAsync_NotFound_ReturnsArgumentNullException()
        {
            //arrange
            _repoWrapper.Setup(r => r.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Club, bool>>>(),
                    It.IsAny<Func<IQueryable<Club>, IIncludableQueryable<Club, object>>>()))
                .ReturnsAsync((Club)null);

            //act
            async Task Act() => await _clubService.GetClubInfoByIdAsync(It.IsAny<int>());

            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(Act);
            _mapper.Verify(m => m.Map<Club, ClubDTO>(new Club()), Times.Never);
        }

    }
}