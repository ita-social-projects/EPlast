using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Services.Club;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Xunit;

namespace EPlast.XUnitTest.Services.ClubTests
{
    public class ClubMembersServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _repoWrapper;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IClubService> _clubService;
        private readonly Mock<IUserManagerService> _userManagerService;
        private readonly IClubMembersService _clubMemberService;

        public ClubMembersServiceTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _clubService = new Mock<IClubService>();
            _userManagerService = new Mock<IUserManagerService>();
            _clubMemberService = new ClubMembersService(_repoWrapper.Object, _mapper.Object, _clubService.Object,
                _userManagerService.Object);
        }

        [Fact]
        public async Task ToggleIsApprovedInClubMembersAsyncTest()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.ClubMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubMembers, bool>>>(), It.IsAny<Func<IQueryable<ClubMembers>, IIncludableQueryable<ClubMembers, object>>>()))
                .ReturnsAsync(new ClubMembers());

            //Act
            await _clubMemberService.ToggleIsApprovedInClubMembersAsync(It.IsAny<int>(), It.IsAny<int>());

            //Assert
            _repoWrapper.Verify(i => i.ClubMembers.Update(It.IsAny<ClubMembers>()), Times.Once());
        }

        [Fact]
        public async Task ToggleIsApprovedInClubMembersAsync_FailureTest()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.ClubMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubMembers, bool>>>(), null))
                .ReturnsAsync((ClubMembers) null);

            //Act
            async Task Act() => await _clubMemberService.ToggleIsApprovedInClubMembersAsync(It.IsAny<int>(), 1);

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(Act);
            _repoWrapper.Verify(i => i.ClubMembers.Update(It.IsAny<ClubMembers>()), Times.Never);
        }

        [Fact]
        public async Task AddFollowerAsyncTest()
        {
            //Arrange
            _clubService.Setup(s => s.GetClubInfoByIdAsync(It.IsAny<int>())).ReturnsAsync(new ClubDTO());
            _userManagerService.Setup(s => s.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new UserDTO());
            _repoWrapper
                .Setup(s => s.ClubMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubMembers, bool>>>(), null))
                .ReturnsAsync(new ClubMembers());

            //Act
            await _clubMemberService.AddFollowerAsync(It.IsAny<int>(), It.IsAny<string>());

            //Assert
            _repoWrapper.Verify(i => i.ClubMembers.CreateAsync(It.IsAny<ClubMembers>()), Times.Once());
            _repoWrapper.Verify(i => i.ClubMembers.Delete(It.IsAny<ClubMembers>()), Times.Once());
        }

        [Fact]
        public async Task AddFollowerAsync_NotFoundClub_ReturnsArgumentNullException()
        {
            //Arrange
            _repoWrapper.Setup(r => r.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Club, bool>>>(),
                    It.IsAny<Func<IQueryable<Club>, IIncludableQueryable<Club, object>>>()))
                .ReturnsAsync((Club) null);
            _clubService.Setup(s => s.GetClubInfoByIdAsync(It.IsAny<int>())).Throws<ArgumentNullException>();
            _repoWrapper
                .Setup(s => s.ClubMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubMembers, bool>>>(), null))
                .ReturnsAsync((ClubMembers) null);

            //Act
            async Task Act() => await _clubMemberService.AddFollowerAsync(It.IsAny<int>(), It.IsAny<string>());

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(Act);
            _repoWrapper.Verify(i => i.ClubMembers.CreateAsync(It.IsAny<ClubMembers>()), Times.Never);
            _repoWrapper.Verify(i => i.ClubMembers.Delete(It.IsAny<ClubMembers>()), Times.Never);
        }
    }
}