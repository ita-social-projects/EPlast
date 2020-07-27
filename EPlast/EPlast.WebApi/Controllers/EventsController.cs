using EPlast.BLL.Interfaces.Events;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("types")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetTypes()
        {
            try
            {
                return Ok(await _actionManager.GetEventTypesAsync());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("types/{typeId:int}/categories")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetCategories(int typeId)
        {
            try
            {
                return Ok(await _actionManager.GetCategoriesByTypeIdAsync(typeId));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("~/api/types/{typeId:int}/categories/{categoryId:int}/events")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetEvents(int typeId, int categoryId)
        {
            try
            {
                var model = await _actionManager.GetEventsAsync(categoryId, typeId, User);

                return Ok(model);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id:int}/details")]
        [Authorize(AuthenticationSchemes = "Bearer")]
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
        [Authorize(AuthenticationSchemes = "Bearer")]
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
        [Authorize(AuthenticationSchemes = "Bearer")]
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
        [Authorize(AuthenticationSchemes = "Bearer")]
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
        [Authorize(AuthenticationSchemes = "Bearer")]
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
        [Authorize(AuthenticationSchemes = "Bearer")]
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
        [Authorize(AuthenticationSchemes = "Bearer")]
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
        [Authorize(AuthenticationSchemes = "Bearer")]
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
        [Authorize(AuthenticationSchemes = "Bearer")]
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
