using AutoMapper;
using EPlast.BLL.DTO.Course;
using EPlast.BLL.Services.Blank;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Blank;
using EPlast.DataAccess.Entities.Course;
using EPlast.DataAccess.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EPlast.XUnitTest.Services.Blank
{
    public class UserCourseServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _repoWrapper;
        private readonly Mock<IMapper> _mapper;

        public UserCourseServiceTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task GetCourseByUserIdTest()
        {
           
            _repoWrapper.Setup(r => r.Course.GetAllAsync(It.IsAny<Expression<Func<Course, bool>>>(), null)).ReturnsAsync(new List<Course>()
             {new Course
             {
                Name = "Course Name",
                Link = "https://www.google.com",
                AchievementDocuments=new List<AchievementDocuments>(){
                    new AchievementDocuments
                {
                   UserId="1",
                }
                }
             }});
          
            _mapper.Setup(x => x.Map<Course, CourseDto>(It.IsAny<Course>())).Returns(new CourseDto());
            var service = new UserCourseService(_repoWrapper.Object, _mapper.Object);
            // Act

            var result = await service.GetCourseByUserIdAsync("1");
            // Assert
         
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<CourseDto>>(result);
           
        }

    }
    
}
