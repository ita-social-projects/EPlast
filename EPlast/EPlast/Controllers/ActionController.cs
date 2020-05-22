using EPlast.ViewModels.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AutoMapper;
using EPlast.BussinessLayer.DTO.Events;
using EPlast.BussinessLayer.Interfaces;
using EPlast.BussinessLayer.Interfaces.Events;


namespace EPlast.Controllers
{
    public class ActionController : Controller
    {
        private readonly IActionManager _actionManager;
        private readonly IMapper _mapper;

        public ActionController(IActionManager actionManager, IMapper mapper)
        {
            _actionManager = actionManager;
            _mapper = mapper;
        }

        [Authorize]
        public IActionResult GetAction()
        {
            try
            {
                var dto = _actionManager.GetActionCategories();
                var model = _mapper.Map<List<EventCategoryDTO>, List<EventCategoryViewModel>>(dto);
                return View(model);
            }
            catch
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [Authorize]
        public IActionResult Events(int ID)
        {
            try
            {
                var dto = _actionManager.GetEvents(ID, User);
                var model = _mapper.Map<List<GeneralEventDTO>, List<GeneralEventViewModel>>(dto);
                return View(model);
            }
            catch
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }


        [Authorize]
        public IActionResult EventInfo(int ID)
        {
            try
            {
                var dto = _actionManager.GetEventInfo(ID, User);
                var model = _mapper.Map<EventDTO, EventViewModel>(dto);
                return View(model);
            }
            catch
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult DeleteEvent(int ID)
        {
            var code = _actionManager.DeleteEvent(ID);
            return StatusCode(code);
        }

        [HttpPost]
        [Authorize]
        public IActionResult DeletePicture(int ID)
        {
            var code = _actionManager.DeletePicture(ID);
            return StatusCode(code);
        }

        [HttpPost]
        [Authorize]
        public IActionResult SubscribeOnEvent(int ID)
        {
            var code = _actionManager.SubscribeOnEvent(ID, User);
            return StatusCode(code);
        }

        [HttpPost]
        [Authorize]
        public IActionResult UnSubscribeOnEvent(int ID)
        {
            var code = _actionManager.UnSubscribeOnEvent(ID, User);
            return StatusCode(code);
        }


        [Authorize]
        public IActionResult ApproveParticipant(int ID)
        {
            var code = _actionManager.ApproveParticipant(ID);
            return StatusCode(code);

        }

        [Authorize]
        public IActionResult UndetermineParticipant(int ID)
        {
            var code = _actionManager.UnderReviewParticipant(ID);
            return StatusCode(code);
        }

        [Authorize]
        public IActionResult RejectParticipant(int ID)
        {
            var code = _actionManager.RejectParticipant(ID);
            return StatusCode(code);
        }

        [HttpPost]
        public IActionResult FillEventGallery(int ID, IList<IFormFile> files)
        {
            var code = _actionManager.FillEventGallery(ID, files);
            return StatusCode(code);
        }
    }
}