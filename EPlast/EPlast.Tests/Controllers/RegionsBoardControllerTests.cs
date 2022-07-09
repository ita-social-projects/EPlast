using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.Interfaces.Region;
using EPlast.BLL.Interfaces.RegionBoard;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Controllers
{
    internal class RegionsBoardControllerTests
    {
        private Mock<IRegionsBoardService> _regionsBoardService;
        private Mock<IRegionService> _regionService;
        private RegionsBoardController _controller;

        [SetUp]
        public void SetUp()
        {
            _regionsBoardService = new Mock<IRegionsBoardService>();
            _regionService = new Mock<IRegionService>();
            _controller = new RegionsBoardController(_regionsBoardService.Object, _regionService.Object);
        }

        [Test]
        public async Task GetUserAccesses_ReturnsAccessList()
        {
            //Arrange
            Dictionary<string, bool> dict = new Dictionary<string, bool>();
            dict.Add("action", It.IsAny<bool>());
            _regionsBoardService
                .Setup(x => x.GetUserAccessAsync(It.IsAny<string>()))
                .ReturnsAsync(dict);

            //Act
            var result = await _controller.GetUserAccess(It.IsAny<string>());
            var resultValue = (result as ObjectResult)?.Value;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotEmpty(resultValue as Dictionary<string, bool>);
            Assert.IsInstanceOf<Dictionary<string, bool>>(resultValue);
        }

        [Test]
        public async Task GetDocuments_ReturnsDocumentsList()
        {
            //Arrange
            List<RegionDocumentDto> testDocuments = new List<RegionDocumentDto>() { new RegionDocumentDto() };
            _regionService
                .Setup(x => x.GetRegionDocsAsync(It.IsAny<int>()))
                .ReturnsAsync(testDocuments);

            //Act
            var result = await _controller.GetRegionDocs(1);
            var resultValue = (result as ObjectResult)?.Value;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<List<RegionDocumentDto>>(resultValue);
            Assert.AreEqual(testDocuments, resultValue as List<RegionDocumentDto>);
        }
    }
}
