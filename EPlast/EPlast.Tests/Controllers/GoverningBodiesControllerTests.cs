﻿using EPlast.BLL.DTO;
using EPlast.BLL.Interfaces.GoverningBodies;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces.Logging;

namespace EPlast.Tests.Controllers
{
    class GoverningBodiesControllerTests
    {
        private Mock<IGoverningBodiesService> _governingBodiesService;
        private Mock<ILoggerService<GoverningBodiesController>> _logger;
        private GoverningBodiesController _controller;

        [SetUp]
        public void SetUp()
        {
            _governingBodiesService = new Mock<IGoverningBodiesService>();
            _logger = new Mock<ILoggerService<GoverningBodiesController>>();
            _controller = new GoverningBodiesController(
                _governingBodiesService.Object,
                _logger.Object);
        }

        [Test]
        public async Task getOrganizations_ReturnsOrganizationsList()
        {
            //Arrange
            _governingBodiesService
                .Setup(x=>x.GetGoverningBodiesListAsync()).ReturnsAsync(new List<GoverningBodyDTO>());
            //Act
            var result = await _controller.GetGoverningBodies();
            var resultValue = (result as ObjectResult)?.Value;
            //Assert

            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<IEnumerable<GoverningBodyDTO>>(resultValue);
        }

        [Test]
        public async Task Create_ModelStateNotValid_Test()
        {
            // Arrange
            var testDTO = CreateGoverningBodyDTO;
            _controller.ModelState.AddModelError("NameError", "Required");

            // Act
            var result = await _controller.Create(testDTO);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [TestCase(2)]
        public async Task Create_Valid_Test(int serviceReturnedId)
        {
            // Arrange
            var testDTO = CreateGoverningBodyDTO;
            _governingBodiesService.Setup(x => x.CreateAsync(It.IsAny<GoverningBodyDTO>())).ReturnsAsync(serviceReturnedId);

            // Act
            var result = await _controller.Create(testDTO);
            var resultObject = result as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(resultObject?.Value , serviceReturnedId);
        }

        [TestCase("logopath", "logo64path")]
        public async Task GetPhotoBase64_Test(string logopath, string logo64Path)
        {
            //Arrange
            _governingBodiesService.Setup(x => x.GetLogoBase64(It.IsAny<string>())).ReturnsAsync(logo64Path);

            //Act
            var result = await _controller.GetPhotoBase64(logopath);
            var resultObject = result as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(resultObject?.Value, logo64Path);
        }

        private GoverningBodyDTO CreateGoverningBodyDTO => new GoverningBodyDTO()
        {
            ID = 1,
            GoverningBodyName = "gbName",
            Description = "gbDesc",
            Email = "gbEmail",
            Logo = null,
            PhoneNumber = "12345"
        };
    }
}
