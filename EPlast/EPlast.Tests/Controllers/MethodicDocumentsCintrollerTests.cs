using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.DTO;
using EPlast.WebApi.Controllers;
using EPlast.WebApi.Models.MethodicDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.Tests.Controllers
{
    internal class MethodicDocumentsControllerTests
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
                .Setup(x => x.GetOrganizationListAsync())
                .ReturnsAsync(GetFakeOrganizationDtos());

            _service
                .Setup(x => x.GetMethodicDocumentTypes())
                .Returns(GetFakeSelectListItems);

            //Act
            var result = await _controller.GetMetaData();
            var methodicDocument = (result.Result as OkObjectResult).Value;
            var organizations = (methodicDocument as MethodicDocumentCreateViewModel)
                .Organizations;
            var methodicDocumentTypes = (methodicDocument as MethodicDocumentCreateViewModel)
                .MethodicDocumentTypesItems;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult<MethodicDocumentCreateViewModel>>(result);
            Assert.AreEqual(2, organizations.Count());
            Assert.AreEqual(2, methodicDocumentTypes.Count());
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
        public async Task Update_ReturnsNoContentResult()
        {
            //Arrange
            var mockDoc = new MethodicDocumentDTO();
            _service
                .Setup(x => x.ChangeMethodicDocumentAsync(It.IsAny<MethodicDocumentDTO>()));

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
            var organizationName = "SomeName";
            MethodicDocumentWraperDTO docWrapperDTO = new MethodicDocumentWraperDTO()
            {
                MethodicDocument = new MethodicDocumentDTO()
                {
                    Organization = new OrganizationDTO
                    {
                        OrganizationName = organizationName
                    }
                }
            };
            _service
                .Setup(x => x.SaveMethodicDocumentAsync(docWrapperDTO))
                .ReturnsAsync(docWrapperDTO.MethodicDocument.ID);
            _service
                .Setup(x => x.GetMethodicDocumentOrganizationAsync(docWrapperDTO.MethodicDocument.Organization))
                .ReturnsAsync(docWrapperDTO.MethodicDocument.Organization);

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
                .ReturnsAsync(GetFakeDocumentWraperDtos());
            _mapper
                .Setup(m => m.Map<MethodicDocumentViewModel>(It.IsAny<MethodicDocumentDTO>()))
                .Returns(GetFakeDocumentViewModel());
            _service
                .Setup(x => x.GetMethodicDocumentTypes())
                .Returns(GetFakeSelectListItems());

            //Act
            var result = await _controller.Get();
            var resultValue = (result as OkObjectResult).Value;
            var methodicDocuments = resultValue as List<MethodicDocumentViewModel>;

            //Assert
            _service.Verify();
            _mapper.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<List<MethodicDocumentViewModel>>(resultValue);
            Assert.AreEqual(2, methodicDocuments.Count);
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
            Assert.IsInstanceOf<string>(resultValue);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        public List<MethodicDocumentWraperDTO> GetFakeDocumentWraperDtos()
            => new List<MethodicDocumentWraperDTO>
            {
                new MethodicDocumentWraperDTO
                {
                    MethodicDocument = new MethodicDocumentDTO
                    {
                        ID = 1,
                        Type = MethodicDocumentTypeDTO.Other
                    },
                    FileAsBase64 = "file1"
                },
                new MethodicDocumentWraperDTO
                {
                    MethodicDocument = new MethodicDocumentDTO
                    {
                        ID = 2,
                        Type = MethodicDocumentTypeDTO.Other
                    },
                    FileAsBase64 = "file2"
                }
            };

        public MethodicDocumentViewModel GetFakeDocumentViewModel()
            => new MethodicDocumentViewModel
            {
                Id = 1,
                Type = "Value1"
            };

        public List<OrganizationDTO> GetFakeOrganizationDtos()
            => new List<OrganizationDTO>
            {
                new OrganizationDTO
                {
                    ID = 1,
                    OrganizationName = "OrganisationName1"
                },
                new OrganizationDTO
                {
                    ID = 2,
                    OrganizationName = "OrganisationName2"
                }
            };

        public List<SelectListItem> GetFakeSelectListItems()
            => new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "Other",
                    Text = "Text1"
                },
                new SelectListItem
                {
                    Value = "Other",
                    Text = "Text2"
                },
            };
    }
}
