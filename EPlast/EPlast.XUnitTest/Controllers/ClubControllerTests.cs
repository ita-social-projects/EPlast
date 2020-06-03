//using AutoMapper;
//using EPlast.BussinessLayer.DTO.UserProfiles;
//using EPlast.BussinessLayer.Interfaces.Club;
//using EPlast.BussinessLayer.Services.Interfaces;
//using EPlast.Controllers;
//using EPlast.DataAccess.Entities;
//using EPlast.DataAccess.Repositories;
//using EPlast.ViewModels;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using Xunit;

//namespace EPlast.XUnitTest
//{
//    public class ClubControllerTests
//    {
//        private Mock<IRepositoryWrapper> _repository;
//        private Mock<IMapper> _mapper;
//        private Mock<IClubService> _clubService;
//        private Mock<IClubMembersService> _clubMembersService;
//        private Mock<IClubAdministrationService> _clubAdministrationService;
//        //private Mock<IUserStore<User>> _store;
//        //private Mock<UserManager<User>> _userManager;
//        private Mock<IUserManagerService> _userManagerService;
//        //private Mock<IHostingEnvironment> _hostingEnvironment;
//        private Mock<ILoggerService<ClubController>> _loggerService;
//        private ClubController controller;

//        public ClubControllerTests()
//        {
//            _repository = new Mock<IRepositoryWrapper>();
//           // _store = new Mock<IUserStore<User>>();
//            //_userManager = new Mock<UserManager<User>>(_store.Object, null, null, null, null, null, null, null, null);
//            _mapper = new Mock<IMapper>();
//            //_hostingEnvironment = new Mock<IHostingEnvironment>();
//            _loggerService=new Mock<ILoggerService<ClubController>>();
//            _clubService = new Mock<IClubService>();//_repository.Object, _mapper.Object, _hostingEnvironment.Object);
//            _clubMembersService = new Mock<IClubMembersService>();//_repository.Object, _mapper.Object);
//            _clubAdministrationService = new Mock<IClubAdministrationService>();//_repository.Object, _mapper.Object);
//            var user = new UserDTO();
//            _userManagerService = new Mock<IUserManagerService>();
//            _userManagerService.Setup(x => x.FindById(It.IsAny<string>())).ReturnsAsync(user);
//            _userManagerService.Setup(x => x.GetRoles(It.IsAny<UserDTO>())).ReturnsAsync(new List<string>());
//            controller = new ClubController(_clubService.Object, _clubAdministrationService.Object, _clubMembersService.Object, _mapper.Object, _loggerService.Object,_userManagerService.Object);
//        }

//        [Fact]
//        public void Club_FindByID_ReturnsAView()
//        {
//            //arrange
//            _repository.Setup(c => c.Club.FindByCondition(It.IsAny<Expression<Func<Club, bool>>>())).Returns(GetTestClubs());

//            //action         
//            var result = controller.Club(1);

//            //assert
//            var viewResult = Assert.IsType<ViewResult>(result);
//            Assert.IsAssignableFrom<ClubProfileViewModel>(viewResult.Model);
//        }

//        [Fact]
//        public void Index_ReturnsAViewResult_WithAListOfClubs()
//        {
//            //arrange
//            _repository.Setup(c => c.Club.FindByCondition(It.IsAny<Expression<Func<Club, bool>>>())).Returns(GetTestClubs());

//            //action
//            var result = controller.Index();

//            //assert
//            var viewResult = Assert.IsType<ViewResult>(result);
//            Assert.IsAssignableFrom<List<ClubViewModel>>(viewResult.Model);
//        }

//        private IQueryable<Club> GetTestClubs()
//        {
//            var club = new List<Club>{
//                 new Club {
//                     ID = 1,
//                     Logo = "null",
//                     Description = "Some Text",
//                     ClubName = "Клуб номер 1",
//                     ClubAdministration = new List<ClubAdministration>{
//                         new ClubAdministration{
//                             ClubMembers = new ClubMembers{
//                                    IsApproved = true,
//                                    User = new User
//                                    {
//                                        LastName = "Andrii",
//                                        FirstName = "Ivanenko"
//                                    }
//                             },
//                             StartDate = DateTime.Today,
//                             AdminType = new AdminType
//                             {
//                                 AdminTypeName = "Курінний"
//                             }
//                         }
//                     },
//                     ClubMembers = new List<ClubMembers>{
//                         new ClubMembers{
//                            IsApproved = true,
//                            User = new User
//                            {
//                                LastName = "Andrii",
//                                FirstName = "Ivanenko"
//                            }
//                         },
//                         new ClubMembers{
//                            IsApproved = false,
//                            User = new User
//                            {
//                                LastName = "Ivan",
//                                FirstName = "Ivanenko"
//                            }
//                         },
//                         new ClubMembers{
//                            IsApproved = false,
//                            User = new User
//                            {
//                                LastName = "Orest",
//                                FirstName = "Ivanenko"
//                            }
//                         }
//                     }

