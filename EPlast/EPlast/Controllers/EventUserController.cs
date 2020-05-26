using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Models.ViewModelInitializations.Interfaces;
using EPlast.ViewModels.Events;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EPlast.BussinessLayer.DTO.EventUser;
using EPlast.BussinessLayer.Interfaces.EventUser;
using EPlast.ViewModels.EventUser;

namespace EPlast.Controllers
{
    public class EventUserController : Controller
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;
        private readonly ICreateEventVMInitializer _createEventVMInitializer;
        private readonly IEventUserManager _eventUserManager;
        private readonly IMapper _mapper;



        public EventUserController(IRepositoryWrapper repoWrapper, UserManager<User> userManager, ICreateEventVMInitializer createEventVMInitializer, IEventUserManager eventUserManager, IMapper mapper)

        {
            _userManager = userManager;
            _repoWrapper = repoWrapper;
            _createEventVMInitializer = createEventVMInitializer;
            _eventUserManager = eventUserManager;
            _mapper = mapper;
        }

        public IActionResult EventUser(string userId)
        {
            try
            {
                var dto = _eventUserManager.EventUser(userId, User);
                var model = _mapper.Map<EventUserDTO, EventUserViewModel>(dto);
                return View(model);
            }
            catch
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [HttpGet]
        public IActionResult EventCreate()
        {
            try
            {
                var eventCategories = _repoWrapper.EventCategory.FindAll();
                var model = new EventCreateViewModel()
                {
                    Users = _repoWrapper.User.FindAll(),
                    EventTypes = _repoWrapper.EventType.FindAll(),
                    EventCategories = _createEventVMInitializer.GetEventCategories(eventCategories)
                };
                return View(model);
            }
            catch
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }
        [HttpPost]
        public IActionResult EventCreate(EventCreateViewModel createVM)
        {
            try
            {
                EventStatus status = _repoWrapper.EventStatus.
                    FindByCondition(i => i.EventStatusName == "Не затверджені").
                    FirstOrDefault();
                createVM.Event.EventStatusID = status.ID;
                EventAdmin eventAdmin = new EventAdmin()
                {
                    Event = createVM.Event,
                    UserID = createVM.EventAdmin.UserID
                };
                EventAdministration eventAdministration = new EventAdministration()
                {
                    Event = createVM.Event,
                    AdministrationType = "Бунчужний/на",
                    UserID = createVM.EventAdministration.UserID
                };
                if (ModelState.IsValid)
                {
                    _repoWrapper.EventAdmin.Create(eventAdmin);
                    _repoWrapper.EventAdministration.Create(eventAdministration);
                    _repoWrapper.Event.Create(createVM.Event);
                    _repoWrapper.Save();
                    return RedirectToAction("SetAdministration", new { idUser = createVM.EventAdmin.UserID, id = createVM.Event.ID });
                }
                else
                {
                    var eventCategories = _repoWrapper.EventCategory.FindAll();
                    var model = new EventCreateViewModel()
                    {
                        Users = _repoWrapper.User.FindAll(),
                        EventTypes = _repoWrapper.EventType.FindAll(),
                        EventCategories = _createEventVMInitializer.GetEventCategories(eventCategories)
                    };
                    return View(model);
                }
            }
            catch
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }
        [HttpGet]
        public IActionResult SetAdministration(string idUser, int id)
        {
            var model = new EventCreateViewModel()
            {
                Event = _repoWrapper.Event.
                FindByCondition(i => i.ID == id).
                FirstOrDefault(),
                Users = _repoWrapper.User.FindByCondition(i => i.Id != idUser)
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult SetAdministration(EventCreateViewModel createVM)
        {
            EventAdmin eventAdmin = new EventAdmin()
            {
                EventID = createVM.Event.ID,
                UserID = createVM.EventAdmin.UserID
            };
            EventAdministration eventAdministration = new EventAdministration()
            {
                EventID = createVM.Event.ID,
                AdministrationType = "Писар",
                UserID = createVM.EventAdministration.UserID
            };
            if (ModelState.IsValid)
            {
                _repoWrapper.EventAdmin.Create(eventAdmin);
                _repoWrapper.EventAdministration.Create(eventAdministration);
                _repoWrapper.Save();
                return RedirectToAction("EventInfo", "Action", new { id = createVM.Event.ID });
            }
            Event events = _repoWrapper.Event.FindByCondition(i => i.ID == createVM.Event.ID).FirstOrDefault();
            var model = new EventCreateViewModel()
            {
                Event = events,
                Users = _repoWrapper.User.FindAll()
            };
            return View(model);
        }

        //[HttpGet]
        //public IActionResult EventEdit(int id)
        //{
        //    var @event = _repoWrapper.Event.
        //        FindByCondition(q => q.ID == id).
        //        Include(i => i.EventType).
        //        Include(g => g.EventStatus).
        //        Include(g => g.EventGallarys).
        //        Include(g => g.EventCategory).
        //        Include(g => g.EventAdmins).
        //        Include(g => g.EventAdministrations).
        //        FirstOrDefault();
        //    var eventCategories = _repoWrapper.EventCategory.FindAll();
        //    var model = new EventCreateViewModel()
        //    {
        //        Users = _repoWrapper.User.FindAll(),
        //        Event = @event,
        //        EventTypes = _repoWrapper.EventType.FindAll(),
        //        EventCategories = _createEventVMInitializer.GetEventCategories(eventCategories)
        //    };
        //    return View(model);
        //}

        //[HttpPost]
        //public IActionResult EventEdit(EventCreateViewModel model)
        //{
        //    _repoWrapper.Event.Update(model.Event);
        //    _repoWrapper.Save();
        //    return RedirectToAction("EventInfo", "Action", new { id = model.Event.ID });
        //}
    }
}