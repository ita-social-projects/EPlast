//using System;
//using System.Collections.Generic;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using AutoMapper;
//using EPlast.BussinessLayer.DTO.EventUser;
//using EPlast.BussinessLayer.Interfaces.EventUser;
//using EPlast.Controllers;
//using EPlast.ViewModels.Events;
//using EPlast.ViewModels.EventUser;
//using EPlast.ViewModels.UserInformation.UserProfile;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using Xunit;

//namespace EPlast.XUnitTest.Controllers
//{
//    public class EventUserControllerTests
//    {
//        private readonly Mock<IEventUserManager> _eventUserManager;
//        private readonly Mock<IMapper> _mapper;

//        public EventUserControllerTests()
//        {
//            _eventUserManager = new Mock<IEventUserManager>();
//            _mapper = new Mock<IMapper>();
//        }

//        [Fact]
//        public async Task EventUserSuccssesTest()
//        {
//            _eventUserManager.Setup(am => am.EventUserAsync(It.IsAny<string>(), It.IsAny<ClaimsPrincipal>()))
//              .ReturnsAsync(new EventUserDTO());
//            _mapper.Setup(m => m.Map<EventUserDTO, EventUserViewModel>(It.IsAny<EventUserDTO>())).Returns(GetEventUserViewModel());
//            string userId = "3";
//            //Act  
//            var eventUserController = new EventUserController(_eventUserManager.Object, _mapper.Object);
//            var actionResult = await eventUserController.EventUser(userId);
//            //Arrange
//            var viewResult = Assert.IsType<ViewResult>(actionResult);
//            var viewModel = Assert.IsType<EventUserViewModel>(viewResult.Model);
//            Assert.NotNull(actionResult);
//        }

//        [Fact]
//        public async Task EventUserFailureTest()
//        {
//            //Arrange
//            _eventUserManager.Setup(x => x.EventUserAsync(It.IsAny<string>(), It.IsAny<ClaimsPrincipal>()))
//                .ThrowsAsync(new Exception());
//            _mapper.Setup(m => m.Map<EventUserDTO, EventUserViewModel>(It.IsAny<EventUserDTO>()))
//                .Returns(new EventUserViewModel());
//            //Act  
//            var eventUserController = new EventUserController(_eventUserManager.Object, _mapper.Object);
//            var actionResult = await eventUserController.EventUser("");
//            //Arrange
//            Assert.NotNull(actionResult);
//            var viewResult = Assert.IsType<RedirectToActionResult>(actionResult);
//            Assert.Equal("HandleError", viewResult.ActionName);
//            Assert.Equal("Error", viewResult.ControllerName);
//        }

//        [Fact]
//        public async Task EventCreateGetSuccessTest()
//        {
//            _eventUserManager.Setup(x => x.InitializeEventCreateDTOAsync()).ReturnsAsync(new EventCreateDTO());

//            _mapper.Setup(m => m.Map<EventCreateDTO, EventCreateViewModel>(It.IsAny<EventCreateDTO>()))
//                .Returns(GetEventCreateViewModel());
//            //Act  
//            var eventUserController = new EventUserController(_eventUserManager.Object, _mapper.Object);
//            var actionResult = await eventUserController.EventCreate();
//            //Arrange
//            var viewResult = Assert.IsType<ViewResult>(actionResult);
//            var viewModel = Assert.IsType<EventCreateViewModel>(viewResult.Model);
//            Assert.NotNull(actionResult);
//        }

//        [Fact]
//        public async Task EventCreateGetFailureTest()
//        {
//            //Arrange
//            _eventUserManager.Setup(x => x.InitializeEventCreateDTOAsync())
//                .ThrowsAsync(new Exception());
//            _mapper.Setup(m => m.Map<EventCreateDTO, EventCreateViewModel>(It.IsAny<EventCreateDTO>()))
//                     .Returns(new EventCreateViewModel());
//            //Act  
//            var eventUserController = new EventUserController(_eventUserManager.Object, _mapper.Object);
//            var actionResult = await eventUserController.EventCreate();
//            //Arrange
//            Assert.NotNull(actionResult);
//            var viewResult = Assert.IsType<RedirectToActionResult>(actionResult);
//            Assert.Equal("HandleError", viewResult.ActionName);
//            Assert.Equal("Error", viewResult.ControllerName);
//        }

//        [Fact]
//        public async Task EventCreatePostSuccessTest()
//        {
//            _mapper.Setup(x => x.Map<EventCreateViewModel, EventCreateDTO>(It.IsAny<EventCreateViewModel>())).Returns(new EventCreateDTO());
//            _eventUserManager.Setup(x => x.CreateEventAsync(new EventCreateDTO()));
//            // Act
//            var eventUserController = new EventUserController(_eventUserManager.Object, _mapper.Object);
//            var actionResult = await eventUserController.EventCreate(GetEventCreateViewModel());
//            // Assert
//            var viewResult = Assert.IsType<RedirectToActionResult>(actionResult);
//            Assert.Equal("SetAdministration", viewResult.ActionName);
//            Assert.NotNull(viewResult);
//        }

