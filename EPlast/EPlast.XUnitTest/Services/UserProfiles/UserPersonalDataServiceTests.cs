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
    public class UserPersonalDataServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _repoWrapper;
        private readonly Mock<IMapper> _mapper;

        public UserPersonalDataServiceTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
        }
        [Fact]
        public async Task GetAllDegreesTest()
        {
            _repoWrapper.Setup(r => r.Degree.GetAllAsync(null, null)).ReturnsAsync(new List<Degree>().AsQueryable());

            var service = new UserPersonalDataService(_repoWrapper.Object, _mapper.Object);
            _mapper.Setup(x => x.Map<Degree, DegreeDTO>(It.IsAny<Degree>())).Returns(new DegreeDTO());
            // Act
            var result = await service.GetAllDegreesAsync();
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<DegreeDTO>>(result);
        }

        [Fact]
        public async Task GetAllEducationsGroupByPlaceTest()
        {
            _repoWrapper.Setup(r => r.Education.GetAllAsync(null, null)).ReturnsAsync(new List<Education>().AsQueryable());

            var service = new UserPersonalDataService(_repoWrapper.Object, _mapper.Object);
            _mapper.Setup(x => x.Map<Education, EducationDTO>(It.IsAny<Education>())).Returns(new EducationDTO());
            // Act
            var result = await service.GetAllEducationsGroupByPlaceAsync();
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<EducationDTO>>(result);
        }
        [Fact]
        public async Task GetAllEducationsGroupBySpecialityTest()
        {
            _repoWrapper.Setup(r => r.Education.GetAllAsync(null, null)).ReturnsAsync(new List<Education>().AsQueryable());

            var service = new UserPersonalDataService(_repoWrapper.Object, _mapper.Object);
            _mapper.Setup(x => x.Map<Education, EducationDTO>(It.IsAny<Education>())).Returns(new EducationDTO());
            // Act
            var result = await service.GetAllEducationsGroupBySpecialityAsync();
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<EducationDTO>>(result);
        }
        [Fact]
        public async Task GetEducationsByIdTest()
        {
            _repoWrapper.Setup(r => r.Education.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Education, bool>>>(), null)).ReturnsAsync(new Education());

            var service = new UserPersonalDataService(_repoWrapper.Object, _mapper.Object);
            _mapper.Setup(x => x.Map<Education, EducationDTO>(It.IsAny<Education>())).Returns(new EducationDTO());
            // Act
            var result = await service.GetEducationsByIdAsync(1);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<EducationDTO>(result);
        }

        [Fact]
        public async Task GetGendersAllTest()
        {
            _repoWrapper.Setup(r => r.Gender.GetAllAsync(null, null)).ReturnsAsync(new List<Gender>().AsQueryable());

            var service = new UserPersonalDataService(_repoWrapper.Object, _mapper.Object);
            _mapper.Setup(x => x.Map<Gender, GenderDTO>(It.IsAny<Gender>())).Returns(new GenderDTO());
            // Act
            var result = await service.GetAllGendersAsync();
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<GenderDTO>>(result);
        }

        [Fact]
        public async Task GetAllNationalityTest()
        {
            _repoWrapper.Setup(r => r.Nationality.GetAllAsync(null, null)).ReturnsAsync(new List<Nationality>().AsQueryable());

            var service = new UserPersonalDataService(_repoWrapper.Object, _mapper.Object);
            _mapper.Setup(x => x.Map<Nationality, NationalityDTO>(It.IsAny<Nationality>())).Returns(new NationalityDTO());
            // Act
            var result = await service.GetAllNationalityAsync();
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<NationalityDTO>>(result);
        }

        [Fact]
        public async Task GetAllReligionsTest()
        {
            _repoWrapper.Setup(r => r.Religion.GetAllAsync(null, null)).ReturnsAsync(new List<Religion>().AsQueryable());

            var service = new UserPersonalDataService(_repoWrapper.Object, _mapper.Object);
            _mapper.Setup(x => x.Map<Religion, ReligionDTO>(It.IsAny<Religion>())).Returns(new ReligionDTO());
            // Act
            var result = await service.GetAllReligionsAsync();
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<ReligionDTO>>(result);
        }

        [Fact]
        public async Task GetAllWorkGroupByPlaceTest()
        {
            _repoWrapper.Setup(r => r.Work.GetAllAsync(null, null)).ReturnsAsync(new List<Work>().AsQueryable());

            var service = new UserPersonalDataService(_repoWrapper.Object, _mapper.Object);
            _mapper.Setup(x => x.Map<Work, WorkDTO>(It.IsAny<Work>())).Returns(new WorkDTO());
            // Act
            var result = await service.GetAllWorkGroupByPlaceAsync();
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<WorkDTO>>(result);
        }
        [Fact]
        public async Task GetAllWorkGroupByPositionTest()
        {
            _repoWrapper.Setup(r => r.Work.GetAllAsync(null, null)).ReturnsAsync(new List<Work>().AsQueryable());

            var service = new UserPersonalDataService(_repoWrapper.Object, _mapper.Object);
            _mapper.Setup(x => x.Map<Work, WorkDTO>(It.IsAny<Work>())).Returns(new WorkDTO());
            // Act
            var result = await service.GetAllWorkGroupByPositionAsync();
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<WorkDTO>>(result);
        }
        [Fact]
        public async Task GetWorkByIdTest()
        {
            _repoWrapper.Setup(r => r.Work.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Work, bool>>>(), null)).ReturnsAsync(new Work());

            var service = new UserPersonalDataService(_repoWrapper.Object, _mapper.Object);
            _mapper.Setup(x => x.Map<Work, WorkDTO>(It.IsAny<Work>())).Returns(new WorkDTO());
            // Act
            var result = await service.GetWorkByIdAsync(1);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<WorkDTO>(result);
        }
    }
}
