using EPlast.BLL.Interfaces.City;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using EPlast.WebApi.Controllers;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using EPlast.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using EPlast.WebApi.Models.City;
using System.Linq;
using EPlast.DataAccess.Entities;
using EPlast.BLL.DTO.City;

namespace EPlast.Tests.Controllers
{
    class CityCacheControllerTests
    {
        private Mock<ICityService> _cityService;
        private Mock<IDistributedCache> _cache;

        private CitiesCacheController _citiesCacheController;

        [SetUp]
        public void SetUp()
        {
            _cityService = new Mock<ICityService>();
            _cache = new Mock<IDistributedCache>();
            var store = new Mock<Microsoft.AspNetCore.Identity.IUserStore<User>>();
            _citiesCacheController = new CitiesCacheController(
                _cityService.Object, 
                _cache.Object);
        }

        [TestCase(1, 1, "City")]
        public async Task GetCities_ReturnsOk(int page, int pageSize, string cityName)
        {
            // Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _citiesCacheController.ControllerContext = context;
            _cityService
                .Setup(c => c.GetAllDTOAsync(It.IsAny<string>()))
                .ReturnsAsync(GetCitiesBySearch());
            byte[] bytes = Encoding.ASCII.GetBytes("[]");
            _cache.Setup(x => x.GetAsync(It.IsAny<string>(), default)).ReturnsAsync(bytes);

            // Act
            var result = await _citiesCacheController.GetCities(page, pageSize, cityName);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(((result as ObjectResult).Value as CitiesViewModel)
                .Cities.Where(c => c.Name.Equals("City")));
        }

        [TestCase(1, 1, "City")]
        public async Task GetCities_ReturnsNull(int page, int pageSize, string cityName)
        {
            // Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _citiesCacheController.ControllerContext = context;
            _cityService
                .Setup(c => c.GetAllDTOAsync(It.IsAny<string>()))
                .ReturnsAsync(GetCitiesBySearch());
            _cache.Setup(x => x.GetAsync(It.IsAny<string>(), default)).ReturnsAsync((byte[])null);

            // Act
            var result = await _citiesCacheController.GetCities(page, pageSize, cityName);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(((result as ObjectResult).Value as CitiesViewModel)
                .Cities.Where(c => c.Name.Equals("City")));
        }


        private List<CityDTO> GetCitiesBySearch()
        {
            return new List<CityDTO>()
            {
                new CityDTO()
                {
                    Name = "City",
                }
            };
        }
    }
}