//        [Fact]
//        public async Task EventCreatePostFailureTest()
//        {
//            _mapper.Setup(x => x.Map<EventCreateViewModel, EventCreateDTO>(It.IsAny<EventCreateViewModel>())).Returns(new EventCreateDTO());
//            _eventUserManager.Setup(x => x.CreateEventAsync(new EventCreateDTO()));
//            // Act
//            var eventUserController = new EventUserController(_eventUserManager.Object, _mapper.Object);
//            var actionResult = await eventUserController.EventCreate(GetEventCreateViewModel());
//            eventUserController.ModelState.AddModelError("NameError", "Required");
//            // Assert
//            var viewResult = Assert.IsType<RedirectToActionResult>(actionResult);
//            Assert.Equal("SetAdministration", viewResult.ActionName);
//            Assert.NotNull(viewResult);
//        }

//        [Fact]
//        public async Task SetAdministrationGetSuccessTest()
//        {
//            _eventUserManager.Setup(am => am.InitializeEventCreateDTOAsync(It.IsAny<int>()))
//             .ReturnsAsync(new EventCreateDTO());
//            _mapper.Setup(m => m.Map<EventCreateDTO, EventCreateViewModel>(It.IsAny<EventCreateDTO>())).Returns(GetEventCreateViewModel());
//            int id = 1;
//            //Act  
//            var eventUserController = new EventUserController(_eventUserManager.Object, _mapper.Object);
//            var actionResult = await eventUserController.SetAdministration(id);
//            //Arrange
//            var viewResult = Assert.IsType<ViewResult>(actionResult);
//            var viewModel = Assert.IsType<EventCreateViewModel>(viewResult.Model);
//            Assert.NotNull(actionResult);
//        }

//        [Fact]
//        public async Task SetAdministrationGetFailureTest()
//        {
//            _eventUserManager.Setup(am => am.InitializeEventCreateDTOAsync(It.IsAny<int>()))
//              .ReturnsAsync(new EventCreateDTO());
//            _mapper.Setup(m => m.Map<EventCreateDTO, EventCreateViewModel>(It.IsAny<EventCreateDTO>())).Returns(new EventCreateViewModel());
//            int id = 1;
//            //Act  
//            var eventUserController = new EventUserController(_eventUserManager.Object, _mapper.Object);
//            var actionResult = await eventUserController.SetAdministration(id);
//            //Arrange
//            var viewResult = Assert.IsType<ViewResult>(actionResult);
//            var viewModel = Assert.IsType<EventCreateViewModel>(viewResult.Model);
//            Assert.NotNull(actionResult);
//        }

//        [Fact]
//        public async Task SetAdministrationPostSuccessTest()
//        {
//            _mapper.Setup(x => x.Map<EventCreateViewModel, EventCreateDTO>(It.IsAny<EventCreateViewModel>())).Returns(new EventCreateDTO());
//            _eventUserManager.Setup(x => x.SetAdministrationAsync(new EventCreateDTO()));
//            // Act
//            var eventUserController = new EventUserController(_eventUserManager.Object, _mapper.Object);
//            var actionResult = await eventUserController.SetAdministration(GetEventCreateViewModel());
//            // Assert
//            var viewResult = Assert.IsType<RedirectToActionResult>(actionResult);
//            Assert.Equal("EventInfo", viewResult.ActionName);
//            Assert.NotNull(viewResult);
//        }

//        [Fact]
//        public async Task SetAdministrationPostFailureTest()
//        {
//            //Arrange
//            _mapper.Setup(x => x.Map<EventCreateViewModel, EventCreateDTO>(It.IsAny<EventCreateViewModel>())).Returns(new EventCreateDTO());
//            _eventUserManager.Setup(x => x.SetAdministrationAsync(new EventCreateDTO()));
//            // Act
//            var eventUserController = new EventUserController(_eventUserManager.Object, _mapper.Object);
//            var actionResult = await eventUserController.SetAdministration(GetEventCreateViewModel());
//            eventUserController.ModelState.AddModelError("NameError", "Required");
//            // Assert
//            var viewResult = Assert.IsType<RedirectToActionResult>(actionResult);
//            Assert.Equal("EventInfo", viewResult.ActionName);
//            Assert.NotNull(viewResult);
//        }

//        [Fact]
//        public async Task EventEditGetSuccessTest()
//        {
//            //Arrange
//            int eventId = 1;
//            _mapper.Setup(m => m.Map<EventCreateDTO, EventCreateViewModel>(It.IsAny<EventCreateDTO>())).Returns(GetEventCreateViewModel());
//            _eventUserManager.Setup(x => x.InitializeEventEditDTOAsync(eventId));

//            //Act
//            var eventUserController = new EventUserController(_eventUserManager.Object, _mapper.Object);
//            var actionResult = await eventUserController.EventEdit(eventId);

//            //Assert
//            var viewResult = Assert.IsType<ViewResult>(actionResult);
//            var viewModel = Assert.IsType<EventCreateViewModel>(viewResult.Model);
//            Assert.NotNull(actionResult);
//        }

