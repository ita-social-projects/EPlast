using EPlast.BLL.Interfaces.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IActionManager _actionManager;
        public EventsController(IActionManager actionManager)
        {
            _actionManager = actionManager;
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                return Ok(await _actionManager.GetActionCategoriesAsync());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("~/api/categories/{categoryId:int}/events")]
        public async Task<IActionResult> GetEventsByCategoryId(int categoryId)
        {
            try
            {
                var model = await _actionManager.GetEventsAsync(categoryId, User);

                return Ok(model);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id:int}/details")]
        public async Task<IActionResult> GetEventDetail(int id)
        {
            try
            {
                var model = await _actionManager.GetEventInfoAsync(id, User);

                return Ok(model);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var code = await _actionManager.DeleteEventAsync(id);
                return StatusCode(code);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("pictures/{pictureId:int}")]
        public async Task<IActionResult> DeletePicture(int pictureId)
        {
            try
            {
                var code = await _actionManager.DeletePictureAsync(pictureId);
                return StatusCode(code);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("{eventId:int}/participants")]
        public async Task<IActionResult> SubscribeOnEvent(int eventId)
        {
            try
            {
                var code = await _actionManager.SubscribeOnEventAsync(eventId, User);
                return StatusCode(code);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{eventId:int}/participants")]
        public async Task<IActionResult> UnSubscribeOnEvent(int eventId)
        {
            try
            {
                var code = await _actionManager.UnSubscribeOnEventAsync(eventId, User);
                return StatusCode(code);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("participants/{participantId:int}/status/approved")]
        public async Task<IActionResult> ApproveParticipant(int participantId)
        {
            try
            {
                var code = await _actionManager.ApproveParticipantAsync(participantId);
                return StatusCode(code);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("participants/{participantId:int}/status/underReviewed")]
        public async Task<IActionResult> UnderReviewParticipant(int participantId)
        {
            try
            {
                var code = await _actionManager.UnderReviewParticipantAsync(participantId);
                return StatusCode(code);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("participants/{participantId:int}/status/rejected")]
        public async Task<IActionResult> RejectParticipant(int participantId)
        {
            try
            {
                var code = await _actionManager.RejectParticipantAsync(participantId);
                return StatusCode(code);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("{eventId:int}/eventGallery")]
        public async Task<IActionResult> FillEventGallery(int eventId, [FromForm] IList<IFormFile> files)
        {
            try
            {
                var code = await _actionManager.FillEventGalleryAsync(eventId, files);
                return StatusCode(code);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
