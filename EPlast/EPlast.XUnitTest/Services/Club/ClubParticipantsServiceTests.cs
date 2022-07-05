using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Services.Club;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Xunit;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.XUnitTest.Services.ClubTests
{
    public class ClubParticipantsServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _repositoryWrapper = new Mock<IRepositoryWrapper>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IAdminTypeService> _adminTypeService = new Mock<IAdminTypeService>();
        private readonly IClubParticipantsService _ClubParticipantsService;

        public ClubParticipantsServiceTests()
        {
            _ClubParticipantsService = new ClubParticipantsService(_repositoryWrapper.Object,
                _mapper.Object,
                _adminTypeService.Object,
                null);
        }

        [Fact]
        public async Task GetByClubIdAsyncCorrect()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.ClubAdministration.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.ClubAdministration, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.ClubAdministration>, IIncludableQueryable<DatabaseEntities.ClubAdministration, object>>>()))
                    .ReturnsAsync(new List<DatabaseEntities.ClubAdministration> { new DatabaseEntities.ClubAdministration() });

            // Act
            await _ClubParticipantsService.GetAdministrationByIdAsync(It.IsAny<int>());

            // Assert
            _mapper.Verify(m => m.Map<IEnumerable<DatabaseEntities.ClubAdministration>, IEnumerable<ClubAdministrationDto>>(It.IsAny<IEnumerable<DatabaseEntities.ClubAdministration>>()));
        }

        [Fact]
        public async Task GetCurrentByClubIdAsyncCorrect()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.ClubMembers.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.ClubMembers, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.ClubMembers>, IIncludableQueryable<DatabaseEntities.ClubMembers, object>>>()))
                    .ReturnsAsync(new List<DatabaseEntities.ClubMembers> { new DatabaseEntities.ClubMembers() });

            // Act
            await _ClubParticipantsService.GetMembersByClubIdAsync(It.IsAny<int>());

            // Assert
            _mapper.Verify(m => m.Map<IEnumerable<DatabaseEntities.ClubMembers>, IEnumerable<ClubMembersDto>>(It.IsAny<IEnumerable<DatabaseEntities.ClubMembers>>()));
        }
    }
}