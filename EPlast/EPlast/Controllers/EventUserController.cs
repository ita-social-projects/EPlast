using AutoMapper;
using EPlast.BussinessLayer.DTO.EventUser;
using EPlast.BussinessLayer.Interfaces.EventUser;
using EPlast.ViewModels.EventUser;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public IActionResult EventEdit(int eventId)
        {
            try
            {
                var dto = _eventUserManager.InitializeEventEditDTO(eventId);
                var model = _mapper.Map<EventCreateDTO, EventCreateViewModel>(dto);
                return View(model);
            }
            catch
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [HttpPost]
        public IActionResult EventEdit(EventCreateViewModel createVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _eventUserManager.EditEvent(_mapper.Map<EventCreateViewModel, EventCreateDTO>(createVM));
                    return RedirectToAction("EventUser", new { id = createVM.Event.ID });
                }
                else
                {
                    var dto = _eventUserManager.InitializeEventEditDTO(createVM.Event.ID);
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