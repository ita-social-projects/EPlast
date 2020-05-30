using EPlast.BussinessLayer.Interfaces;
using EPlast.Controllers;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace EPlast.XUnitTest
{
    public class CityControllerTests
    {
        //a
        //    var viewModel = indexResult.Model as List<CityProfileViewModel>;
        //    Assert.NotNull(viewModel);

        //    Assert.NotNull(viewModel[0].City);
        //    Assert.Equal("Харків", viewModel[0].City.Name);

        //}

        //[Fact]
        //public void CityProfileTest()
        //{
        //    _repoWrapper.Setup(c => c.City.FindByCondition(It.IsAny<Expression<Func<City, bool>>>())).Returns(
        //        new List<City>
        //        {
        //            new City
        //            {
        //                ID=1,
        //                Logo = null,
        //                Description = "City Description",
        //                Name= "Львів",
        //                PhoneNumber = "+380934353139",
        //                Email= "lviv@eplast.org",
        //                CityURL = "lviv.eplast.org",
        //                Street = "Шевченка",
        //                HouseNumber="5",
        //                OfficeNumber = "7",
        //                PostIndex = "79000",
        //                CityAdministration = new List<CityAdministration>
        //                {
        //                    new CityAdministration
        //                    {
        //                        User = new User{
        //                            FirstName = "Микола",
        //                            LastName = "Тищенко"
        //                        },
        //                        StartDate = DateTime.Now,
        //                        AdminType = new AdminType
        //                        {
        //                            AdminTypeName = "Голова Станиці"
        //                        }
        //                    },
        //                    new CityAdministration
        //                    {
        //                        User = new User{
        //                            FirstName = "Тарас",
        //                            LastName = "Солоха"
        //                        },
        //                        StartDate = DateTime.Now,
        //                        AdminType = new AdminType
        //                        {
        //                            AdminTypeName = "Писар"
        //                        }
        //                    }
        //                },
        //                CityMembers = new List<CityMembers>
        //                {
        //                    new CityMembers
        //                    {
        //                        CityId = 1,
        //                        StartDate = DateTime.Now,
        //                        User = new User
        //                        {
        //                            FirstName = "Роман",
        //                            LastName = "Корновий"
        //                        }
        //                    },
        //                    new CityMembers
        //                    {
        //                        CityId = 1,
        //                        StartDate = DateTime.Now,
        //                        User = new User
        //                        {
        //                            FirstName = "Назар",
        //                            LastName = "Бунчужний"
        //                        }
        //                    },
        //                    new CityMembers
        //                    {
        //                        CityId = 1,
        //                        StartDate = DateTime.Now,
        //                        User = new User
        //                        {
        //                            FirstName = "Олег",
        //                            LastName = "Перепалко"
        //                        }
        //                    },
        //                    new CityMembers
        //                    {
        //                        CityId = 1,
        //                        StartDate = DateTime.Now,
        //                        User = new User
        //                        {
        //                            FirstName = "Андрій",
        //                            LastName = "Синиця"
        //                        }
        //                    }
        //                },
        //                CityDocuments = new List<CityDocuments>
        //                {
        //                    new CityDocuments
        //                    {
                                
        //                        DocumentURL = null,
        //                        City = new City
        //                        {
        //                            ID=1,
        //                            Logo = null,
        //                            Description = "City Description",
        //                            Name= "Львів",
        //                            PhoneNumber = "+380934353139",
        //                            Email= "lviv@eplast.org",
        //                            CityURL = "lviv.eplast.org",
        //                            Street = "Шевченка",
        //                            HouseNumber="5",
        //                            OfficeNumber = "7",
        //                            PostIndex = "79000"
        //                        },
        //                        CityDocumentType = new CityDocumentType{Name = "Збір ЗСС"},
        //                        SubmitDate = DateTime.Now

        //                    }
        //                }
                        
        //            }
        //        }.AsQueryable());

        //    var citycontroller = new CityController(_repoWrapper.Object, _userManager.Object, _logger.Object, _env.Object);
        //    var cityProfileResult = citycontroller.CityProfile(1) as ViewResult;

        //    Assert.NotNull(cityProfileResult);
        //    Assert.NotNull(cityProfileResult.Model);

        //}
    }
}
