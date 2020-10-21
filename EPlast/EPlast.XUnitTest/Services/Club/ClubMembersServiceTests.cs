using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Services.Club;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Xunit;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.XUnitTest.Services.ClubTests
{
    public class ClubMembersServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _repositoryWrapper = new Mock<IRepositoryWrapper>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly IClubMembersService _ClubMembersService;

        public ClubMembersServiceTests()
        {
            _ClubMembersService = new ClubMembersService(_repositoryWrapper.Object, _mapper.Object, null, null);
        }

        [Fact]
        public async Task GetCurrentByClubIdAsyncCorrect()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.ClubMembers.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.ClubMembers, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.ClubMembers>, IIncludableQueryable<DatabaseEntities.ClubMembers, object>>>()))
                    .ReturnsAsync(new List<DatabaseEntities.ClubMembers> { new DatabaseEntities.ClubMembers() });

            // Act
            await _ClubMembersService.GetMembersByClubIdAsync(It.IsAny<int>());

            // Assert
            _mapper.Verify(m => m.Map<IEnumerable<DatabaseEntities.ClubMembers>, IEnumerable<ClubMembersDTO>>(It.IsAny<IEnumerable<DatabaseEntities.ClubMembers>>()));
        }
    }
}