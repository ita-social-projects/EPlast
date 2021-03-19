using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.DTO;
using EPlast.WebApi.Controllers;
using EPlast.WebApi.Models.MethodicDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.Tests.Controllers
{
    class MethodicDocumentsCintrollerTests
    {
        private Mock<IMethodicDocumentService> _service;
        private Mock<IMapper> _mapper;

        private MethodicDocumentsController _controller;

        [SetUp]
        public void SetUp()
        {
            _service = new Mock<IMethodicDocumentService>();
            _mapper = new Mock<IMapper>();

            _controller = new MethodicDocumentsController(
                _service.Object,
                _mapper.Object);
        }

        [Test]
        public async Task GetMetaData_DocumentById_ReturnsOkObjectResult()
        {
            //Arrange
            _service
                .Setup(x => x.GetGoverningBodyListAsync())
                .ReturnsAsync(new List<GoverningBodyDTO>().AsEnumerable());
           
            _service
                .Setup(x => x.GetMethodicDocumentTypes())
                .Returns(new List<SelectListItem>().AsEnumerable());

            //Act
            var result = await _controller.GetMetaData();

            //Assert
            _service.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult<MethodicDocumentCreateViewModel>>(result);
        }

        [Test]
        public async Task Get_DecisionById_ReturnsOkObjectResult()
        {
            //Arrange
            _service
                .Setup(x => x.GetMethodicDocumentAsync(It.IsAny<int>()))
                .ReturnsAsync(new MethodicDocumentDTO());

            //Act
            var result = await _controller.Get(It.IsAny<int>());
            var decisionDTO = (result as ObjectResult).Value as MethodicDocumentDTO;

            //Assert
            _service.Verify();
            Assert.IsInstanceOf<MethodicDocumentDTO>(decisionDTO);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Get_ReturnsNotFoundResult()
        {
            //Arrange
            _service
                .Setup(x => x.GetMethodicDocumentAsync(It.IsAny<int>()))
                .ReturnsAsync((MethodicDocumentDTO)null);

            //Act
            var result = await _controller.Get(It.IsAny<int>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Update_ReturnsOkObjectResult()
        {
            //Arrange
            var mockDoc = new MethodicDocumentDTO();
            _service
                .Setup(x => x.ChangeMethodicDocumentAsync(mockDoc));

            //Act
            var result = await _controller.Update(It.IsAny<int>(), mockDoc);

            //Assert
            _service.Verify();
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task Update_InvalidID()
        {
            //Arrange
            var expected = 1;
            var mockDoc = new MethodicDocumentDTO();
            mockDoc.ID = 2;
            _service
                .Setup(x => x.ChangeMethodicDocumentAsync(mockDoc));

            //Act
            var result = await _controller.Update(expected, mockDoc);

            //Assert
            _service.Verify();
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task Save_ReturnsCreatedResult()
        {
            //Arrange
            var str = "file";
            MethodicDocumentWraperDTO docWrapperDTO = new MethodicDocumentWraperDTO()
            {
                MethodicDocument = new MethodicDocumentDTO()
                {
                    GoverningBody = new GoverningBodyDTO
                    {
                        GoverningBodyName = str
                    }
                }
            };
            _service
                .Setup(x => x.SaveMethodicDocumentAsync(docWrapperDTO))
                .ReturnsAsync(docWrapperDTO.MethodicDocument.ID);
            _service
                .Setup(x => x.GetMethodicDocumentOrganizationAsync(docWrapperDTO.MethodicDocument.GoverningBody))
                .ReturnsAsync(docWrapperDTO.MethodicDocument.GoverningBody);

            //Act
            var result = await _controller.Save(docWrapperDTO);

            //Assert
            _service.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public async Task Save_ReturnsBadRequestResult()
        {
            //Arrange
            var docWrapper = new MethodicDocumentWraperDTO()
            {
                MethodicDocument = new MethodicDocumentDTO
                {
                    FileName = "string"
                },
                FileAsBase64 = null
            };

            //Act
            var result = await _controller.Save(docWrapper);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Delete_ReturnsNoContent()
        {
            //Arrange
            _service
                .Setup(x => x.DeleteMethodicDocumentAsync(It.IsAny<int>()));

            //Act
            var result = await _controller.Delete(It.IsAny<int>());

            //Assert
            _service.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task Get_ReturnsOkObjectResult()
        {
            //Arrange
            _service
                .Setup(x => x.GetMethodicDocumentListAsync())
                .ReturnsAsync(new List<MethodicDocumentWraperDTO>().AsEnumerable());
            _mapper
                .Setup(m => m.Map<MethodicDocumentViewModel>(It.IsAny<MethodicDocumentDTO>()))
                .Returns(new MethodicDocumentViewModel());
            _service
                .Setup(x => x.GetMethodicDocumentTypes())
                .Returns(new List<SelectListItem>().AsEnumerable());

            //Act
            var result = await _controller.Get();

            //Assert
            _service.Verify();
            _mapper.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Download_ReturnsOkObjectResult()
        {
            //Arrange
            var returnResult = "result";
            _service
                .Setup(x => x.DownloadMethodicDocumentFileFromBlobAsync(It.IsAny<string>()))
                .ReturnsAsync(returnResult);

            //Act
            var result = await _controller.Download(It.IsAny<string>());
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            _service.Verify();
            Assert.IsNotNull(resultValue);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
    }
}
