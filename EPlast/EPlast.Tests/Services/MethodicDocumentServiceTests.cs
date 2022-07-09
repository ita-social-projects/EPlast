using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.DTO;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Services;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.GoverningBody;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services
{
    class MethodicDocumentServiceTests
    {
        private MethodicDocumentService _service;
        private Mock<IMapper> _mapper;
        private Mock<IRepositoryWrapper> _repository;
        private Mock<IMethodicDocumentBlobStorageRepository> _blobStorage;

        [SetUp]
        public void SetUp()
        {
            _blobStorage = new Mock<IMethodicDocumentBlobStorageRepository>();
            _repository = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _service = new MethodicDocumentService(
                _repository.Object,
                _mapper.Object,
                _blobStorage.Object
            );
        }

        [Test]
        public void CreateMethodicDocumentTest_ReturnsNewDecision()
        {
            //Act
            var result = _service.CreateMethodicDocument();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<MethodicDocumentWraperDto>(result);
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
                .Setup(x => x.Map<MethodicDocumentDto>(It.IsAny<MethodicDocument>()))
                .Returns(new MethodicDocumentDto() { ID = 2 });

            //Act
            var result = await _service.GetMethodicDocumentAsync(It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<MethodicDocumentDto>(result);
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
            Assert.IsInstanceOf<List<MethodicDocumentWraperDto>>(result);
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
            var DocsDto = new MethodicDocumentDto();
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

        [TestCase(1)]
        public void DeleteMethodicDocumentAsyncTest_ThrowsArgumentNullException(int docId)
        {
            //Arrange
            _repository
                .Setup(rep => rep.MethodicDocument.GetFirstAsync(
                    It.IsAny<Expression<Func<MethodicDocument, bool>>>(),
                    It.IsAny<Func<IQueryable<MethodicDocument>, IIncludableQueryable<MethodicDocument, object>>>()))
                .Returns(Task.FromResult<MethodicDocument>(null));

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _service.DeleteMethodicDocumentAsync(docId));
        }

        [TestCase(1)]
        public async Task DeleteMethodicDocumentAsyncTest_DeletesBlob(int docId)
        {
            //Arrange
            _repository
                .Setup(rep => rep.MethodicDocument.GetFirstAsync(
                    It.IsAny<Expression<Func<MethodicDocument, bool>>>(),
                    It.IsAny<Func<IQueryable<MethodicDocument>, IIncludableQueryable<MethodicDocument, object>>>()))
                .ReturnsAsync(new MethodicDocument() { FileName = "someName" });

            //Act
            await _service.DeleteMethodicDocumentAsync(docId);

            //Assert
            _blobStorage.Verify(bs => bs.DeleteBlobAsync(It.IsAny<string>()), Times.Once);
        }

        [TestCase(2)]
        [TestCase(4)]
        [TestCase(1)]
        [TestCase(3)]
        public async Task SaveMethodicDocumentTest(int Id)
        {
            //Arrange
            _mapper
                .Setup(x => x.Map<MethodicDocument>(new MethodicDocumentDto { ID = Id })).Returns(new MethodicDocument() { ID = Id });
            _repository.Setup(rep => rep.MethodicDocument.Attach(new MethodicDocument()));
            _repository.Setup(rep => rep.MethodicDocument.Create(new MethodicDocument()));

            //Act
            var actualReturn = await _service.SaveMethodicDocumentAsync(new MethodicDocumentWraperDto() { MethodicDocument = new MethodicDocumentDto() { ID = Id } });

            //Assert
            Assert.AreEqual(Id, actualReturn);
        }

        [TestCase(1)]
        public async Task SaveMethodicDocumentAsyncTest_UploadsFileToBlob(int id)
        {
            //Arrange
            _mapper
                .Setup(x => x.Map<MethodicDocument>(It.IsAny<MethodicDocumentDto>()))
                .Returns(new MethodicDocument() { ID = id, FileName = "name" });
            _repository
                .Setup(rep => rep.MethodicDocument.Attach(new MethodicDocument()));
            _repository
                .Setup(rep => rep.MethodicDocument.Create(new MethodicDocument()));

            //Act
            int res = await _service.SaveMethodicDocumentAsync(
                new MethodicDocumentWraperDto() { MethodicDocument = new MethodicDocumentDto() { ID = id }, FileAsBase64 = "someName" });

            //Assert
            _blobStorage.Verify(bs => bs.UploadBlobForBase64Async(
                It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.AreEqual(id, res);
        }

        [Test]
        public void GetDocumentsForTable_ReturnsDocumentsTableObject()
        {
            //Arange
            _repository
                .Setup(x => x.MethodicDocument.GetFirstAsync(It.IsAny<Expression<Func<MethodicDocument, bool>>>(),
                    It.IsAny<Func<IQueryable<MethodicDocument>, IIncludableQueryable<MethodicDocument, object>>>()))
                .ReturnsAsync(new MethodicDocument());

            //Act
            var result = _service.GetDocumentsForTable(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(),
                It.IsAny<string>());

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetDecisionOrganizationAsyncWithRightParameterTest()
        {
            //Arrange
            GoverningBodyDto organization = GetTestOrganizationDtoList()[0];
            _repository.Setup(rep => rep.GoverningBody.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                It.IsAny<Func<IQueryable<Organization>, IIncludableQueryable<Organization, object>>>()))
                .ReturnsAsync(new Organization() { OrganizationName = organization.GoverningBodyName });
            _mapper
                .Setup(x => x.Map<GoverningBodyDto>(It.IsAny<string>()))
                .Returns(new GoverningBodyDto() { GoverningBodyName = organization.GoverningBodyName });
            //Act
            var actualReturn = await _service.GetMethodicDocumentOrganizationAsync(organization);

            //Assert
            Assert.AreEqual(organization.GoverningBodyName, actualReturn.GoverningBodyName);
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
            
            _repository.Setup(rep => rep.GoverningBody.GetAllAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                It.IsAny<Func<IQueryable<Organization>, IIncludableQueryable<Organization, object>>>())).ReturnsAsync(new List<Organization>());
            _mapper
                .Setup(x => x.Map<IEnumerable<GoverningBodyDto>>(new List<Organization>()))
                .Returns(GetTestOrganizationDtoList());

            //Act
            var actualReturn = await _service.GetGoverningBodyListAsync();

            //Assert
            Assert.IsInstanceOf<List<GoverningBodyDto>>(actualReturn);
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
        private static List<GoverningBodyDto> GetTestOrganizationDtoList()
        {
            return new List<GoverningBodyDto>
            {
                new GoverningBodyDto {Id = 1, GoverningBodyName = "Organization1"},
                new GoverningBodyDto {Id = 2, GoverningBodyName = "Organization2"},
            };
        }
    }
}
