using EPlast.BLL.DTO.Events;
using EPlast.BLL.DTO.EventUser;
using EPlast.BLL.Interfaces.Events;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using EPlast.WebApi.Controllers;
using EPlast.WebApi.Models.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.Tests.Controllers
{
    [TestFixture]
    public class EventsControllerTests
    {
        private Mock<IActionManager> _actionManager;

        private EventsController _eventsController;
        private Mock<UserManager<User>> _userManager;
        private Mock<IEventCategoryManager> _eventCategoryManager;
        private Mock<IEventStatusManager> _eventStatusManager;

        [SetUp]
        public void SetUp()
        {
            _actionManager = new Mock<IActionManager>();
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _eventCategoryManager = new Mock<IEventCategoryManager>();
            _eventStatusManager = new Mock<IEventStatusManager>();
            _eventsController = new EventsController(
                _actionManager.Object,
                _userManager.Object,
                _eventStatusManager.Object,
                _eventCategoryManager.Object);
        }

        [Test]
        public async Task GetTypes_ReturnsOkObjectResult()
        {
            //Arrange
            var expectedCount = 2;
            _actionManager
                .Setup((x) => x.GetEventTypesAsync())
                .ReturnsAsync(CreateListOfFakeEventTypes());

            // Act
            var result = await _eventsController.GetTypes();
            var types = (result as ObjectResult).Value as IEnumerable<EventTypeDTO>;
            var typesList = types as List<EventTypeDTO>;

            // Assert
            Assert.NotNull(types);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(expectedCount, typesList.Count);
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
            var listCount = 2;

            // Act
            var result = await _eventsController.GetCategories(It.IsAny<int>());
            var categoryList = (result as ObjectResult).Value as List<EventCategoryDTO>;

            // Assert
            Assert.NotNull((result as ObjectResult).Value);
            Assert.AreEqual(listCount, categoryList.Count);
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

            // Act
            var result = await _eventsController.GetCategories(It.IsAny<int>());
            var actual = (result as ObjectResult).Value as List<EventCategoryDTO>;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull((result as ObjectResult).Value);
            Assert.AreEqual(listCount, actual.Count);
        }

        [Test]
        public async Task CreateEventCategory_ReturnsCreatedResult()
        {
            // Arrange
            _eventCategoryManager
                .Setup((x) => x.CreateEventCategoryAsync(CreateFakeEventCategory()))
                .ReturnsAsync(It.IsAny<int>());
            // Act
            var result = await _eventsController.CreateEventCategory(CreateFakeEventCategory());
            var resultValue = (result as OkObjectResult).Value;

            // Assert
            Assert.NotNull(resultValue);
            Assert.IsInstanceOf<EventCategoryCreateDTO>(resultValue);
            Assert.IsInstanceOf<OkObjectResult>(result);

        }

        [Test]
        public async Task GetEvents_ReturnsOkObjectResult()
        {
            // Arrange
            _actionManager
                .Setup((x) => x.GetEventsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(CreateListOfFakeGeneralEvents());
            var expectedCount = 2;

            // Act
            var result = await _eventsController.GetEvents(It.IsAny<int>(), It.IsAny<int>());
            var resultObject = (result as ObjectResult).Value;
            var eventList = resultObject as List<GeneralEventDTO>;

            // Assert
            Assert.NotNull(resultObject);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(expectedCount, eventList.Count);

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
        public async Task GetSections_ReturnsOkObjectResult()
        {
            // Arrange
            _actionManager
                .Setup((x) => x.GetEventSectionsAsync())
                .ReturnsAsync(CreateListOfFakeEventSections());
            var expectedCount = 2;

            // Act
            var result = await _eventsController.GetSections();
            var resultObject = (result as OkObjectResult).Value;
            var eventList = resultObject as List<EventSectionDTO>;

            // Assert
            Assert.NotNull(resultObject);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(expectedCount, eventList.Count);

        }

        [Test]
        public async Task GetSections_ListWithTwoItems_ReturnsListWithTwoCategories()
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
        public async Task GetEventStatusId_ReturnsOkObjectResult()
        {
            // Arrange
            const int ID = 1;
            _eventStatusManager
                .Setup((x) => x.GetStatusIdAsync(It.IsAny<string>()))
                .ReturnsAsync(ID);

            // Act
            var result = await _eventsController.GetEventStatusId(It.IsAny<string>());

            // Assert
            Assert.NotNull((result as ObjectResult).Value);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task EstimateEvent_OkObjectResult_ReturnsOkObjectResult()
        {
            // Arrange
            _actionManager
                .Setup(x => x.EstimateEventAsync(It.IsAny<int>(), It.IsAny<User>(), It.IsAny<double>()))
                .ReturnsAsync(200);

            // Act
            var result = await _eventsController.EstimateEvent(It.IsAny<int>(), It.IsAny<double>());
            var resultValue = (result as OkObjectResult).Value;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(200, resultValue);
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
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>());
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
        public async Task SubscribeOnEvent_Status403Forbidden_Returns403Forbidden()
        {
            // Arrange
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>(){Roles.RegisteredUser});
           

            var expected = StatusCodes.Status403Forbidden;

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
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>());
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
        public async Task SubscribeOnEvent_Status403Forbidden_ReturnsStatus403Forbidden()
        {
            // Arrange
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.RegisteredUser });

            var expected = StatusCodes.Status403Forbidden;

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
            const int expectedCount = 2;
            _actionManager
                .Setup((x) => x.FillEventGalleryAsync(It.IsAny<int>(), It.IsAny<IList<IFormFile>>()))
                .ReturnsAsync(CreateListOfFakeEventGallery());

            // Act
            var result = await _eventsController.FillEventGallery(It.IsAny<int>(), It.IsAny<IList<IFormFile>>());
            var resultObject = (result as ObjectResult).Value as IList<EventGalleryDTO>;


            // Assert
            Assert.NotNull(result);
            Assert.NotNull(resultObject);
            Assert.AreEqual(expectedCount, resultObject.Count);
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
        public async Task GetPictures_ReturnsOkObjectResult_GetTwoPicture()
        {
            // Arrange
            _actionManager
                .Setup((x) => x.GetPicturesAsync(It.IsAny<int>()))
                .ReturnsAsync(CreateListOfFakeEventGallery());
            const int countPicture = 2;

            // Act
            var result = await _eventsController.GetPictures(It.IsAny<int>());
            var okResult = result as ObjectResult;
            var pictures = okResult.Value as IEnumerable<EventGalleryDTO>;
            var picturesAsList = pictures as IList<EventGalleryDTO>;



            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(okResult);
            Assert.NotNull(pictures);
            Assert.NotNull(picturesAsList);
            Assert.AreEqual(countPicture, picturesAsList.Count);
        }

        [Test]
        public async Task GetPictures_ListOfTwoItems_ReturnsListOfTwoItems()
        {
            // Arrange
            const int expectedCount = 2;
        
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
        public async Task GetEventsByCategoryAndStatus_ReturnOkObjectResult()
        {
            // Arrange
            const int  expectedCount = 2;
            _actionManager
                .Setup((x) => x.GetEventsByStatusAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(CreateListOfFakeGeneralEvents());

            // Act
            var result = await _eventsController.GetEventsByCategoryAndStatus(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());
            var okObject = result as ObjectResult;
            var category = okObject?.Value as IEnumerable<GeneralEventDTO>;
            var categoryList = category as List<GeneralEventDTO>;


            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(okObject);
            Assert.NotNull(category);
            Assert.NotNull(categoryList);
            Assert.AreEqual(expectedCount, categoryList.Count);

        }

        [Test]
        public async Task GetCategoriesByTypeAndPage_ReturnOkObjectResultTestAsync()
        {
            //Arrange
            var expectedCategories = 2;
            _actionManager
                .Setup(x => x.GetCategoriesByTypeIdAsync(It.IsAny<int>()))
                .ReturnsAsync(CreateListOfFakeEventCategories());
            
            //Act
            var result = await _eventsController.GetCategoriesByTypeAndPageAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());
            var categories = (result as ObjectResult).Value as EventsCategoryViewModel;
            
            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(categories);
            Assert.AreEqual(expectedCategories, categories.Total);
            Assert.IsInstanceOf<EventsCategoryViewModel>(categories);
        }

        [Test]
        public async Task GetCategoriesByTypeAndPage_FirstType_ReturnOkObjectResultTestAsync()
        {
            //Arrange
            var expectedCategories = 2;
            int typeId = 1;
            _actionManager
                .Setup(x => x.GetActionCategoriesAsync())
                .ReturnsAsync(CreateListOfFakeEventCategories());
            //Act
            var result = await _eventsController.GetCategoriesByTypeAndPageAsync(typeId, It.IsAny<int>(), It.IsAny<int>());
            var categories = (result as ObjectResult).Value as EventsCategoryViewModel;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(categories);
            Assert.AreEqual(expectedCategories, categories.Total);
            Assert.IsInstanceOf<EventsCategoryViewModel>(categories);
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

        private List<EventSectionDTO> CreateListOfFakeEventSections()
            => new List<EventSectionDTO>()
            {
                new EventSectionDTO()
                {
                    EventSectionId = 0,
                    EventSectionName = "SomeEventSectionName",

                },
                new EventSectionDTO()
                {
                    EventSectionId = 1,
                    EventSectionName = "AnotherEventSectionName",
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

        private EventCategoryCreateDTO CreateFakeEventCategory()
            => new EventCategoryCreateDTO()
            {
                EventCategory = new EventCategoryDTO()
                {
                    EventCategoryId = 1,
                    EventCategoryName = "new category",
                    EventSectionId = 2
                },
                EventTypeId = 3
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
