using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Events;
using EPlast.BLL.DTO.EventUser;
using EPlast.BLL.Interfaces.Events;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Controllers
{
    [TestFixture]
    public class EventsControllerTests
    {
        private Mock<IActionManager> _actionManager;

        private EventsController _sut;

        [SetUp]
        public void SetUp()
        {
            _actionManager = new Mock<IActionManager>();

            _sut = new EventsController(
                _actionManager.Object);
        }

        [Test]
        public async Task GetTypes_ReturnsOkObjectResult()
        {
            // Arrange

            // Act

            var result = await _sut.GetTypes();

            // Assert

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetTypes_ActionManagerGetEventTypesAsyncReturnsListWithTwoItems_ReturnsListWithTwoEventTypes()
        {
            // Arrange

            var eventTypes = new List<EventTypeDTO>() { new EventTypeDTO(), new EventTypeDTO(), };

            _actionManager
                .Setup((x) => x.GetEventTypesAsync())
                .ReturnsAsync(eventTypes);

            var expected = 2;

            // Act

            var result = await _sut.GetTypes();

            var actual = (result as ObjectResult).Value as List<EventTypeDTO>;

            // Assert

            Assert.AreEqual(expected, actual.Count);
        }

        [Test]
        public async Task GetTypes_ActionManagerGetEventTypesAsyncThrowsException_ReturnsBadRequestResult()
        {
            // Arrange

            _actionManager
                .Setup((x) => x.GetEventTypesAsync())
                .ThrowsAsync(new Exception());

            // Act

            var result = await _sut.GetTypes();

            // Assert

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task GetCategories_ReturnsOkObjectResult()
        {
            // Arrange

            // Act

            var result = await _sut.GetCategories(It.IsAny<int>());

            // Assert

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetCategories_ActionManagerGetCategoriesByTypeIdAsyncReturnsListWithTwoItems_ReturnsListWithTwoCategories()
        {
            // Arrange

            var categories = new List<EventCategoryDTO>() { new EventCategoryDTO(), new EventCategoryDTO(), };

            _actionManager
                .Setup((x) => x.GetCategoriesByTypeIdAsync(It.IsAny<int>()))
                .ReturnsAsync(categories);

            var expected = 2;

            // Act

            var result = await _sut.GetCategories(It.IsAny<int>());

            var actual = (result as ObjectResult).Value as List<EventCategoryDTO>;

            // Assert

            Assert.AreEqual(expected, actual.Count);
        }

        [Test]
        public async Task GetCategories_ActionManagerGetCategoriesByTypeIdAsyncThrowsException_ReturnsBadRequestResult()
        {
            // Arrange

            _actionManager
                .Setup((x) => x.GetCategoriesByTypeIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            // Act

            var result = await _sut.GetCategories(It.IsAny<int>());

            // Assert

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task GetEvents_ReturnsOkObjectResult()
        {
            // Arrange

            // Act

            var result = await _sut.GetEvents(It.IsAny<int>(), It.IsAny<int>());

            // Assert

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetEvents_ActionManagerGetEventsAsyncThrowsException_ReturnsBadRequestResult()
        {
            // Arrange

            _actionManager
                .Setup((x) => x.GetEventsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .ThrowsAsync(new Exception());

            // Act

            var result = await _sut.GetEvents(It.IsAny<int>(), It.IsAny<int>());

            // Assert

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task GetEvents_ActionManagerGetEventsAsyncReturnsListWithTwoItems_ReturnsListWithTwoCategories()
        {
            // Arrange

            var events = new List<GeneralEventDTO>() { new GeneralEventDTO(), new GeneralEventDTO(), };

            _actionManager
                .Setup((x) => x.GetEventsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(events);

            var expected = 2;

            // Act

            var result = await _sut.GetEvents(It.IsAny<int>(), It.IsAny<int>());

            var actual = (result as ObjectResult).Value as List<GeneralEventDTO>;

            // Assert

            Assert.AreEqual(expected, actual.Count);
        }

        [Test]
        public async Task GetEventDetail_ReturnsOkObjectResult()
        {
            // Arrange

            // Act

            var result = await _sut.GetEventDetail(It.IsAny<int>());

            // Assert

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetEventDetail_ActionManagerGetEventInfoAsyncThrowsException_ReturnsBadRequestResult()
        {
            // Arrange

            _actionManager
                .Setup((x) => x.GetEventInfoAsync(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .ThrowsAsync(new Exception());

            // Act

            var result = await _sut.GetEventDetail(It.IsAny<int>());

            // Assert

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task GetEventDetail_ActionManagerGetEventInfoAsyncReturnsEventDTO_ReturnsNotNullEventDTO()
        {
            // Arrange

            _actionManager
                .Setup((x) => x.GetEventInfoAsync(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new EventDTO());

            // Act

            var result = await _sut.GetEventDetail(It.IsAny<int>());

            var actual = (result as ObjectResult).Value as EventDTO;

            // Assert

            Assert.IsNotNull(actual);
        }

        [Test]
        public async Task Delete_ActionManagerDeleteEventAsyncReturnsStatus200OK_ReturnsStatus200OK()
        {
            // Arrange

            _actionManager
                .Setup((x) => x.DeleteEventAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status200OK);

            var expected = StatusCodes.Status200OK;

            // Act

            var result = await _sut.Delete(It.IsAny<int>());

            var actual = (result as StatusCodeResult).StatusCode;

            // Assert

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task Delete_ActionManagerDeleteEventAsyncThrowsException_ReturnsBadRequestResult()
        {
            // Arrange

            _actionManager
                .Setup((x) => x.DeleteEventAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            // Act

            var result = await _sut.Delete(It.IsAny<int>());

            // Assert

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task Delete_ActionManagerDeleteEventAsyncReturnsStatus400BadRequest_ReturnsStatus400BadRequest()
        {
            // Arrange

            _actionManager
                .Setup((x) => x.DeleteEventAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status400BadRequest);

            var expected = StatusCodes.Status400BadRequest;

            // Act

            var result = await _sut.Delete(It.IsAny<int>());

            var actual = (result as StatusCodeResult).StatusCode;

            // Assert

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task DeletePicture_ActionManagerDeletePictureAsyncReturnsStatus200OK_ReturnsStatus200OK()
        {
            // Arrange

            _actionManager
                .Setup((x) => x.DeletePictureAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status200OK);

            var expected = StatusCodes.Status200OK;

            // Act

            var result = await _sut.DeletePicture(It.IsAny<int>());

            var actual = (result as StatusCodeResult).StatusCode;

            // Assert

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task DeletePicture_ActionManagerDeletePictureAsyncStatus400BadRequest_ReturnsStatus400BadRequest()
        {
            // Arrange

            _actionManager
                .Setup((x) => x.DeletePictureAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status400BadRequest);

            var expected = StatusCodes.Status400BadRequest;

            // Act

            var result = await _sut.DeletePicture(It.IsAny<int>());

            var actual = (result as StatusCodeResult).StatusCode;

            // Assert

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task DeletePicture_ActionManagerDeletePictureAsyncThrowsException_ReturnsBadRequestResult()
        {
            // Arrange

            _actionManager
                .Setup((x) => x.DeletePictureAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            // Act

            var result = await _sut.DeletePicture(It.IsAny<int>());

            // Assert

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task SubscribeOnEvent_ActionManagerSubscribeOnEventAsyncReturnsStatus200OK_ReturnsStatus200OK()
        {
            // Arrange

            _actionManager
                .Setup((x) => x.SubscribeOnEventAsync(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(StatusCodes.Status200OK);

            var expected = StatusCodes.Status200OK;

            // Act

            var result = await _sut.SubscribeOnEvent(It.IsAny<int>());

            var actual = (result as StatusCodeResult).StatusCode;

            // Assert

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task SubscribeOnEvent_ActionManagerSubscribeOnEventAsyncThrowsException_ReturnsBadRequestResult()
        {
            // Arrange

            _actionManager
                .Setup((x) => x.SubscribeOnEventAsync(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .ThrowsAsync(new Exception());

            // Act

            var result = await _sut.SubscribeOnEvent(It.IsAny<int>());

            // Assert

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task SubscribeOnEvent_ActionManagerSubscribeOnEventAsyncReturnsStatus400BadRequest_ReturnsStatus400BadRequest()
        {
            // Arrange

            _actionManager
                .Setup((x) => x.SubscribeOnEventAsync(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(StatusCodes.Status400BadRequest);

            var expected = StatusCodes.Status400BadRequest;

            // Act

            var result = await _sut.SubscribeOnEvent(It.IsAny<int>());

            var actual = (result as StatusCodeResult).StatusCode;

            // Assert

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task UnSubscribeOnEvent_ActionManagerUnSubscribeOnEventAsyncReturnsStatus200OK_ReturnsStatus200OK()
        {
            // Arrange

            _actionManager
                .Setup((x) => x.UnSubscribeOnEventAsync(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(StatusCodes.Status200OK);

            var expected = StatusCodes.Status200OK;

            // Act

            var result = await _sut.UnSubscribeOnEvent(It.IsAny<int>());

            var actual = (result as StatusCodeResult).StatusCode;

            // Assert

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task UnSubscribeOnEvent_ActionManagerUnSubscribeOnEventAsyncThrowsException_ReturnsBadRequestResult()
        {
            // Arrange

            _actionManager
                .Setup((x) => x.UnSubscribeOnEventAsync(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .ThrowsAsync(new Exception());

            // Act

            var result = await _sut.UnSubscribeOnEvent(It.IsAny<int>());

            // Assert

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task UnSubscribeOnEvent_ActionManagerUnSubscribeOnEventAsyncReturnsStatus400BadRequest_ReturnsStatus400BadRequest()
        {
            // Arrange

            _actionManager
                .Setup((x) => x.UnSubscribeOnEventAsync(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(StatusCodes.Status400BadRequest);

            var expected = StatusCodes.Status400BadRequest;

            // Act

            var result = await _sut.UnSubscribeOnEvent(It.IsAny<int>());

            var actual = (result as StatusCodeResult).StatusCode;

            // Assert

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task ApproveParticipant_ActionManagerApproveParticipantAsyncReturnsStatus200OK_ReturnsStatus200OK()
        {
            // Arrange

            _actionManager
                .Setup((x) => x.ApproveParticipantAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status200OK);

            var expected = StatusCodes.Status200OK;

            // Act

            var result = await _sut.ApproveParticipant(It.IsAny<int>());

            var actual = (result as StatusCodeResult).StatusCode;

            // Assert

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task ApproveParticipant_ActionManagerApproveParticipantAsyncThrowsException_ReturnsBadRequestResult()
        {
            // Arrange

            _actionManager
                .Setup((x) => x.ApproveParticipantAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            // Act

            var result = await _sut.ApproveParticipant(It.IsAny<int>());

            // Assert

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task ApproveParticipant_ActionManagerApproveParticipantAsyncReturnsStatus400BadRequest_ReturnsStatus400BadRequest()
        {
            // Arrange

            _actionManager
                .Setup((x) => x.ApproveParticipantAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status400BadRequest);

            var expected = StatusCodes.Status400BadRequest;

            // Act

            var result = await _sut.ApproveParticipant(It.IsAny<int>());

            var actual = (result as StatusCodeResult).StatusCode;

            // Assert

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task UnderReviewParticipant_ActionManagerUnderReviewParticipantAsyncReturnsStatus200OK_ReturnsStatus200OK()
        {
            // Arrange

            _actionManager
                .Setup((x) => x.UnderReviewParticipantAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status200OK);

            var expected = StatusCodes.Status200OK;

            // Act

            var result = await _sut.UnderReviewParticipant(It.IsAny<int>());

            var actual = (result as StatusCodeResult).StatusCode;

            // Assert

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task UnderReviewParticipant_ActionManagerUnderReviewParticipantAsyncReturnsStatus400BadRequest_ReturnsStatus400BadRequest()
        {
            // Arrange

            _actionManager
                .Setup((x) => x.UnderReviewParticipantAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status400BadRequest);

            var expected = StatusCodes.Status400BadRequest;

            // Act

            var result = await _sut.UnderReviewParticipant(It.IsAny<int>());

            var actual = (result as StatusCodeResult).StatusCode;

            // Assert

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task UnderReviewParticipant_ActionManagerUnderReviewParticipantAsyncThrowsException_ReturnsBadRequestResult()
        {
            // Arrange

            _actionManager
                .Setup((x) => x.UnderReviewParticipantAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            // Act

            var result = await _sut.UnderReviewParticipant(It.IsAny<int>());

            // Assert

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task RejectParticipant_ActionManagerRejectParticipantAsyncReturnsStatus200OK_ReturnsStatus200OK()
        {
            // Arrange

            _actionManager
                .Setup((x) => x.RejectParticipantAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status200OK);

            var expected = StatusCodes.Status200OK;

            // Act

            var result = await _sut.RejectParticipant(It.IsAny<int>());

            var actual = (result as StatusCodeResult).StatusCode;

            // Assert

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task RejectParticipant_ActionManagerRejectParticipantAsyncReturnsStatus400BadRequest_ReturnsStatus400BadRequest()
        {
            // Arrange

            _actionManager
                .Setup((x) => x.RejectParticipantAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status400BadRequest);

            var expected = StatusCodes.Status400BadRequest;

            // Act

            var result = await _sut.RejectParticipant(It.IsAny<int>());

            var actual = (result as StatusCodeResult).StatusCode;

            // Assert

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task RejectParticipant_ActionManagerRejectParticipantAsyncThrowsException_ReturnsBadRequestResult()
        {
            // Arrange

            _actionManager
                .Setup((x) => x.RejectParticipantAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            // Act

            var result = await _sut.RejectParticipant(It.IsAny<int>());

            // Assert

            Assert.IsInstanceOf<BadRequestResult>(result);
        }
    }
}