using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.Commands.Decision;
using EPlast.BLL.DTO;
using EPlast.BLL.Interfaces.GoverningBodies;
using EPlast.BLL.Queries.Decision;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities.Decision;
using EPlast.WebApi.Controllers;
using EPlast.WebApi.Models.Decision;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Controllers
{
    [TestFixture]
    internal class DecisionsControllerTests
    {
        private Mock<IPdfService> _pdfService = new Mock<IPdfService>();
        private Mock<IMapper> _mapper;
        private Mock<IGoverningBodiesService> _goverrningBodiesService;
        private Mock<IUserManagerService> _userManagerService;
        private Mock<HttpContext> _httpContext;
        private ControllerContext _context;
        private Mock<IMediator> _mockMediator;
        private Mock<IDecisionVmInitializer> _mockGetDecisionStatusTypesExtention;
        private DecisionsController _decisionsController;

        [SetUp]
        public void SetUp()
        {
            _mockMediator = new Mock<IMediator>();
            _pdfService = new Mock<IPdfService>();
            _mapper = new Mock<IMapper>();
            _userManagerService = new Mock<IUserManagerService>();
            _goverrningBodiesService = new Mock<IGoverningBodiesService>();
            _httpContext = new Mock<HttpContext>();
            _mockGetDecisionStatusTypesExtention = new Mock<IDecisionVmInitializer>();
            _context = new ControllerContext(
               new ActionContext(
                   _httpContext.Object, new RouteData(),
                   new ControllerActionDescriptor()));
            _decisionsController = new DecisionsController(
                _pdfService.Object,
                _userManagerService.Object,
                _mapper.Object,
                _goverrningBodiesService.Object, _mockMediator.Object);
        }
        
        [Test]
        public async Task GetMetaData_DecisionById_ReturnsOkObjectResult()
        {
            //Arrange
            _goverrningBodiesService
                .Setup(x => x.GetGoverningBodiesListAsync())
                .ReturnsAsync(new List<GoverningBodyDTO>().AsEnumerable());
            _mockGetDecisionStatusTypesExtention.Setup(x => x.GetDecesionStatusTypes()).Returns(GetFakeSelectListItems());

            //Act
            var result = await _decisionsController.GetMetaData();
            var resultValue = (result.Result as OkObjectResult).Value;
            var decisionStatusTypes = (resultValue as DecisionCreateViewModel).DecisionStatusTypeListItems;

            //Assert
            Assert.IsNotNull(result);
            Assert.NotNull(resultValue);
            Assert.IsInstanceOf<DecisionCreateViewModel>(resultValue);
            Assert.AreEqual(3, decisionStatusTypes.Count());
            Assert.IsInstanceOf<ActionResult<DecisionCreateViewModel>>(result);
        }

        [Test]
        public async Task Get_DecisionById_ReturnsOkObjectResult()
        {
            //Arrange
            _mockMediator.Setup(x=>x.Send(It.IsAny<GetDecisionAsyncQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new DecisionDTO());

            //Act
            var result = await _decisionsController.Get(It.IsAny<int>());
            var decisionDTO = (result as ObjectResult).Value as DecisionDTO;

            //Assert
            Assert.IsInstanceOf<DecisionDTO>(decisionDTO);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Get_ReturnsNotFoundResult()
        {
            //Arrange
            //Act
            var result = await _decisionsController.Get(It.IsAny<int>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Update_ReturnsNoContentResult()
        {
            //Arrange
            _httpContext = new Mock<HttpContext>();

            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);

            _httpContext.Setup(t => t.User).Returns(principal);
            DecisionsController decisionsController = _decisionsController;
            decisionsController.ControllerContext = _context;
            var mockDecision = new DecisionDTO() { UserId = "qwerty"};
            _mockMediator.Setup(x=>x.Send(It.IsAny<GetDecisionAsyncQuery>(),It.IsAny<CancellationToken>())).ReturnsAsync(mockDecision);
            _mockMediator.Setup(x => x.Send(It.IsAny<UpdateCommand>(), It.IsAny<CancellationToken>()));
            _userManagerService.Setup(x => x.GetCurrentUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("qwerty");
            //Act
            var result = await _decisionsController.Update(It.IsAny<int>(), mockDecision);

            //Assert
            _mockMediator.Verify();
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task Update_Returns403Result()
        {
            //Arrange
            _httpContext = new Mock<HttpContext>();

            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);
            var expected = StatusCodes.Status403Forbidden;
            _httpContext.Setup(t => t.User).Returns(principal);
            DecisionsController decisionsController = _decisionsController;
            decisionsController.ControllerContext = _context;
            var mockDecision = new DecisionDTO() { UserId = "qwerty1" };
            _mockMediator.Setup(x=>x.Send(It.IsAny<GetDecisionAsyncQuery>(),It.IsAny<CancellationToken>())).ReturnsAsync(mockDecision);
            _mockMediator.Setup(x => x.Send(It.IsAny<UpdateCommand>(), It.IsAny<CancellationToken>()));           
            _userManagerService.Setup(x => x.GetCurrentUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("qwerty");
            //Act
            var result = await _decisionsController.Update(It.IsAny<int>(), mockDecision);
            var actual = (result as StatusCodeResult).StatusCode;
            //Assert
            _mockMediator.Verify();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task Update_InvalidID_ReturnsBadRequestResult()
        {
            //Arrange
            var expected = 1;
            var mockDecision = new DecisionDTO();

            //Act
            var result = await _decisionsController.Update(expected, mockDecision);

            //Assert
            _mockMediator.Verify();
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task Save_ReturnsCreatedResult()
        {
            //Arrange
            var governingBodyName = "SomeName";
            DecisionWrapperDTO decisionWrapperDTO = new DecisionWrapperDTO()
            {
                Decision = new DecisionDTO()
                {
                    GoverningBody = new GoverningBodyDTO
                    {
                        GoverningBodyName = governingBodyName
                    }
                }
            };
            _mockMediator.Setup(x=>x.Send(It.IsAny<SaveDecisionAsyncCommand>(),It.IsAny<CancellationToken>())).ReturnsAsync(decisionWrapperDTO.Decision.ID);
            _mockMediator.Setup(x=>x.Send(It.IsAny<GetDecisionOrganizationAsyncQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(decisionWrapperDTO.Decision.GoverningBody);
           

            //Act
            var result = await _decisionsController.Save(decisionWrapperDTO);

            //Assert
            _mockMediator.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public async Task Save_ReturnsBadRequestResult()
        {
            //Arrange
            var decisionWrapper = new DecisionWrapperDTO()
            {
                Decision = new DecisionDTO
                {
                    FileName = "string"
                },
                FileAsBase64 = null
            };

            //Act
            var result = await _decisionsController.Save(decisionWrapper);
            var resultValue = (result as BadRequestObjectResult).Value;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.IsInstanceOf<string>(resultValue);
        }

        [Test]
        public async Task Delete_ReturnsNoContent()
        {
            //Arrange
            //Act
            var result = await _decisionsController.Delete(It.IsAny<int>());

            //Assert
            _mockMediator.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task Get_ReturnsOkObjectResult()
        {
            //Arrange
            _mockMediator.Setup(x=>x.Send(It.IsAny<GetDecisionListAsyncQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(GetFakeDecisionWrapperDtos());
            _mapper
                .Setup(m => m.Map<DecisionViewModel>(It.IsAny<DecisionDTO>()))
                .Returns(GetFakeDecisionViewModel());
            _mockGetDecisionStatusTypesExtention.Setup(x => x.GetDecesionStatusTypes()).Returns(GetFakeSelectListItems());

            //Act
            var result = await _decisionsController.Get();
            var resultValue = (result as OkObjectResult).Value;
            var decisions = resultValue as List<DecisionViewModel>;

            //Assert
            _mockMediator.Verify();
            _mapper.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<List<DecisionViewModel>>(resultValue);
            Assert.AreEqual(2, decisions.Count);
        }

        [Test]
        public void GetDecisionsForTable_ReturnsOkObjectResult()
        {
            //Arrange
            _mockMediator.Setup(x => x.Send(It.IsAny<GetDecisionsForTableQuery>(),It.IsAny<CancellationToken>())).Returns(FakedDecisionTableObject);

            //Act
            var result = _decisionsController.GetDecisionsForTable(It.IsAny<string>(),
                It.IsAny<int>(), It.IsAny<int>());
            var resultValue = (result as OkObjectResult)?.Value;

            //Assert
            _mockMediator.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<List<DecisionTableObject>>(resultValue);
        }

        [Test]
        public async Task Download_ReturnsOkObjectResult()
        {
            //Arrange
            var returnResult = "result";
            _mockMediator
                .Setup(x => x.Send(It.IsAny<DownloadDecisionFileFromBlobAsyncQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnResult);

            //Act
            var result = await _decisionsController.Download(It.IsAny<string>());
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            _mockMediator.Verify();
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<string>(resultValue);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task CreatePdf_ReturnsOkObjectResult()
        {
            //Arrange
            byte[] bytesReturn = new byte[3] { 0, 2, 3 };
            _pdfService
                .Setup(x => x.DecisionCreatePDFAsync(It.IsAny<int>()))
                .ReturnsAsync(bytesReturn);

            //Act
            var result = await _decisionsController.CreatePdf(It.IsAny<int>());
            var resultValue = (result as ObjectResult).Value;

            //Assert
            _pdfService.Verify();
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<string>(resultValue);
            Assert.AreNotEqual(string.Empty, resultValue);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
        [Test]
        public async Task GetTargetList_ReturnsOkObjectResult()
        {
            //Act
            var result = await _decisionsController.GetDecisionTargetSearchList(It.IsAny<string>());

            //Assert
            _mockMediator.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
        public DecisionViewModel GetFakeDecisionViewModel()
            => new DecisionViewModel
            {
                Id = 1
            };

        public List<DecisionWrapperDTO> GetFakeDecisionWrapperDtos()
            => new List<DecisionWrapperDTO>
            {
                new DecisionWrapperDTO
                {
                    Decision = new DecisionDTO()
                    {
                        ID = 1,
                        DecisionStatusType = DecisionStatusTypeDTO.Confirmed
                    },
                    FileAsBase64 = "file1"
                },
                new DecisionWrapperDTO
                {
                    Decision = new DecisionDTO()
                    {
                        ID = 2,
                        DecisionStatusType = DecisionStatusTypeDTO.Confirmed
                    },
                    FileAsBase64 = "file2"
                }
            };


        public List<DecisionTargetDTO> GetFakeDecisionTargetDtosDtos()
            => new List<DecisionTargetDTO>
            {
                new DecisionTargetDTO
                {
                    ID = 1,
                    TargetName = "Name1"
                },
                new DecisionTargetDTO
                {
                    ID = 2,
                    TargetName = "Name2"
                }
            };

        public List<SelectListItem> GetFakeSelectListItems()
            => new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "Confirmed",
                    Text = "Text1"
                },
                new SelectListItem
                {
                    Value = "Confirmed",
                    Text = "Text2"
                },
            };
            
        public Task<IEnumerable<DecisionTableObject>> FakedDecisionTableObject()
        {
            return Task.FromResult(new List<DecisionTableObject>().AsEnumerable());
        }
    }
}
