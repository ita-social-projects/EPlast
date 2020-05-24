using AutoMapper;
using EPlast.BussinessLayer.DTO.Events;
using EPlast.BussinessLayer.Interfaces.Events;
using EPlast.Controllers;
using EPlast.ViewModels.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using Xunit;

namespace EPlast.XUnitTest
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
        public void DeletePictureSuccessTest()
        {
            //Arrange
            int testGalleryId = 2;
            _actionManager.Setup(x => x.DeletePicture(It.IsAny<int>()))
                .Returns(StatusCodes.Status200OK);
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = actionsController.DeletePicture(testGalleryId);
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status200OK, codeResult?.StatusCode);
        }

        [Fact]
        public void DeletePictureFailTest()
        {
            //Arrange
            int testGalleryId = 2;
            _actionManager.Setup(x => x.DeletePicture(It.IsAny<int>()))
                .Throws(new Exception());
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = actionsController.DeletePicture(testGalleryId);
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, codeResult?.StatusCode);
        }

        [Fact]
        public void DeleteEventSuccessTest()
        {
            //Arrange
            int eventId = 2;
            _actionManager.Setup(x => x.DeleteEvent(It.IsAny<int>()))
                .Returns(StatusCodes.Status200OK);
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = actionsController.DeleteEvent(eventId);
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status200OK, codeResult?.StatusCode);
        }

        [Fact]
        public void DeleteEventFailTest()
        {
            //Arrange
            int eventId = 2;
            _actionManager.Setup(x => x.DeleteEvent(It.IsAny<int>()))
                .Throws(new Exception());
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = actionsController.DeleteEvent(eventId);
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, codeResult?.StatusCode);
        }


        [Fact]
        public void UnsubscribeOnEventSuccessTest()
        {
            //Arrange
            int eventId = 2;
            _actionManager.Setup(x => x.UnSubscribeOnEvent(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .Returns(StatusCodes.Status200OK);
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = actionsController.UnSubscribeOnEvent(eventId);
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status200OK, codeResult?.StatusCode);
        }

        [Fact]
        public void UnsubscribeOnEventFailTest()
        {
            //Arrange
            int eventId = 2;
            _actionManager.Setup(x => x.UnSubscribeOnEvent(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .Throws(new Exception());
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = actionsController.UnSubscribeOnEvent(eventId);
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, codeResult?.StatusCode);
        }

        [Fact]
        public void SubscribeOnEventSuccessTest()
        {
            //Arrange
            int eventId = 2;
            _actionManager.Setup(x => x.SubscribeOnEvent(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .Returns(StatusCodes.Status200OK);
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = actionsController.SubscribeOnEvent(eventId);
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status200OK, codeResult?.StatusCode);
        }

        [Fact]
        public void SubscribeOnEventFailTest()
        {
            //Arrange
            int eventId = 2;
            _actionManager.Setup(x => x.SubscribeOnEvent(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .Throws(new Exception());
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = actionsController.SubscribeOnEvent(eventId);
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, codeResult?.StatusCode);
        }

        [Fact]
        public void ApproveParticipantSuccessTest()
        {
            //Arrange
            int participantId = 2;
            _actionManager.Setup(x => x.ApproveParticipant(It.IsAny<int>()))
                .Returns(StatusCodes.Status200OK);
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = actionsController.ApproveParticipant(participantId);
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status200OK, codeResult?.StatusCode);
        }

        [Fact]
        public void UndetermineParticipantSuccessTest()
        {
            //Arrange
            int participantId = 2;
            _actionManager.Setup(x => x.UnderReviewParticipant(It.IsAny<int>()))
                .Returns(StatusCodes.Status200OK);
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = actionsController.UndetermineParticipant(participantId);
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status200OK, codeResult?.StatusCode);
        }

        [Fact]
        public void RejectParticipantSuccessTest()
        {
            //Arrange
            int participantId = 2;
            _actionManager.Setup(x => x.RejectParticipant(It.IsAny<int>()))
                .Returns(StatusCodes.Status200OK);
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = actionsController.RejectParticipant(participantId);
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status200OK, codeResult?.StatusCode);
        }

        [Fact]
        public void ApproveParticipantFailTest()
        {
            //Arrange
            int participantId = 2;
            _actionManager.Setup(x => x.ApproveParticipant(It.IsAny<int>()))
                .Throws(new Exception());
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = actionsController.ApproveParticipant(participantId);
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, codeResult?.StatusCode);
        }

        [Fact]
        public void UndetermineParticipantFailTest()
        {
            //Arrange
            int participantId = 2;
            _actionManager.Setup(x => x.UnderReviewParticipant(It.IsAny<int>()))
                .Throws(new Exception());
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = actionsController.UndetermineParticipant(participantId);
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, codeResult?.StatusCode);
        }

        [Fact]
        public void RejectParticipantFailTest()
        {
            //Arrange
            int participantId = 2;
            _actionManager.Setup(x => x.RejectParticipant(It.IsAny<int>()))
                .Throws(new Exception());
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = actionsController.RejectParticipant(participantId);
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, codeResult?.StatusCode);
        }

        [Fact]
        public void EventInfoFailureTest()
        {
            //Arrange
            _actionManager.Setup(am => am.GetEventInfo(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .Throws(new Exception());
            _mapper.Setup(m => m.Map<EventDTO, EventViewModel>(It.IsAny<EventDTO>())).Returns(new EventViewModel());
            int eventID = 3;
            //Act  
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = actionsController.EventInfo(eventID);
            //Arrange
            Assert.NotNull(actionResult);
            var viewResult = Assert.IsType<RedirectToActionResult>(actionResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public void EventInfoSuccessTest()
        {
            //Arrange
            _actionManager.Setup(am => am.GetEventInfo(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .Returns(new EventDTO());
            _mapper.Setup(m => m.Map<EventDTO, EventViewModel>(It.IsAny<EventDTO>())).Returns(GetEventVM());
            int eventID = 3;
            //Act  
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = actionsController.EventInfo(eventID);
            //Arrange
            Assert.NotNull(actionResult);
            var viewResult = Assert.IsType<ViewResult>(actionResult);
            var viewModel = Assert.IsType<EventViewModel>(viewResult.Model);
            Assert.Equal(3, viewModel.Event.EventParticipants.Count());
        }

        [Fact]
        public void GetActionsFailureTest()
        {
            //Arrange
            _actionManager.Setup(am => am.GetActionCategories())
                .Throws(new Exception());
            _mapper.Setup(m => m.Map<List<EventCategoryDTO>, List<EventCategoryViewModel>>(It.IsAny<List<EventCategoryDTO>>())).Returns(new List<EventCategoryViewModel>());
            //Act  
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = actionsController.GetAction();
            //Arrange
            Assert.NotNull(actionResult);
            var viewResult = Assert.IsType<RedirectToActionResult>(actionResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public void GetActionEmptyTest()
        {
            //Arrange
            _actionManager.Setup(am => am.GetActionCategories())
                .Returns(new List<EventCategoryDTO>());
            _mapper.Setup(m => m.Map<List<EventCategoryDTO>, List<EventCategoryViewModel>>(It.IsAny<List<EventCategoryDTO>>())).Returns(new List<EventCategoryViewModel>());
            //Act  
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = actionsController.GetAction() as ViewResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult.Model);
            var viewModel = actionResult.Model as List<EventCategoryViewModel>;
            Assert.NotNull(viewModel);
            Assert.Empty(viewModel);
        }

        [Fact]
        public void GetEventsFailureTest()
        {
            //Arrange
            _actionManager.Setup(am => am.GetEvents(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .Throws(new Exception());
            _mapper.Setup(m => m.Map<List<GeneralEventDTO>, List<GeneralEventViewModel>>(It.IsAny<List<GeneralEventDTO>>())).Returns(new List<GeneralEventViewModel>());
            int id = 3;
            //Act  
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = actionsController.Events(id);
            //Arrange
            Assert.NotNull(actionResult);
            var viewResult = Assert.IsType<RedirectToActionResult>(actionResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public void GetEventsEmptyTest()
        {
            //Arrange
            _actionManager.Setup(am => am.GetEvents(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .Returns(new List<GeneralEventDTO>());
            _mapper.Setup(m => m.Map<List<GeneralEventDTO>, List<GeneralEventViewModel>>(It.IsAny<List<GeneralEventDTO>>())).Returns(new List<GeneralEventViewModel>());
            int id = 3;
            //Act  
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = actionsController.Events(id) as ViewResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult.Model);
            var viewModel = actionResult.Model as List<GeneralEventViewModel>;
            Assert.NotNull(viewModel);
            Assert.Empty(viewModel);
        }

        [Fact]
        public void FillEventGallerySuccessTest()
        {
            //Arrange
            int eventId = 2;
            _actionManager.Setup(x => x.FillEventGallery(It.IsAny<int>(), It.IsAny<IList<IFormFile>>()))
                .Returns(StatusCodes.Status200OK);
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = actionsController.FillEventGallery(eventId, FakeFiles());
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status200OK, codeResult?.StatusCode);
        }

        [Fact]
        public void FillEventGalleryFailTest()
        {
            //Arrange
            int eventId = 2;
            _actionManager.Setup(x => x.FillEventGallery(It.IsAny<int>(), It.IsAny<IList<IFormFile>>()))
                .Throws(new Exception());
            //Act
            var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
            var actionResult = actionsController.FillEventGallery(eventId, FakeFiles());
            var codeResult = actionResult as StatusCodeResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, codeResult?.StatusCode);
        }

        public EventViewModel GetEventVM()
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
            var arr = new IFormFile[] { fileMock.Object, fileMock.Object };
            return arr;
        }
    }
}
