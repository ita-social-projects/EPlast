using EPlast.BLL.Commands.TermsOfUse;
using EPlast.BLL.DTO.Terms;
using EPlast.BLL.Queries.TermsOfUse;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using EPlast.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.Tests.Controllers
{
    [TestFixture]
    internal class TermsControllerTest
    {
        private Mock<IMediator> _mockMediator;
        private Mock<UserManager<User>> _userManager;
        private TermsController _termsController;

        [SetUp]
        public void SetUp()
        {
            _mockMediator = new Mock<IMediator>();
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _termsController = new TermsController(_userManager.Object, _mockMediator.Object);
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
            _mockMediator
                .Setup(x => x.Send(It.IsAny<GetFirstRecordQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TermsDTO());
            //Act
            var result = await _termsController.GetFirstTermsOfUse();
            var resultValue = (result as OkObjectResult).Value;
            //Assert
            _mockMediator.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<TermsDTO>(resultValue);
        }

        [Test]
        public async Task GetTerms_ById_ReturnsNotFoundResult()
        {
            //Arrange
            _mockMediator
                .Setup(x => x.Send(It.IsAny<GetFirstRecordQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((TermsDTO)null);

            //Act
            var result = await _termsController.GetFirstTermsOfUse();
            var resultValue = (result as ObjectResult)?.Value;

            //Assert
            _mockMediator.Verify();
            Assert.IsNotNull(result);
            Assert.IsNull(resultValue);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetAllUsersIdWithoutSender_ReturnsOkObjectResult()
        {
            //Arrange
            _mockMediator
                .Setup(x => x.Send(It.IsAny<GetAllUsersIdWithoutSenderQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((new List<string>()).AsEnumerable());

            //Act
            var result = await _termsController.GetAllUsersId();
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            _mockMediator.Verify();
            Assert.IsNotNull(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<List<string>>(resultValue);
        }

        [Test]
        public async Task AddTerms_ReturnsNoContentResult()
        {
            //Arrange
            _mockMediator.Setup(x => x.Send(It.IsAny<AddTermsCommand>(), It.IsAny<CancellationToken>()));

            //Act
            var result = await _termsController.AddTerms(It.IsAny<TermsDTO>());

            //Assert
            _mockMediator.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task AddTerms_ReturnsBadRequestResult()
        {
            //Arrange
            _termsController.ModelState.AddModelError("Title", "title field is required");
            _mockMediator
                .Setup(x => x.Send(It.IsAny<AddTermsCommand>(), It.IsAny<CancellationToken>()));

            //Act
            var result = await _termsController.AddTerms(It.IsAny<TermsDTO>());

            //Assert
            _mockMediator.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task AddTerms_ThrowsException_ReturnsNotFound()
        {
            //Arrange
            _mockMediator
                .Setup(x => x.Send(It.IsAny<AddTermsCommand>(), It.IsAny<CancellationToken>()))
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
            _mockMediator.Setup(x => x.Send(It.IsAny<ChangeTermsCommand>(), It.IsAny<CancellationToken>()));

            //Act
            var result = await _termsController.EditTerms(It.IsAny<TermsDTO>());

            //Assert
            _mockMediator.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task EditTerms_ReturnsBadRequestResult()
        {
            //Arrange
            _termsController.ModelState.AddModelError("Title", "Title field is required");
            _mockMediator.Setup(x => x.Send(It.IsAny<ChangeTermsCommand>(), It.IsAny<CancellationToken>()));

            //Act
            var result = await _termsController.EditTerms(It.IsAny<TermsDTO>());

            //Assert
            _mockMediator.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task EditTerms_ReturnsNotFoundResult()
        {
            //Arrange
            _mockMediator.Setup(x => x.Send(It.IsAny<ChangeTermsCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NullReferenceException("Not found"));
            //Act
            var result = await _termsController.EditTerms(It.IsAny<TermsDTO>());
            //Assert
            _mockMediator.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task EditTerms_ThrowsNullReferenceException_ReturnsNotFound()
        {
            //Arrange
            _mockMediator
                .Setup(x => x.Send(It.IsAny<ChangeTermsCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new NullReferenceException());

            //Act
            var result = await _termsController.EditTerms(new TermsDTO());

            //Assert   
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}