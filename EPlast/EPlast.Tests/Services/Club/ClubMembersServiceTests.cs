using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Services.Club;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
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
    public class ClubMembersServiceTests
    {
        private ClubMembersService _clubMembersService;
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private Mock<UserManager<User>> _userManager;
        private Mock<IUserStore<User>> _user;
        private Mock<IClubAdministrationService> _clubAdministrationService;
        private Mock<IClubService> _clubService;
        private Mock<IUserManagerService> _userManagerService;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _clubAdministrationService = new Mock<IClubAdministrationService>();
            _user = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(_user.Object, null, null, null, null, null, null, null, null);
            _userManagerService = new Mock<IUserManagerService>();
            _clubService = new Mock<IClubService>();
            _clubMembersService = new ClubMembersService(_repoWrapper.Object, _mapper.Object, _userManager.Object,
                _clubAdministrationService.Object);
        }


        [Test]
        public async Task GetMembersByClubIdAsync_ReturnsMembers()
        {
            // Arrange
            _repoWrapper.Setup(r => r.ClubMembers.GetAllAsync(It.IsAny<Expression<Func<DataAccessClub.ClubMembers, bool>>>(),
            It.IsAny<Func<IQueryable<DataAccessClub.ClubMembers>, IIncludableQueryable<DataAccessClub.ClubMembers, object>>>()))
            .ReturnsAsync(new List<DataAccessClub.ClubMembers> { new DataAccessClub.ClubMembers() }); ;

            // Act
            var result = await _clubMembersService.GetMembersByClubIdAsync(It.IsAny<int>());

            // Assert
            Assert.NotNull(result);
            _mapper.Verify(m => m.Map<IEnumerable<DataAccessClub.ClubMembers>, IEnumerable<ClubMembersDTO>>(It.IsAny<IEnumerable<DataAccessClub.ClubMembers>>()));
        }

        [Test]
        public async Task ToggleApproveStatusAsyncTest()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.ClubMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessClub.ClubMembers, bool>>>(),
            It.IsAny<Func<IQueryable<DataAccessClub.ClubMembers>, IIncludableQueryable<DataAccessClub.ClubMembers, object>>>()))
                .ReturnsAsync(new ClubMembers());

            //Act
            await _clubMembersService.ToggleApproveStatusAsync(It.IsAny<int>());

            //Assert
            _repoWrapper.Verify(i => i.ClubMembers.Update(It.IsAny<ClubMembers>()), Times.Once());
        }
    }
}

