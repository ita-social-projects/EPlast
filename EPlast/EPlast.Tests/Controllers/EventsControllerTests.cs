﻿using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Events;
using EPlast.BLL.DTO.EventUser;
using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;
using EPlast.Resources;
using EPlast.WebApi.Controllers;
using EPlast.WebApi.Models.Events;
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
        private Mock<IEventCategoryManager> _eventCategoryManager;
        private Mock<IEventStatusManager> _eventStatusManager;
        private Mock<IEventUserAccessService> _eventUserAccessService;
        private Mock<IParticipantManager> _participantManager;

        [SetUp]
        public void SetUp()
        {
            _actionManager = new Mock<IActionManager>();
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _eventCategoryManager = new Mock<IEventCategoryManager>();
            _eventStatusManager = new Mock<IEventStatusManager>();
            _eventUserAccessService = new Mock<IEventUserAccessService>();
            _participantManager = new Mock<IParticipantManager>();
            _eventsController = new EventsController(
                _actionManager.Object,
                _userManager.Object,
                _eventStatusManager.Object,
                _eventCategoryManager.Object,
                _eventUserAccessService.Object,
                _participantManager.Object);
        }

        [Test]
        public async Task GetTypes_ReturnsOkObjectResult()
        {
            //Arrange
            var expectedListCount = 2;
            _actionManager
                .Setup((x) => x.GetEventTypesAsync())
                .ReturnsAsync(CreateListOfFakeEventTypes());

            // Act
            var result = await _eventsController.GetTypes();
            var types = (result as ObjectResult).Value as IEnumerable<EventTypeDto>;
            var typesList = types as List<EventTypeDto>;

            // Assert
            Assert.NotNull(types);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(expectedListCount, typesList.Count);
        }

        [Test]
        public async Task GetCategories_ReturnsOkObjectResult()
        {
            //Assert
            var expectedListCount = 2;
            _actionManager
                .Setup((x) => x.GetCategoriesByTypeIdAsync(It.IsAny<int>()))
                .ReturnsAsync(CreateListOfFakeEventCategories());

            // Act
            var result = await _eventsController.GetCategories(It.IsAny<int>());
            var categoryList = (result as ObjectResult).Value as List<EventCategoryDto>;

            // Assert
            Assert.NotNull((result as ObjectResult).Value);
            Assert.AreEqual(expectedListCount, categoryList.Count);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task CreateEventCategory_ReturnsOkObjectResult()
        {
            // Arrange
            int eventCategoryId = 1;

            _eventCategoryManager
                .Setup(x => x.CreateEventCategoryAsync(It.IsAny<EventCategoryCreateDto>()))
                .ReturnsAsync(eventCategoryId);

            // Act
            var result = await _eventsController.CreateEventCategory(CreateFakeEventCategory());
            var resultValue = (result as OkObjectResult).Value;

            // Assert
            Assert.NotNull(resultValue);
            Assert.IsInstanceOf<EventCategoryCreateDto>(resultValue);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task CreateEventCategory_ReturnsStatus400BadRequest()
        {
            // Arrange
            int expectedCode = StatusCodes.Status400BadRequest;
            int eventCategoryId = 0;

            _eventCategoryManager
                .Setup(x => x.CreateEventCategoryAsync(CreateFakeEventCategory()))
                .ReturnsAsync(eventCategoryId);

            // Act
            var result = await _eventsController.CreateEventCategory(CreateFakeEventCategory());
            var actualCode = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.AreEqual(expectedCode, actualCode);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task UpdateEventCategory_Status204NoContent_ReturnsStatus204NoContent()
        {
            // Arrange
            _eventCategoryManager
                .Setup(ecm => ecm.UpdateEventCategoryAsync(It.IsAny<EventCategoryDto>()))
                .ReturnsAsync(true);

            var expectedCode = StatusCodes.Status204NoContent;

            // Act
            var result = await _eventsController.UpdateEventCategory(It.IsAny<EventCategoryDto>());
            var actualCode = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.AreEqual(expectedCode, actualCode);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task UpdateEventCategory_Status400BadRequest_ReturnsStatus400BadRequest()
        {
            // Arrange
            _eventCategoryManager
                .Setup(ecm => ecm.UpdateEventCategoryAsync(It.IsAny<EventCategoryDto>()))
                .ReturnsAsync(false);

            var expectedCode = StatusCodes.Status400BadRequest;

            // Act
            var result = await _eventsController.UpdateEventCategory(It.IsAny<EventCategoryDto>());
            var actualCode = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.AreEqual(expectedCode, actualCode);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task DeleteEventCategory_Status204NoContent_ReturnsStatus204NoContent()
        {
            // Arrange
            _eventCategoryManager
                .Setup(ecm => ecm.DeleteEventCategoryAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            var expectedCode = StatusCodes.Status204NoContent;

            // Act
            var result = await _eventsController.DeleteEventCategory(It.IsAny<int>());
            var actualCode = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.AreEqual(expectedCode, actualCode);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteEventCategory_Status400BadRequest_ReturnsStatus400BadRequest()
        {
            // Arrange
            _eventCategoryManager
                .Setup(ecm => ecm.DeleteEventCategoryAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            var expectedCode = StatusCodes.Status400BadRequest;

            // Act
            var result = await _eventsController.DeleteEventCategory(It.IsAny<int>());
            var actualCode = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.AreEqual(expectedCode, actualCode);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }


        [Test]
        public async Task GetEvents_ReturnsOkObjectResult()
        {
            // Arrange
            var expectedListCount = 2;
            _actionManager
                .Setup((x) => x.GetEventsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(CreateListOfFakeGeneralEvents());

            // Act
            var result = await _eventsController.GetEvents(It.IsAny<int>(), It.IsAny<int>());
            var resultObject = (result as ObjectResult).Value;
            var eventList = resultObject as List<GeneralEventDto>;

            // Assert
            Assert.NotNull(resultObject);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(expectedListCount, eventList.Count);

        }

        [Test]
        public async Task GetSections_ReturnsOkObjectResult()
        {
            // Arrange
            var expectedListCount = 2;
            _actionManager
                .Setup((x) => x.GetEventSectionsAsync())
                .ReturnsAsync(CreateListOfFakeEventSections());

            // Act
            var result = await _eventsController.GetSections();
            var resultObject = (result as OkObjectResult).Value;
            var eventList = resultObject as List<EventSectionDto>;

            // Assert
            Assert.NotNull(resultObject);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(expectedListCount, eventList.Count);

        }

        [Test]
        public async Task GetEventDetail_EventExists_ReturnsOkObjectResult()
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
        public async Task GetEventDetail_EventDoesntExist_ReturnsNotFound()
        {
            // Arrange
            _actionManager
                .Setup((x) => x.GetEventInfoAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync((EventDto)null);

            // Act
            var result = await _eventsController.GetEventDetail(It.IsAny<int>());

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
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
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.RegisteredUser });

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
            const int expectedListCount = 2;
            _actionManager
                .Setup((x) => x.FillEventGalleryAsync(It.IsAny<int>(), It.IsAny<IList<IFormFile>>()))
                .ReturnsAsync(new List<int>() { 1, 2 });

            // Act
            var result = await _eventsController.FillEventGallery(It.IsAny<int>(), It.IsAny<IList<IFormFile>>());
            var resultObject = (result as ObjectResult).Value as List<int>;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(resultObject);
            Assert.AreEqual(expectedListCount, resultObject.Count);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetPictures_ReturnsOkObjectResult_GetTwoPicture()
        {
            // Arrange
            int expectedListCount = 2;
            _actionManager
                .Setup((x) => x.GetPicturesAsync(It.IsAny<int>()))
                .ReturnsAsync(CreateListOfFakeEventGallery());

            // Act
            var result = await _eventsController.GetPictures(It.IsAny<int>());
            var pictures = (result as ObjectResult).Value as IEnumerable<EventGalleryDto>;
            var picturesAsList = pictures as IList<EventGalleryDto>;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(pictures);
            Assert.NotNull(picturesAsList);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(expectedListCount, picturesAsList.Count);
        }
 
        [Test]
        public async Task GetEventsByCategoryAndStatus_ReturnOkObjectResult()
        {
            // Arrange
            const int expectedListCount = 2;
            _actionManager
                .Setup((x) => x.GetEventsByStatusAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(CreateListOfFakeGeneralEvents());

            // Act
            var result = await _eventsController.GetEventsByCategoryAndStatus(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());
            var okObject = result as ObjectResult;
            var category = okObject?.Value as IEnumerable<GeneralEventDto>;
            var categoryList = category as List<GeneralEventDto>;

            Assert.NotNull(okObject);
            Assert.NotNull(category);
            Assert.NotNull(categoryList);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(expectedListCount, categoryList.Count);
        }

        [Test]
        public async Task GetCategoriesById_CategoryExists_ReturnsOkObjectResult()
        {
            //Arrange
            int categoryId = 1;
            _actionManager
                .Setup(x => x.GetCategoryByIdAsync(categoryId)).ReturnsAsync(new EventCategoryDto());

            //Act
            var result = await _eventsController.GetCategoryById(categoryId);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull((result as OkObjectResult).Value);
        }

        [Test]
        public async Task GetCategoriesById_CategoryDoesntExist_ReturnsNotFound()
        {
            //Arrange
            int categoryId = 1;
            _actionManager
                .Setup(x => x.GetCategoryByIdAsync(categoryId)).ReturnsAsync((EventCategoryDto)null);

            //Act
            var result = await _eventsController.GetCategoryById(categoryId);

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetCategoriesByTypeAndPage_ReturnOkObjectResultTestAsync()
        {
            //Arrange
            var expectedListCount = 2;
            _actionManager
                .Setup(x => x.GetCategoriesByTypeIdAsync(It.IsAny<int>()))
                .ReturnsAsync(CreateListOfFakeEventCategories());

            //Act
            var result = await _eventsController.GetCategoriesByTypeAndPageAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());
            var categories = (result as ObjectResult).Value as EventsCategoryViewModel;

            //Assert
            Assert.NotNull(categories);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<EventsCategoryViewModel>(categories);
            Assert.AreEqual(expectedListCount, categories.Total);
        }

        [Test]
        public async Task GetCategoriesByTypeAndPage_FirstType_ReturnOkObjectResultTestAsync()
        {
            //Arrange
            var expectedListCount = 2;
            int typeId = 1;
            _actionManager
                .Setup(x => x.GetActionCategoriesAsync())
                .ReturnsAsync(CreateListOfFakeEventCategories());

            //Act
            var result = await _eventsController.GetCategoriesByTypeAndPageAsync(typeId, It.IsAny<int>(), It.IsAny<int>());
            var categories = (result as ObjectResult).Value as EventsCategoryViewModel;

            //Assert
            Assert.NotNull(categories);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<EventsCategoryViewModel>(categories);
            Assert.AreEqual(expectedListCount, categories.Total);
        }

        [Test]
        public async Task LeaveFeedback_FeedbackIsPosted_ReturnsOk()
        {
            _actionManager
                .Setup(x => x.GetEventAsync(It.IsAny<int>()))
                .ReturnsAsync(new Event());
            _userManager
                .Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());
            _participantManager
                .Setup(x => x.GetParticipantByEventIdAndUserIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(new Participant());
            _eventUserAccessService
                .Setup(x => x.CanPostFeedback(It.IsAny<Participant>(), It.IsAny<int>()))
                .ReturnsAsync(false);
            _actionManager
                .Setup(x => x.LeaveFeedbackAsync(It.IsAny<EventFeedbackDto>(), It.IsAny<Participant>()))
                .Returns(Task.CompletedTask);
            _actionManager
                .Setup(x => x.LeaveFeedbackAsync(It.IsAny<EventFeedbackDto>(), It.IsAny<Participant>()))
                .Returns(Task.CompletedTask);

            //Act
            var result = await _eventsController.LeaveFeedback(It.IsAny<int>(), It.IsAny<EventFeedbackDto>());

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<StatusCodeResult>(result);
        }

        [Test]
        public async Task LeaveFeedback_EventNotFound_ReturnsNotFound()
        {
            //Arrange
            int eventId = 1;

            _actionManager
                .Setup(x => x.GetEventAsync(eventId))
                .ReturnsAsync((Event)null);

            //Act
            var result = await _eventsController.LeaveFeedback(eventId, It.IsAny<EventFeedbackDto>());

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task LeaveFeedback_PostFeedbackAccessCheckFails_ReturnsForbidden()
        {
            //Arrange
            int eventId = 1;
            int expectedStatus = StatusCodes.Status403Forbidden;

            _actionManager
                .Setup(x => x.GetEventAsync(eventId))
                .ReturnsAsync(new Event());
            _userManager
                .Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());
            _participantManager
                .Setup(x => x.GetParticipantByEventIdAndUserIdAsync(eventId, It.IsAny<string>()))
                .ReturnsAsync(new Participant());
            _eventUserAccessService
                .Setup(x => x.CanPostFeedback(It.IsAny<Participant>(), eventId))
                .ReturnsAsync(false);
            _actionManager
                .Setup(x => x.LeaveFeedbackAsync(It.IsAny<EventFeedbackDto>(), It.IsAny<Participant>()))
                .Returns(Task.CompletedTask);

            //Act
            var result = await _eventsController.LeaveFeedback(eventId, It.IsAny<EventFeedbackDto>());
            var actualStatus = (result as StatusCodeResult).StatusCode;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<StatusCodeResult>(result);
            Assert.AreEqual(expectedStatus, actualStatus);
        }

        [Test]
        public async Task DeleteFeedback_ParticipantCheckFails_ReturnsForbidden()
        {
            //Arrange
            int eventId = 1;
            int feedbackId = 1;
            int expectedStatus = StatusCodes.Status403Forbidden;

            _actionManager
                .Setup(x => x.GetEventAsync(eventId))
                .ReturnsAsync(new Event());
            _participantManager
                .Setup(x => x.GetEventFeedbackByIdAsync(feedbackId))
                .ReturnsAsync(new EventFeedback());
            _userManager
                .Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());
            _eventUserAccessService
                .Setup(x => x.CanDeleteFeedback(It.IsAny<User>(), It.IsAny<EventFeedback>()))
                .ReturnsAsync(false);

            //Act
            var result = await _eventsController.DeleteFeedback(eventId, feedbackId);
            var actualStatus = (result as StatusCodeResult).StatusCode;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedStatus, actualStatus);
        }

        [Test]
        public async Task DeleteFeedback_EventOrFeedbackNotFound_ReturnsNotFound()
        {
            //Arrange
            int eventId = 1;
            int feedbackId = 0;

            _actionManager
                .Setup(x => x.GetEventAsync(eventId))
                .ReturnsAsync(new Event());
            _participantManager
                .Setup(x => x.GetEventFeedbackByIdAsync(feedbackId))
                .ReturnsAsync((EventFeedback)null);

            //Act
            var result = await _eventsController.DeleteFeedback(eventId, feedbackId);

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task DeleteFeedback_AllChecksPassed_ReturnsNoContent()
        {
            //Arrange
            int eventId = 1;
            int feedbackId = 1;

            _actionManager
                .Setup(x => x.GetEventAsync(eventId))
                .ReturnsAsync(new Event());
            _participantManager
                .Setup(x => x.GetEventFeedbackByIdAsync(feedbackId))
                .ReturnsAsync((new EventFeedback()));
            _userManager
                .Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());
            _eventUserAccessService
                .Setup(x => x.CanDeleteFeedback(It.IsAny<User>(), It.IsAny<EventFeedback>()))
                .ReturnsAsync(false);
            _actionManager
                .Setup(x => x.DeleteFeedbackAsync(feedbackId))
                .Returns(Task.CompletedTask);

            //Act
            var result = await _eventsController.DeleteFeedback(eventId, feedbackId);

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<StatusCodeResult>(result);
        }

        [Test]
        public async Task GetPicture_PictureExists_ReturnsOkObject()
        {
            //Arrange
            int pictureId = 1;

            _actionManager
                .Setup(x => x.GetPictureAsync(pictureId))
                .ReturnsAsync(new EventGalleryDto());

            //Act
            var result = await _eventsController.GetPicture(pictureId);

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull((result as OkObjectResult).Value);
        }

        [Test]
        public async Task GetPicture_PictureDoesntExist_ReturnsNotFound()
        {
            //Arrange
            int pictureId = 1;

            _actionManager
                .Setup(x => x.GetPictureAsync(pictureId))
                .ReturnsAsync((EventGalleryDto)null);

            //Act
            var result = await _eventsController.GetPicture(pictureId);

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        private List<EventTypeDto> CreateListOfFakeEventTypes()
        => new List<EventTypeDto>()
        {
            new EventTypeDto()
            {
                ID = 0,
                EventTypeName = "SomeEventTypeName",
            },
            new EventTypeDto()
            {
                ID = 1,
                EventTypeName = "AnotherEventTypeName",
            },
        };

        private List<EventCategoryDto> CreateListOfFakeEventCategories()
            => new List<EventCategoryDto>()
            {
                new EventCategoryDto()
                {
                    EventCategoryId = 0,
                    EventCategoryName = "SomeEventCategoryName",
                },
                new EventCategoryDto()
                {
                    EventCategoryId = 1,
                    EventCategoryName = "AnotherEventCategoryName",
                },
            };

        private List<GeneralEventDto> CreateListOfFakeGeneralEvents()
            => new List<GeneralEventDto>()
            {
                new GeneralEventDto()
                {
                    EventId = 0,
                    EventName = "SomeGeneralEventName",

                },
                new GeneralEventDto()
                {
                    EventId = 1,
                    EventName = "AnotherGeneralEventName",
                },
            };

        private List<EventSectionDto> CreateListOfFakeEventSections()
            => new List<EventSectionDto>()
            {
                new EventSectionDto()
                {
                    EventSectionId = 0,
                    EventSectionName = "SomeEventSectionName",

                },
                new EventSectionDto()
                {
                    EventSectionId = 1,
                    EventSectionName = "AnotherEventSectionName",
                },
            };

        private EventDto CreateFakeEvent()
            => new EventDto()
            {
                Event = new EventInfoDto()
                {
                    EventId = 0,
                    EventName = "SomeEventName",
                },

            };

        private EventCategoryCreateDto CreateFakeEventCategory()
            => new EventCategoryCreateDto()
            {
                EventCategory = new EventCategoryDto()
                {
                    EventCategoryId = 1,
                    EventCategoryName = "new category",
                    EventSectionId = 2
                },
                EventTypeId = 3
            };

        private List<EventGalleryDto> CreateListOfFakeEventGallery()
            => new List<EventGalleryDto>()
            {
                new EventGalleryDto()
                {
                    GalleryId = 1,
                    EncodedData = "SomeFilenameID1"
                },
                new EventGalleryDto()
                {
                    GalleryId = 2,
                    EncodedData = "SomeFilenameID2"
                },

            };

    }
}
