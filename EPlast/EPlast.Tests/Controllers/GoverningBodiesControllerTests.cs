using EPlast.BLL.DTO;
using EPlast.BLL.Interfaces.GoverningBodies;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.Interfaces.Logging;

namespace EPlast.Tests.Controllers
{
    class GoverningBodiesControllerTests
    {
        private Mock<IGoverningBodiesService> _governingBodiesService;
        private Mock<ILoggerService<GoverningBodiesController>> _logger;
        private Mock<IMapper> _mapper; 
        private GoverningBodiesController _controller;

        [SetUp]
        public void SetUp()
        {
            _governingBodiesService = new Mock<IGoverningBodiesService>();
            _controller = new GoverningBodiesController(
                _governingBodiesService.Object,
                _logger.Object,
                _mapper.Object);
        }

        [Test]
        public async Task getOrganizations_ReturnsOrganizationsList()
        {
            //Arrange
            _governingBodiesService
                .Setup(x=>x.GetGoverningBodiesListAsync()).ReturnsAsync(new List<GoverningBodyDTO>());
            //Act
            var result = await _controller.GetGoverningBodies();
            var resultValue = (result as ObjectResult).Value;
            //Assert

            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<IEnumerable<GoverningBodyDTO>>(resultValue);
        }
    }
}
