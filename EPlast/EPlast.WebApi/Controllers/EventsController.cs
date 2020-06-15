using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BussinessLayer.DTO.Events;
using EPlast.BussinessLayer.Interfaces.Events;
using EPlast.WebApi.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("authors")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var model = await _actionManager.GetActionCategoriesAsync();
                return Ok(model);
            }
            catch
            {
                return BadRequest();
            }
        }

        //[HttpGet]
        //public async Task<IActionResult> GetEventsByCategoryId(int categoryId)
        //{
        //    try
        //    {
        //        var model = await _actionManager.GetEventsAsync(categoryId, User);

        //        return Ok(model);
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //}

        //[HttpGet]
        //public async Task<IActionResult> Get(int id)
        //{
        //    try
        //    {
        //        var model = await _actionManager.GetEventInfoAsync(id, User);

        //        return Ok(model);
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //}

        //[HttpDelete]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    try
        //    {
        //        var code = await _actionManager.DeleteEventAsync(id);

        //        return StatusCode(code);
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //}

        //[HttpDelete]
        //public async Task<IActionResult> DeletePicture(int id)
        //{
        //    try
        //    {
        //        var code = await _actionManager.DeletePictureAsync(id);
        //        return StatusCode(code);
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //}

        //[HttpPost("/vouchers/client/{clientId}/voucher/{voucherId}/comments")]
        //public async Task<IActionResult> SubscribeOnEvent(int id)
        //{
        //    try
        //    {
        //        var code = await _actionManager.SubscribeOnEventAsync(id, User);
        //        return StatusCode(code);
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //}

        //[HttpDelete]
        //public async Task<IActionResult> UnSubscribeOnEvent(int id)
        //{
        //    try
        //    {
        //        var code = await _actionManager.UnSubscribeOnEventAsync(id, User);
        //        return StatusCode(code);
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //}

        //[HttpPut]
        //public async Task<IActionResult> ApproveParticipant(int id)
        //{
        //    try
        //    {
        //        var code = await _actionManager.ApproveParticipantAsync(id);
        //        return StatusCode(code);
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //}

        //[HttpPut]
        //public async Task<IActionResult> UndetermineParticipant(int id)
        //{
        //    try
        //    {
        //        var code = await _actionManager.UnderReviewParticipantAsync(id);
        //        return StatusCode(code);
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //}

        //[HttpPut]
        //public async Task<IActionResult> RejectParticipant(int id)
        //{
        //    try
        //    {
        //        var code = await _actionManager.RejectParticipantAsync(id);
        //        return StatusCode(code);
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> FillEventGallery(int id, IList<IFormFile> files)
        //{
        //    try
        //    {
        //        var code = await _actionManager.FillEventGalleryAsync(id, files);
        //        return StatusCode(code);
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //}
    }
}
