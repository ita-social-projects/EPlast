using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Identity;
using EPlast.BLL.Interfaces.AboutBase;
using EPlast.DataAccess.Entities;
using EPlast.BLL.DTO.AboutBase;
using EPlast.WebApi.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Http;
using EPlast.Resources;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;

namespace EPlast.Tests.Controllers
{
    [TestFixture]
    internal class AboutBaseControllerTests
    {
        private Mock<IAboutBaseSectionService> _sectionService;
        private Mock<IAboutBaseSubsectionService> _subsectionSercive;
        private Mock<UserManager<User>> _userManager;
        private Mock<IPicturesManager> _picturesManager;

        private AboutBaseController _aboutbaseController;

        [SetUp]
        public void SetUp()
        {
            _sectionService = new Mock<IAboutBaseSectionService>();
            _subsectionSercive = new Mock<IAboutBaseSubsectionService>();
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _picturesManager = new Mock<IPicturesManager>();

            _aboutbaseController = new AboutBaseController(
                _sectionService.Object,
                _subsectionSercive.Object,
                _userManager.Object,
                _picturesManager.Object);
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(u => u.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _aboutbaseController.ControllerContext = context;

        }
        [Test]
        public async Task GetAboutBaseSection_ById_returnsOkObjectResult()
        {
            //Arrange
            _sectionService
                .Setup(x => x.GetSection(It.IsAny<int>()))
                .ReturnsAsync(new SectionDTO());
            //Act
            var result = await _aboutbaseController.GetAboutBaseSection(It.IsAny<int>());
            var resultValue = (result as ObjectResult).Value;
            //Assert
            _sectionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<SectionDTO>(resultValue);
        }
        [Test]
        public async Task GetAboutBaseSection_ById_ReturnNotFoundResult()
        {
            //Arrange
            _sectionService
                .Setup(x => x.GetSection(It.IsAny<int>()))
                .ReturnsAsync((SectionDTO)null);
            //Act
            var result = await _aboutbaseController.GetAboutBaseSection(It.IsAny<int>());
            var resultValue = (result as ObjectResult)?.Value;
            //Assert
            _sectionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsNull(resultValue);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public async Task GetAboutBaseSections_ReturnsOkObjectResult()
        {
            //Arrange
            _sectionService
                .Setup(x => x.GetAllSectionAsync())
                .ReturnsAsync((new List<SectionDTO>()).AsEnumerable());
            //Act
            var result = await _aboutbaseController.GetAboutBaseSections();
            var resultValue = (result as OkObjectResult).Value;
            //Assert
            _sectionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<List<SectionDTO>>(resultValue);
        }
        
        [Test]
        public async Task GetAboutBaseSubsection_ById_ReturnsOkOdjectResult()
        {
            //Arrange
            _subsectionSercive
                .Setup(x => x.GetSubsection(It.IsAny<int>()))
                .ReturnsAsync(new SubsectionDTO());
            //Act
            var result = await _aboutbaseController.GetAboutBaseSubsection(It.IsAny<int>());
            var resultValue = (result as OkObjectResult).Value;
            //Assert
            _subsectionSercive.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<SubsectionDTO>(resultValue);
        }
        
        [Test]
        public async Task GetAboutBaseSubsection_ById_ReturnsNotFoundResult()
        {
            //Arrange
            _subsectionSercive
                .Setup(x => x.GetSubsection(It.IsAny<int>()))
                .ReturnsAsync((SubsectionDTO)null);
            //Act
            var result = await _aboutbaseController.GetAboutBaseSubsection(It.IsAny<int>());
            var resultValue = (result as ObjectResult)?.Value;
            //Assert
            _subsectionSercive.Verify();
            Assert.IsNotNull(result);
            Assert.IsNull(resultValue);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        
        [Test]
        public async Task GetAboutBaseSubsections_ReturnsOkObjectResult()
        {
            //Arrange
            _subsectionSercive
                .Setup(x => x.GetAllSubsectionAsync())
                .ReturnsAsync(new List <SubsectionDTO>().AsEnumerable());
            //Act
            var result = await _aboutbaseController.GetAboutBaseSubsections();
            var resultValue = (result as OkObjectResult).Value;
            //Assert
            _subsectionSercive.Verify();
            Assert.IsNotNull(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<List<SubsectionDTO>>(resultValue);
        }

        [Test]
        public async Task FillEventGallery_ReturnsOkObjectResult()
        {
            // Arrange
            const int expectedCount = 2;
            _picturesManager
                .Setup((x) => x.FillSubsectionPicturesAsync(It.IsAny<int>(), It.IsAny<IList<IFormFile>>()))
                .ReturnsAsync(CreateListOfFakeSubsectionPictures());

            // Act
            var result = await _aboutbaseController.FillSubsectionPictures(It.IsAny<int>(), It.IsAny<IList<IFormFile>>());
            var resultObject = (result as ObjectResult).Value as IList<SubsectionPicturesDTO>;


            // Assert
            Assert.NotNull(result);
            Assert.NotNull(resultObject);
            Assert.AreEqual(expectedCount, resultObject.Count);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task FillEventGallery_ListOfTwoItems_ReturnsListOfTwoItems()
        {
            // Arrange
            var expectedCount = 2;

            _picturesManager
                .Setup((x) => x.FillSubsectionPicturesAsync(It.IsAny<int>(), It.IsAny<IList<IFormFile>>()))
                .ReturnsAsync(CreateListOfFakeSubsectionPictures());

            // Act
            var result = await _aboutbaseController.FillSubsectionPictures(It.IsAny<int>(), It.IsAny<IList<IFormFile>>());

            var actual = ((result as ObjectResult).Value as List<SubsectionPicturesDTO>).Count;

            // Assert
            Assert.AreEqual(expectedCount, actual);
        }

        [Test]
        public async Task GetPictures_ReturnsOkObjectResult_GetTwoPicture()
        {
            // Arrange
            _picturesManager
                .Setup((x) => x.GetPicturesAsync(It.IsAny<int>()))
                .ReturnsAsync(CreateListOfFakeSubsectionPictures());
            const int countPicture = 2;

            // Act
            var result = await _aboutbaseController.GetPictures(It.IsAny<int>());
            var okResult = result as ObjectResult;
            var pictures = okResult.Value as IEnumerable<SubsectionPicturesDTO>;
            var picturesAsList = pictures as IList<SubsectionPicturesDTO>;



            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(okResult);
            Assert.NotNull(pictures);
            Assert.NotNull(picturesAsList);
            Assert.AreEqual(countPicture, picturesAsList.Count);
        }

        [Test]
        public async Task GetPictures_ListOfTwoItems_ReturnsListOfTwoItems()
        {
            // Arrange
            const int expectedCount = 2;

            _picturesManager
                .Setup((x) => x.GetPicturesAsync(It.IsAny<int>()))
                .ReturnsAsync(CreateListOfFakeSubsectionPictures());

            // Act
            var result = await _aboutbaseController.GetPictures(It.IsAny<int>());

            var actual = ((result as ObjectResult).Value as List<SubsectionPicturesDTO>).Count;

            // Assert
            Assert.AreEqual(expectedCount, actual);
        }

        [Test]
        public async Task DeleteAboutBaseSubsection_ReturnsNoContentResult()
        {
            //Arrange
            _subsectionSercive
                .Setup(x=>x.DeleteSubsection(It.IsAny<int>(),It.IsAny<User>()));
            //Act
            var result = await _aboutbaseController.DeleteAboutBaseSubsection(It.IsAny<int>());
            //Assert
            _subsectionSercive.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteAboutBaseSubsection_ReturnsNotFound()
        {
            //Arrange
            _subsectionSercive
                .Setup(x => x.DeleteSubsection(It.IsAny<int>(), It.IsAny<User>())).ThrowsAsync(new NullReferenceException("Not found"));
            //Act
            var result = await _aboutbaseController.DeleteAboutBaseSubsection(It.IsAny<int>());
            //Assert
            _subsectionSercive.Verify();
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task DeleteAboutBaseSection_ReturnsNoContentResult()
        {
            //Arrange
            
            _sectionService
                .Setup(x => x.DeleteSection(It.IsAny<int>(), It.IsAny<User>()));
            //Act
            var result = await _aboutbaseController.DeleteAboutBaseSection(It.IsAny<int>());
            //Assert
            _sectionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeletePicture_Status200OK_ReturnsStatus200OK()
        {
            // Arrange
            _picturesManager
                .Setup((x) => x.DeletePictureAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status200OK);

            var expected = StatusCodes.Status200OK;

            // Act
            var result = await _aboutbaseController.DeletePicture(It.IsAny<int>());

            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task DeletePicture_Status400BadRequest_ReturnsStatus400BadRequest()
        {
            // Arrange
            _picturesManager
                .Setup((x) => x.DeletePictureAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status400BadRequest);

            var expected = StatusCodes.Status400BadRequest;

            // Act
            var result = await _aboutbaseController.DeletePicture(It.IsAny<int>());

            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task AddAboutBaseSection_ReturnsNoContentResult()
        {
            //Arrange
            
            _sectionService
                .Setup(x => x.AddSection(It.IsAny<SectionDTO>(), It.IsAny<User>()));
            
            //Act
            var result = await _aboutbaseController.AddAboutBaseSection(It.IsAny<SectionDTO>());
            
            //Assert
            _sectionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }
        
        [Test]
        public async Task AddAboutBaseSection_ReturnsBadRequestResult()
        {
            //Arrange
            _aboutbaseController.ModelState.AddModelError("Title", "title field is required");
            _sectionService
                .Setup(x => x.AddSection(It.IsAny<SectionDTO>(), It.IsAny<User>()));
            
            //Act
            var result = await _aboutbaseController.AddAboutBaseSection(It.IsAny<SectionDTO>());
            
            //Assert
            _sectionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
        
        [Test]
        public async Task AddAboutBaseSubsection_ReturnsNoContentResult()
        {
            //Arrange
            _subsectionSercive
                .Setup(x=>x.AddSubsection(It.IsAny<SubsectionDTO>(),It.IsAny<User>()));
            
            //Act
            var result = await _aboutbaseController.AddAboutBaseSubsection(It.IsAny<SubsectionDTO>());
            
            //Assert
            _subsectionSercive.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }
        
        [Test]
        public async Task AddAboutBaseSubsection_ReturnsBadRequestResult()
        {
            //Arrange
            _aboutbaseController.ModelState.AddModelError("Title", "Title field is required");
            _subsectionSercive
                .Setup(x => x.AddSubsection(It.IsAny<SubsectionDTO>(), It.IsAny<User>()));
            
            //Act
            var result = await _aboutbaseController.AddAboutBaseSubsection(It.IsAny<SubsectionDTO>());
            
            //Assert
            _subsectionSercive.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
        
        [Test]
        public async Task EditAboutBaseSection_ReturnsNoContentResult()
        {
            //Arrange
            _sectionService
                .Setup(x => x.ChangeSection(It.IsAny<SectionDTO>(), It.IsAny<User>()));
            
            //Act
            var result = await _aboutbaseController.EditAboutBaseSection(It.IsAny<SectionDTO>());
            
            //Assert
            _sectionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }
        
        [Test]
        public async Task EditAboutBaseSection_ReturnsBadRequestResult()
        {
            //Arrange
            _aboutbaseController.ModelState.AddModelError("Title", "Title field is required");
            _sectionService
                .Setup(x => x.ChangeSection(It.IsAny<SectionDTO>(), It.IsAny<User>()));
            
            //Act
            var result = await _aboutbaseController.EditAboutBaseSection(It.IsAny<SectionDTO>());
            
            //Assert
            _sectionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task EditAboutBaseSubsection_ReturnsNoContentResult()
        {
            //Arrange
            _subsectionSercive
                .Setup(x => x.ChangeSubsection(It.IsAny<SubsectionDTO>(), It.IsAny<User>()));
            
            //Act
            var result = await _aboutbaseController.EditAboutBaseSubsection(It.IsAny<SubsectionDTO>());
            
            //Assert
            _subsectionSercive.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }
      
        [Test]
        public async Task EditAboutBaseSubsection_ReturnsNotFoundResult()
        {
            //Arrange
            _subsectionSercive
                .Setup(x => x.ChangeSubsection(It.IsAny<SubsectionDTO>(), It.IsAny<User>())).ThrowsAsync(new NullReferenceException("Not found"));
            //Act
            var result = await _aboutbaseController.EditAboutBaseSubsection(It.IsAny<SubsectionDTO>());
            //Assert
            _subsectionSercive.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task EditAboutBaseSubsection_ReturnsBadRequestResult()
        {
            //Arrange
            _aboutbaseController.ModelState.AddModelError("Title", "Title field is required");
            _subsectionSercive
                .Setup(x => x.ChangeSubsection(It.IsAny<SubsectionDTO>(), It.IsAny<User>()));
            
            //Act
            var result = await _aboutbaseController.EditAboutBaseSubsection(It.IsAny<SubsectionDTO>());
            
            //Assert
            _subsectionSercive.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
        
        [Test]
        public async Task DeleteAboutBaseSection_ThrowsNullReferenceException_ReturnsNotFound()
        {
            //Arrange
            _sectionService
                .Setup(x => x.DeleteSection(It.IsAny<int>(), It.IsAny<User>()))
                .Throws( new NullReferenceException());

            //Act 
            var result = await _aboutbaseController.DeleteAboutBaseSection(0);

            //Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task DeleteAboutBaseSubsection_ThrowsNullReferenceException_ReturnsNotFound()
        {
            //Arrange
            _subsectionSercive
                .Setup(x => x.DeleteSubsection(It.IsAny<int>(), It.IsAny<User>()))
                .Throws(new NullReferenceException());

            //Act 
            var result = await _aboutbaseController.DeleteAboutBaseSubsection(0);

            //Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task AddAboutBaseSection_ThrowsException_ReturnsNotFound()
        {
            //Arrange
            _sectionService
                .Setup(x => x.AddSection(It.IsAny<SectionDTO>(), It.IsAny<User>()))
                .Throws(new Exception());

            //Act 
            var result = await _aboutbaseController.AddAboutBaseSection(new SectionDTO());

            //Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task AddAboutBaseSubsection_ThrowsException_ReturnsNotFound()
        {
            //Arrange
            _subsectionSercive
                .Setup(x => x.AddSubsection(It.IsAny<SubsectionDTO>(), It.IsAny<User>()))
               .Throws(new Exception());

            //Act 
            var result = await _aboutbaseController.AddAboutBaseSubsection(new SubsectionDTO());

            //Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task EditAboutBaseSection_ThrowsNullReferenceException_ReturnsNotFound()
        {
            //Arrange
            _sectionService
                .Setup(x => x.ChangeSection(It.IsAny<SectionDTO>(), It.IsAny<User>()))
                .Throws(new NullReferenceException());

            //Act
            var result = await _aboutbaseController.EditAboutBaseSection(new SectionDTO());

            //Assert   
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        
        [Test]
        public async Task EditAboutBaseSubsection_ThrowsNullReferenceException_ReturnsNotFound()
        {
            //Arrange
            _subsectionSercive
                .Setup(x => x.ChangeSubsection(It.IsAny<SubsectionDTO>(), It.IsAny<User>()))
                .Throws(new NullReferenceException());

            //Act
            var result = await _aboutbaseController.EditAboutBaseSubsection(new SubsectionDTO());

            //Assert   
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        private List<SubsectionPicturesDTO> CreateListOfFakeSubsectionPictures()
            => new List<SubsectionPicturesDTO>()
            {
                new SubsectionPicturesDTO()
                {
                    PictureId = 1,
                    FileName = "SomeFilenameID1"
                },
                new SubsectionPicturesDTO()
                {
                    PictureId = 2,
                    FileName = "SomeFilenameID2"
                },

            };
    }
}
