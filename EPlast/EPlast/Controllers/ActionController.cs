using AutoMapper;
using EPlast.BLL.DTO.Events;
using EPlast.BLL.Interfaces.Events;
using EPlast.ViewModels.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetAction()
        {
            try
            {
                var dto = await _actionManager.GetActionCategoriesAsync();
                var model = _mapper.Map<IEnumerable<EventCategoryDTO>, IEnumerable<EventCategoryViewModel>>(dto);
                return View(model);
            }
            catch
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [Authorize]
        public async Task<IActionResult> Events(int id)
        {
            try
            {
                var dto = await _actionManager.GetEventsAsync(id, User);
                var model = _mapper.Map<List<GeneralEventDTO>, List<GeneralEventViewModel>>(dto);
                return View(model);
            }
            catch
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }


        [Authorize]
        public async Task<IActionResult> EventInfo(int id)
        {
            try
            {
                var dto = await _actionManager.GetEventInfoAsync(id, User);
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
        public async Task<IActionResult> DeleteEvent(int id)
        {
            try
            {
                var code = await _actionManager.DeleteEventAsync(id);
                return StatusCode(code);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeletePicture(int id)
        {
            try
            {
                var code = await _actionManager.DeletePictureAsync(id);
                return StatusCode(code);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SubscribeOnEvent(int id)
        {
            try
            {
                var code = await _actionManager.SubscribeOnEventAsync(id, User);
                return StatusCode(code);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UnSubscribeOnEvent(int id)
        {
            try
            {
                var code = await _actionManager.UnSubscribeOnEventAsync(id, User);
                return StatusCode(code);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [Authorize]
        public async Task<IActionResult> ApproveParticipant(int id)
        {
            try
            {
                var code = await _actionManager.ApproveParticipantAsync(id);
                return StatusCode(code);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        public async Task<IActionResult> UndetermineParticipant(int id)
        {
            try
            {
                var code = await _actionManager.UnderReviewParticipantAsync(id);
                return StatusCode(code);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        public async Task<IActionResult> RejectParticipant(int id)
        {
            try
            {
                var code = await _actionManager.RejectParticipantAsync(id);
                return StatusCode(code);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> FillEventGallery(int id, IList<IFormFile> files)
        {
            try
            {
                var code =await _actionManager.FillEventGalleryAsync(id, files);
                return StatusCode(code);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}