//        [Fact]
//        public async Task EventEditGetFailureTest()
//        {
//            //Arrange
//            int eventId = 1;
//            _mapper.Setup(m => m.Map<EventCreateDTO, EventCreateViewModel>(It.IsAny<EventCreateDTO>())).Returns(new EventCreateViewModel());
//            _eventUserManager.Setup(x => x.InitializeEventEditDTOAsync(eventId)).ThrowsAsync(new Exception());

//            //Act
//            var eventUserController = new EventUserController(_eventUserManager.Object, _mapper.Object);
//            var actionResult = await eventUserController.EventEdit(eventId);

//            //Assert
//            var viewResult = Assert.IsType<RedirectToActionResult>(actionResult);
//            Assert.Equal("HandleError", viewResult.ActionName);
//            Assert.NotNull(actionResult);
//        }

//        [Fact]
//        public async Task EventEditPostSuccessTest()
//        {
//            _mapper.Setup(x => x.Map<EventCreateViewModel, EventCreateDTO>(It.IsAny<EventCreateViewModel>())).Returns(new EventCreateDTO());
//            _eventUserManager.Setup(x => x.EditEventAsync(new EventCreateDTO()));
//            // Act
//            var eventUserController = new EventUserController(_eventUserManager.Object, _mapper.Object);
//            var actionResult = await eventUserController.EventEdit(GetEventCreateViewModel());
//            // Assert
//            var viewResult = Assert.IsType<RedirectToActionResult>(actionResult);
//            Assert.Equal("EventUser", viewResult.ActionName);
//            Assert.NotNull(viewResult);
//        }

//        [Fact]
//        public async Task EventEditPostFailureTest()
//        {
//            _mapper.Setup(x => x.Map<EventCreateViewModel, EventCreateDTO>(It.IsAny<EventCreateViewModel>())).Returns(new EventCreateDTO());
//            _eventUserManager.Setup(x => x.EditEventAsync(new EventCreateDTO()));
//            // Act
//            var eventUserController = new EventUserController(_eventUserManager.Object, _mapper.Object);
//            var actionResult = await eventUserController.EventEdit(GetEventCreateViewModel());
//            eventUserController.ModelState.AddModelError("NameError", "Required");
//            // Assert
//            var viewResult = Assert.IsType<RedirectToActionResult>(actionResult);
//            Assert.Equal("EventUser", viewResult.ActionName);
//            Assert.NotNull(viewResult);
//        }

//        public EventUserViewModel GetEventUserViewModel()
//        {
//            var model = new EventUserViewModel()
//            {
//                User = new UserViewModel()
//                {
//                    Id = "1",
//                    FirstName = "Ігор",
//                    LastName = "Ігоренко",
//                    ImagePath = "picture.jpg",
//                },
//                PlanedEvents = new List<EventGeneralInfoViewModel>
//                                 {
//                                     new EventGeneralInfoViewModel{ID = 1,EventName = "hello", EventDateStart = DateTime.Now,
//                                         EventDateEnd = DateTime.Now }
//                                 },
//                CreatedEvents = new List<EventGeneralInfoViewModel>
//                                 {
//                                     new EventGeneralInfoViewModel{ID = 2,EventName = "world", EventDateStart = DateTime.Now,
//                                         EventDateEnd = DateTime.Now }
//                                 },
//                VisitedEvents = new List<EventGeneralInfoViewModel>
//                                 {
//                                     new EventGeneralInfoViewModel{ID = 3,EventName = "!!!", EventDateStart = DateTime.Now,
//                                         EventDateEnd = DateTime.Now }
//                                 }
//            };
//            return model;
//        }

//        public EventCreateViewModel GetEventCreateViewModel()
//        {
//            var model = new EventCreateViewModel
//            {
//                Event = new EventCreationViewModel
//                {
//                    ID = 1,
//                    EventName = "тест",
//                    Description = "опис",
//                    Questions = "питання",
//                    EventDateStart = DateTime.Now,
//                    EventDateEnd = DateTime.Now,
//                    Eventlocation = "Львів",
//                    EventCategoryID = 3,
//                    EventStatusID = 2,
//                    EventTypeID = 1,
//                    FormOfHolding = "абв",
//                    ForWhom = "дітей",
//                    NumberOfPartisipants = 1
//                },
//                EventAdmin = new CreateEventAdminViewModel
//                {
//                    UserID = "1",
//                    User = new UserViewModel { }
//                },
//                EventAdministration = new EventAdministrationViewModel
//                {
//                    UserID = "2",
//                    User = new UserViewModel { }
//                },
//                EventCategories = new List<EventCategoryViewModel>
//                {
//                    new EventCategoryViewModel
//                    {
//                        EventCategoryId = 1,
//                        EventCategoryName = "КПЗ"
//                    }
//                },
//                EventTypes = new List<EventTypeViewModel>
//                {
//                    new EventTypeViewModel
//                    {
//                        ID = 1,
//                        EventTypeName = "подія"
//                    }
//                },
//                Users = new List<UserViewModel> { }
//            };
//            return model;
//        }









//    }
//}


