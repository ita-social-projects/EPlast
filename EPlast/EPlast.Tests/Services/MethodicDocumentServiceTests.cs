﻿using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.DTO;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.Tests.Services
{
    class MethodicDocumentServiceTests
    {
        private MethodicDocumentService _service;
        private Mock<IMapper> _mapper;
        private Mock<IRepositoryWrapper> _repository;
        private Mock<IMethodicDocumentBlobStorageRepository> _blobStorage;
        private Mock<IUniqueIdService> _uniqueId;

        [SetUp]
        public void SetUp()
        {
            _blobStorage = new Mock<IMethodicDocumentBlobStorageRepository>();
            _repository = new Mock<IRepositoryWrapper>();
            _uniqueId = new Mock<IUniqueIdService>();
            _mapper = new Mock<IMapper>();
            _service = new MethodicDocumentService(_repository.Object, _mapper.Object, _blobStorage.Object, _uniqueId.Object);
        }

        [Test]
        public void CreateMethodicDocumentTest_ReturnsNewDecision()
        {
            //Act
            var result = _service.CreateMethodicDocument();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<MethodicDocumentWraperDTO>(result);
        }

        [Test]
        public async Task GetMethodicDocumentTest_ReturnsCorrect()
        {
            //Arrange
            _repository
                .Setup(x => x.MethodicDocument.GetFirstAsync(It.IsAny<Expression<Func<MethodicDocument, bool>>>(),
                    It.IsAny<Func<IQueryable<MethodicDocument>, IIncludableQueryable<MethodicDocument, object>>>()))
                .ReturnsAsync(new MethodicDocument());
            _mapper
                .Setup(x => x.Map<MethodicDocumentDTO>(It.IsAny<MethodicDocument>()))
                .Returns(new MethodicDocumentDTO() { ID = 2 });

            //Act
            var result = await _service.GetMethodicDocumentAsync(It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<MethodicDocumentDTO>(result);
        }

        [Test]
        public async Task GetMethodicDocumentListTest_ReturnMethodicDocumentList()
        {
            //Arrange
            _repository
                .Setup(rep => rep.MethodicDocument.GetAllAsync(It.IsAny<Expression<Func<MethodicDocument, bool>>>(),
                    It.IsAny<Func<IQueryable<MethodicDocument>, IIncludableQueryable<MethodicDocument, object>>>()))
                .ReturnsAsync(GetTestDocsQueryable().AsEnumerable);

            //Act
            var result = (await _service.GetMethodicDocumentListAsync()).ToList();

            //Assert
            Assert.IsInstanceOf<List<MethodicDocumentWraperDTO>>(result);
        }


        [TestCase("new name", "new text")]
        [TestCase("", "new text")]
        [TestCase("new name", "")]
        [TestCase("", "")]
        public async Task ChangeMethodicDocumentAsync_Valid_Test(string docNewName, string docnNewDescription)
        {
            //Arrange
            _repository.Setup(rep => rep.MethodicDocument.GetFirstAsync(It.IsAny<Expression<Func<MethodicDocument, bool>>>(),
                    It.IsAny<Func<IQueryable<MethodicDocument>, IIncludableQueryable<MethodicDocument, object>>>()))
                .ReturnsAsync(GetTestDocsQueryable().FirstOrDefault());

            //Act
            var DocsDto = new MethodicDocumentDTO();
            DocsDto.Name = docNewName;
            DocsDto.Description = docnNewDescription;
            await _service.ChangeMethodicDocumentAsync(DocsDto);

            //Assert
            _repository.Verify(rep => rep.MethodicDocument.GetFirstAsync(It.IsAny<Expression<Func<MethodicDocument, bool>>>(),
                   It.IsAny<Func<IQueryable<MethodicDocument>, IIncludableQueryable<MethodicDocument, object>>>()), Times.Once);
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task DeleteMethodicDocumentAsyncTest(int docId)
        {
            //Arrange
            _repository.Setup(rep => rep.MethodicDocument.GetFirstAsync(It.IsAny<Expression<Func<MethodicDocument, bool>>>(),
                    It.IsAny<Func<IQueryable<MethodicDocument>, IIncludableQueryable<MethodicDocument, object>>>()))
                .ReturnsAsync(GetTestDocsQueryable().FirstOrDefault(d => d.ID == docId));

            //Act
            await _service.DeleteMethodicDocumentAsync(docId);

            //Assert
            _repository.Verify(rep => rep.MethodicDocument.GetFirstAsync(It.IsAny<Expression<Func<MethodicDocument, bool>>>(),
                    It.IsAny<Func<IQueryable<MethodicDocument>, IIncludableQueryable<MethodicDocument, object>>>()), Times.Once);
        }

        [TestCase(2)]
        [TestCase(4)]
        [TestCase(1)]
        [TestCase(3)]
        public async Task SaveMethodicDocumentTest(int Id)
        {
            //Arrange
            _mapper
                .Setup(x=>x.Map<MethodicDocument>(new MethodicDocumentDTO { ID = Id })).Returns(new MethodicDocument() { ID = Id });
            _repository.Setup(rep => rep.MethodicDocument.Attach(new MethodicDocument()));
            _repository.Setup(rep => rep.MethodicDocument.Create(new MethodicDocument()));

            //Act
            var actualReturn = await _service.SaveMethodicDocumentAsync(new MethodicDocumentWraperDTO() { MethodicDocument = new MethodicDocumentDTO() { ID= Id } }); 

            //Assert
            Assert.AreEqual(Id, actualReturn);
        }

        [Test]
        public async Task GetDecisionOrganizationAsyncWithRightParameterTest()
        {
            //Arrange
            OrganizationDTO organization = GetTestOrganizationDtoList()[0];
            _repository.Setup(rep => rep.Organization.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                It.IsAny<Func<IQueryable<Organization>, IIncludableQueryable<Organization, object>>>()))
                .ReturnsAsync(new Organization() { OrganizationName = organization.OrganizationName });
            _mapper
                .Setup(x => x.Map<OrganizationDTO>(It.IsAny<string>()))
                .Returns(new OrganizationDTO() { OrganizationName = organization.OrganizationName });
            //Act
            var actualReturn = await _service.GetMethodicDocumentOrganizationAsync(organization);

            //Assert
            Assert.AreEqual(organization.OrganizationName, actualReturn.OrganizationName);
        }

        [TestCase("filename1")]
        [TestCase("filename2")]
        public async Task DownloadMethodicDocumentFileFromBlobAsyncTest(string fileName)
        {
            //Arrange
            _blobStorage.Setup(blobStorage => blobStorage.GetBlobBase64Async(It.IsAny<string>())).ReturnsAsync(fileName);

            //Act
            var actualReturn = await _service.DownloadMethodicDocumentFileFromBlobAsync(fileName);

            //Assert
            Assert.AreEqual(fileName, actualReturn);
        }

        [Test]
        public async Task GetOrganizationListAsyncTest()
        {
            //Arrange
            
            _repository.Setup(rep => rep.Organization.GetAllAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                It.IsAny<Func<IQueryable<Organization>, IIncludableQueryable<Organization, object>>>())).ReturnsAsync(new List<Organization>());
            _mapper
                .Setup(x => x.Map<IEnumerable<OrganizationDTO>>(new List<Organization>()))
                .Returns(GetTestOrganizationDtoList());

            //Act
            var actualReturn = await _service.GetOrganizationListAsync();

            //Assert
            Assert.IsInstanceOf<List<OrganizationDTO>>(actualReturn);
        }

        [Test]
        public void GetMethodicDocumentTypesTest()
        {
            //Act
            var actualReturn = _service.GetMethodicDocumentTypes();

            //Assert
            Assert.IsInstanceOf<List<SelectListItem>>(actualReturn);
        }
        private static IQueryable<MethodicDocument> GetTestDocsQueryable()
        {
            return new List<MethodicDocument>
            {
                new MethodicDocument  {ID = 1,Description = "old"},
                new MethodicDocument  {ID = 2,Description = "old"},
                new MethodicDocument  {ID = 3,Description = "old"},
                new MethodicDocument  {ID = 4,Description = "old"}
            }.AsQueryable();
        }
         private static List<OrganizationDTO> GetTestOrganizationDtoList()
        {
            return new List<OrganizationDTO>
            {
                new OrganizationDTO {ID = 1,OrganizationName = "Organization1"},
                new OrganizationDTO {ID = 2,OrganizationName = "Organization2"},
            };
        }
    }
}
