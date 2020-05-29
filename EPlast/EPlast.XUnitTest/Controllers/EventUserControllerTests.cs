using AutoMapper;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using EPlast.BussinessLayer.DTO.Events;
using EPlast.BussinessLayer.DTO.EventUser;
using EPlast.BussinessLayer.DTO.UserProfiles;
using EPlast.BussinessLayer.Interfaces.EventUser;
using EPlast.Controllers;
using EPlast.DataAccess.Entities;
using EPlast.Models.ViewModelInitializations;
using EPlast.ViewModels.Events;
using EPlast.ViewModels.EventUser;
using EPlast.ViewModels.UserInformation;
using EPlast.ViewModels.UserInformation.UserProfile;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Cryptography;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using Xunit;

namespace EPlast.XUnitTest
{
    public class EventUserControllerTests
    {
        private readonly Mock<IEventUserManager> _eventUserManager;
        private readonly Mock<IMapper> _mapper;

        public EventUserControllerTests()
        {
            _eventUserManager = new Mock<IEventUserManager>();
            _mapper = new Mock<IMapper>();
        }

        [Fact]
        public void EventUserSuccssesTest()
        {
            _eventUserManager.Setup(am => am.EventUser(It.IsAny<string>(), It.IsAny<ClaimsPrincipal>()))
              .Returns(new EventUserDTO());
            _mapper.Setup(m => m.Map<EventUserDTO, EventUserViewModel>(It.IsAny<EventUserDTO>())).Returns(GetEventUserViewModel());
            string userId = "3";
            //Act  
            var eventUserController = new EventUserController(_eventUserManager.Object, _mapper.Object);
            var actionResult = eventUserController.EventUser(userId);
            //Arrange
            var viewResult = Assert.IsType<ViewResult>(actionResult);
            var viewModel = Assert.IsType<EventUserViewModel>(viewResult.Model);
            Assert.NotNull(actionResult);
        }

