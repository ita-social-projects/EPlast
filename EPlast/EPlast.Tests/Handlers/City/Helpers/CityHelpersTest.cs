using System.Collections.Generic;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Handlers.CityHandlers.Helpers;
using EPlast.Resources;
using NUnit.Framework;

namespace EPlast.Tests.Handlers.City.Helpers
{
    public static class CityHelpersTest
    {
        [Test]
        public static void GetCityHead_ReturnsCityHead()
        {
            //Arrange
            var city = new CityDto
            {
                CityAdministration = new List<CityAdministrationDto>()
            {
                new CityAdministrationDto()
                {
                    AdminType = new AdminTypeDto()
                    {
                        AdminTypeName = Roles.CityHead
                    },
                    Status = true
                }
            }
            };

            //Act
            var result = CityHelpers.GetCityHead(city.CityAdministration);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CityAdministrationDto>(result);
        }

        [Test]
        public static void GetCityHeadDeputy_ReturnsCityHeadDeputy()
        {
            //Arrange
            var city = new CityDto
            {
                CityAdministration = new List<CityAdministrationDto>()
                {
                    new CityAdministrationDto()
                    {
                        AdminType = new AdminTypeDto()
                        {
                            AdminTypeName = Roles.CityHeadDeputy
                        },
                        Status = true
                    }
                }
            };

            //Act
            var result = CityHelpers.GetCityHeadDeputy(city.CityAdministration);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CityAdministrationDto>(result);
        }

        [Test]
        public static void GetCityAdmins_ReturnsCityAdmins()
        {
            //Arrange
            var city = new CityDto
            {
                CityAdministration = new List<CityAdministrationDto>()
                {
                    new CityAdministrationDto()
                    {
                        AdminType = new AdminTypeDto()
                        {
                            AdminTypeName = Roles.CityHeadDeputy
                        },
                        Status = true
                    },
                    new CityAdministrationDto()
                    {
                        AdminType = new AdminTypeDto()
                        {
                            AdminTypeName = Roles.CityHead
                        },
                        Status = true
                    }
                }
            };

            //Act
            var result = CityHelpers.GetCityAdmins(city.CityAdministration);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<CityAdministrationDto>>(result);
        }
    }
}
