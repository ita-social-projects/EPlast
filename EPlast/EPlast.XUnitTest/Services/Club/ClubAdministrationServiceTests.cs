using AutoMapper;
using EPlast.BusinessLogicLayer.DTO;
using EPlast.BusinessLogicLayer.DTO.Admin;
using EPlast.BusinessLogicLayer.DTO.Club;
using EPlast.BusinessLogicLayer.DTO.UserProfiles;
using EPlast.BusinessLogicLayer.Interfaces.Club;
using EPlast.BusinessLogicLayer.Services.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace EPlast.XUnitTest.Services.ClubTests
{
    public class ClubAdministrationServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _repoWrapper;
        private readonly Mock<IMapper> _mapper;
        private readonly IClubAdministrationService _clubAdministrationService;

        public ClubAdministrationServiceTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _clubAdministrationService = new ClubAdministrationService(_repoWrapper.Object, _mapper.Object);
        }

        [Fact]
        public async Task GetCurrentClubAdministrationByIDAsyncTest()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Club, bool>>>(),
                    It.IsAny<Func<IQueryable<Club>,
                    IIncludableQueryable<Club, object>>>()))
                .ReturnsAsync(GetTestClub());

            _mapper
                .Setup(s => s.Map<Club, ClubDTO>(It.IsAny<Club>()))
                .Returns(GetTestClubDTO());

            //Act
            var result = await _clubAdministrationService.GetCurrentClubAdministrationByIDAsync(1);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ClubProfileDTO>(result);
        }

        [Fact]
        public async Task DeleteClubAdminAsyncTestReturnTrue()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.GetClubAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubAdministration, bool>>>(), null))
                .ReturnsAsync(GetTestClubAdministration());

            //Act
            var result = await _clubAdministrationService.DeleteClubAdminAsync(1);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteClubAdminAsyncTestReturnFalse()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.GetClubAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubAdministration, bool>>>(), null))
                .ReturnsAsync((ClubAdministration)null);

            //Act
            var result = await _clubAdministrationService.DeleteClubAdminAsync(1);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async Task SetAdminEndDateAsyncTest()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.GetClubAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubAdministration, bool>>>(), null))
                .ReturnsAsync(GetTestClubAdministration());

            //Act
            await _clubAdministrationService.SetAdminEndDateAsync(GetTestAdminEndDate());

            //Assert
            _repoWrapper.Verify(i => i.GetClubAdministration.Update(It.IsAny<ClubAdministration>()), Times.Once());
        }

        [Fact]
        public async Task AddClubAdminAsync()
        {
            //Arrange
            _repoWrapper
               .Setup(s => s.AdminType.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<AdminType, bool>>>(), null))
               .ReturnsAsync(GetTestAdminType());

            _repoWrapper
                .Setup(s => s.GetClubAdministration.CreateAsync(It.IsAny<ClubAdministration>()));

            //Act
            await _clubAdministrationService.AddClubAdminAsync(GetTestClubAdministrationDTO());

            //Assert
            _repoWrapper.Verify(i => i.GetClubAdministration.CreateAsync(It.IsAny<ClubAdministration>()), Times.Once());
            _repoWrapper.Verify(i => i.SaveAsync(), Times.Once());
        }

        private ClubAdministration GetTestClubAdministration()
        {
            return new ClubAdministration()
            {
                AdminTypeId = 1,
                ClubId = 1,
            };
        }

        private ClubMembers GetTestClubMembers()
        {
            return new ClubMembers()
            {
                ID = 1,
                IsApproved = true,
                User = new User
                {
                    LastName = "Andrii",
                    FirstName = "Ivanenko"
                },
            };
        }

        private Club GetTestClub()
        {
            return new Club()
            {
                ClubName = "Club",
                Description = "Club",
                ID = 1,
                ClubAdministration = new List<ClubAdministration>()
                {
                    GetTestClubAdministration()
                },
                ClubMembers = new List<ClubMembers>()
                {
                    GetTestClubMembers()
                },
            };
        }

        private ClubAdministrationDTO GetTestClubAdministrationDTO()
        {
            return new ClubAdministrationDTO()
            {
                AdminTypeName = "Admin",
                AdminTypeId = 1,
                ClubId = 1,
                ClubMembersID = 1,
                StartDate = new DateTime(2020, 5, 10),
                EndDate = new DateTime(2020, 10, 20),
            };
        }

        private AdminType GetTestAdminType()
        {
            return new AdminType()
            {
                ID = 1,
                AdminTypeName = "Admin",
                ClubAdministration = new List<ClubAdministration>()
                {
                    GetTestClubAdministration()
                },
            };
        }

        private ClubMembersDTO GetTestClubMembersDTO()
        {
            return new ClubMembersDTO()
            {
                ID = 1,
                IsApproved = true,
                User = new UserDTO
                {
                    LastName = "Andrii",
                    FirstName = "Ivanenko"
                },
            };
        }

        private AdminEndDateDTO GetTestAdminEndDate()
        {
            return new AdminEndDateDTO()
            {
                AdminId = 1,
                ClubIndex = 1,
                EndDate = new DateTime(2021, 12, 10)
            };
        }

        private ClubDTO GetTestClubDTO()
        {
            return new ClubDTO()
            {
                ClubName = "Club",
                Description = "Club",
                ID = 1,
                ClubAdministration = new List<ClubAdministrationDTO>()
                {
                    GetTestClubAdministrationDTO()
                },
                ClubMembers = new List<ClubMembersDTO>()
                {
                    GetTestClubMembersDTO()
                },
            };
        }
    }
}
