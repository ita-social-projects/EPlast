using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EPlast.BLL;
using EPlast.BLL.DTO.Blank;
using EPlast.BLL.Interfaces.Blank;
using EPlast.BLL.Interfaces.Logging;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Controllers
{
    [TestFixture]
    class BlanksControllerTests
    {
        Mock<IBlankBiographyDocumentService> _mockBiographyService;
        Mock<IBlankAchievementDocumentService> _mockBlankAchievementDocumentService;
        Mock<IBlankExtractFromUpuDocumentService> _mockBlankExtractFromUPUDocumentService;
        Mock<IPdfService> _pdfService;
        Mock<ILoggerService<BlanksController>> _mockLoggerService;
        private Mock<UserManager<User>> _mockUserManager;
        BlanksController _blanksController;


        [SetUp]
        public void SetUp()
        {
            _mockBiographyService = new Mock<IBlankBiographyDocumentService>();
            _mockBlankAchievementDocumentService = new Mock<IBlankAchievementDocumentService>();
            _mockBlankExtractFromUPUDocumentService = new Mock<IBlankExtractFromUpuDocumentService>();
            _pdfService = new Mock<IPdfService>();
            _mockLoggerService = new Mock<ILoggerService<BlanksController>>();
            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _blanksController = new BlanksController(_mockBiographyService.Object,
                _mockBlankAchievementDocumentService.Object,
                _mockBlankExtractFromUPUDocumentService.Object, _mockLoggerService.Object,
                _pdfService.Object, _mockUserManager.Object);
        }

        [Test]
        public async Task AddBiographyDocument_ReturnsCreatedObjectResult()
        {
            //Arrange
            _mockBiographyService
                .Setup(x => x.AddDocumentAsync(It.IsAny<BlankBiographyDocumentsDto>()))
                .ReturnsAsync(GetBlankBiographyDocumentDTO());

            //Act
            var document = await _blanksController.AddBiographyDocument(GetBlankBiographyDocumentDTO());
            CreatedResult createdResult = document as CreatedResult;

            //Assert
            Assert.NotNull(document);
            Assert.NotNull(createdResult.Value);
            Assert.IsInstanceOf<ObjectResult>(document);
            Assert.AreEqual(StatusCodes.Status201Created, createdResult.StatusCode);
        }

        [Test]
        public async Task AddAchievementDocument_ReturnsCreatedObjectResult()
        {
            //Arrange
            _mockBlankAchievementDocumentService
                .Setup(x => x.AddDocumentAsync(It.IsAny<List<AchievementDocumentsDto>>()))
                .ReturnsAsync(new List<AchievementDocumentsDto>());

            //Act
            var document = await _blanksController.AddAchievementDocument(new List<AchievementDocumentsDto>());
            CreatedResult createdResult = document as CreatedResult;

            //Assert
            _mockBlankAchievementDocumentService.Verify();
            Assert.NotNull(document);
            Assert.NotNull(createdResult.Value);
            Assert.NotNull(createdResult);
            Assert.IsInstanceOf<ObjectResult>(document);
            Assert.AreEqual(StatusCodes.Status201Created, createdResult.StatusCode);
        }

        [Test]
        public async Task AddExtractFromUPUDocument_ReturnsCreatedObjectResult()
        {
            //Arrange
            _mockBlankExtractFromUPUDocumentService
                .Setup(x => x.AddDocumentAsync(It.IsAny<ExtractFromUpuDocumentsDto>()))
                .ReturnsAsync(new ExtractFromUpuDocumentsDto());

            //Act
            var document = await _blanksController.AddExtractFromUPUDocument(new ExtractFromUpuDocumentsDto());
            CreatedResult createdResult = document as CreatedResult;

            //Assert
            _mockBlankExtractFromUPUDocumentService.Verify();
            Assert.NotNull(document);
            Assert.NotNull(createdResult.Value);
            Assert.IsInstanceOf<ObjectResult>(document);
            Assert.AreEqual(StatusCodes.Status201Created, createdResult.StatusCode);
        }

        [Test]
        public async Task GetDocumentByUserId_ReturnsOkObjectResult()
        {
            //Arrange
            string userId = "gh34tg";
            _mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User() { Id = userId });
            _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Admin});
            _mockBiographyService
                .Setup(x => x.GetDocumentByUserId(userId))
                .ReturnsAsync(GetBlankBiographyDocumentDTO());

            //Act
            var document = await _blanksController.GetDocumentByUserId("1");
            OkObjectResult result = document as OkObjectResult;

            //Assert
            _mockBiographyService.Verify();
            Assert.NotNull(document);
            Assert.IsInstanceOf<ObjectResult>(document);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        }

        [Test]
        public async Task GetDocumentByUserId_Returns403Forbidden()
        {
            //Arrange
            string userId = "gh34tg";
            _mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User() { Id = "" });
            _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>());
            _mockBiographyService
                .Setup(x => x.GetDocumentByUserId(userId))
                .ReturnsAsync(GetBlankBiographyDocumentDTO());

            var expected = StatusCodes.Status403Forbidden;

            // Act
            var result = await _blanksController.GetDocumentByUserId(userId);
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            _mockLoggerService.Verify((x) => x.LogError(It.IsAny<string>()), Times.Once);
            _mockUserManager.Verify();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetExtractFromUPUByUserId_ReturnsOkObjectResult()
        {
            //Arrange
            string userId = "gh34tg";
            _mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User() { Id = userId });
            _mockBlankExtractFromUPUDocumentService
                .Setup(x => x.GetDocumentByUserId(userId))
                .ReturnsAsync(GetExtractFromUPUDocumentsDTO());

            //Act
            var document = await _blanksController.GetExtractFromUPUByUserId(userId);
            OkObjectResult result = document as OkObjectResult;

            //Assert
            _mockBlankExtractFromUPUDocumentService.Verify();
            Assert.NotNull(result.Value);
            Assert.IsInstanceOf<ObjectResult>(document);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        }

        [Test]
        public async Task GetExtractFromUPUByUserId_Returns403Forbidden()
        {
            //Arrange
            string userId = "gh34tg";
            _mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User() { Id = "" });
            _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>());
            _mockBlankExtractFromUPUDocumentService
                .Setup(x => x.GetDocumentByUserId(userId))
                .ReturnsAsync(GetExtractFromUPUDocumentsDTO());

            var expected = StatusCodes.Status403Forbidden;

            //Act
            var result = await _blanksController.GetExtractFromUPUByUserId(userId);
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            _mockLoggerService.Verify((x) => x.LogError(It.IsAny<string>()), Times.Once);
            _mockUserManager.Verify();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task RemoveExtractFromUPUDocument_ReturnsNoContent()
        {
            //Arrange
            int documentId = 1;
            _mockBlankExtractFromUPUDocumentService
                .Setup(x => x.DeleteFileAsync(documentId));

            //Act
            var document = await _blanksController.RemoveExtractFromUPUDocument(documentId);
            var statusCodeDocument = document as StatusCodeResult;

            //Assert
            _mockBlankExtractFromUPUDocumentService.Verify();
            Assert.NotNull(document);
            Assert.AreEqual(StatusCodes.Status204NoContent, statusCodeDocument.StatusCode);
        }

        [Test]
        public async Task RemoveDocument_ReturnsNoContent()
        {
            //Arrange
            int documentId = 1;
            _mockBiographyService
                .Setup(x => x.DeleteFileAsync(documentId));

            //Act
            var document = await _blanksController.RemoveDocument(documentId);
            var statusCodeDocument = document as StatusCodeResult;

            //Assert
            _mockBiographyService.Verify();
            Assert.NotNull(document);
            Assert.AreEqual(StatusCodes.Status204NoContent, statusCodeDocument.StatusCode);
        }

        [Test]
        public async Task GetFileBase64_ReturnsOkObjectResult()
        {
            // Arrange
            _mockBiographyService
               .Setup(x => x.DownloadFileAsync(It.IsAny<string>()))
               .ReturnsAsync(GetExtractFromUPUDocumentsDTO().FileName);

            //Act
            var document = await _blanksController.GetFileBase64(GetExtractFromUPUDocumentsDTO().FileName);
            OkObjectResult result = document as OkObjectResult;

            //Assert
            _mockBiographyService.Verify();
            Assert.NotNull(document);
            Assert.IsInstanceOf<ObjectResult>(document);
            Assert.AreEqual(GetExtractFromUPUDocumentsDTO().FileName, result.Value);
        }

        [Test]
        public async Task GetFileExtractFromUPUBase64_ReturnsOkObjectResult()
        {
            // Arrange
            _mockBlankExtractFromUPUDocumentService
               .Setup(x => x.DownloadFileAsync(new string("Dogovir")))
               .ReturnsAsync(new string("Dogovir"));

            //Act
            var document = await _blanksController.GetFileExtractFromUPUBase64("Dogovir");
            OkObjectResult result = document as OkObjectResult;

            //Assert
            _mockBlankExtractFromUPUDocumentService.Verify();
            Assert.NotNull(document);
            Assert.IsInstanceOf<ObjectResult>(document);
            Assert.AreEqual("Dogovir", result.Value);
        }

        [TestCase(1, 1, "userId", 0)]
        [TestCase(1, 1, "userId", 1)]
        public async Task GetPartOfAchievementByUserId_ReturnsOkObjectResult(int pageNumber, int pageSize, string userId, int courseId)
        {
            //Arrange
            _mockBlankAchievementDocumentService
               .Setup(x => x.GetPartOfAchievementByUserIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
               .ReturnsAsync(new List<AchievementDocumentsDto>());
            _mockBlankAchievementDocumentService
               .Setup(x => x.GetPartOfAchievementByUserIdAndCourseIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
               .ReturnsAsync(new List<AchievementDocumentsDto>());

            //Act
            var result = await _blanksController.GetPartOfAchievement(pageNumber, pageSize, userId, courseId);
            OkObjectResult okObjectResult = result as OkObjectResult;

            //Assert
            _mockBlankAchievementDocumentService.Verify();
            Assert.NotNull(result);
            Assert.NotNull(okObjectResult.Value);
            Assert.AreEqual("List`1",okObjectResult.Value.GetType().Name);
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [TestCase("userId")]
        public async Task GetAchievementDocumentsByUserId_ReturnsOkObjectResult(string userId)
        {
            //Arrange
            _mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User() {Id = userId});
            _mockBlankAchievementDocumentService.Setup(x => x.GetDocumentsByUserIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<AchievementDocumentsDto>());

            //Act
            var result = await _blanksController.GetAchievementDocumentsByUserId(userId);
            OkObjectResult okObjectResult = result as OkObjectResult;

            //Assert
            _mockBlankAchievementDocumentService.Verify();
            Assert.NotNull(result);
            Assert.NotNull(okObjectResult.Value);
            Assert.AreEqual("List`1", okObjectResult.Value.GetType().Name);
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [TestCase("userId")]
        public async Task GetAchievementDocumentsByUserId_Returns403Forbidden(string userId)
        {
            //Arrange
            _mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User() { Id = "" });
            _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>());
            _mockBlankAchievementDocumentService.Setup(x => x.GetDocumentsByUserIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<AchievementDocumentsDto>());

            var expected = StatusCodes.Status403Forbidden;

            // Act
            var result = await _blanksController.GetAchievementDocumentsByUserId(userId);
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            _mockLoggerService.Verify((x) => x.LogError(It.IsAny<string>()), Times.Once);
            _mockUserManager.Verify();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task RemoveAchievementDocument_ReturnsNoContent()
        {
            //Arrange
            int documentId = 1;

            _mockBlankAchievementDocumentService
                .Setup(x => x.DeleteFileAsync(documentId, It.IsAny<string>()));

            //Act
            var document = await _blanksController.RemoveAchievementDocument(documentId,  It.IsAny<string>());
            var statusCodeDocument = document as StatusCodeResult;

            //Assert
            _mockBlankAchievementDocumentService.Verify();
            Assert.NotNull(document);
            Assert.AreEqual(StatusCodes.Status204NoContent, statusCodeDocument.StatusCode);
        }

        [Test]
        public async Task GetFileAchievementBase64_ReturnsOkObjectResult()
        {
            // Arrange
            _mockBlankAchievementDocumentService
               .Setup(x => x.DownloadFileAsync(It.IsAny<string>()))
               .ReturnsAsync(GetExtractFromUPUDocumentsDTO().FileName);

            //Act
            var document = await _blanksController.GetFileAchievementBase64(GetExtractFromUPUDocumentsDTO().FileName);
            OkObjectResult result = document as OkObjectResult;

            //Assert
            _mockBlankAchievementDocumentService.Verify();
            Assert.NotNull(document);
            Assert.IsInstanceOf<ObjectResult>(document);
            Assert.AreEqual(GetExtractFromUPUDocumentsDTO().FileName, result.Value);
        }

        private BlankBiographyDocumentsDto GetBlankBiographyDocumentDTO()
        {
            return new BlankBiographyDocumentsDto
            {
                ID = 1,
                FileName = "Dogovir",
                BlobName = "BlobName",
                UserId = "gh34tg"
            };
        }

        public ExtractFromUpuDocumentsDto GetExtractFromUPUDocumentsDTO()
        {
            return new ExtractFromUpuDocumentsDto
            {
                ID = 1,
                FileName = "Dogovir",
                BlobName = "BlobName",
                UserId = "gh34tg"

            };
        }

        [Test]
        public async Task GetPdfService_OkReturnsObjRes()
        {
            //Arrange
            _pdfService
                .Setup(p => p.BlankCreatePDFAsync(It.IsAny<string>()));

            //Act
            var result = await _blanksController.GetGenerationFile(It.IsAny<string>());

            //Assert
            _pdfService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
    }
}
