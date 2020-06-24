using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.BussinessLayer.Interfaces.Admin;
using EPlast.BussinessLayer.Interfaces.Club;
using EPlast.BussinessLayer.Services.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Xunit;

namespace EPlast.XUnitTest.Services.ClubTests
{
    public class ClubAdministrationServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _repoWrapper;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IAdminTypeService> _adminTypeService;
        private readonly IClubAdministrationService _clubAdministrationService;

        public ClubAdministrationServiceTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _adminTypeService = new Mock<IAdminTypeService>();
            _clubAdministrationService =
                new ClubAdministrationService(_repoWrapper.Object, _mapper.Object, _adminTypeService.Object);
        }

        [Fact]
        public async Task GetClubAdministrationByIdAsyncTest()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Club, bool>>>(),
                    It.IsAny<Func<IQueryable<Club>,
                        IIncludableQueryable<Club, object>>>()))
                .ReturnsAsync(new Club());

            _mapper
                .Setup(s => s.Map<Club, ClubDTO>(It.IsAny<Club>()))
                .Returns(new ClubDTO());

            //Act
            var result = await _clubAdministrationService.GetClubAdministrationByIdAsync(It.IsAny<int>());

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ClubProfileDTO>(result);
        }

        [Fact]
        public async Task GetClubAdministrationByIdAsync_NotFound_ReturnsArgumentNullException()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Club, bool>>>(),
                    null))
                .ReturnsAsync((Club) null);
            _mapper
                .Setup(s => s.Map<Club, ClubDTO>(It.IsAny<Club>()))
                .Returns(new ClubDTO());

            //Act
            async Task Act() => await _clubAdministrationService.GetClubAdministrationByIdAsync(It.IsAny<int>());

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(Act);
            _mapper.Verify(m => m.Map<Club, ClubDTO>(new Club()), Times.Never);
        }

        [Fact]
        public async Task DeleteClubAdminAsyncTest_ReturnsTrue()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.ClubAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<ClubAdministration, bool>>>(), null))
                .ReturnsAsync(new ClubAdministration());

            //Act
            var result = await _clubAdministrationService.DeleteClubAdminAsync(It.IsAny<int>());

            //Assert
            Assert.True(result);
            _repoWrapper.Verify(i => i.ClubAdministration.Delete(It.IsAny<ClubAdministration>()), Times.Once);
            _repoWrapper.Verify(r => r.SaveAsync());
        }

        [Fact]
        public async Task DeleteClubAdminAsync_NotFound_ReturnsArgumentNullException()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.ClubAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<ClubAdministration, bool>>>(), null))
                .ReturnsAsync((ClubAdministration) null);

            //Act
            async Task Act() => await _clubAdministrationService.DeleteClubAdminAsync(It.IsAny<int>());

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(Act);
            _repoWrapper.Verify(i => i.ClubAdministration.Delete(It.IsAny<ClubAdministration>()), Times.Never);
        }

        [Fact]
        public async Task SetAdminEndDateAsyncTest_ReturnsClubAdministrationDto()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.ClubAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<ClubAdministration, bool>>>(), null))
                .ReturnsAsync(new ClubAdministration());
            _mapper
                .Setup(s => s.Map<ClubAdministration, ClubAdministrationDTO>(It.IsAny<ClubAdministration>()))
                .Returns(new ClubAdministrationDTO());

            //Act
            var result = await _clubAdministrationService.SetAdminEndDateAsync(It.IsAny<int>(), It.IsAny<DateTime>());

            //Assert
            _repoWrapper.Verify(i => i.ClubAdministration.Update(It.IsAny<ClubAdministration>()), Times.Once());
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once());
            Assert.IsType<ClubAdministrationDTO>(result);
        }

        [Fact]
        public async Task SetAdminEndDateAsync_NotFound_ReturnsArgumentNullException()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.ClubAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<ClubAdministration, bool>>>(), null))
                .ReturnsAsync((ClubAdministration) null);

            //Act
            async Task Act() =>
                await _clubAdministrationService.SetAdminEndDateAsync(It.IsAny<int>(), It.IsAny<DateTime>());

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(Act);
            _repoWrapper.Verify(i => i.ClubAdministration.Update(It.IsAny<ClubAdministration>()), Times.Never);
        }

        [Fact]
        public async Task AddClubAdminAsync()
        {
            //Arrange
            _adminTypeService
                .Setup(s => s.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDTO());
            _repoWrapper
                .Setup(s => s.ClubAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<ClubAdministration, bool>>>(), null))
                .ReturnsAsync(new ClubAdministration());

            //Act
            await _clubAdministrationService.AddClubAdminAsync(new ClubAdministrationDTO());

            //Assert
            _repoWrapper.Verify(i => i.ClubAdministration.CreateAsync(It.IsAny<ClubAdministration>()), Times.Once());
            _repoWrapper.Verify(i => i.SaveAsync(), Times.Once());
        }
    }
}