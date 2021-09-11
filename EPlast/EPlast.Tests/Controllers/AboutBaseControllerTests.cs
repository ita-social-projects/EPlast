using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Identity;
using EPlast.BLL.Interfaces.AboutBase;
using EPlast.DataAccess.Entities;
using EPlast.BLL.DTO.AboutBase;
using EPlast.WebApi.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Http;
using EPlast.Resources;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace EPlast.Tests.Controllers
{
    [TestFixture]
    internal class AboutBaseControllerTests
    {
        private Mock<IAboutBaseSectionService> _sectionService;
        private Mock<IAboutBaseSubsectionService> _subsectionSercive;
        private Mock<UserManager<User>> _userManager;

        private AboutBaseController _aboutbaseController;

        [SetUp]
        public void SetUp()
        {
            _sectionService = new Mock<IAboutBaseSectionService>();
            _subsectionSercive = new Mock<IAboutBaseSubsectionService>();
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            _aboutbaseController = new AboutBaseController(
                _sectionService.Object,
                _subsectionSercive.Object,
                _userManager.Object);

        }
        [Test]
        public async Task GetAboutBaseSection_ById_returnsOkObjectResult()
        {
            //Arrange
            _sectionService
                .Setup(x => x.GetSection(It.IsAny<int>()))
                .ReturnsAsync(new SectionDTO());
            //Act
            var result = await _aboutbaseController.GetAboutBaseSection(It.IsAny<int>());
            var resultValue = (result as ObjectResult).Value;
            //Assert
            _sectionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<SectionDTO>(resultValue);
        }
        [Test]
        public async Task GetAboutBaseSection_ById_returnsNotFoundResult()
        {
            //Arrange
            _sectionService
                .Setup(x => x.GetSection(It.IsAny<int>()))
                .ReturnsAsync((SectionDTO)null);
            //Act
            var result = await _aboutbaseController.GetAboutBaseSection(It.IsAny<int>());
            var resultValue = (result as ObjectResult)?.Value;
            //Assert
            _sectionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsNull(resultValue);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public async Task GetAboutBaseSections_ReturnsOkObjectResult()
        {
            //Arrange
            _sectionService
                .Setup(x => x.GetAllSectionAsync())
                .ReturnsAsync((new List<SectionDTO>()).AsEnumerable());
            //Act
            var result = await _aboutbaseController.GetAboutBaseSections();
            var resultValue = (result as OkObjectResult).Value;
            //Assert
            _sectionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<List<SectionDTO>>(resultValue);
        }
        [Test]
        public async Task GetAboutBaseSubsection_ById_ReturnsOkOdjectResult()
        {
            //Arrange
            _subsectionSercive
                .Setup(x => x.GetSubsection(It.IsAny<int>()))
                .ReturnsAsync(new SubsectionDTO());
            //Act
            var result = await _aboutbaseController.GetAboutBaseSubsection(It.IsAny<int>());
            var resultValue = (result as OkObjectResult).Value;
            //Assert
            _subsectionSercive.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<SubsectionDTO>(resultValue);
        }
        [Test]
        public async Task GetAboutBaseSubsection_ById_ReturnsNotFoundResult()
        {
            //Arrange
            _subsectionSercive
                .Setup(x => x.GetSubsection(It.IsAny<int>()))
                .ReturnsAsync((SubsectionDTO)null);
            //Act
            var result = await _aboutbaseController.GetAboutBaseSubsection(It.IsAny<int>());
            var resultValue = (result as ObjectResult)?.Value;
            //Assert
            _subsectionSercive.Verify();
            Assert.IsNotNull(result);
            Assert.IsNull(resultValue);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public async Task GetAboutBaseSubsections_ReturnsOkObjectResult()
        {
            //Arrange
            _subsectionSercive
                .Setup(x => x.GetAllSubsectionAsync())
                .ReturnsAsync(new List <SubsectionDTO>().AsEnumerable());
            //Act
            var result = await _aboutbaseController.GetAboutBaseSubsections();
            var resultValue = (result as OkObjectResult).Value;
            //Assert
            _subsectionSercive.Verify();
            Assert.IsNotNull(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<List<SubsectionDTO>>(resultValue);
        }

        [Test]
        public async Task DeleteAboutBaseSubsection_ReturnsNoContentResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(u => u.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _aboutbaseController.ControllerContext = context;
            _subsectionSercive
                .Setup(x=>x.DeleteSubsection(It.IsAny<int>(),It.IsAny<User>()));
            //Act
            var result = await _aboutbaseController.DeleteAboutBaseSubsection(It.IsAny<int>());
            //Assert
            _subsectionSercive.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteAboutBaseSubsection_ReturnsNotFound()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(u => u.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _aboutbaseController.ControllerContext = context;
            _subsectionSercive
                .Setup(x => x.DeleteSubsection(It.IsAny<int>(), It.IsAny<User>())).ThrowsAsync(new NullReferenceException("Not found"));
            //Act
            var result = await _aboutbaseController.DeleteAboutBaseSubsection(It.IsAny<int>());
            //Assert
            _subsectionSercive.Verify();
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task DeleteAboutBaseSection_ReturnsNoContentResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(u => u.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _aboutbaseController.ControllerContext = context;
            _sectionService
                .Setup(x => x.DeleteSection(It.IsAny<int>(), It.IsAny<User>()));
            //Act
            var result = await _aboutbaseController.DeleteAboutBaseSection(It.IsAny<int>());
            //Assert
            _sectionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }
        [Test]
        public async Task AddAboutBaseSection_ReturnsNoContentResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(u => u.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _aboutbaseController.ControllerContext = context;
            _sectionService
                .Setup(x => x.AddSection(It.IsAny<SectionDTO>(), It.IsAny<User>()));
            //Act
            var result = await _aboutbaseController.AddAboutBaseSection(It.IsAny<SectionDTO>());
            //Assert
            _sectionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }
        [Test]
        public async Task AddAboutBaseSection_ReturnsBadRequestResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(u => u.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _aboutbaseController.ControllerContext = context;
            _aboutbaseController.ModelState.AddModelError("Title", "title field is required");
            _sectionService
                .Setup(x => x.AddSection(It.IsAny<SectionDTO>(), It.IsAny<User>()));
            //Act
            var result = await _aboutbaseController.AddAboutBaseSection(It.IsAny<SectionDTO>());
            //Assert
            _sectionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
        [Test]
        public async Task AddAboutBaseSubsection_ReturnsNoContentResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(u => u.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _aboutbaseController.ControllerContext = context;
            _subsectionSercive
                .Setup(x=>x.AddSubsection(It.IsAny<SubsectionDTO>(),It.IsAny<User>()));
            //Act
            var result = await _aboutbaseController.AddAboutBaseSubsection(It.IsAny<SubsectionDTO>());
            //Assert
            _subsectionSercive.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }
        [Test]
        public async Task AddAboutBaseSubsection_ReturnsBadRequestResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(u => u.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _aboutbaseController.ControllerContext = context;
            _aboutbaseController.ModelState.AddModelError("Title", "Title field is required");
            _subsectionSercive
                .Setup(x => x.AddSubsection(It.IsAny<SubsectionDTO>(), It.IsAny<User>()));
            //Act
            var result = await _aboutbaseController.AddAboutBaseSubsection(It.IsAny<SubsectionDTO>());
            //Assert
            _subsectionSercive.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
        [Test]
        public async Task EditAboutBaseSection_ReturnsNoContentResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(u => u.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _aboutbaseController.ControllerContext = context;
            _sectionService
                .Setup(x => x.ChangeSection(It.IsAny<SectionDTO>(), It.IsAny<User>()));
            //Act
            var result = await _aboutbaseController.EditAboutBaseSection(It.IsAny<SectionDTO>());
            //Assert
            _sectionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }
        [Test]
        public async Task EditAboutBaseSection_ReturnsBadRequestResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(u => u.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _aboutbaseController.ControllerContext = context;
            _aboutbaseController.ModelState.AddModelError("Title", "Title field is required");
            _sectionService
                .Setup(x => x.ChangeSection(It.IsAny<SectionDTO>(), It.IsAny<User>()));
            //Act
            var result = await _aboutbaseController.EditAboutBaseSection(It.IsAny<SectionDTO>());
            //Assert
            _sectionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task EditAboutBaseSubsection_ReturnsNoContentResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(u => u.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _aboutbaseController.ControllerContext = context;
            _subsectionSercive
                .Setup(x => x.ChangeSubsection(It.IsAny<SubsectionDTO>(), It.IsAny<User>()));
            //Act
            var result = await _aboutbaseController.EditAboutBaseSubsection(It.IsAny<SubsectionDTO>());
            //Assert
            _subsectionSercive.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task EditAboutBaseSubsection_ReturnsNotFoundResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(u => u.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _aboutbaseController.ControllerContext = context;
            _subsectionSercive
                .Setup(x => x.ChangeSubsection(It.IsAny<SubsectionDTO>(), It.IsAny<User>())).ThrowsAsync(new NullReferenceException("Not found"));
            //Act
            var result = await _aboutbaseController.EditAboutBaseSubsection(It.IsAny<SubsectionDTO>());
            //Assert
            _subsectionSercive.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task EditAboutBaseSubsection_ReturnsBadRequestResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(u => u.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _aboutbaseController.ControllerContext = context;
            _aboutbaseController.ModelState.AddModelError("Title", "Title field is required");
            _subsectionSercive
                .Setup(x => x.ChangeSubsection(It.IsAny<SubsectionDTO>(), It.IsAny<User>()));
            //Act
            var result = await _aboutbaseController.EditAboutBaseSubsection(It.IsAny<SubsectionDTO>());
            //Assert
            _subsectionSercive.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
    }
}
