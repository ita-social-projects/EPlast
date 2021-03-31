using System.Collections.Generic;
using System.Threading.Tasks;
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
        private RegionsBoardController _controller;

        [SetUp]
        public void SetUp()
        {
            _regionsBoardService = new Mock<IRegionsBoardService>();
            _controller = new RegionsBoardController(_regionsBoardService.Object);
        }

        [Test]
        public async Task getOrganizations_ReturnsOrganizationsList()
        {
            //Arrange
            Dictionary<string, bool> dict = new Dictionary<string, bool>();
            dict.Add("action", It.IsAny<bool>());
            _regionsBoardService.Setup(x => x.GetUserAccessAsync(It.IsAny<string>())).ReturnsAsync(dict);

            //Act
            var result = await _controller.GetUserAccess(It.IsAny<string>());
            var resultValue = (result as ObjectResult)?.Value;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotEmpty(resultValue as Dictionary<string, bool>);
            Assert.IsInstanceOf<Dictionary<string, bool>>(resultValue);
        }
    }
}
