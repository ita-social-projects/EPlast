using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Services.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace EPlast.XUnitTest.Services.UserProfiles
{
    public class EducationServiceTests
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;

        public EducationServiceTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
        }
        [Fact]
        public async Task GetAllGroupByPlaceTest()
        {
            _repoWrapper.Setup(r => r.Education.GetAllAsync(null,null)).ReturnsAsync(new List<Education>().AsQueryable());

            var service = new EducationService(_repoWrapper.Object, _mapper.Object);
            _mapper.Setup(x => x.Map<Education, EducationDTO>(It.IsAny<Education>())).Returns(new EducationDTO());
            // Act
            var result = await service.GetAllGroupByPlaceAsync();
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<EducationDTO>>(result);
        }
        [Fact]
        public async Task GetAllGroupBySpecialityTest()
        {
            _repoWrapper.Setup(r => r.Education.GetAllAsync(null, null)).ReturnsAsync(new List<Education>().AsQueryable());

            var service = new EducationService(_repoWrapper.Object, _mapper.Object);
            _mapper.Setup(x => x.Map<Education, EducationDTO>(It.IsAny<Education>())).Returns(new EducationDTO());
            // Act
            var result = await service.GetAllGroupBySpecialityAsync();
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<EducationDTO>>(result);
        }
        [Fact]
        public async Task GetByIdTest()
        {
            _repoWrapper.Setup(r => r.Education.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Education, bool>>>(),null)).ReturnsAsync(new Education());

            var service = new EducationService(_repoWrapper.Object, _mapper.Object);
            _mapper.Setup(x => x.Map<Education, EducationDTO>(It.IsAny<Education>())).Returns(new EducationDTO());
            // Act
            var result = await service.GetByIdAsync(1);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<EducationDTO>(result);
        }
    }
}
