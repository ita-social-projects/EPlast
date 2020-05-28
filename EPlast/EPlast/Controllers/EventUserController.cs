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

                var dto = _eventUserManager.InitializeEventCreateDTO();
                var model = _mapper.Map<EventCreateDTO, EventCreateViewModel>(dto);
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
                if (ModelState.IsValid)
                {
                    var createDto = _mapper.Map<EventCreateViewModel, EventCreateDTO>(createVM);
                    var eventId = _eventUserManager.CreateEvent(createDto);
                    return RedirectToAction("SetAdministration", new { id = eventId });
                }
                var dto = _eventUserManager.InitializeEventCreateDTO();
                var model = _mapper.Map<EventCreateDTO, EventCreateViewModel>(dto);
                return View(model);
            }
            catch
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [HttpGet]
        public IActionResult SetAdministration(int id)
        {
            var dto = _eventUserManager.InitializeEventCreateDTO(id);
            var model = _mapper.Map<EventCreateDTO, EventCreateViewModel>(dto);
            return View(model);
        }

        [HttpPost]
        public IActionResult SetAdministration(EventCreateViewModel createVM)
        {
            if (ModelState.IsValid)
            {
                var dto1 = _mapper.Map<EventCreateViewModel, EventCreateDTO>(createVM);
                _eventUserManager.SetAdministration(dto1);
                return RedirectToAction("EventInfo", "Action", new { id = createVM.Event.ID });
            }
            var dto2 = _eventUserManager.InitializeEventCreateDTO(createVM.Event.ID);
            var model = _mapper.Map<EventCreateDTO, EventCreateViewModel>(dto2);
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