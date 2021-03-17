using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Events;
using EPlast.BLL.DTO.EventUser;
using EPlast.BLL.Interfaces.Events;
using EPlast.DataAccess.Entities;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Controllers
{
    [TestFixture]
    public class EventsControllerTests
    {
        private Mock<IActionManager> _actionManager;

        private EventsController _eventsController;
        private Mock<UserManager<User>> _userManager;

        [SetUp]
        public void SetUp()
        {
            _actionManager = new Mock<IActionManager>();
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            _eventsController = new EventsController(
                _actionManager.Object,
                _userManager.Object);
        }

        [Test]
        public async Task GetTypes_ReturnsOkObjectResult()
        {
            //Arrange
            _actionManager
                .Setup((x) => x.GetEventTypesAsync())
                .ReturnsAsync(CreateListOfFakeEventTypes());

            // Act
            var result = await _eventsController.GetTypes();

            // Assert
            Assert.NotNull((result as ObjectResult).Value);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetTypes_ListWithTwoItems_ReturnsListWithTwoItems()
        {
            // Arrange
            var listCount = 2;

            _actionManager
                .Setup((x) => x.GetEventTypesAsync())
                .ReturnsAsync(CreateListOfFakeEventTypes());

            var expected = listCount;

            // Act
            var result = await _eventsController.GetTypes();

            var actual = (result as ObjectResult).Value as List<EventTypeDTO>;

            // Assert
            Assert.NotNull((result as ObjectResult).Value);
            Assert.AreEqual(expected, actual.Count);
        }

        [Test]
        public async Task GetCategories_ReturnsOkObjectResult()
        {
            //Assert
            _actionManager
                .Setup((x) => x.GetCategoriesByTypeIdAsync(It.IsAny<int>()))
                .ReturnsAsync(CreateListOfFakeEventCategories());

            // Act
            var result = await _eventsController.GetCategories(It.IsAny<int>());

            // Assert
            Assert.NotNull((result as ObjectResult).Value);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetCategories_ListWithTwoItems_ReturnsListWithItems()
        {
            // Arrange
            var listCount = 2;

            _actionManager
                .Setup((x) => x.GetCategoriesByTypeIdAsync(It.IsAny<int>()))
                .ReturnsAsync(CreateListOfFakeEventCategories());

            var expected = listCount;

            // Act
            var result = await _eventsController.GetCategories(It.IsAny<int>());

            var actual = (result as ObjectResult).Value as List<EventCategoryDTO>;

            // Assert
            Assert.NotNull((result as ObjectResult).Value);
            Assert.AreEqual(expected, actual.Count);
        }

        [Test]
        public async Task GetEvents_ReturnsOkObjectResult()
        {
            // Arrange
            _actionManager
                .Setup((x) => x.GetEventsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(CreateListOfFakeGeneralEvents());

            // Act
            var result = await _eventsController.GetEvents(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.NotNull((result as ObjectResult).Value);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetEvents_ListWithTwoItems_ReturnsListWithTwoCategories()
        {
            // Arrange
            var listCount = 2;

            _actionManager
                .Setup((x) => x.GetEventsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(CreateListOfFakeGeneralEvents());

            var expected = listCount;

            // Act
            var result = await _eventsController.GetEvents(It.IsAny<int>(), It.IsAny<int>());

            var actual = (result as ObjectResult).Value as List<GeneralEventDTO>;

            // Assert
            Assert.NotNull((result as ObjectResult).Value);
            Assert.AreEqual(expected, actual.Count);
        }

        [Test]
        public async Task GetEventDetail_ReturnsOkObjectResult()
        {
            // Arrange
            _actionManager
                .Setup((x) => x.GetEventInfoAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(CreateFakeEvent());

            // Act
            var result = await _eventsController.GetEventDetail(It.IsAny<int>());

            // Assert
            Assert.NotNull((result as ObjectResult).Value);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetEventDetail_EventDTO_ReturnsNotNullEventDTO()
        {
            // Arrange
            _actionManager
                .Setup((x) => x.GetEventInfoAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(CreateFakeEvent());

            // Act
            var result = await _eventsController.GetEventDetail(It.IsAny<int>());

            var actual = (result as ObjectResult).Value as EventDTO;

            // Assert
            Assert.IsNotNull(actual);
        }

        [Test]
        public async Task Delete_Status200OK_ReturnsStatus200OK()
        {
            // Arrange
            _actionManager
                .Setup((x) => x.DeleteEventAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status200OK);

            var expected = StatusCodes.Status200OK;

            // Act
            var result = await _eventsController.Delete(It.IsAny<int>());

            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task Delete_Status400BadRequest_ReturnsStatus400BadRequest()
        {
            // Arrange
            _actionManager
                .Setup((x) => x.DeleteEventAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status400BadRequest);

            var expected = StatusCodes.Status400BadRequest;

            // Act
            var result = await _eventsController.Delete(It.IsAny<int>());

            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task DeletePicture_Status200OK_ReturnsStatus200OK()
        {
            // Arrange
            _actionManager
                .Setup((x) => x.DeletePictureAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status200OK);

            var expected = StatusCodes.Status200OK;

            // Act
            var result = await _eventsController.DeletePicture(It.IsAny<int>());

            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task DeletePicture_Status400BadRequest_ReturnsStatus400BadRequest()
        {
            // Arrange
            _actionManager
                .Setup((x) => x.DeletePictureAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status400BadRequest);

            var expected = StatusCodes.Status400BadRequest;

            // Act
            var result = await _eventsController.DeletePicture(It.IsAny<int>());

            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task SubscribeOnEvent_Status200OK_ReturnsStatus200OK()
        {
            // Arrange
            _actionManager
                .Setup((x) => x.SubscribeOnEventAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(StatusCodes.Status200OK);

            var expected = StatusCodes.Status200OK;

            // Act
            var result = await _eventsController.SubscribeOnEvent(It.IsAny<int>());

            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task SubscribeOnEvent_Status400BadRequest_ReturnsStatus400BadRequest()
        {
            // Arrange
            _actionManager
                .Setup((x) => x.SubscribeOnEventAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(StatusCodes.Status400BadRequest);

            var expected = StatusCodes.Status400BadRequest;

            // Act
            var result = await _eventsController.SubscribeOnEvent(It.IsAny<int>());

            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task UnSubscribeOnEvent_Status200OK_ReturnsStatus200OK()
        {
            // Arrange
            _actionManager
                .Setup((x) => x.UnSubscribeOnEventAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(StatusCodes.Status200OK);

            var expected = StatusCodes.Status200OK;

            // Act
            var result = await _eventsController.UnSubscribeOnEvent(It.IsAny<int>());

            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task UnSubscribeOnEvent_Status400BadRequest_ReturnsStatus400BadRequest()
        {
            // Arrange
            _actionManager
                .Setup((x) => x.UnSubscribeOnEventAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(StatusCodes.Status400BadRequest);

            var expected = StatusCodes.Status400BadRequest;

            // Act
            var result = await _eventsController.UnSubscribeOnEvent(It.IsAny<int>());

            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task ApproveParticipant_Status200OK_ReturnsStatus200OK()
        {
            // Arrange
            _actionManager
                .Setup((x) => x.ApproveParticipantAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status200OK);

            var expected = StatusCodes.Status200OK;

            // Act
            var result = await _eventsController.ApproveParticipant(It.IsAny<int>());

            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task ApproveParticipant_Status400BadRequest_ReturnsStatus400BadRequest()
        {
            // Arrange
            _actionManager
                .Setup((x) => x.ApproveParticipantAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status400BadRequest);

            var expected = StatusCodes.Status400BadRequest;

            // Act
            var result = await _eventsController.ApproveParticipant(It.IsAny<int>());

            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task UnderReviewParticipant_Status200OK_ReturnsStatus200OK()
        {
            // Arrange
            _actionManager
                .Setup((x) => x.UnderReviewParticipantAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status200OK);

            var expected = StatusCodes.Status200OK;

            // Act
            var result = await _eventsController.UnderReviewParticipant(It.IsAny<int>());

            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task UnderReviewParticipant_Status400BadRequest_ReturnsStatus400BadRequest()
        {
            // Arrange
            _actionManager
                .Setup((x) => x.UnderReviewParticipantAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status400BadRequest);

            var expected = StatusCodes.Status400BadRequest;

            // Act
            var result = await _eventsController.UnderReviewParticipant(It.IsAny<int>());

            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task RejectParticipant_ReturnsStatus200OK_ReturnsStatus200OK()
        {
            // Arrange
            _actionManager
                .Setup((x) => x.RejectParticipantAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status200OK);

            var expected = StatusCodes.Status200OK;

            // Act
            var result = await _eventsController.RejectParticipant(It.IsAny<int>());

            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task RejectParticipant_Status400BadRequest_ReturnsStatus400BadRequest()
        {
            // Arrange
            _actionManager
                .Setup((x) => x.RejectParticipantAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status400BadRequest);

            var expected = StatusCodes.Status400BadRequest;

            // Act
            var result = await _eventsController.RejectParticipant(It.IsAny<int>());

            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task FillEventGallery_ReturnsOkObjectResult()
        {
            // Arrange
            _actionManager
                .Setup((x) => x.FillEventGalleryAsync(It.IsAny<int>(), It.IsAny<IList<IFormFile>>()))
                .ReturnsAsync(CreateListOfFakeEventGallery());

            // Act
            var result = await _eventsController.FillEventGallery(It.IsAny<int>(), It.IsAny<IList<IFormFile>>());

            // Assert
            Assert.NotNull((result as ObjectResult).Value);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task FillEventGallery_ListOfTwoItems_ReturnsListOfTwoItems()
        {
            // Arrange
            var expectedCount = 2;

            _actionManager
                .Setup((x) => x.FillEventGalleryAsync(It.IsAny<int>(), It.IsAny<IList<IFormFile>>()))
                .ReturnsAsync(CreateListOfFakeEventGallery());

            // Act
            var result = await _eventsController.FillEventGallery(It.IsAny<int>(), It.IsAny<IList<IFormFile>>());

            var actual = ((result as ObjectResult).Value as List<EventGalleryDTO>).Count;

            // Assert
            Assert.AreEqual(expectedCount, actual);
        }

        [Test]
        public async Task GetPictures_ReturnsOkObjectResult()
        {
            // Arrange
            _actionManager
                .Setup((x) => x.GetPicturesAsync(It.IsAny<int>()))
                .ReturnsAsync(CreateListOfFakeEventGallery());

            // Act
            var result = await _eventsController.GetPictures(It.IsAny<int>());

            // Assert
            Assert.NotNull((result as ObjectResult).Value);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetPictures_ListOfTwoItems_ReturnsListOfTwoItems()
        {
            // Arrange
            var expectedCount = 2;

            _actionManager
                .Setup((x) => x.GetPicturesAsync(It.IsAny<int>()))
                .ReturnsAsync(CreateListOfFakeEventGallery());

            // Act
            var result = await _eventsController.GetPictures(It.IsAny<int>());

            var actual = ((result as ObjectResult).Value as List<EventGalleryDTO>).Count;

            // Assert
            Assert.AreEqual(expectedCount, actual);
        }
        [Test]
        public async Task GetEventsByCategory_ReturnOkObjectResult()
        {
            // Arrange
            _actionManager
                .Setup((x) => x.GetEventsByStatusAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(CreateListOfFakeGeneralEvents());

            // Act
            var result = await _eventsController.GetEventsByCategory(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.NotNull((result as ObjectResult).Value);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
        [Test]
        public async Task GetCategoriesByPage_ReturnOkObjectResult()
        {
            //Arrange
            _actionManager
                .Setup(x => x.GetActionCategoriesAsync());
                
            //Act
            var result = await _eventsController.GetCategoriesByPage(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>());

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        private List<EventTypeDTO> CreateListOfFakeEventTypes()
            => new List<EventTypeDTO>()
            { 
                new EventTypeDTO()
                { 
                    ID = 0,
                    EventTypeName = "SomeEventTypeName",
                },
                new EventTypeDTO()
                { 
                    ID = 1,
                    EventTypeName = "AnotherEventTypeName",
                },
            };

        private List<EventCategoryDTO> CreateListOfFakeEventCategories()
            => new List<EventCategoryDTO>()
            {
                new EventCategoryDTO()
                { 
                    EventCategoryId = 0,
                    EventCategoryName = "SomeEventCategoryName",
                },
                new EventCategoryDTO()
                { 
                    EventCategoryId = 1,
                    EventCategoryName = "AnotherEventCategoryName",
                },
            };

        private List<GeneralEventDTO> CreateListOfFakeGeneralEvents()
            => new List<GeneralEventDTO>()
            {
                new GeneralEventDTO()
                { 
                    EventId = 0, 
                    EventName = "SomeGeneralEventName",
                    
                },
                new GeneralEventDTO()
                { 
                    EventId = 1, 
                    EventName = "AnotherGeneralEventName",
                },
            };

        private EventDTO CreateFakeEvent()
            => new EventDTO()
            {
                Event = new EventInfoDTO()
                { 
                    EventId = 0, 
                    EventName = "SomeEventName",
                },
            };

        private List<EventGalleryDTO> CreateListOfFakeEventGallery()
            => new List<EventGalleryDTO>()
            {
                new EventGalleryDTO()
                {
                    GalleryId = 1,
                    FileName = "SomeFilenameID1"
                },
                new EventGalleryDTO()
                {
                    GalleryId = 2,
                    FileName = "SomeFilenameID2"
                },

            };

    }
}