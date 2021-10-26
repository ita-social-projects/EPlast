using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.DTO;
using EPlast.BLL.Interfaces.GoverningBodies;
using EPlast.DataAccess.Entities.Decision;
using EPlast.WebApi.Controllers;
using EPlast.WebApi.Models.Decision;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.Tests.Controllers
{
    [TestFixture]
    internal class DecisionsControllerTests
    {
        private Mock<IDecisionService> _decisionService;
        private Mock<IPdfService> _pdfService = new Mock<IPdfService>();
        private Mock<IMapper> _mapper;
        private Mock<IGoverningBodiesService> _goverrningBodiesService;

        private DecisionsController _decisionsController;

        [SetUp]
        public void SetUp()
        {
            _decisionService = new Mock<IDecisionService>();
            _pdfService = new Mock<IPdfService>();
            _mapper = new Mock<IMapper>();
            _goverrningBodiesService = new Mock<IGoverningBodiesService>();

            _decisionsController = new DecisionsController(
                _pdfService.Object,
                _decisionService.Object,
                _mapper.Object,
                _goverrningBodiesService.Object);
        }

        [Test]
        public async Task GetMetaData_DecisionById_ReturnsOkObjectResult()
        {
            //Arrange
            _goverrningBodiesService
                .Setup(x => x.GetGoverningBodiesListAsync())
                .ReturnsAsync(new List<GoverningBodyDTO>().AsEnumerable());
            _decisionService
                .Setup(x => x.GetDecisionTargetListAsync())
                .ReturnsAsync(GetFakeDecisionTargetDtosDtos());
            _decisionService
                .Setup(x => x.GetDecisionStatusTypes())
                .Returns(GetFakeSelectListItems());

            //Act
            var result = await _decisionsController.GetMetaData();
            var resultValue = (result.Result as OkObjectResult).Value;
            var decisionStatusTypes = (resultValue as DecisionCreateViewModel).DecisionStatusTypeListItems;
            var decisionTargets = (resultValue as DecisionCreateViewModel).DecisionTargets;

            //Assert
            _decisionService.Verify();
            Assert.IsNotNull(result);
            Assert.NotNull(resultValue);
            Assert.IsInstanceOf<DecisionCreateViewModel>(resultValue);
            Assert.AreEqual(2, decisionStatusTypes.Count());
            Assert.AreEqual(2, decisionTargets.Count());
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
        public async Task Update_ReturnsNoContentResult()
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
        public async Task Update_InvalidID_ReturnsBadRequestResult()
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
                .ReturnsAsync(GetFakeDecisionWrapperDtos());
            _mapper
                .Setup(m => m.Map<DecisionViewModel>(It.IsAny<DecisionDTO>()))
                .Returns(GetFakeDecisionViewModel());
            _decisionService
                .Setup(x => x.GetDecisionStatusTypes())
                .Returns(GetFakeSelectListItems());

            //Act
            var result = await _decisionsController.Get();
            var resultValue = (result as OkObjectResult).Value;
            var decisions = resultValue as List<DecisionViewModel>;

            //Assert
            _decisionService.Verify();
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
            _decisionService
                .Setup(x => x.GetDecisionsForTable(It.IsAny<string>(),
                    It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new List<DecisionTableObject>());

            //Act
            var result = _decisionsController.GetDecisionsForTable(It.IsAny<string>(),
                It.IsAny<int>(), It.IsAny<int>());
            var resultValue = (result as OkObjectResult)?.Value;

            //Assert
            _decisionService.Verify();
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
            _decisionService
                .Setup(x => x.DownloadDecisionFileFromBlobAsync(It.IsAny<string>()))
                .ReturnsAsync(returnResult);

            //Act
            var result = await _decisionsController.Download(It.IsAny<string>());
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            _decisionService.Verify();
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
    }
}
