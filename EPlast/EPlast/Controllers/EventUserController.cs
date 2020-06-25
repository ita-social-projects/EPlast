using AutoMapper;
using EPlast.BussinessLayer.DTO.EventUser;
using EPlast.BussinessLayer.Interfaces.EventUser;
using EPlast.ViewModels.EventUser;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EPlast.Controllers
{
    public class EventUserController : Controller
    {
        private readonly IEventUserManager _eventUserManager;
        private readonly IMapper _mapper;

        public EventUserController(IEventUserManager eventUserManager, IMapper mapper)
        {
            _eventUserManager = eventUserManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> EventUser(string userId)
        {
            try
            {
                var dto = await _eventUserManager.EventUserAsync(userId, User);
                var model = _mapper.Map<EventUserDTO, EventUserViewModel>(dto);

                return View(model);
            }
            catch
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EventCreate()
        {
            try
            {
                var dto = await _eventUserManager.InitializeEventCreateDTOAsync();
                var model = _mapper.Map<EventCreateDTO, EventCreateViewModel>(dto);

                return View(model);
            }
            catch
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }
        [HttpPost]
        public async Task<IActionResult> EventCreate(EventCreateViewModel createVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // проблема з мапінгом
                    var createDto = _mapper.Map<EventCreateViewModel, EventCreateDTO>(createVM);
                    await _eventUserManager.CreateEventAsync(createDto);

                    return RedirectToAction("EventInfo", "Action", new { id = createDto.Event.ID });
                }
                var dto = await _eventUserManager.InitializeEventCreateDTOAsync();
                var model = _mapper.Map<EventCreateDTO, EventCreateViewModel>(dto);

                return View(model);
            }
            catch(Exception e)
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EventEdit(int eventId)
        {
            try
            {
                var dto = await _eventUserManager.InitializeEventEditDTOAsync(eventId);
                var model = _mapper.Map<EventCreateDTO, EventCreateViewModel>(dto);

                return View(model);
            }
            catch(Exception e)
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EventEdit(EventCreateViewModel createVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _eventUserManager.EditEventAsync(_mapper.Map<EventCreateViewModel, EventCreateDTO>(createVM));

                    return RedirectToAction("EventInfo", "Action", new { id = createVM.Event.ID });
                }
                else
                {
                    var dto = await _eventUserManager.InitializeEventEditDTOAsync(createVM.Event.ID);
                    var model = _mapper.Map<EventCreateDTO, EventCreateViewModel>(dto);

                    return View(model);
                }
            }
            catch
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }


    }
}