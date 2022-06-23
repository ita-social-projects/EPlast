using EPlast.BLL.DTO;
using EPlast.BLL.Services.Interfaces;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.Tests.Controllers
{
    [TestFixture]
    public class AreaControllerTests
    {
        private Mock<IAreaService> _areaService;

        [SetUp]
        public void SetUp()
        {
            _areaService = new Mock<IAreaService>();
        }

        [Test]
        public async Task GetAreas_Valid_ReturnOkObjectResult()
        {
            // Arrange
            AreaController controller = CreateAreaController();
            _areaService.Setup(a => a.GetAllAsync())
                .ReturnsAsync(Areas);

            // Act
            var result = await controller.GetAreas();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }       
        
        [Test]
        public async Task GetAreaById_Valid_ReturnOkObjectResult()
        {
            // Arrange
            AreaController controller = CreateAreaController();
            _areaService.Setup(a => a.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(Area);

            // Act
            var result = await controller.GetAreaById(It.IsAny<int>());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }      
        
        [Test]
        public async Task GetAreaById_InValidId_ReturnNotFoundResult()
        {
            // Arrange
            AreaController controller = CreateAreaController();
            _areaService.Setup(a => a.GetByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new ArgumentNullException());

            // Act
            var result = await controller.GetAreaById(It.IsAny<int>());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        private IEnumerable<AreaDTO> Areas => new List<AreaDTO>
        {
            new AreaDTO { Id = 1, Name = "Чернівецька" },
            new AreaDTO { Id = 2, Name = "Львівська" },
            new AreaDTO { Id = 3, Name = "Чернігівська" },
        };

        private AreaDTO Area => Areas.First();

        private AreaController CreateAreaController()
        {
            return new AreaController(_areaService.Object);
        }
    }
}
