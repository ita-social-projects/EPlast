using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using EPlast.BusinessLogicLayer.DTO.Events;
using EPlast.BusinessLogicLayer.Interfaces.Events;
using EPlast.Controllers;
using EPlast.ViewModels.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace EPlast.XUnitTest.Controllers
{
    public class ActionControllerTests
    {
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IActionManager> _actionManager;

        public ActionControllerTests()
        {
            _mapper = new Mock<IMapper>();
            _actionManager = new Mock<IActionManager>();
        }

        [Fact]
        public async void DeletePictureSuccessTest()
        {
            //Arrange
            int testGalleryId = 2;
            _actionManager.Setup(x => x.DeletePictureAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status200OK);
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = await actionsController.DeletePicture(testGalleryId);
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status200OK, codeResult?.StatusCode);
        }

        [Fact]
        public async void DeletePictureFailTest()
        {
            //Arrange
            int testGalleryId = 2;
            _actionManager.Setup(x => x.DeletePictureAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception());
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = await actionsController.DeletePicture(testGalleryId);
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, codeResult?.StatusCode);
        }

        [Fact]
        public async void DeleteEventSuccessTest()
        {
            //Arrange
            int eventId = 2;
            _actionManager.Setup(x => x.DeleteEventAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status200OK);
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = await actionsController.DeleteEvent(eventId);
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status200OK, codeResult?.StatusCode);
        }

        [Fact]
        public async void DeleteEventFailTest()
        {
            //Arrange
            int eventId = 2;
            _actionManager.Setup(x => x.DeleteEventAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception());
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = await actionsController.DeleteEvent(eventId);
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, codeResult?.StatusCode);
        }


        [Fact]
        public async void UnsubscribeOnEventSuccessTest()
        {
            //Arrange
            int eventId = 2;
            _actionManager.Setup(x => x.UnSubscribeOnEventAsync(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(StatusCodes.Status200OK);
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = await actionsController.UnSubscribeOnEvent(eventId);
            var codeResult =  actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status200OK, codeResult?.StatusCode);
        }

        [Fact]
        public async void UnsubscribeOnEventFailTest()
        {
            //Arrange
            int eventId = 2;
            _actionManager.Setup(x => x.UnSubscribeOnEventAsync(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .ThrowsAsync(new Exception());
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = await actionsController.UnSubscribeOnEvent(eventId);
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, codeResult?.StatusCode);
        }

        [Fact]
        public async void SubscribeOnEventSuccessTest()
        {
            //Arrange
            int eventId = 2;
            _actionManager.Setup(x => x.SubscribeOnEventAsync(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(StatusCodes.Status200OK);
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = await actionsController.SubscribeOnEvent(eventId);
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status200OK, codeResult?.StatusCode);
        }

        [Fact]
        public async void SubscribeOnEventFailTest()
        {
            //Arrange
            int eventId = 2;
            _actionManager.Setup(x => x.SubscribeOnEventAsync(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .ThrowsAsync(new Exception());
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = await actionsController.SubscribeOnEvent(eventId);
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, codeResult?.StatusCode);
        }

        [Fact]
        public async void ApproveParticipantSuccessTest()
        {
            //Arrange
            int participantId = 2;
            _actionManager.Setup(x => x.ApproveParticipantAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status200OK);
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = await actionsController.ApproveParticipant(participantId);
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status200OK, codeResult?.StatusCode);
        }

        [Fact]
        public async void UndetermineParticipantSuccessTest()
        {
            //Arrange
            int participantId = 2;
            _actionManager.Setup(x => x.UnderReviewParticipantAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status200OK);
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = await actionsController.UndetermineParticipant(participantId);
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status200OK, codeResult?.StatusCode);
        }

        [Fact]
        public async void RejectParticipantSuccessTest()
        {
            //Arrange
            int participantId = 2;
            _actionManager.Setup(x => x.RejectParticipantAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status200OK);
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = await actionsController.RejectParticipant(participantId);
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status200OK, codeResult?.StatusCode);
        }

        [Fact]
        public async void ApproveParticipantFailTest()
        {
            //Arrange
            int participantId = 2;
            _actionManager.Setup(x => x.ApproveParticipantAsync(It.IsAny<int>()))
                .Throws(new Exception());
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = await actionsController.ApproveParticipant(participantId);
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, codeResult?.StatusCode);
        }

        [Fact]
        public async void UndetermineParticipantFailTest()
        {
            //Arrange
            int participantId = 2;
            _actionManager.Setup(x => x.UnderReviewParticipantAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception());
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = await actionsController.UndetermineParticipant(participantId);
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, codeResult?.StatusCode);
        }

        [Fact]
        public async void RejectParticipantFailTest()
        {
            //Arrange
            int participantId = 2;
            _actionManager.Setup(x => x.RejectParticipantAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception());
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = await actionsController.RejectParticipant(participantId);
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, codeResult?.StatusCode);
        }

        [Fact]
        public async void EventInfoFailureTest()
        {
            //Arrange
            _actionManager.Setup(am => am.GetEventInfoAsync(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .ThrowsAsync(new Exception());
            _mapper.Setup(m => m.Map<EventDTO, EventViewModel>(It.IsAny<EventDTO>())).Returns(new EventViewModel());
            int eventID = 3;
            //Act  
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = await actionsController.EventInfo(eventID);
            //Arrange
            Assert.NotNull(actionResult);
            var viewResult = Assert.IsType<RedirectToActionResult>(actionResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public async void EventInfoSuccessTest()
        {
            //Arrange
            _actionManager.Setup(am => am.GetEventInfoAsync(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new EventDTO());
            _mapper.Setup(m => m.Map<EventDTO, EventViewModel>(It.IsAny<EventDTO>())).Returns(GetEventViewModel());
            int eventID = 3;
            //Act  
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = await actionsController.EventInfo(eventID);
            //Arrange
            Assert.NotNull(actionResult);
            var viewResult = Assert.IsType<ViewResult>(actionResult);
            var viewModel = Assert.IsType<EventViewModel>(viewResult.Model);
            Assert.Equal(3, viewModel.Event.EventParticipants.Count());
        }

        [Fact]
        public async void GetActionsFailureTest()
        {
            //Arrange
            _actionManager.Setup(am => am.GetActionCategoriesAsync())
                .ThrowsAsync(new Exception());
            _mapper.Setup(m => m.Map<List<EventCategoryDTO>, List<EventCategoryViewModel>>(It.IsAny<List<EventCategoryDTO>>())).Returns(new List<EventCategoryViewModel>());
            //Act  
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = await actionsController.GetAction();
            //Arrange
            Assert.NotNull(actionResult);
            var viewResult = Assert.IsType<RedirectToActionResult>(actionResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public async void GetActionEmptyTest()
        {
            //Arrange
            _actionManager.Setup(am => am.GetActionCategoriesAsync())
                .ReturnsAsync(new List<EventCategoryDTO>());
            _mapper.Setup(m => m.Map<List<EventCategoryDTO>, List<EventCategoryViewModel>>(It.IsAny<List<EventCategoryDTO>>())).Returns(new List<EventCategoryViewModel>());
            //Act  
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = await actionsController.GetAction() as ViewResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult.Model);
            var viewModel = actionResult.Model as List<EventCategoryViewModel>;
            Assert.NotNull(viewModel);
            Assert.Empty(viewModel);
        }

        [Fact]
        public async void GetEventsFailureTest()
        {
            //Arrange
            _actionManager.Setup(am => am.GetEventsAsync(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .ThrowsAsync(new Exception());
            _mapper.Setup(m => m.Map<List<GeneralEventDTO>, List<GeneralEventViewModel>>(It.IsAny<List<GeneralEventDTO>>())).Returns(new List<GeneralEventViewModel>());
            int id = 3;
            //Act  
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = await actionsController.Events(id);
            //Arrange
            Assert.NotNull(actionResult);
            var viewResult = Assert.IsType<RedirectToActionResult>(actionResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public async void GetEventsEmptyTest()
        {
            //Arrange
            _actionManager.Setup(am => am.GetEventsAsync(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new List<GeneralEventDTO>());
            _mapper.Setup(m => m.Map<List<GeneralEventDTO>, List<GeneralEventViewModel>>(It.IsAny<List<GeneralEventDTO>>())).Returns(new List<GeneralEventViewModel>());
            int id = 3;
            //Act  
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = await actionsController.Events(id) as ViewResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult.Model);
            var viewModel = actionResult.Model as List<GeneralEventViewModel>;
            Assert.NotNull(viewModel);
            Assert.Empty(viewModel);
        }

        [Fact]
        public async void FillEventGallerySuccessTest()
        {
            //Arrange
            int eventId = 2;
            _actionManager.Setup(x => x.FillEventGalleryAsync(It.IsAny<int>(), It.IsAny<IList<IFormFile>>()))
                .ReturnsAsync(StatusCodes.Status200OK);
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = await actionsController.FillEventGallery(eventId, FakeFiles());
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status200OK, codeResult?.StatusCode);
        }

        [Fact]
        public async void FillEventGalleryFailTest()
        {
            //Arrange
            int eventId = 2;
            _actionManager.Setup(x => x.FillEventGalleryAsync(It.IsAny<int>(), It.IsAny<IList<IFormFile>>()))
                .ThrowsAsync(new Exception());
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = await actionsController.FillEventGallery(eventId, FakeFiles());
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, codeResult?.StatusCode);
        }

        public EventViewModel GetEventViewModel()
        {
            var model = new EventViewModel()
            {
                Event = new EventInfoViewModel()
                {
                    EventId = 3,
                    EventName = "Event",
                    EventDateStart = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                    EventDateEnd = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                    EventParticipants = new List<EventParticipantViewModel>()
                    {
                        new EventParticipantViewModel(),
                        new EventParticipantViewModel(),
                        new EventParticipantViewModel()
                    }

                },
                IsEventFinished = true,
                IsUserApprovedParticipant = true,
                IsUserEventAdmin = true,
                IsUserParticipant = false,
                IsUserRejectedParticipant = false,
                IsUserUndeterminedParticipant = false
            };
            return model;
        }

        public static IList<IFormFile> FakeFiles()
        {
            var fileMock = new Mock<IFormFile>();
            Icon icon1 = new Icon(SystemIcons.Exclamation, 40, 40);
            var content = icon1.ToBitmap();
            var fileName = "picture.png";
            var ms = new MemoryStream();
            content.Save(ms, ImageFormat.Png);
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            var arr = new[] { fileMock.Object, fileMock.Object };
            return arr;
        }
    }
}
