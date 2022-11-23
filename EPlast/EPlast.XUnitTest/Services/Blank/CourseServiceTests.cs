using AutoMapper;
using EPlast.BLL.DTO.Course;
using EPlast.BLL.Services.Blank;
using EPlast.DataAccess.Entities.Course;
using EPlast.DataAccess.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EPlast.XUnitTest.Services.Blank
{
    public class CourseServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _repoWrapper;
        private readonly Mock<IMapper> _mapper;

        public CourseServiceTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
        }
        [Fact]
        public async Task GetAllCourseTest()
        {
            // Arrange
            _repoWrapper.Setup(r => r.Course.GetAllAsync(null, null)).ReturnsAsync(new List<Course>().AsQueryable());
            _mapper.Setup(x => x.Map<Course, CourseDto>(It.IsAny<Course>())).Returns(new CourseDto());
            var service = new CourseService(_repoWrapper.Object, _mapper.Object);
            // Act
            var result = await service.GetAllAsync();
            // Assert
            Assert.NotNull(result); 
            Assert.IsAssignableFrom<IEnumerable<CourseDto>>(result);
        }
        [Fact]
        public async Task AddCourseTests()
        {
            // Arrange
            _mapper.Setup(m => m.Map<CourseDto, DataAccess.Entities.Course.Course>(It.IsAny<CourseDto>()))
               .Returns(() => new DataAccess.Entities.Course.Course());
            _repoWrapper.Setup(r => r.Course.Create(It.IsAny<Course>()))
               .Verifiable();
            var service = new CourseService(_repoWrapper.Object, _mapper.Object);
          
            CourseDto courseDto = new CourseDto { 
                ID=0
            };
            // Act 
            var result = await service.AddCourseAsync(courseDto);
            // Assert
            Assert.IsType<CourseDto>(result);
        }
       
    }
}
