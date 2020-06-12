using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.BussinessLayer.DTO.UserProfiles;
using EPlast.BussinessLayer.Interfaces.Club;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.Controllers;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace EPlast.XUnitTest
{
    public class ClubControllerTests
    {
        private Mock<IRepositoryWrapper> _repository;
        private Mock<IMapper> _mapper;
        private Mock<IClubService> _clubService;
        private Mock<IClubMembersService> _clubMembersService;
        private Mock<IClubAdministrationService> _clubAdministrationService;
        //private Mock<IUserStore<User>> _store;
        //private Mock<UserManager<User>> _userManager;
        private Mock<IUserManagerService> _userManagerService;
        //private Mock<IHostingEnvironment> _hostingEnvironment;
        private Mock<ILoggerService<ClubController>> _loggerService;
        private ClubController controller;

        public ClubControllerTests()
        {
            _repository = new Mock<IRepositoryWrapper>();
            // _store = new Mock<IUserStore<User>>();
            //_userManager = new Mock<UserManager<User>>(_store.Object, null, null, null, null, null, null, null, null);
            _mapper = new Mock<IMapper>();
            //_hostingEnvironment = new Mock<IHostingEnvironment>();
            _loggerService = new Mock<ILoggerService<ClubController>>();

            _clubService = new Mock<IClubService>();//_repository.Object, _mapper.Object, _hostingEnvironment.Object);
            _clubMembersService = new Mock<IClubMembersService>();//_repository.Object, _mapper.Object);
            _clubAdministrationService = new Mock<IClubAdministrationService>();//_repository.Object, _mapper.Object);

            var user = new UserDTO();
            _userManagerService = new Mock<IUserManagerService>();
            _userManagerService.Setup(x => x.FindById(It.IsAny<string>())).ReturnsAsync(user);
            _userManagerService.Setup(x => x.GetRoles(It.IsAny<UserDTO>())).ReturnsAsync(new List<string>());

            controller = new ClubController(_clubService.Object,
                _clubAdministrationService.Object,
                _clubMembersService.Object,
                _mapper.Object,
                _loggerService.Object,
                _userManagerService.Object);
        }

        //[Fact]
        //public void Club_FindByID_ReturnsAView()
        //{
        //    //arrange
        //    _repository.Setup(c => c.Club.FindByCondition(It.IsAny<Expression<Func<Club, bool>>>())).Returns(GetTestClubs());

        //    //action         
        //    var result = controller.Club(1);

        //    //assert
        //    var viewResult = Assert.IsType<ViewResult>(result);
        //    Assert.IsAssignableFrom<ClubProfileViewModel>(viewResult.Model);
        //}

        //[Fact]
        //public void Index_ReturnsAViewResult_WithAListOfClubs()
        //{
        //    //arrange
        //    _repository.Setup(c => c.Club.FindByCondition(It.IsAny<Expression<Func<Club, bool>>>())).Returns(GetTestClubs());

        //    //action
        //    var result = controller.Index();

        //    //assert
        //    var viewResult = Assert.IsType<ViewResult>(result);
        //    Assert.IsAssignableFrom<List<ClubViewModel>>(viewResult.Model);
        //}

        //private IQueryable<Club> GetTestClubs()
        //{
        //    var club = new List<Club>{
        //         new Club {
        //             ID = 1,
        //             Logo = "null",
        //             Description = "Some Text",
        //             ClubName = "Клуб номер 1",
        //             ClubAdministration = new List<ClubAdministration>{
        //                 new ClubAdministration{
        //                     ClubMembers = new ClubMembers{
        //                            IsApproved = true,
        //                            User = new User
        //                            {
        //                                LastName = "Andrii",
        //                                FirstName = "Ivanenko"
        //                            }
        //                     },
        //                     StartDate = DateTime.Today,
        //                     AdminType = new AdminType
        //                     {
        //                         AdminTypeName = "Курінний"
        //                     }
        //                 }
        //             },
        //             ClubMembers = new List<ClubMembers>{
        //                 new ClubMembers{
        //                    IsApproved = true,
        //                    User = new User
        //                    {
        //                        LastName = "Andrii",
        //                        FirstName = "Ivanenko"
        //                    }
        //                 },
        //                 new ClubMembers{
        //                    IsApproved = false,
        //                    User = new User
        //                    {
        //                        LastName = "Ivan",
        //                        FirstName = "Ivanenko"
        //                    }
        //                 },
        //                 new ClubMembers{
        //                    IsApproved = false,
        //                    User = new User
        //                    {
        //                        LastName = "Orest",
        //                        FirstName = "Ivanenko"
        //                    }
        //                 }
        //             }

        //         }
        //    }.AsQueryable();
        //    return club;
        //}

        //[Fact]
        //public void EditClub_AddLogo_ClubShouldBeUpdated()
        //{
        //    //arrange
        //    var mockFile = new Mock<IFormFile>();
        //    _repository.Setup(r => r.Club.FindByCondition(It.IsAny<Expression<Func<Club, bool>>>())).Returns(
        //        new List<Club>
        //        {
        //             new Club{
        //                 ClubName = "Club 2",
        //                 Description = "Some text"
        //         }
        //        }.AsQueryable());
        //    var expected = new Club
        //    {
        //        ClubName = "Club 2",
        //        Description = "Some text"
        //    };
        //    var viewModel = new ClubViewModel { Club = expected };

        //    //action
        //    controller.EditClub(viewModel, mockFile.Object);

        //    //assert
        //    _repository.Verify(r => r.Club.Update(It.IsAny<Club>()), Times.Once());
        //}

        //[Fact]
        //public void EditClub_WithoutClubName_Redirect()
        //{
        //    //arrange
        //    var mockFile = new Mock<IFormFile>();
        //    var viewModel = new ClubViewModel();

        //    //action
        //    var result = controller.EditClub(viewModel, mockFile.Object);

        //    //assert
        //    Assert.IsType<RedirectToActionResult>(result);
        //}

        //[Fact]
        //public void ChangeIsApprovedStatus_ClubMembersUpdated()
        //{
        //    //arrange
        //    ChangeIsApprovedStatusRepositorySetup();

        //    //action            
        //    controller.ChangeIsApprovedStatus(1, 1);

        //    //assert
        //    _repository.Verify(r => r.ClubMembers.Update(It.IsAny<ClubMembers>()), Times.Once());
        //}

        [Fact]
        public async Task ClubAdminsRetursView()
        {
            //Arrange
            _mapper
                .Setup(s => s.Map<ClubProfileDTO>(It.IsAny<ClubProfileViewModel>()))
                .Returns(GetTestClubProfileDTO());

            //Act
            var result = await controller.ClubAdmins(1);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ClubAdminsRetursHandleError()
        {
            //Arrange
            _mapper
                .Setup(s => s.Map<ClubProfileDTO>(It.IsAny<ClubProfileViewModel>()))
                .Returns((ClubProfileDTO)null);

            //Act
            var result = await controller.ClubAdmins(1);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ChangeIsApprovedStatusClubReturnClub()
        {
            //Act
            var result = await controller.ChangeIsApprovedStatusClub(1, 1);

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("Club", viewResult.ActionName);
        }

        [Fact]
        public async Task ChangeIsApprovedStatusClubReturnHandleError()
        {
            //Arrange
            _clubMembersService
                .Setup(s => s.ToggleIsApprovedInClubMembersAsync(1, 1))
                .Returns((Task)null);

            //Act
            var result = await controller.ChangeIsApprovedStatusClub(1, 1);

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
        }

        [Fact]
        public async Task DeleteFromAdminsReturnsClubAdmins()
        {
            //Arrange
            _clubAdministrationService
                .Setup(s => s.DeleteClubAdminAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            //Act
            var result = await controller.DeleteFromAdmins(1, 1);

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("ClubAdmins", viewResult.ActionName);
        }

        [Fact]
        public async Task DeleteFromAdminsReturnsHandleError()
        {
            //Arrange
            _clubAdministrationService
                .Setup(s => s.DeleteClubAdminAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            //Act
            var result = await controller.DeleteFromAdmins(1, 1);

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
        }

        [Fact]
        public async Task AddEndDateReturns1()
        {
            //Act
            var result = await controller.AddEndDate(GetTestAdminEndDateDTO());

            //Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task AddEndDateReturns0()
        {
            //Arrange
            _clubAdministrationService
                .Setup(s => s.SetAdminEndDateAsync(It.IsAny<AdminEndDateDTO>()))
                .Returns((Task)null);

            //Act
            var result = await controller.AddEndDate(GetTestAdminEndDateDTO());

            //Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task AddToClubAdministrationReturnJsonTrue()
        {
            //Act
            var result = await controller.AddToClubAdministration(GetTestClubAdministrationDTO());

            //Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Contains("True", jsonResult.Value.ToString());
        }

        [Fact]
        public async Task AddToClubAdministrationReturnJsonFalse()
        {
            //Arrange
            _clubAdministrationService
                .Setup(s => s.AddClubAdminAsync(It.IsAny<ClubAdministrationDTO>()))
                .Returns((Task)null);

            //Act
            var result = await controller.AddToClubAdministration(GetTestClubAdministrationDTO());

            //Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Contains("False", jsonResult.Value.ToString());
        }

        [Fact]
        public async Task AddAsClubFollowerReturnsUserProfile()
        {
            //Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "example name"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            _userManagerService
                .Setup(s => s.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns("aaaa-bbbb-cccc");

            //Act
            var result = await controller.AddAsClubFollower(1, "aaaa-bbbb-cccc");

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("UserProfile", viewResult.ActionName);
        }

        private static ClubProfileDTO GetTestClubProfileDTO()
        {
            return new ClubProfileDTO
            {
                Club = new ClubDTO()
                {
                    ClubName = "Club",
                    Description = "New club",
                    ID = 1
                }
            };
        }

        private static AdminEndDateDTO GetTestAdminEndDateDTO()
        {
            return new AdminEndDateDTO
            {
                AdminId = 1,
                ClubIndex = 1,
                EndDate = new DateTime(2030, 10, 5)
            };
        }

        private static ClubAdministrationDTO GetTestClubAdministrationDTO()
        {
            return new ClubAdministrationDTO
            {
                AdminType = new AdminType()
                {
                    AdminTypeName = "Admin",
                },
                AdminTypeName = "Admin",
                AdminTypeId = 1,
                ClubId = 1,
                ID = 1,
                StartDate = new DateTime(2020, 5, 10),
                EndDate = new DateTime(2030, 10, 5)
            };
        }

        //private static IQueryable<ClubMembers> GetTestClubMembersQueryable()
        //{
        //    return new List<ClubMembers>
        //    {
        //        new ClubMembers {
        //            ID = 1,
        //            IsApproved = true,
        //            User = new User
        //            {
        //                LastName = "Andrii",
        //                FirstName = "Ivanenko"
        //            }},
        //        new ClubMembers {
        //            ID = 2,
        //            IsApproved = true,
        //            User = new User
        //            {
        //                LastName = "Ivan",
        //                FirstName = "Andrienko"
        //            }},
        //        new ClubMembers {
        //            ID = 3,
        //            IsApproved = false,
        //            User = new User
        //            {
        //                LastName = "Vova",
        //                FirstName = "Volodyr"
        //            }},
        //        new ClubMembers {
        //            ID = 4,
        //            IsApproved = true,
        //            User = new User
        //            {
        //                LastName = "Ostap",
        //                FirstName = "Bereza"
        //            }}
        //    }.AsQueryable();
        //}

        //[Fact]
        //public void ChangeIsApprovedStatusFollowers_ClubMembersUpdated()
        //{
        //    //arrange
        //    ChangeIsApprovedStatusRepositorySetup();

        //    //action           
        //    controller.ChangeIsApprovedStatusFollowers(1, 1);

        //    //assert
        //    _repository.Verify(r => r.ClubMembers.Update(It.IsAny<ClubMembers>()), Times.Once());
        //}

        //private void ChangeIsApprovedStatusRepositorySetup()
        //{
        //    _repository.Setup(c => c.Club.FindByCondition(It.IsAny<Expression<Func<Club, bool>>>())).Returns(
        //        new List<Club>{
        //            new Club {
        //                ID = 1,
        //                Logo = "null",
        //                Description = "Some Text",
        //                ClubName = "Клуб номер 1",
        //                ClubMembers = new List<ClubMembers>{
        //                new ClubMembers{
        //                    ID = 1,
        //                    IsApproved = true,
        //                    User = new User
        //                    {
        //                        LastName = "Andrii",
        //                        FirstName = "Ivanenko"
        //                    }
        //                },
        //             }
        //         }
        //    }.AsQueryable());

        //    _repository.Setup(c => c.ClubMembers.FindByCondition(It.IsAny<Expression<Func<ClubMembers, bool>>>())).Returns(
        //        new List<ClubMembers>{
        //            new ClubMembers{
        //                ID = 1,
        //                IsApproved = true,
        //                User = new User
        //                {
        //                    LastName = "Andrii",
        //                    FirstName = "Ivanenko"
        //                }
        //            }
        //   }.AsQueryable());
        //}
    }
}
