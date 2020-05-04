using EPlast.BussinessLayer.Interfaces;
using EPlast.Controllers;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.ViewModels;
using EPlast.ViewModels.Events;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace EPlast.XUnitTest
{
    public class EventUserControllerTests
    {
        private Mock<IRepositoryWrapper> _repository;
        private Mock<IUserStore<User>> _store;
        private Mock<UserManager<User>> _usermanager;
        private Mock<Models.ViewModelInitializations.Interfaces.ICreateEventVMInitializer> _iCreateEventVM;
        public EventUserControllerTests()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _store = new Mock<IUserStore<User>>();
            _usermanager = new Mock<UserManager<User>>(_store.Object, null, null, null, null, null, null, null, null);
            _iCreateEventVM = new Mock<Models.ViewModelInitializations.Interfaces.ICreateEventVMInitializer>();
        }
        [Fact]
        public void EventCreateMethodGetReturnNull()
        {
            //arrange
            _repository.Setup(c => c.User.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).
                Returns(GetUsersForTests());
            _repository.Setup(c => c.EventCategory.FindByCondition(It.IsAny<Expression<Func<EventCategory, bool>>>())).
                Returns(GetEventsCategoriesForTests());
            _repository.Setup(c => c.EventType.FindByCondition(It.IsAny<Expression<Func<EventType, bool>>>())).
                Returns(GetEventTypesForTests());
            //action
            var controller = new EventUserController(_repository.Object, _usermanager.Object, _iCreateEventVM.Object);
            var result = controller.EventCreate();
            //assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(null, viewResult.ViewName);
            Assert.NotNull(viewResult);
        }
        [Fact]
        public void EventCreateMethodGetNotReturnError()
        {
            //arrange
            _repository.Setup(c => c.User.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).
                Returns(GetBadUserFields());
            _repository.Setup(c => c.EventCategory.FindByCondition(It.IsAny<Expression<Func<EventCategory, bool>>>())).
                Returns(GetBadEventsCategoriesForTests());
            _repository.Setup(c => c.EventType.FindByCondition(It.IsAny<Expression<Func<EventType, bool>>>())).
                Returns(GetBadEventsTypeForTests());
            //action
            var controller = new EventUserController(_repository.Object, _usermanager.Object, _iCreateEventVM.Object);
            var result = controller.EventCreate();
            //assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(null, viewResult.ViewName);
            Assert.NotNull(viewResult);
        }
        [Fact]
        public void EventUserPageNotnull()
        {
            //arrange
            _repository.Setup(c => c.Event.FindByCondition(It.IsAny<Expression<Func<Event, bool>>>())).
                Returns(GetEventsEorTest());
            _repository.Setup(c => c.User.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).
                Returns(GetUsersForTests());
            _repository.Setup(c => c.EventAdmin.FindByCondition(It.IsAny<Expression<Func<EventAdmin, bool>>>())).
                Returns(GetEventAdminsForTests());
            _repository.Setup(c => c.Participant.FindByCondition(It.IsAny<Expression<Func<Participant, bool>>>())).
                Returns(GetParticipantsForTests());
            //action
            var controller = new EventUserController(_repository.Object, _usermanager.Object, _iCreateEventVM.Object);
            var result = controller.EventUser();

            //assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }
        [Fact]
        public void EventUserPageReprezentation()
        {
            //arrange
            _repository.Setup(c => c.Event.FindByCondition(It.IsAny<Expression<Func<Event, bool>>>())).
                Returns(GetEventsEorTest());
            _repository.Setup(c => c.User.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).
                Returns(GetUsersForTests());
            _repository.Setup(c => c.EventAdmin.FindByCondition(It.IsAny<Expression<Func<EventAdmin, bool>>>())).
                Returns(GetEventAdminsForTests());
            _repository.Setup(c => c.Participant.FindByCondition(It.IsAny<Expression<Func<Participant, bool>>>())).
                Returns(GetParticipantsForTests());
            //action
            var controller = new EventUserController(_repository.Object, _usermanager.Object, _iCreateEventVM.Object);
            var result = controller.EventUser();

            //assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<EventUserViewModel>(viewResult.Model);
        }
        [Fact]
        public void EventUserPageIsNullReturnError()
        {
            //arrange
            _repository.Setup(c => c.Event.FindByCondition(It.IsAny<Expression<Func<Event, bool>>>())).
                Returns(GetBadEventFields());
            _repository.Setup(c => c.User.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).
                Returns(GetBadUserFields());
            _repository.Setup(c => c.EventAdmin.FindByCondition(It.IsAny<Expression<Func<EventAdmin, bool>>>())).
                Returns(GetBadEventAdminFields());
            _repository.Setup(c => c.Participant.FindByCondition(It.IsAny<Expression<Func<Participant, bool>>>())).
                Returns(GetBadParticipantFields());
            //action
            var controller = new EventUserController(_repository.Object, _usermanager.Object, _iCreateEventVM.Object);
            var result = controller.EventUser();

            //assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", redirectToActionResult.ActionName);
            Assert.NotNull(redirectToActionResult);
        }
        private IQueryable<EventCategory> GetEventsCategoriesForTests()
        {
            var eventCategory = new List<EventCategory>
            {
                new EventCategory
                {
                    ID = 1,
                    EventCategoryName = "Some Category"
                }
            }.AsQueryable();
            return eventCategory;
        }
        private IQueryable<EventCategory> GetBadEventsCategoriesForTests()
        {
            return null;
        }
        private IQueryable<EventType> GetEventTypesForTests()
        {
            var eventType = new List<EventType>
            {
                new EventType
                {
                    ID = 1,
                    EventTypeName = "Some Type"
                }
            }.AsQueryable();
            return eventType;
        }
        private IQueryable<EventType> GetBadEventsTypeForTests()
        {
            return null;
        }
        private IQueryable<Event> GetEventsEorTest()
        {
            var @event = new List<Event>{
                 new Event {
                     ID = 1,
                     EventName = "Some name",
                     EventCategoryID = 6,
                     EventTypeID = 6,
                     EventStatusID = 7,
                     EventDateStart = DateTime.Now,
                     EventDateEnd = DateTime.Now,
                     ForWhom = "Some people",
                     FormOfHolding = "Somewhare",
                     Description = "Some description",
                     Eventlocation = "Some plase",
                     Questions = "Some questions",
                     NumberOfPartisipants = 10,
                     EventAdmins = new List<EventAdmin>
                     {
                         new EventAdmin
                         {
                             UserID = "12345",
                             EventID = 1
                         }
                     }
                 }
            }.AsQueryable();
            return @event;
        }
        private IQueryable<User> GetUsersForTests()
        {
            var user = new List<User>
            {
                new User
                {
                    Id = "12345",
                    FirstName = "Orest",
                    LastName = "Onyshchenko",
                    ImagePath = "default.png"
                }
            }.AsQueryable();
            return user;
        }
        private IQueryable<EventAdmin> GetEventAdminsForTests()
        {
            var eventAdmin = new List<EventAdmin>
            {
                new EventAdmin
                {
                    UserID = "12345",
                    EventID = 1
                }
            }.AsQueryable();
            return eventAdmin;
        }
        private IQueryable<Participant> GetParticipantsForTests()
        {
            var partisipant = new List<Participant>
            {
                new Participant
                {
                    ID = 1,
                    EventId = 1,
                    UserId = "12345",
                    ParticipantStatusId = 1
                }
            }.AsQueryable();
            return partisipant;
        }
        private IQueryable<Event> GetBadEventFields()
        {
            return null;
        }
        private IQueryable<User> GetBadUserFields()
        {
            return null;
        }
        private IQueryable<EventAdmin> GetBadEventAdminFields()
        {
            return null;
        }
        private IQueryable<Participant> GetBadParticipantFields()
        {
            return null;
        }
    }
}
