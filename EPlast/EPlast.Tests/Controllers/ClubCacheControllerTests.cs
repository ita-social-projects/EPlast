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
using EPlast.WebApi.Models.Club;
using System.Linq;
using EPlast.DataAccess.Entities;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.Club;

namespace EPlast.Tests.Controllers
{
    class ClubCacheControllerTests
    {
        private Mock<IClubService> _clubService;
        private Mock<IDistributedCache> _cache;

        private ClubCacheController _clubCacheController;

        [SetUp]
        public void SetUp()
        {
            _clubService = new Mock<IClubService>();
            _cache = new Mock<IDistributedCache>();
            var store = new Mock<Microsoft.AspNetCore.Identity.IUserStore<User>>();
            _clubCacheController = new ClubCacheController(
                _clubService.Object,
                _cache.Object);
        }

        [TestCase(1, 1, "Club")]
        public async Task GetCities_ReturnsOk(int page, int pageSize, string clubName)
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
            _clubCacheController.ControllerContext = context;
            _clubService
                .Setup(c => c.GetAllDTOAsync(It.IsAny<string>()))
                .ReturnsAsync(GetClubBySearch());
            byte[] bytes = Encoding.ASCII.GetBytes("[]");
            _cache.Setup(x => x.GetAsync(It.IsAny<string>(), default)).ReturnsAsync(bytes);

            // Act
            var result = await _clubCacheController.GetClubs(page, pageSize, clubName);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(((result as ObjectResult).Value as ClubsViewModel)
                .Clubs.Where(c => c.Name.Equals("Club")));
        }

        [TestCase(1, 1, "Club")]
        public async Task GetCities_ReturnsNull(int page, int pageSize, string clubName)
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
            _clubCacheController.ControllerContext = context;
            _clubService
                .Setup(c => c.GetAllDTOAsync(It.IsAny<string>()))
                .ReturnsAsync(GetClubBySearch());
            _cache.Setup(x => x.GetAsync(It.IsAny<string>(), default)).ReturnsAsync((byte[])null);

            // Act
            var result = await _clubCacheController.GetClubs(page, pageSize, clubName);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(((result as ObjectResult).Value as ClubsViewModel)
                .Clubs.Where(c => c.Name.Equals("Club")));
        }


        private List<ClubDTO> GetClubBySearch()
        {
            return new List<ClubDTO>()
            {
                new ClubDTO()
                {
                    Name = "Club",
                }
            };
        }
    }
}