//                 }
//            }.AsQueryable();
//            return club;
//        }
//        /* 
//         [Fact]
//         public void EditClub_AddLogo_ClubShouldBeUpdated()
//         {
//             //arrange
//             var mockFile = new Mock<IFormFile>();
//             repository.Setup(r => r.Club.FindByCondition(It.IsAny<Expression<Func<Club, bool>>>())).Returns(
//                 new List<Club>
//                 {
//                     new Club{
//                         ClubName = "Club 2",
//                         Description = "Some text"
//                 }
//                 }.AsQueryable());
//             var expected = new Club
//             {
//                 ClubName = "Club 2",
//                 Description = "Some text"
//             };
//             var viewModel = new ClubViewModel { Club = expected };

//             //action
//             controller.EditClub(viewModel, mockFile.Object);

//             //assert
//             repository.Verify(r => r.Club.Update(It.IsAny<Club>()), Times.Once());
//         }
//         */
//        [Fact]
//        public void EditClub_WithoutClubName_Redirect()
//        {
//            //arrange
//            var mockFile = new Mock<IFormFile>();
//            var viewModel = new ClubViewModel();

//            //action
//            var result = controller.EditClub(viewModel, mockFile.Object);

//            //assert
//            Assert.IsType<RedirectToActionResult>(result);
//        }

//        [Fact]
//        public void ChangeIsApprovedStatus_ClubMembersUpdated()
//        {
//            //arrange
//            ChangeIsApprovedStatusRepositorySetup();

//            //action            
//            controller.ChangeIsApprovedStatus(1, 1);

//            //assert
//            _repository.Verify(r => r.ClubMembers.Update(It.IsAny<ClubMembers>()), Times.Once());
//        }

//        [Fact]
//        public void ChangeIsApprovedStatusClub_ClubMembersUpdated()
//        {
//            //arrange
//            ChangeIsApprovedStatusRepositorySetup();

//            //action
//            controller.ChangeIsApprovedStatusClub(1, 1);

//            // assert
//            _repository.Verify(r => r.ClubMembers.Update(It.IsAny<ClubMembers>()), Times.Once());
//        }

//        [Fact]
//        public void ChangeIsApprovedStatusFollowers_ClubMembersUpdated()
//        {
//            //arrange
//            ChangeIsApprovedStatusRepositorySetup();

//            //action           
//            controller.ChangeIsApprovedStatusFollowers(1, 1);

//            //assert
//            _repository.Verify(r => r.ClubMembers.Update(It.IsAny<ClubMembers>()), Times.Once());
//        }

//        private void ChangeIsApprovedStatusRepositorySetup()
//        {
//            _repository.Setup(c => c.Club.FindByCondition(It.IsAny<Expression<Func<Club, bool>>>())).Returns(
//                new List<Club>{
//                 new Club {
//                     ID = 1,
//                     Logo = "null",
//                     Description = "Some Text",
//                     ClubName = "Клуб номер 1",
//                     ClubMembers = new List<ClubMembers>{
//                         new ClubMembers{
//                            ID = 1,
//                            IsApproved = true,
//                            User = new User
//                            {
//                                LastName = "Andrii",
//                                FirstName = "Ivanenko"
//                            }
//                         }
//                     }
//                 }
//            }.AsQueryable());

//            _repository.Setup(c => c.ClubMembers.FindByCondition(It.IsAny<Expression<Func<ClubMembers, bool>>>())).Returns(
//               new List<ClubMembers>{
//                         new ClubMembers{
//                            ID = 1,
//                            IsApproved = true,
//                            User = new User
//                            {
//                                LastName = "Andrii",
//                                FirstName = "Ivanenko"
//                            }
//                         }
//           }.AsQueryable());
//        }
//    }
//}
