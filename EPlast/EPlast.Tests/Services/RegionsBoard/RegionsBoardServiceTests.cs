using EPlast.BLL.Interfaces;
using EPlast.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.Tests.Services.RegionsBoard
{
    public class RegionsBoardServiceTests
    {
        private  Mock<ISecurityModel> _securityModel;
        private RegionsBoardService _regionsBoardService;

        [SetUp]
        public void SetUp()
        {
            _securityModel = new Mock<ISecurityModel>();
            _regionsBoardService = new RegionsBoardService(_securityModel.Object);
        }

        [Test]
        public async Task getOrganizations_ReturnsOrganizationsList()
        {
            //Arrange
            Dictionary<string, bool> dict = new Dictionary<string, bool>();
            dict.Add("action", It.IsAny<bool>());
            _securityModel.Setup(x => x.GetUserAccess(It.IsAny<string>(), It.IsAny<IEnumerable<string>>())).Returns(dict);

            //Act
            var result = _regionsBoardService.GetUserAccess(It.IsAny<string>());

            //Assert
            Assert.IsNotEmpty(result);
            Assert.IsInstanceOf<Dictionary<string, bool>>(result);
        }
    }
}
