using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Principal;
using AutoMapper;
using EPlast.BussinessLayer.DTO.Events;
using EPlast.BussinessLayer.Interfaces;
using EPlast.BussinessLayer.Interfaces.Events;
using EPlast.Controllers;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.ViewModels.Events;
using EPlast.Wrapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace EPlast.XUnitTest
{
    public class ActionControllerTests
    {
        //private readonly Mock<IRepositoryWrapper> _repoWrapper;
        //private readonly Mock<IHostingEnvironment> _env;
        //private readonly Mock<UserManager<User>> _userManager;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IActionManager> _actionManager;


        public ActionControllerTests()
        {
            //_repoWrapper = new Mock<IRepositoryWrapper>();
            //_env = new Mock<IHostingEnvironment>();
            _mapper = new Mock<IMapper>();
            _actionManager = new Mock<IActionManager>();

            var userStoreMock = new Mock<IUserStore<User>>();
            //_userManager = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
        }

        //[Fact]
        //public void DeletePictureSuccessTest()
        //{
        //    //Arrange
        //    int testGalleryId = 2;
        //    _repoWrapper.Setup(x => x.Gallary.FindByCondition(It.IsAny<Expression<Func<Gallary, bool>>>()))
        //        .Returns(new List<Gallary> { new Gallary { ID = 2, GalaryFileName = "picture.jpj" } }.AsQueryable());
        //    _env.Setup(e => e.WebRootPath).Returns("Webroot\\");
        //    //Act
        //    var actionsController = new ActionController(_userManager.Object, _repoWrapper.Object, _env.Object);
        //    var actionResult = actionsController.DeletePicture(testGalleryId);
        //    var codeResult = actionResult as StatusCodeResult;
        //    //Assert
        //    Assert.NotNull(actionResult);
        //    Assert.IsType<StatusCodeResult>(actionResult);
        //    _repoWrapper.Verify(r => r.Save(), Times.Once());
        //    Assert.Equal(200, codeResult.StatusCode);
        //}

        //[Fact]
        //public void DeletePictureFailTest()
        //{
        //    //Arrange
        //    int testGalleryId = 2;
        //    _repoWrapper.Setup(x => x.Gallary.FindByCondition(It.IsAny<Expression<Func<Gallary, bool>>>()))
        //        .Throws(new Exception());
        //    //Act
        //    var actionsController = new ActionController(_userManager.Object, _repoWrapper.Object, _env.Object);
        //    var actionResult = actionsController.DeletePicture(testGalleryId);
        //    var codeResult = actionResult as StatusCodeResult;
        //    //Assert
        //    Assert.NotNull(actionResult);
        //    Assert.IsType<StatusCodeResult>(actionResult);
        //    Assert.Equal(500, codeResult.StatusCode);
        //}

        //[Fact]
        //public void DeleteEventSuccessTest()
        //{
        //    //Arrange
        //    int testEventId = 2;
        //    _repoWrapper.Setup(x => x.Event.FindByCondition(It.IsAny<Expression<Func<Event, bool>>>()))
        //        .Returns(GetEvents());
        //    //Act
        //    var actionsController = new ActionController(_userManager.Object, _repoWrapper.Object, _env.Object);
        //    var actionResult = actionsController.DeleteEvent(testEventId);
        //    var codeResult = actionResult as StatusCodeResult;
        //    //Assert
        //    Assert.NotNull(actionResult);
        //    Assert.IsType<StatusCodeResult>(actionResult);
        //    _repoWrapper.Verify(r => r.Event.Delete(It.IsAny<Event>()), Times.Once());
        //    _repoWrapper.Verify(r => r.Save(), Times.Once());
        //    Assert.Equal(200, codeResult.StatusCode);
        //}

        //[Fact]
        //public void DeleteEventFailTest()
        //{
        //    //Arrange
        //    int testEventId = 2;
        //    _repoWrapper.Setup(x => x.Event.FindByCondition(It.IsAny<Expression<Func<Event, bool>>>()))
        //        .Throws(new Exception());
        //    //Act
        //    var actionsController = new ActionController(_userManager.Object, _repoWrapper.Object, _env.Object);
        //    var actionResult = actionsController.DeleteEvent(testEventId);
        //    var codeResult = actionResult as StatusCodeResult;
        //    //Assert
        //    Assert.NotNull(actionResult);
        //    Assert.IsType<StatusCodeResult>(actionResult);
        //    Assert.Equal(500, codeResult.StatusCode);
        //}


        //[Fact]
        //public void UnsubscribeOnEventSuccessTest()
        //{
        //    //Arrange
        //    int testParticipantId = 2;
        //    _repoWrapper.Setup(x => x.ParticipantStatus.FindByCondition(It.IsAny<Expression<Func<ParticipantStatus, bool>>>()))
        //    .Returns(new List<ParticipantStatus>
        //     {
        //            new ParticipantStatus{ID=2 ,ParticipantStatusName = "Розглядається"},
        //     }.AsQueryable());

        //    _repoWrapper.Setup(x => x.Participant.FindByCondition(It.IsAny<Expression<Func<Participant, bool>>>()))
        //        .Returns(new List<Participant>
        //            {
        //              new Participant{ID=1,ParticipantStatusId=3,EventId=1,UserId="abc-1"},
        //              new Participant{ID=2,ParticipantStatusId=3,EventId=1,UserId="abc-2"},
        //              new Participant{ID=3,ParticipantStatusId=1,EventId=1,UserId="abc-3"}
        //            }.AsQueryable());
        //    _repoWrapper.Setup(x => x.EventStatus.FindByCondition(It.IsAny<Expression<Func<EventStatus, bool>>>()))
        //        .Returns(new List<EventStatus> { new EventStatus { ID = 1 } }.AsQueryable());
        //    _repoWrapper.Setup(x => x.Event.FindByCondition(It.IsAny<Expression<Func<Event, bool>>>())).Returns(GetEvents());
        //    //Act
        //    var actionsController = new ActionController(_userManager.Object, _repoWrapper.Object, _env.Object);
        //    var actionResult = actionsController.UnSubscribeOnEvent(testParticipantId);
        //    var codeResult = actionResult as StatusCodeResult;
        //    //Assert
        //    Assert.NotNull(actionResult);
        //    Assert.IsType<StatusCodeResult>(actionResult);
        //    _repoWrapper.Verify(r => r.Participant.Delete(It.IsAny<Participant>()), Times.Once());
        //    _repoWrapper.Verify(r => r.Save(), Times.Once());
        //    Assert.Equal(200, codeResult.StatusCode);
        //}

        //[Fact]
        //public void UnsubscribeOnEventConflictTest()
        //{
        //    //Arrange
        //    int testParticipantId = 2;
        //    _repoWrapper.Setup(x => x.ParticipantStatus.FindByCondition(It.IsAny<Expression<Func<ParticipantStatus, bool>>>()))
        //    .Returns(new List<ParticipantStatus>
        //     {
        //            new ParticipantStatus{ID=3 ,ParticipantStatusName = "Розглядається"},
        //     }.AsQueryable());
        //    _repoWrapper.Setup(x => x.Participant.FindByCondition(It.IsAny<Expression<Func<Participant, bool>>>()))
        //        .Returns(new List<Participant>
        //            {
        //              new Participant{ID=1,ParticipantStatusId=3,EventId=1,UserId="abc-1"},
        //              new Participant{ID=2,ParticipantStatusId=3,EventId=1,UserId="abc-2"},
        //              new Participant{ID=3,ParticipantStatusId=1,EventId=1,UserId="abc-3"}
        //            }.AsQueryable());
        //    _repoWrapper.Setup(x => x.EventStatus.FindByCondition(It.IsAny<Expression<Func<EventStatus, bool>>>()))
        //        .Returns(new List<EventStatus> { new EventStatus { ID = 1 } }.AsQueryable());
        //    _repoWrapper.Setup(x => x.Event.FindByCondition(It.IsAny<Expression<Func<Event, bool>>>())).Returns(GetEvents());
        //    //Act
        //    var actionsController = new ActionController(_userManager.Object, _repoWrapper.Object, _env.Object);
        //    var actionResult = actionsController.UnSubscribeOnEvent(testParticipantId);
        //    var codeResult = actionResult as StatusCodeResult;
        //    //Assert
        //    Assert.NotNull(actionResult);
        //    Assert.IsType<StatusCodeResult>(actionResult);
        //    Assert.Equal(409, codeResult.StatusCode);
        //}

        //[Fact]
        //public void UnsubscribeOnEventFailTest()
        //{
        //    //Arrange
        //    int testParticipantId = 2;
        //    _repoWrapper.Setup(x => x.Participant.FindByCondition(It.IsAny<Expression<Func<Participant, bool>>>()))
        //        .Throws(new Exception());
        //    //Act
        //    var actionsController = new ActionController(_userManager.Object, _repoWrapper.Object, _env.Object);
        //    var actionResult = actionsController.UnSubscribeOnEvent(testParticipantId);
        //    var codeResult = actionResult as StatusCodeResult;
        //    //Assert
        //    Assert.NotNull(actionResult);
        //    Assert.IsType<StatusCodeResult>(actionResult);
        //    Assert.Equal(500, codeResult.StatusCode);
        //}

        //[Fact]
        //public void SubscribeOnEventSuccessTest()
        //{
        //    //Arrange
        //    int testEventId = 2;
        //    string expectedID = "abc-1";
        //    _userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(expectedID);
        //    _repoWrapper.Setup(x => x.ParticipantStatus.FindByCondition(It.IsAny<Expression<Func<ParticipantStatus, bool>>>()))
        //        .Returns(new List<ParticipantStatus>
        //        {
        //            new ParticipantStatus{ID=3 ,ParticipantStatusName = "Розглядається"},
        //        }.AsQueryable());
        //    _repoWrapper.Setup(x => x.Participant.Create((It.IsAny<Participant>())));
        //    _repoWrapper.Setup(x => x.EventStatus.FindByCondition(It.IsAny<Expression<Func<EventStatus, bool>>>()))
        //        .Returns(new List<EventStatus> { new EventStatus { ID = 1 } }.AsQueryable());
        //    _repoWrapper.Setup(x => x.Event.FindByCondition(It.IsAny<Expression<Func<Event, bool>>>())).Returns(GetEvents());
        //    //Act  
        //    var actionsController = new ActionController(_userManager.Object, _repoWrapper.Object, _env.Object);
        //    var actionResult = actionsController.SubscribeOnEvent(testEventId);
        //    var codeResult = actionResult as StatusCodeResult;
        //    //Assert
        //    Assert.NotNull(actionResult);
        //    Assert.IsType<StatusCodeResult>(actionResult);
        //    _repoWrapper.Verify(r => r.Participant.Create(It.IsAny<Participant>()), Times.Once());
        //    _repoWrapper.Verify(r => r.Save(), Times.Once());
        //    Assert.Equal(200, codeResult.StatusCode);
        //}

        //[Fact]
        //public void SubscribeOnEventConflictTest()
        //{
        //    //Arrange
        //    int testEventId = 2;
        //    string expectedID = "abc-1";
        //    _userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(expectedID);
        //    _repoWrapper.Setup(x => x.ParticipantStatus.FindByCondition(It.IsAny<Expression<Func<ParticipantStatus, bool>>>()))
        //        .Returns(new List<ParticipantStatus>
        //        {
        //            new ParticipantStatus{ID=3 ,ParticipantStatusName = "Розглядається"},
        //        }.AsQueryable());
        //    _repoWrapper.Setup(x => x.Participant.Create((It.IsAny<Participant>())));
        //    _repoWrapper.Setup(x => x.EventStatus.FindByCondition(It.IsAny<Expression<Func<EventStatus, bool>>>()))
        //        .Returns(new List<EventStatus> { new EventStatus { ID = 2 } }.AsQueryable());
        //    _repoWrapper.Setup(x => x.Event.FindByCondition(It.IsAny<Expression<Func<Event, bool>>>())).Returns(GetEvents());
        //    //Act  
        //    var actionsController = new ActionController(_userManager.Object, _repoWrapper.Object, _env.Object);
        //    var actionResult = actionsController.SubscribeOnEvent(testEventId);
        //    var codeResult = actionResult as StatusCodeResult;
        //    //Assert
        //    Assert.NotNull(actionResult);
        //    Assert.IsType<StatusCodeResult>(actionResult);
        //    Assert.Equal(409, codeResult.StatusCode);
        //}

        //[Fact]
        //public void SubscribeOnEventFailTest()
        //{
        //    //Arrange
        //    int testEventId = 2;
        //    string expectedID = "abc-1";
        //    _userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(expectedID);
        //    _repoWrapper.Setup(x => x.ParticipantStatus.FindByCondition(It.IsAny<Expression<Func<ParticipantStatus, bool>>>()))
        //        .Throws(new Exception());
        //    _repoWrapper.Setup(x => x.Participant.Create((It.IsAny<Participant>())));
        //    //Act  
        //    var actionsController = new ActionController(_userManager.Object, _repoWrapper.Object, _env.Object);
        //    var actionResult = actionsController.SubscribeOnEvent(testEventId);
        //    var codeResult = actionResult as StatusCodeResult;
        //    //Assert
        //    Assert.NotNull(actionResult);
        //    Assert.IsType<StatusCodeResult>(actionResult);
        //    Assert.Equal(500, codeResult.StatusCode);
        //}

        //[Fact]
        //public void ApproveParticipantSuccessTest()
        //{
        //    //Arrange
        //    int testParticipantId = 2;
        //    _repoWrapper.Setup(x => x.Participant.FindByCondition(It.IsAny<Expression<Func<Participant, bool>>>()))
        //      .Returns(new List<Participant>
        //          {
        //              new Participant{ID=2,ParticipantStatusId=3,EventId=1,UserId="abc-2"},
        //          }.AsQueryable());
        //    _repoWrapper.Setup(x => x.ParticipantStatus.FindByCondition(It.IsAny<Expression<Func<ParticipantStatus, bool>>>()))
        //        .Returns(new List<ParticipantStatus>
        //        {
        //            new ParticipantStatus{ID=2 ,ParticipantStatusName = "Статус"},
        //        }.AsQueryable());
        //    //Act  
        //    var actionsController = new ActionController(_userManager.Object, _repoWrapper.Object, _env.Object);
        //    var actionResult = actionsController.ApproveParticipant(testParticipantId);
        //    var codeResult = actionResult as StatusCodeResult;
        //    //Assert
        //    Assert.NotNull(actionResult);
        //    Assert.IsType<StatusCodeResult>(actionResult);
        //    _repoWrapper.Verify(r => r.Participant.Update(It.IsAny<Participant>()), Times.Once());
        //    _repoWrapper.Verify(r => r.Save(), Times.Once());
        //    Assert.Equal(200, codeResult.StatusCode);
        //}

        //[Fact]
        //public void UndetermineParticipantSuccessTest()
        //{
        //    //Arrange
        //    int testParticipantId = 2;
        //    _repoWrapper.Setup(x => x.Participant.FindByCondition(It.IsAny<Expression<Func<Participant, bool>>>()))
        //      .Returns(new List<Participant>
        //          {
        //              new Participant{ID=2,ParticipantStatusId=3,EventId=1,UserId="abc-2"},
        //          }.AsQueryable());
        //    _repoWrapper.Setup(x => x.ParticipantStatus.FindByCondition(It.IsAny<Expression<Func<ParticipantStatus, bool>>>()))
        //        .Returns(new List<ParticipantStatus>
        //        {
        //            new ParticipantStatus{ID=2 ,ParticipantStatusName = "Статус"},
        //        }.AsQueryable());
        //    //Act  
        //    var actionsController = new ActionController(_userManager.Object, _repoWrapper.Object, _env.Object);
        //    var actionResult = actionsController.UndetermineParticipant(testParticipantId);
        //    var codeResult = actionResult as StatusCodeResult;
        //    //Assert
        //    Assert.NotNull(actionResult);
        //    Assert.IsType<StatusCodeResult>(actionResult);
        //    _repoWrapper.Verify(r => r.Participant.Update(It.IsAny<Participant>()), Times.Once());
        //    _repoWrapper.Verify(r => r.Save(), Times.Once());
        //    Assert.Equal(200, codeResult.StatusCode);
        //}

        //[Fact]
        //public void RejectParticipantSuccessTest()
        //{
        //    //Arrange
        //    int testParticipantId = 2;
        //    _repoWrapper.Setup(x => x.Participant.FindByCondition(It.IsAny<Expression<Func<Participant, bool>>>()))
        //      .Returns(new List<Participant>
        //          {
        //              new Participant{ID=2,ParticipantStatusId=3,EventId=1,UserId="abc-2"},
        //          }.AsQueryable());
        //    _repoWrapper.Setup(x => x.ParticipantStatus.FindByCondition(It.IsAny<Expression<Func<ParticipantStatus, bool>>>()))
        //        .Returns(new List<ParticipantStatus>
        //        {
        //            new ParticipantStatus{ID=2 ,ParticipantStatusName = "Статус"},
        //        }.AsQueryable());
        //    //Act  
        //    var actionsController = new ActionController(_userManager.Object, _repoWrapper.Object, _env.Object);
        //    var actionResult = actionsController.RejectParticipant(testParticipantId);
        //    var codeResult = actionResult as StatusCodeResult;
        //    //Assert
        //    Assert.NotNull(actionResult);
        //    Assert.IsType<StatusCodeResult>(actionResult);
        //    _repoWrapper.Verify(r => r.Participant.Update(It.IsAny<Participant>()), Times.Once());
        //    _repoWrapper.Verify(r => r.Save(), Times.Once());
        //    Assert.Equal(200, codeResult.StatusCode);
        //}

        //[Fact]
        //public void ApproveParticipantFailTest()
        //{
        //    //Arrange
        //    int testParticipantId = 2;
        //    _repoWrapper.Setup(x => x.Participant.FindByCondition(It.IsAny<Expression<Func<Participant, bool>>>()))
        //      .Returns(new List<Participant>
        //          {
        //              new Participant{ID=2,ParticipantStatusId=3,EventId=1,UserId="abc-2"},
        //          }.AsQueryable());
        //    _repoWrapper.Setup(x => x.ParticipantStatus.FindByCondition(It.IsAny<Expression<Func<ParticipantStatus, bool>>>()))
        //        .Returns(new List<ParticipantStatus>
        //        {
        //            new ParticipantStatus{ID=2 ,ParticipantStatusName = "Статус"},
        //        }.AsQueryable());
        //    _repoWrapper.Setup(x => x.Participant.Update((It.IsAny<Participant>()))).Throws(new Exception());
        //    //Act  
        //    var actionsController = new ActionController(_userManager.Object, _repoWrapper.Object, _env.Object);
        //    var actionResult = actionsController.ApproveParticipant(testParticipantId);
        //    var codeResult = actionResult as StatusCodeResult;
        //    //Assert
        //    Assert.NotNull(actionResult);
        //    Assert.IsType<StatusCodeResult>(actionResult);
        //    Assert.Equal(500, codeResult.StatusCode);
        //}

        //[Fact]
        //public void UndetermineParticipantFailTest()
        //{
        //    //Arrange
        //    int testParticipantId = 2;
        //    _repoWrapper.Setup(x => x.Participant.FindByCondition(It.IsAny<Expression<Func<Participant, bool>>>()))
        //      .Returns(new List<Participant>
        //          {
        //              new Participant{ID=2,ParticipantStatusId=3,EventId=1,UserId="abc-2"},
        //          }.AsQueryable());
        //    _repoWrapper.Setup(x => x.ParticipantStatus.FindByCondition(It.IsAny<Expression<Func<ParticipantStatus, bool>>>()))
        //        .Returns(new List<ParticipantStatus>
        //        {
        //            new ParticipantStatus{ID=2 ,ParticipantStatusName = "Статус"},
        //        }.AsQueryable());
        //    _repoWrapper.Setup(x => x.Participant.Update((It.IsAny<Participant>()))).Throws(new Exception());
        //    //Act  
        //    var actionsController = new ActionController(_userManager.Object, _repoWrapper.Object, _env.Object);
        //    var actionResult = actionsController.UndetermineParticipant(testParticipantId);
        //    var codeResult = actionResult as StatusCodeResult;
        //    //Assert
        //    Assert.NotNull(actionResult);
        //    Assert.IsType<StatusCodeResult>(actionResult);
        //    Assert.Equal(500, codeResult.StatusCode);
        //}

        //[Fact]
        //public void RejectParticipantFailTest()
        //{
        //    //Arrange
        //    int testParticipantId = 2;
        //    _repoWrapper.Setup(x => x.Participant.FindByCondition(It.IsAny<Expression<Func<Participant, bool>>>()))
        //      .Returns(new List<Participant>
        //          {
        //              new Participant{ID=2,ParticipantStatusId=3,EventId=1,UserId="abc-2"},
        //          }.AsQueryable());
        //    _repoWrapper.Setup(x => x.ParticipantStatus.FindByCondition(It.IsAny<Expression<Func<ParticipantStatus, bool>>>()))
        //        .Returns(new List<ParticipantStatus>
        //        {
        //            new ParticipantStatus{ID=2 ,ParticipantStatusName = "Статус"},
        //        }.AsQueryable());
        //    _repoWrapper.Setup(x => x.Participant.Update(It.IsAny<Participant>())).Throws(new Exception());
        //    //Act  
        //    var actionsController = new ActionController(_userManager.Object, _repoWrapper.Object, _env.Object);
        //    var actionResult = actionsController.RejectParticipant(testParticipantId);
        //    var codeResult = actionResult as StatusCodeResult;
        //    //Assert
        //    Assert.NotNull(actionResult);
        //    Assert.IsType<StatusCodeResult>(actionResult);
        //    Assert.Equal(500, codeResult.StatusCode);
        //}

        //[Fact]
        //public void EventinfoFailureTest()
        //{
        //    //Arrange
        //    _actionManager.Setup(am => am.GetEventInfo(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
        //        .Throws(new Exception());
        //    _mapper.Setup(m => m.Map<EventDTO, EventViewModel>(It.IsAny<EventDTO>())).Returns(new EventViewModel());
        //    int eventID = 3;
        //    //Act  
        //    var actionsController = new ActionController(_actionManager.Object,_mapper.Object);
        //    var actionResult = actionsController.EventInfo(eventID);
        //    //Arrange
        //    Assert.NotNull(actionResult);
        //    var viewResult = Assert.IsType<RedirectToActionResult>(actionResult);
        //    Assert.Equal("HandleError", viewResult.ActionName);
        //    Assert.Equal("Error", viewResult.ControllerName);
        //}

        //[Fact]
        //public void EventinfoSuccessTest()
        //{
        //    //Arrange
        //    _actionManager.Setup(am => am.GetEventInfo(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
        //        .Returns(GetEventDTO());
        //    _mapper.Setup(m => m.Map<EventDTO, EventViewModel>(It.IsAny<EventDTO>())).Returns(GetEventVM());
        //    int eventID = 3;
        //    //Act  
        //    var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
        //    var actionResult = actionsController.EventInfo(eventID);
        //    //Arrange
        //    Assert.NotNull(actionResult);
        //    var viewResult = Assert.IsType<ViewResult>(actionResult);
        //    var viewModel = Assert.IsType<EventViewModel>(viewResult.Model);
        //    Assert.Equal(3, viewModel.EventParticipants.Count());
        //}

        //[Fact]
        //public void GetActionsFailureTest()
        //{
        //    //Arrange
        //    _actionManager.Setup(am => am.GetActionCategories())
        //        .Throws(new Exception());
        //    _mapper.Setup(m => m.Map<List<EventCategoryDTO>, List<EventCategoryViewModel>>(It.IsAny<List<EventCategoryDTO>>())).Returns(new List<EventCategoryViewModel>());
        //    //Act  
        //    var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
        //    var actionResult = actionsController.GetAction();
        //    //Arrange
        //    Assert.NotNull(actionResult);
        //    var viewResult = Assert.IsType<RedirectToActionResult>(actionResult);
        //    Assert.Equal("HandleError", viewResult.ActionName);
        //    Assert.Equal("Error", viewResult.ControllerName);
        //}

        //[Fact]
        //public void GetActionEmptyTest()
        //{
        //    //Arrange
        //    _actionManager.Setup(am => am.GetActionCategories())
        //        .Returns(new List<EventCategoryDTO>());
        //    _mapper.Setup(m => m.Map<List<EventCategoryDTO>, List<EventCategoryViewModel>>(It.IsAny<List<EventCategoryDTO>>())).Returns(new List<EventCategoryViewModel>());
        //    //Act  
        //    var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
        //    var actionResult = actionsController.GetAction() as ViewResult;
        //    //Assert
        //    Assert.NotNull(actionResult);
        //    Assert.NotNull(actionResult.Model);
        //    var viewModel = actionResult.Model as List<EventCategoryViewModel>;
        //    Assert.NotNull(viewModel);
        //    Assert.Empty(viewModel);
        //}

        //[Fact]
        //public void GetEventsFailureTest()
        //{
        //    //Arrange
        //    _actionManager.Setup(am => am.GetEvents(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
        //        .Throws(new Exception());
        //    _mapper.Setup(m => m.Map<List<GeneralEventDTO>, List<GeneralEventViewModel>>(It.IsAny<List<GeneralEventDTO>>())).Returns(new List<GeneralEventViewModel>());
        //    int id = 3;
        //    //Act  
        //    var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
        //    var actionResult = actionsController.Events(id);
        //    //Arrange
        //    Assert.NotNull(actionResult);
        //    var viewResult = Assert.IsType<RedirectToActionResult>(actionResult);
        //    Assert.Equal("HandleError", viewResult.ActionName);
        //    Assert.Equal("Error", viewResult.ControllerName);
        //}

        //[Fact]
        //public void GetEventsEmptyTest()
        //{
        //    //Arrange
        //    _actionManager.Setup(am => am.GetEvents(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
        //        .Returns(new List<GeneralEventDTO>());
        //    _mapper.Setup(m => m.Map<List<GeneralEventDTO>, List<GeneralEventViewModel>>(It.IsAny<List<GeneralEventDTO>>())).Returns(new List<GeneralEventViewModel>());
        //    int id = 3;
        //    //Act  
        //    var actionsController = new ActionController(_actionManager.Object, _mapper.Object);
        //    var actionResult = actionsController.Events(id) as ViewResult;
        //    //Assert
        //    Assert.NotNull(actionResult);
        //    Assert.NotNull(actionResult.Model);
        //    var viewModel = actionResult.Model as List<GeneralEventViewModel>;
        //    Assert.NotNull(viewModel);
        //    Assert.Empty(viewModel);
        //}


        //[Fact]
        //public void EventInfoWhenUserIsEventAdminTest()
        //{
        //    //Arrange
        //    var _userClaims = new Mock<IPrincipal>();
        //    _userClaims.Setup(uc => uc.IsInRole(It.IsAny<string>())).Returns(false);
        //    _userClaims.Setup(uc => uc.IsInRole(It.IsAny<string>())).Returns(false);
        //    _repoWrapper.Setup(x => x.ParticipantStatus.FindByCondition(p => p.ParticipantStatusName == "Учасник")).Returns(new List<ParticipantStatus> { new ParticipantStatus { ID = 1 } }.AsQueryable());
        //    _repoWrapper.Setup(x => x.ParticipantStatus.FindByCondition(p => p.ParticipantStatusName == "Розглядається")).Returns(new List<ParticipantStatus> { new ParticipantStatus { ID = 3 } }.AsQueryable());
        //    _repoWrapper.Setup(x => x.ParticipantStatus.FindByCondition(p => p.ParticipantStatusName == "Відмовлено")).Returns(new List<ParticipantStatus> { new ParticipantStatus { ID = 2 } }.AsQueryable());
        //    int eventID = 1;
        //    string expectedID = "abc-1";
        //    _userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(expectedID);
        //    _repoWrapper.Setup(x => x.Event.FindByCondition(It.IsAny<Expression<Func<Event, bool>>>())).Returns(GetEvents());
        //    _repoWrapper.Setup(x => x.EventStatus.FindByCondition(It.IsAny<Expression<Func<EventStatus, bool>>>()))
        //        .Returns(new List<EventStatus> { new EventStatus { ID = 1 } }.AsQueryable());

        //    //Act
        //    var actionsController = new ActionController(_userManager.Object, _repoWrapper.Object, _env.Object);
        //    var actionResult = actionsController.EventInfo(eventID);
        //    //Assert    
        //    Assert.NotNull(actionResult);
        //    var viewResult = Assert.IsType<ViewResult>(actionResult);
        //    var viewModel = Assert.IsType<EventViewModel>(viewResult.Model);
        //    Assert.Equal(3, viewModel.EventParticipants.Count());
        //}

        //[Fact]
        //public void EventInfoWhenUserIsNotEventAdminTest()
        //{
        //    //Arrange
        //    _repoWrapper.Setup(x => x.ParticipantStatus.FindByCondition(p => p.ParticipantStatusName == "Учасник")).Returns(new List<ParticipantStatus> { new ParticipantStatus { ID = 1 } }.AsQueryable());
        //    _repoWrapper.Setup(x => x.ParticipantStatus.FindByCondition(p => p.ParticipantStatusName == "Розглядається")).Returns(new List<ParticipantStatus> { new ParticipantStatus { ID = 3 } }.AsQueryable());
        //    _repoWrapper.Setup(x => x.ParticipantStatus.FindByCondition(p => p.ParticipantStatusName == "Відмовлено")).Returns(new List<ParticipantStatus> { new ParticipantStatus { ID = 2 } }.AsQueryable());
        //    int eventID = 1;
        //    string expectedID = "abc-2";
        //    _userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(expectedID);
        //    _repoWrapper.Setup(x => x.Event.FindByCondition(It.IsAny<Expression<Func<Event, bool>>>())).Returns(GetEvents());
        //    _repoWrapper.Setup(x => x.EventStatus.FindByCondition(It.IsAny<Expression<Func<EventStatus, bool>>>()))
        //     .Returns(new List<EventStatus> { new EventStatus { ID = 1 } }.AsQueryable());
        //    //Act
        //    var actionsController = new ActionController(_userManager.Object, _repoWrapper.Object, _env.Object);
        //    var actionResult = actionsController.EventInfo(eventID);
        //    //Assert    
        //    Assert.NotNull(actionResult);
        //    var viewResult = Assert.IsType<ViewResult>(actionResult);
        //    var viewModel = Assert.IsType<EventViewModel>(viewResult.Model);
        //    Assert.Single(viewModel.EventParticipants);
        //}

        //[Fact]
        //public void GetEventsFailureTest()
        //{
        //    //Arrange
        //    _repoWrapper.Setup(x => x.EventType.FindByCondition(It.IsAny<Expression<Func<EventType, bool>>>())).Returns(new List<EventType> { new EventType { ID = 1, EventTypeName = "Акція" } }.AsQueryable());
        //    _repoWrapper.Setup(x => x.ParticipantStatus.FindByCondition(It.IsAny<Expression<Func<ParticipantStatus, bool>>>())).Returns(new List<ParticipantStatus> { new ParticipantStatus { ID = 1 } }.AsQueryable());
        //    _repoWrapper.Setup(x => x.EventStatus.FindByCondition(It.IsAny<Expression<Func<EventStatus, bool>>>())).Returns(new List<EventStatus> { new EventStatus { ID = 3 } }.AsQueryable());
        //    int eventCategoryID = 3;
        //    string expectedID = "abc-1";
        //    _userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(expectedID);
        //    _repoWrapper.Setup(x => x.Event.FindByCondition(It.IsAny<Expression<Func<Event, bool>>>())).Throws(new Exception());
        //    //Act  
        //    var actionsController = new ActionController(_userManager.Object, _repoWrapper.Object, _env.Object);
        //    var actionResult = actionsController.Events(eventCategoryID);
        //    //Arrange
        //    Assert.NotNull(actionResult);
        //    var viewResult = Assert.IsType<RedirectToActionResult>(actionResult);
        //    Assert.Equal("HandleError", viewResult.ActionName);
        //    Assert.Equal("Error", viewResult.ControllerName);
        //}

        //[Fact]
        //public void GetEventsEmptyTest()
        //{
        //    //Arrange
        //    _repoWrapper.Setup(x => x.EventType.FindByCondition(It.IsAny<Expression<Func<EventType, bool>>>())).Returns(new List<EventType> { new EventType { ID = 1, EventTypeName = "Акція" } }.AsQueryable());
        //    _repoWrapper.Setup(x => x.ParticipantStatus.FindByCondition(It.IsAny<Expression<Func<ParticipantStatus, bool>>>())).Returns(new List<ParticipantStatus> { new ParticipantStatus { ID = 1 } }.AsQueryable());
        //    _repoWrapper.Setup(x => x.EventStatus.FindByCondition(It.IsAny<Expression<Func<EventStatus, bool>>>())).Returns(new List<EventStatus> { new EventStatus { ID = 3 } }.AsQueryable());
        //    var eventList = new List<Event>();
        //    int eventCategoryID = 3;
        //    string expectedID = "abc-1";
        //    _userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(expectedID);
        //    _repoWrapper.Setup(x => x.Event.FindAll()).Returns(eventList.AsQueryable());
        //    //Act
        //    var actionsController = new ActionController(_userManager.Object, _repoWrapper.Object, _env.Object);
        //    var actionResult = actionsController.Events(eventCategoryID) as ViewResult;
        //    //Assert
        //    Assert.NotNull(actionResult);
        //    Assert.NotNull(actionResult.Model);
        //    var viewModel = actionResult.Model as List<GeneralEventViewModel>;
        //    Assert.NotNull(viewModel);
        //    Assert.Empty(viewModel);
        //}

        //[Fact]
        //public void GetEventsTest()
        //{
        //    //Arrange
        //    _repoWrapper.Setup(x => x.EventType.FindByCondition(et => et.EventTypeName == "Акція")).Returns(new List<EventType> { new EventType { ID = 1, EventTypeName = "Акція" } }.AsQueryable());
        //    _repoWrapper.Setup(x => x.ParticipantStatus.FindByCondition(p => p.ParticipantStatusName == "Учасник")).Returns(new List<ParticipantStatus> { new ParticipantStatus { ID = 1 } }.AsQueryable());
        //    _repoWrapper.Setup(x => x.ParticipantStatus.FindByCondition(p => p.ParticipantStatusName == "Розглядається")).Returns(new List<ParticipantStatus> { new ParticipantStatus { ID = 3 } }.AsQueryable());
        //    _repoWrapper.Setup(x => x.ParticipantStatus.FindByCondition(p => p.ParticipantStatusName == "Відмовлено")).Returns(new List<ParticipantStatus> { new ParticipantStatus { ID = 2 } }.AsQueryable());
        //    _repoWrapper.Setup(x => x.EventStatus.FindByCondition(st => st.EventStatusName == "Затверджений(-на)")).Returns(new List<EventStatus> { new EventStatus { ID = 3 } }.AsQueryable());
        //    _repoWrapper.Setup(x => x.EventStatus.FindByCondition(st => st.EventStatusName == "Завершений(-на)")).Returns(new List<EventStatus> { new EventStatus { ID = 1 } }.AsQueryable());
        //    _repoWrapper.Setup(x => x.EventStatus.FindByCondition(st => st.EventStatusName == "Не затверджені")).Returns(new List<EventStatus> { new EventStatus { ID = 2 } }.AsQueryable());
        //    int eventCategoryID = 3;
        //    string expectedID = "abc-1";
        //    _userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(expectedID);
        //    _repoWrapper.Setup(x => x.Event.FindByCondition(It.IsAny<Expression<Func<Event, bool>>>())).Returns(GetEvents());
        //    //Act
        //    var actionsController = new ActionController(_userManager.Object, _repoWrapper.Object, _env.Object);
        //    var actionResult = actionsController.Events(eventCategoryID) as ViewResult;
        //    var viewModel = actionResult.Model as List<GeneralEventViewModel>;
        //    //Assert    
        //    Assert.NotNull(actionResult);
        //    var viewResult = Assert.IsType<ViewResult>(actionResult);
        //    Assert.IsAssignableFrom<List<GeneralEventViewModel>>(viewResult.Model);
        //    Assert.Equal(2, viewModel.Count);
        //}

        //[Fact]
        //public void GetActionEmptyTest()
        //{
        //    //Arrange
        //    var eventCategoryList = new List<EventCategory>();
        //    _repoWrapper.Setup(x => x.EventCategory.FindAll()).Returns(eventCategoryList.AsQueryable());
        //    //Act
        //    var actionsController = new ActionController(_userManager.Object, _repoWrapper.Object, _env.Object);
        //    var actionResult = actionsController.GetAction() as ViewResult;
        //    Assert.NotNull(actionResult);
        //    Assert.NotNull(actionResult.Model);
        //    //Assert
        //    var viewModel = actionResult.Model as List<EventCategoryViewModel>;
        //    Assert.NotNull(viewModel);
        //    Assert.Empty(viewModel);
        //}

        //[Fact]
        //public void GetActionTest()
        //{
        //    //Arrange
        //    var eventCategoryList = new List<EventCategory>()
        //    {
        //        new EventCategory()
        //        {
        //            EventCategoryName = "Some name",
        //            ID = 1,
        //            Events = new List<Event>()
        //        }
        //    };
        //    _repoWrapper.Setup(x => x.EventCategory.FindAll()).Returns(eventCategoryList.AsQueryable());
        //    //Act
        //    var actionsController = new ActionController(_userManager.Object, _repoWrapper.Object, _env.Object);
        //    var actionResult = actionsController.GetAction() as ViewResult;
        //    //Assert
        //    Assert.NotNull(actionResult);
        //    Assert.NotNull(actionResult.Model);
        //    var viewModel = actionResult.Model as List<EventCategoryViewModel>;
        //    Assert.NotNull(viewModel);
        //    Assert.Single(viewModel);
        //    Assert.NotNull(viewModel[0].EventCategory);
        //    Assert.Equal(1, viewModel[0].EventCategory.ID);
        //    Assert.Equal("Some name", viewModel[0].EventCategory.EventCategoryName);
        //    Assert.NotNull(viewModel[0].EventCategory.Events);
        //    Assert.Empty(viewModel[0].EventCategory.Events);

        //}

        //[Fact]
        //public void GetActionFailureTest()
        //{
        //    //Arrange
        //    var eventCategoryList = new List<EventCategory>();
        //    _repoWrapper.Setup(x => x.EventCategory.FindAll()).Throws(new Exception());
        //    //Act
        //    var actionsController = new ActionController(_userManager.Object, _repoWrapper.Object, _env.Object);
        //    var actionResult = actionsController.GetAction();
        //    //Assert
        //    Assert.NotNull(actionResult);
        //    var viewResult = Assert.IsType<RedirectToActionResult>(actionResult);
        //    Assert.Equal("HandleError", viewResult.ActionName);
        //    Assert.Equal("Error", viewResult.ControllerName);
        //}

        //[Fact]
        //public void FillEventGalleryFailureTest()
        //{
        //    //Arrange
        //    _repoWrapper.Setup(x => x.Gallary.Create((It.IsAny<Gallary>())));
        //    _repoWrapper.Setup(x => x.EventGallary.Create((It.IsAny<EventGallary>())));
        //    _env.Setup(e => e.WebRootPath).Returns("Webroot\\");
        //    int eventID = 5;
        //    //Act  
        //    var actionsController = new ActionController(_userManager.Object, _repoWrapper.Object, _env.Object);
        //    var actionResult = actionsController.FillEventGallery(eventID, FakeFiles());
        //    var codeResult = actionResult as StatusCodeResult;
        //    //Assert
        //    Assert.NotNull(actionResult);
        //    Assert.IsType<StatusCodeResult>(actionResult);
        //    Assert.Equal(500, codeResult.StatusCode);
        //}

        //public EventDTO GetEventDTO()
        //{
        //   var dto = new EventDTO()
        //   {
        //       EventParticipants = new List<Participant>()
        //       {
        //           new Participant(),
        //           new Participant(),
        //           new Participant()
        //       },
        //       IsEventFinished = true,
        //       IsUserApprovedParticipant = true,
        //       IsUserEventAdmin = true,
        //       IsUserParticipant = false,
        //       IsUserRejectedParticipant = false,
        //       IsUserUndeterminedParticipant = false
        //   };
        //    return dto;
        //}

        //public EventViewModel GetEventVM()
        //{
        //    var model = new EventViewModel()
        //    {
        //        EventParticipants = new List<Participant>()
        //        {
        //            new Participant(),
        //            new Participant(),
        //            new Participant()
        //        },
        //        IsEventFinished = true,
        //        IsUserApprovedParticipant = true,
        //        IsUserEventAdmin = true,
        //        IsUserParticipant = false,
        //        IsUserRejectedParticipant = false,
        //        IsUserUndeterminedParticipant = false
        //    };
        //    return model;
        //}

        public IQueryable<Event> GetEvents()
        {
            var events = new List<Event>
            {
                new Event{
                    ID=1,
                    EventName="Крайовий Пластовий З`їзд 2015",
                    EventDateStart=new System.DateTime(),
                    EventDateEnd=new System.DateTime(),
                    Eventlocation="Київ",
                    EventTypeID=1,
                    EventCategoryID=3,
                    EventStatusID=2,
                    Description="Event Description",
                    Participants= new List<Participant>
                    {
                      new Participant{ID=1,ParticipantStatusId=3,EventId=1,UserId="abc-1"},
                      new Participant{ID=2,ParticipantStatusId=3,EventId=1,UserId="abc-2"},
                      new Participant{ID=3,ParticipantStatusId=1,EventId=1,UserId="abc-3"}
                    },
                    EventAdmins=  new List<EventAdmin>
                    {
                        new EventAdmin{EventID=1,UserID="abc-1"}
                    }
                },
                 new Event{
                    ID=2,
                    EventName="Крайовий Пластовий З`їзд 2020",
                    EventDateStart=new System.DateTime(),
                    EventDateEnd=new System.DateTime(),
                    Eventlocation="Львів",
                    EventTypeID=1,
                    EventCategoryID=3,
                    EventStatusID=3,
                    Description="Event Description",
                    Participants= new List<Participant>
                    {
                      new Participant{ID=1,ParticipantStatusId=3,EventId=2,UserId="abc-1"},
                      new Participant{ID=2,ParticipantStatusId=3,EventId=2,UserId="abc-2"},
                      new Participant{ID=3,ParticipantStatusId=1,EventId=2,UserId="abc-3"}
                    },
                     EventAdmins=  new List<EventAdmin>
                    {
                        new EventAdmin{EventID=2,UserID="abc-1"}
                    }
                 }
            }.AsQueryable();
            return events;
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
