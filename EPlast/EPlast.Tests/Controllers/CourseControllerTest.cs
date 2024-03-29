﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Course;
using EPlast.BLL.Interfaces.Blank;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Queries.Club;
using EPlast.DataAccess.Entities;
using EPlast.WebApi.Controllers;
using EPlast.WebApi.Models.Club;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Controllers
{
    public class CourseControllerTest
    {
        private readonly Mock<ICourseService> _courseService;
        private readonly Mock<IUserCourseService> _usercourseService;
        private readonly Mock<ILoggerService<CoursesController>> _loggerService;
        private readonly Mock<UserManager<User>> _userManager;
        private readonly CoursesController _controller;
        private readonly Mock<HttpContext> _httpContext;
        private readonly ControllerContext _context;


        public CourseControllerTest()
        {
            _courseService = new Mock<ICourseService>();
            _usercourseService = new Mock<IUserCourseService>();
            _loggerService = new Mock<ILoggerService<CoursesController>>();
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            _httpContext = new Mock<HttpContext>();

            _context = new ControllerContext(
                new ActionContext(
                    _httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));

        }

        private CoursesController CreateCoursesController => new CoursesController(_courseService.Object,
             _usercourseService.Object,
             _userManager.Object,
           _loggerService.Object
          );

        [Test]
        public async Task GetAllTest_ReturnsInstanceOfOkObjectResult()
        {
            // Arrange
            CoursesController controller = CreateCoursesController;

            controller.ControllerContext = _context;
            _courseService
                .Setup(c => c.GetAllAsync())
                .ReturnsAsync(GetFakeCourses());

            const string EXPEXTED_STR = "Course";
            // Act
            var result = await controller.GetAll();

            // Assert
            Assert.Multiple(() => {
                Assert.NotNull(result);
                Assert.IsInstanceOf<OkObjectResult>(result);
                Assert.IsNotNull(((result as ObjectResult).Value as List<CourseDto>)
                .Where(n => n.Name.Equals(EXPEXTED_STR)));
            });
            
        }

        [Test]
        public async Task GetAllCourseByUserIdTest_ReturnsInstanceOfOkObjectResult()
        {
            // Arrange
            CoursesController controller = CreateCoursesController;
            
            controller.ControllerContext = _context;
            _courseService
                .Setup(c => c.GetAllAsync())
                .ReturnsAsync(GetFakeCourses());
            _userManager
                .Setup(c => c.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User { 
                    Approvers = new List<Approver>(),
                    ConfirmedUsers = new List<ConfirmedUser>()
                });
            _usercourseService.
                Setup(c => c.GetCourseByUserIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetFakeCourses());
            const int USER_ID = 1;
            const string EXPEXTED_STR = "Course";
            // Act
            var result = await controller.GetAllCourseByUserId(USER_ID.ToString());

            // Assert
            Assert.Multiple(() => {
                Assert.NotNull(result);
                Assert.IsInstanceOf<OkObjectResult>(result);
                Assert.IsNotNull(((result as ObjectResult).Value as List<CourseDto>)
                .Where(n => n.Name.Equals(EXPEXTED_STR)));
            });
        }

        [Test]
        public async Task AddCourseTest_ReturnsInstanceOfCreatedResult()
        {
            // Arrange
            CoursesController controller = CreateCoursesController;
            
            controller.ControllerContext = _context;
            _courseService
                .Setup(c => c.AddCourseAsync(It.IsAny<CourseDto>()));

            // Act
            var result = await controller.AddCourse(It.IsAny<CourseDto>());

            // Assert
            Assert.IsInstanceOf<CreatedResult>(result);
        }

        private IEnumerable<CourseDto> GetFakeCourses()
        {
            return new List<CourseDto>()
            {
                new CourseDto
                {
                    Name = "Course"
                }
            }.AsEnumerable();
        }



    }
}
