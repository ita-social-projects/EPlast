using AutoMapper;
using EPlast.BussinessLayer.DTO.UserProfiles;
using EPlast.BussinessLayer.Services.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EPlast.XUnitTest.Services.UserProfiles
{
    public class ReligionServiceTest
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;

        public ReligionServiceTest()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
        }
        [Fact]
        public async Task GetAllTest()
        {
            _repoWrapper.Setup(r => r.Religion.GetAllAsync(null, null)).ReturnsAsync(new List<Religion>().AsQueryable());

            var service = new ReligionService(_repoWrapper.Object, _mapper.Object);
            _mapper.Setup(x => x.Map<Religion, ReligionDTO>(It.IsAny<Religion>())).Returns(new ReligionDTO());
            // Act
            var result = await service.GetAllAsync();
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<ReligionDTO>>(result);
        }
    }
}
