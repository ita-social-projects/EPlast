using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.DTO;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Services;
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
using EPlast.DataAccess.Entities.GoverningBody;

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
                .Setup(x=>x.Map<MethodicDocument>(new MethodicDocumentDTO { ID = Id })).Returns(new MethodicDocument() { ID = Id });
            _repository.Setup(rep => rep.MethodicDocument.Attach(new MethodicDocument()));
            _repository.Setup(rep => rep.MethodicDocument.Create(new MethodicDocument()));

            //Act
            var actualReturn = await _service.SaveMethodicDocumentAsync(new MethodicDocumentWraperDTO() { MethodicDocument = new MethodicDocumentDTO() { ID= Id } }); 

            //Assert
            Assert.AreEqual(Id, actualReturn);
        }

        [TestCase(1)]
        public async Task SaveMethodicDocumentAsyncTest_UploadsFileToBlob(int id)
        {
            //Arrange
            _mapper
                .Setup(x => x.Map<MethodicDocument>(It.IsAny<MethodicDocumentDTO>()))
                .Returns(new MethodicDocument() { ID = id, FileName = "name"});
            _repository
                .Setup(rep => rep.MethodicDocument.Attach(new MethodicDocument()));
            _repository
                .Setup(rep => rep.MethodicDocument.Create(new MethodicDocument()));
            _uniqueId
                .Setup(x => x.GetUniqueId())
                .Returns(Guid.NewGuid());

            //Act
            int res = await _service.SaveMethodicDocumentAsync(
                new MethodicDocumentWraperDTO() { MethodicDocument = new MethodicDocumentDTO() { ID = id }, FileAsBase64 = "someName" });

            //Assert
            _blobStorage.Verify(bs => bs.UploadBlobForBase64Async(
                It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.AreEqual(id, res);
        }

        [Test]
        public async Task GetDecisionOrganizationAsyncWithRightParameterTest()
        {
            //Arrange
            GoverningBodyDTO organization = GetTestOrganizationDtoList()[0];
            _repository.Setup(rep => rep.GoverningBody.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                It.IsAny<Func<IQueryable<Organization>, IIncludableQueryable<Organization, object>>>()))
                .ReturnsAsync(new Organization() { OrganizationName = organization.GoverningBodyName });
            _mapper
                .Setup(x => x.Map<GoverningBodyDTO>(It.IsAny<string>()))
                .Returns(new GoverningBodyDTO() { GoverningBodyName = organization.GoverningBodyName });
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
                .Setup(x => x.Map<IEnumerable<GoverningBodyDTO>>(new List<Organization>()))
                .Returns(GetTestOrganizationDtoList());

            //Act
            var actualReturn = await _service.GetGoverningBodyListAsync();

            //Assert
            Assert.IsInstanceOf<List<GoverningBodyDTO>>(actualReturn);
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
         private static List<GoverningBodyDTO> GetTestOrganizationDtoList()
        {
            return new List<GoverningBodyDTO>
            {
                new GoverningBodyDTO {Id = 1, GoverningBodyName = "Organization1"},
                new GoverningBodyDTO {Id = 2, GoverningBodyName = "Organization2"},
            };
        }
    }
}
