using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.DTO;
using EPlast.WebApi.Controllers;
using EPlast.WebApi.Models.Decision;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Controllers
{
    [TestFixture]
    class DecisionsControllerTests
    {
        private Mock<IDecisionService> _decisionService;
        private Mock<IPdfService> _pdfService = new Mock<IPdfService>();
        private Mock<IMapper> _mapper;

        private DecisionsController _decisionsController;

        [SetUp]
        public void SetUp()
        {
            _decisionService = new Mock<IDecisionService>();
            _pdfService = new Mock<IPdfService>();
            _mapper = new Mock<IMapper>();

            _decisionsController = new DecisionsController(
                _pdfService.Object,
                _decisionService.Object,
                _mapper.Object);
        }

        [Test]
        public async Task GetMetaData_DecisionById_ReturnsOkObjectResult()
        {
            //Arrange
            _decisionService
                .Setup(x => x.GetGoverningBodyListAsync())
                .ReturnsAsync(new List<GoverningBodyDTO>().AsEnumerable());
            _decisionService
                .Setup(x => x.GetDecisionTargetListAsync())
                .ReturnsAsync(new List<DecisionTargetDTO>().AsEnumerable());
            _decisionService
                .Setup(x => x.GetDecisionStatusTypes())
                .Returns(new List<SelectListItem>().AsEnumerable());

            //Act
            var result = await _decisionsController.GetMetaData();

            //Assert
            _decisionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult<DecisionCreateViewModel>>(result);
        }

        [Test]
        public async Task Get_DecisionById_ReturnsOkObjectResult()
        {
            //Arrange
            _decisionService
                .Setup(x => x.GetDecisionAsync(It.IsAny<int>()))
                .ReturnsAsync(new DecisionDTO());

            //Act
            var result = await _decisionsController.Get(It.IsAny<int>());
            var decisionDTO = (result as ObjectResult).Value as DecisionDTO;

            //Assert
            _decisionService.Verify();
            Assert.IsInstanceOf<DecisionDTO>(decisionDTO);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
      
        [Test]
        public async Task Get_ReturnsNotFoundResult()
        {
            //Arrange
            _decisionService
                .Setup(x => x.GetDecisionAsync(It.IsAny<int>()))
                .ReturnsAsync((DecisionDTO)null);

            //Act
            var result = await _decisionsController.Get(It.IsAny<int>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Update_ReturnsOkObjectResult()
        {
            //Arrange
            var mockDecision = new DecisionDTO();
            _decisionService
                .Setup(x => x.ChangeDecisionAsync(mockDecision));

            //Act
            var result = await _decisionsController.Update(It.IsAny<int>(), mockDecision);

            //Assert
            _decisionService.Verify();
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task Update_InvalidID()
        {
            //Arrange
            var expected = 1;
            var mockDecision = new DecisionDTO();
            mockDecision.ID = 2;
            _decisionService
                .Setup(x => x.ChangeDecisionAsync(mockDecision));

            //Act
            var result = await _decisionsController.Update(expected, mockDecision);

            //Assert
            _decisionService.Verify();
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task Save_ReturnsCreatedResult()
        {
            //Arrange
            var str = "file";
            DecisionWrapperDTO decisionWrapperDTO = new DecisionWrapperDTO()
            {
                Decision = new DecisionDTO()
                {
                    GoverningBody = new GoverningBodyDTO
                    {
                        GoverningBodyName = str
                    }
                }
            };
            _decisionService
                .Setup(x => x.SaveDecisionAsync(decisionWrapperDTO))
                .ReturnsAsync(decisionWrapperDTO.Decision.ID);
            _decisionService
                .Setup(x => x.GetDecisionOrganizationAsync(decisionWrapperDTO.Decision.GoverningBody))
                .ReturnsAsync(decisionWrapperDTO.Decision.GoverningBody);

            //Act
            var result = await _decisionsController.Save(decisionWrapperDTO);

            //Assert
            _decisionService.Verify();
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

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Delete_ReturnsNoContent()
        {
            //Arrange
            _decisionService
                .Setup(x => x.DeleteDecisionAsync(It.IsAny<int>()));

            //Act
            var result = await _decisionsController.Delete(It.IsAny<int>());

            //Assert
            _decisionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task Get_ReturnsOkObjectResult()
        {
            //Arrange
            _decisionService
                .Setup(x => x.GetDecisionListAsync())
                .ReturnsAsync(new List<DecisionWrapperDTO>().AsEnumerable());
            _mapper
                .Setup(m => m.Map<DecisionViewModel>(It.IsAny<DecisionDTO>()))
                .Returns(new DecisionViewModel());
            _decisionService
                .Setup(x => x.GetDecisionStatusTypes())
                .Returns(new List<SelectListItem>().AsEnumerable());

            //Act
            var result = await _decisionsController.Get();

            //Assert
            _decisionService.Verify();
            _mapper.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Download_ReturnsOkObjectResult()
        {
            //Arrange
            var returnResult = "result";
            _decisionService
                .Setup(x => x.DownloadDecisionFileFromBlobAsync(It.IsAny<string>()))
                .ReturnsAsync(returnResult);

            //Act
            var result = await _decisionsController.Download(It.IsAny<string>());
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            _decisionService.Verify();
            Assert.IsNotNull(resultValue);
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
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);

        }
    }
}
