using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.Interfaces.RegionAdministrations;
using EPlast.BLL.Services.RegionAdministrations;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services.RegionAdministrations
{
    public class RegionAdministrationAccessServiceTests
    {
        private IRegionAdministrationAccessService _regionAdministrationAccessService;

        [SetUp]
        public void SetUp()
        {
            _regionAdministrationAccessService = new RegionAdministrationAccessService();
        }

        [Test]
        public void CanEditRegionAdmin_EditRegionAdminWhenUserHasAccess_ReturnsTrue()
        {
            //Arrange
            Dictionary<string, bool> dict = new Dictionary<string, bool>
            {
                {"EditRegion", true},
                {"EditRegionHead", true}
            };
            RegionAdministrationDto administration = new RegionAdministrationDto
            {
                AdminType = new AdminTypeDto
                {
                    AdminTypeName = Roles.OkrugaHead
                }
            };
            var user = new User();

            //Act
            var result = _regionAdministrationAccessService.CanEditRegionAdmin(dict,administration,user);

            //Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public void CanEditRegionAdmin_UserHasNoAccessToRegion_ReturnsTrue()
        {
            //Arrange
            Dictionary<string, bool> dict = new Dictionary<string, bool>
            {
                {"EditRegion", false}
            };
            RegionAdministrationDto administration = new RegionAdministrationDto();
            var user = new User();

            //Act
            var result = _regionAdministrationAccessService.CanEditRegionAdmin(dict,administration,user);

            //Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void CanEditRegionAdmin_NotRegionAdminWhenUserHasAccessToRegion_ReturnsTrue()
        {
            //Arrange
            Dictionary<string, bool> dict = new Dictionary<string, bool>
            {
                {"EditRegion", true}
            };
            RegionAdministrationDto administration = new RegionAdministrationDto
            {
                AdminType = new AdminTypeDto
                {
                    AdminTypeName = Roles.PlastMember
                }
            };
            var user = new User();

            //Act
            var result = _regionAdministrationAccessService.CanEditRegionAdmin(dict,administration,user);

            //Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public void CanRemoveRegionAdmin_RemoveRegionAdminWhenUserHasAccess_ReturnsTrue()
        {
            //Arrange
            Dictionary<string, bool> dict = new Dictionary<string, bool>
            {
                {"EditRegion", true},
                {"RemoveRegionHead", true}
            };
            RegionAdministrationDto administration = new RegionAdministrationDto
            {
                AdminType = new AdminTypeDto
                {
                    AdminTypeName = Roles.OkrugaHead
                }
            };
            var user = new User();

            //Act
            var result = _regionAdministrationAccessService.CanRemoveRegionAdmin(dict,administration,user);

            //Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public void CanRemoveRegionAdmin_UserHasNoAccessToRegion_ReturnsTrue()
        {
            //Arrange
            Dictionary<string, bool> dict = new Dictionary<string, bool>
            {
                {"EditRegion", false}
            };
            RegionAdministrationDto administration = new RegionAdministrationDto();
            var user = new User();

            //Act
            var result = _regionAdministrationAccessService.CanRemoveRegionAdmin(dict,administration,user);

            //Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void CanRemoveRegionAdmin_NotRegionAdminWhenUserHasAccessToRegion_ReturnsTrue()
        {
            //Arrange
            Dictionary<string, bool> dict = new Dictionary<string, bool>
            {
                {"EditRegion", true}
            };
            RegionAdministrationDto administration = new RegionAdministrationDto
            {
                AdminType = new AdminTypeDto
                {
                    AdminTypeName = Roles.PlastMember
                }
            };
            var user = new User();

            //Act
            var result = _regionAdministrationAccessService.CanRemoveRegionAdmin(dict,administration,user);

            //Assert
            Assert.AreEqual(true, result);
        }
    }
}
