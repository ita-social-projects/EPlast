using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.GoverningBody;
using EPlast.BLL.DTO.GoverningBody.Announcement;
using EPlast.BLL.Interfaces.Announcements;
using EPlast.BLL.Interfaces.GoverningBodies;
using EPlast.BLL.Interfaces.Logging;
using EPlast.Resources;
using EPlast.WebApi.Models.GoverningBody;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementsController : ControllerBase
    {
        private readonly IAnnouncemetsService _announcementService;

        public AnnouncementsController(IAnnouncemetsService announcementService)
        {
            _announcementService = announcementService;
        }

        [HttpGet("GetAnnouncement/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            GoverningBodyAnnouncementUserWithImagesDTO governingBodyAnnouncementUserDTO = await _announcementService.GetAnnouncementByIdAsync(id);

            if (governingBodyAnnouncementUserDTO == null)
            {
                return NotFound();
            }
            return Ok(governingBodyAnnouncementUserDTO);
        }


        /// <summary>
        /// Get specified by page number and page size list of announcements
        /// </summary>
        /// <param name="pageNumber">Number of the page</param>
        /// <param name="pageSize">Size of one page</param>
        /// <returns>Specified by page number and page size list of announcements</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="400">Could not get requested announcements</response>
        [HttpGet("GetAnnouncementsByPage/{pageNumber:int}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAnnouncementsByPage(int pageNumber, [Required] int pageSize)
        {
            var announcements = await _announcementService.GetAnnouncementsByPageAsync(pageNumber, pageSize);

            return Ok(announcements);
        }

        [HttpPut("PinAnnouncement/{id:int}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndGBHead)]
        public async Task<IActionResult> PinAnnouncement(int id)
        {
            if (ModelState.IsValid)
            {
                var ann = await _announcementService.PinAnnouncementAsync(id);
                if (ann == null)
                    return BadRequest();
                return Ok(id);
            }
            return BadRequest(ModelState);
        }

    }
}
