using EPlast.BLL.DTO;
using EPlast.BLL.Interfaces.GoverningBodies;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;

namespace EPlast.Tests.Controllers
{
    class GoverningBodiesControllerTests
    {
        private Mock<IGoverningBodiesService> _governingBodiesService;
        private GoverningBodiesController _controller;

        [SetUp]
        public void SetUp()
        {
            _governingBodiesService = new Mock<IGoverningBodiesService>();
            _controller = new GoverningBodiesController(
                _governingBodiesService.Object);
        }

        [Test]
        public async Task getOrganizations_ReturnsOrganizationsList()
        {
            //Arrange
            List<OrganizationDTO> list = new List<OrganizationDTO>();
            list.Add(new Mock<OrganizationDTO>().Object);
            _governingBodiesService
                .Setup(x=>x.GetOrganizationListAsync()).ReturnsAsync(list);
            
            //Act
            var result = await _controller.GetOrganizations();
            var resultValue = (result as ObjectResult).Value;
            //Assert

            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotEmpty(resultValue as List<OrganizationDTO>);
            Assert.IsInstanceOf<IEnumerable<OrganizationDTO>>(resultValue);
        }
    }
}
