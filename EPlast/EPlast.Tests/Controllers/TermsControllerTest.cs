using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Identity;
using EPlast.BLL.Interfaces.Terms;
using EPlast.DataAccess.Entities;
using EPlast.BLL.DTO.Terms;
using EPlast.WebApi.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Http;
using EPlast.Resources;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;

namespace EPlast.Tests.Controllers
{
    [TestFixture]
    internal class TermsControllerTest
    {
        private Mock<ITermsService> _termsService;
        private Mock<UserManager<User>> _userManager;
        private TermsController _termsController;

        [SetUp]
        public void SetUp()
        {
            _termsService = new Mock<ITermsService>();
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            _termsController = new TermsController(
                _termsService.Object,
                _userManager.Object);
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(u => u.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _termsController.ControllerContext = context;
        }

        [Test]
        public async Task GetTerms_ById_ReturnsOkOdjectResult()
        {
            //Arrange
            _termsService
                .Setup(x => x.GetFirstRecordAsync())
                .ReturnsAsync(new TermsDTO());
            //Act
            var result = await _termsController.GetFirstTermsOfUse();
            var resultValue = (result as OkObjectResult).Value;
            //Assert
            _termsService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<TermsDTO>(resultValue);
        }

        [Test]
        public async Task GetTerms_ById_ReturnsNotFoundResult()
        {
            //Arrange
            _termsService
                .Setup(x => x.GetFirstRecordAsync())
                .ReturnsAsync((TermsDTO)null);
            //Act
            var result = await _termsController.GetFirstTermsOfUse();
            var resultValue = (result as ObjectResult)?.Value;
            //Assert
            _termsService.Verify();
            Assert.IsNotNull(result);
            Assert.IsNull(resultValue);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetAllUsersIdWithoutSender_ReturnsOkObjectResult()
        {
            //Arrange
            _termsService
                .Setup(x => x.GetAllUsersIdWithoutAdminIdAsync(new User()))
                .ReturnsAsync((new List<string>()).AsEnumerable());

            //Act
            var result = await _termsController.GetAllUsersId();
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            _termsService.Verify();
            Assert.IsNotNull(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<OkObjectResult>(result);
            //Assert.IsInstanceOf<List<string>>(resultValue);
        }

        [Test]
        public async Task AddTerms_ReturnsNoContentResult()
        {
            //Arrange
            _termsService.Setup(x => x.AddTermsAsync(It.IsAny<TermsDTO>(), It.IsAny<User>()));

            //Act
            var result = await _termsController.AddTerms(It.IsAny<TermsDTO>());

            //Assert
            _termsService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task AddTerms_ReturnsBadRequestResult()
        {
            //Arrange
            _termsController.ModelState.AddModelError("Title", "title field is required");
            _termsService
                .Setup(x => x.AddTermsAsync(It.IsAny<TermsDTO>(), It.IsAny<User>()));

            //Act
            var result = await _termsController.AddTerms(It.IsAny<TermsDTO>());

            //Assert
            _termsService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task AddTerms_ThrowsException_ReturnsNotFound()
        {
            //Arrange
            _termsService
                .Setup(x => x.AddTermsAsync(It.IsAny<TermsDTO>(), It.IsAny<User>()))
                .Throws(new Exception());

            //Act 
            var result = await _termsController.AddTerms(new TermsDTO());

            //Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task EditTerms_ReturnsNoContentResult()
        {
            //Arrange
            _termsService.Setup(x => x.ChangeTermsAsync(It.IsAny<TermsDTO>(), It.IsAny<User>()));

            //Act
            var result = await _termsController.EditTerms(It.IsAny<TermsDTO>());

            //Assert
            _termsService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task EditTerms_ReturnsBadRequestResult()
        {
            //Arrange
            _termsController.ModelState.AddModelError("Title", "Title field is required");
            _termsService.Setup(x => x.ChangeTermsAsync(It.IsAny<TermsDTO>(), It.IsAny<User>()));

            //Act
            var result = await _termsController.EditTerms(It.IsAny<TermsDTO>());

            //Assert
            _termsService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task EditTerms_ReturnsNotFoundResult()
        {
            //Arrange
            _termsService.Setup(x => x.ChangeTermsAsync(It.IsAny<TermsDTO>(), It.IsAny<User>())).ThrowsAsync(new NullReferenceException("Not found"));
            //Act
            var result = await _termsController.EditTerms(It.IsAny<TermsDTO>());
            //Assert
            _termsService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task EditTerms_ThrowsNullReferenceException_ReturnsNotFound()
        {
            //Arrange
            _termsService
                .Setup(x => x.ChangeTermsAsync(It.IsAny<TermsDTO>(), It.IsAny<User>()))
                .Throws(new NullReferenceException());

            //Act
            var result = await _termsController.EditTerms(new TermsDTO());

            //Assert   
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}