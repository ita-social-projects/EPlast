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
    public class ClubAdministrationServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _repositoryWrapper = new Mock<IRepositoryWrapper>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IAdminTypeService> _adminTypeService = new Mock<IAdminTypeService>();
        private readonly IClubAdministrationService _ClubAdministrationService;

        public ClubAdministrationServiceTests()
        {
            _ClubAdministrationService = new ClubAdministrationService(_repositoryWrapper.Object,
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
            await _ClubAdministrationService.GetAdministrationByIdAsync(It.IsAny<int>());

            // Assert
            _mapper.Verify(m => m.Map<IEnumerable<DatabaseEntities.ClubAdministration>, IEnumerable<ClubAdministrationDTO>>(It.IsAny<IEnumerable<DatabaseEntities.ClubAdministration>>()));
        }
    }
}