        [Fact]
        public void EventUserFailureTest()
        {
            //Arrange
            _eventUserManager.Setup(x => x.EventUser(It.IsAny<string>(), It.IsAny<ClaimsPrincipal>()))
                .Throws(new Exception());
            _mapper.Setup(m => m.Map<EventUserDTO, EventUserViewModel>(It.IsAny<EventUserDTO>()))
                .Returns(new EventUserViewModel());
            //Act  
            var eventUserController = new EventUserController(_eventUserManager.Object, _mapper.Object);
            var actionResult = eventUserController.EventUser("");
            //Arrange
            Assert.NotNull(actionResult);
            var viewResult = Assert.IsType<RedirectToActionResult>(actionResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public void EventCreateGetSuccessTest()
        {
            _eventUserManager.Setup(x => x.InitializeEventCreateDTO()).Returns(new EventCreateDTO());

            _mapper.Setup(m => m.Map<EventCreateDTO, EventCreateViewModel>(It.IsAny<EventCreateDTO>()))
                .Returns(GetEventCreateViewModel());
            //Act  
            var eventUserController = new EventUserController(_eventUserManager.Object, _mapper.Object);
            var actionResult = eventUserController.EventCreate();
            //Arrange
            var viewResult = Assert.IsType<ViewResult>(actionResult);
            var viewModel = Assert.IsType<EventCreateViewModel>(viewResult.Model);
            Assert.NotNull(actionResult);
        }

        [Fact]
        public void EventCreateGetFailureTest()
        {
            //Arrange
            _eventUserManager.Setup(x => x.InitializeEventCreateDTO())
                .Throws(new Exception());
            _mapper.Setup(m => m.Map<EventCreateDTO, EventCreateViewModel>(It.IsAny<EventCreateDTO>()))
                     .Returns(new EventCreateViewModel());
            //Act  
            var eventUserController = new EventUserController(_eventUserManager.Object, _mapper.Object);
            var actionResult = eventUserController.EventCreate();
            //Arrange
            Assert.NotNull(actionResult);
            var viewResult = Assert.IsType<RedirectToActionResult>(actionResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public void EventCreatePostSuccessTest()
        {
            _mapper.Setup(x => x.Map<EventCreateViewModel, EventCreateDTO>(It.IsAny<EventCreateViewModel>())).Returns(new EventCreateDTO());
            _eventUserManager.Setup(x => x.CreateEvent(new EventCreateDTO()));
            // Act
            var eventUserController = new EventUserController(_eventUserManager.Object, _mapper.Object);
            var actionResult = eventUserController.EventCreate(GetEventCreateViewModel());
            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(actionResult);
            Assert.Equal("SetAdministration", viewResult.ActionName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void EventCreatePostFailureTest()
        {
            _mapper.Setup(x => x.Map<EventCreateViewModel, EventCreateDTO>(It.IsAny<EventCreateViewModel>())).Returns(new EventCreateDTO());
            _eventUserManager.Setup(x => x.CreateEvent(new EventCreateDTO()));
            // Act
            var eventUserController = new EventUserController(_eventUserManager.Object, _mapper.Object);
            var actionResult = eventUserController.EventCreate(GetEventCreateViewModel());
            eventUserController.ModelState.AddModelError("NameError", "Required");
            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(actionResult);
            Assert.Equal("SetAdministration", viewResult.ActionName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void SetAdministrationGetSuccessTest()
        {
            _eventUserManager.Setup(am => am.InitializeEventCreateDTO(It.IsAny<int>()))
             .Returns(new EventCreateDTO());
            _mapper.Setup(m => m.Map<EventCreateDTO, EventCreateViewModel>(It.IsAny<EventCreateDTO>())).Returns(GetEventCreateViewModel());
            int id = 1;
            //Act  
            var eventUserController = new EventUserController(_eventUserManager.Object, _mapper.Object);
            var actionResult = eventUserController.SetAdministration(id);
            //Arrange
            var viewResult = Assert.IsType<ViewResult>(actionResult);
            var viewModel = Assert.IsType<EventCreateViewModel>(viewResult.Model);
            Assert.NotNull(actionResult);
        }

        [Fact]
        public void SetAdministrationGetFailureTest()
        {
            _eventUserManager.Setup(am => am.InitializeEventCreateDTO(It.IsAny<int>()))
              .Returns(new EventCreateDTO());
            _mapper.Setup(m => m.Map<EventCreateDTO, EventCreateViewModel>(It.IsAny<EventCreateDTO>())).Returns(new EventCreateViewModel());
            int id = 1;
            //Act  
            var eventUserController = new EventUserController(_eventUserManager.Object, _mapper.Object);
            var actionResult = eventUserController.SetAdministration(id);
            //Arrange
            var viewResult = Assert.IsType<ViewResult>(actionResult);
            var viewModel = Assert.IsType<EventCreateViewModel>(viewResult.Model);
            Assert.NotNull(actionResult);
        }

        [Fact]
        public void SetAdministrationPostSuccessTest()
        {
            _mapper.Setup(x => x.Map<EventCreateViewModel, EventCreateDTO>(It.IsAny<EventCreateViewModel>())).Returns(new EventCreateDTO());
            _eventUserManager.Setup(x => x.SetAdministration(new EventCreateDTO()));
            // Act
            var eventUserController = new EventUserController(_eventUserManager.Object, _mapper.Object);
            var actionResult = eventUserController.SetAdministration(GetEventCreateViewModel());
            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(actionResult);
            Assert.Equal("EventInfo", viewResult.ActionName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void SetAdministrationPostFailureTest()
        {
            _mapper.Setup(x => x.Map<EventCreateViewModel, EventCreateDTO>(It.IsAny<EventCreateViewModel>())).Returns(new EventCreateDTO());
            _eventUserManager.Setup(x => x.SetAdministration(new EventCreateDTO()));
            // Act
            var eventUserController = new EventUserController(_eventUserManager.Object, _mapper.Object);
            var actionResult = eventUserController.SetAdministration(GetEventCreateViewModel());
            eventUserController.ModelState.AddModelError("NameError", "Required");
            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(actionResult);
            Assert.Equal("EventInfo", viewResult.ActionName);
            Assert.NotNull(viewResult);
        }

        public EventUserViewModel GetEventUserViewModel()
        {
            var model = new EventUserViewModel()
            {
                User = new UserViewModel()
                {
                    Id = "1",
                    FirstName = "Ігор",
                    LastName = "Ігоренко",
                    ImagePath = "picture.jpg",
                },
                PlanedEvents = new List<EventGeneralInfoViewModel>
                                 {
                                     new EventGeneralInfoViewModel{ID = 1,EventName = "hello", EventDateStart = DateTime.Now,
                                         EventDateEnd = DateTime.Now }
                                 },
                CreatedEvents = new List<EventGeneralInfoViewModel>
                                 {
                                     new EventGeneralInfoViewModel{ID = 2,EventName = "world", EventDateStart = DateTime.Now,
                                         EventDateEnd = DateTime.Now }
                                 },
                VisitedEvents = new List<EventGeneralInfoViewModel>
                                 {
                                     new EventGeneralInfoViewModel{ID = 3,EventName = "!!!", EventDateStart = DateTime.Now,
                                         EventDateEnd = DateTime.Now }
                                 }
            };
            return model;
        }

        public EventCreateViewModel GetEventCreateViewModel()
        {
            var model = new EventCreateViewModel
            {
                Event = new EventCreationViewModel
                {
                    ID = 1,
                    EventName = "тест",
                    Description = "опис",
                    Questions = "питання",
                    EventDateStart = DateTime.Now,
                    EventDateEnd = DateTime.Now,
                    Eventlocation = "Львів",
                    EventCategoryID = 3,
                    EventStatusID = 2,
                    EventTypeID = 1,
                    FormOfHolding = "абв",
                    ForWhom = "дітей",
                    NumberOfPartisipants = 1
                },
                EventAdmin = new CreateEventAdminViewModel
                {
                    UserID = "1",
                    User = new UserViewModel { }
                },
                EventAdministration = new EventAdministrationViewModel
                {
                    UserID = "2",
                    User = new UserViewModel { }
                },
                EventCategories = new List<EventCategoryViewModel>
                {
                    new EventCategoryViewModel
                    {
                        EventCategoryId = 1,
                        EventCategoryName = "КПЗ"
                    }
                },
                EventTypes = new List<EventTypeViewModel>
                {
                    new EventTypeViewModel
                    {
                        ID = 1,
                        EventTypeName = "подія"
                    }
                },
                Users = new List<UserViewModel> { }
            };
            return model;
        }









    }
}


