using EPlast.BussinessLayer.Interfaces.Club;
using EPlast.BussinessLayer.Services.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace EPlast.XUnitTest.Services.Club
{
    public class ClubMembersServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _repoWrapper;
        private readonly IClubMembersService _clubMemberService;

        public ClubMembersServiceTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _clubMemberService = new ClubMembersService(_repoWrapper.Object);
        }

        [Fact]
        public async Task ToggleIsApprovedInClubMembersAsyncTest()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.ClubMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubMembers, bool>>>(), null))
                .ReturnsAsync(GetTestClubMembers());

            //Act
            await _clubMemberService.ToggleIsApprovedInClubMembersAsync(1, 1);

            //Assert
            _repoWrapper.Verify(i => i.ClubMembers.Update(It.IsAny<ClubMembers>()), Times.Once());
        }

        [Fact]
        public async Task AddFollowerAsyncTest()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.ClubMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubMembers, bool>>>(), null))
                .ReturnsAsync(GetTestClubMembers());

            //Act
            await _clubMemberService.AddFollowerAsync(1, "aaaa-bbbb-cccc");

            //Assert
            _repoWrapper.Verify(i => i.ClubMembers.CreateAsync(It.IsAny<ClubMembers>()), Times.Once());
            _repoWrapper.Verify(i => i.ClubMembers.Delete(It.IsAny<ClubMembers>()), Times.Once());
        }

        private static ClubMembers GetTestClubMembers()
        {
            return new ClubMembers()
            {
                ID = 1,
                IsApproved = true,
                User = new User
                {
                    LastName = "Andrii",
                    FirstName = "Ivanenko"
                }
            };
        }

        //[Fact]
        //public void ChangeIsApprovedStatus_ClubMembersUpdated()
        //{
        //    //arrange
        //    ChangeIsApprovedStatusRepositorySetup();

        //    //action            
        //    controller.ChangeIsApprovedStatus(1, 1);

        //    //assert
        //    _repository.Verify(r => r.ClubMembers.Update(It.IsAny<ClubMembers>()), Times.Once());
        //}
    }
}
