using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.AboutBase;
using EPlast.BLL.Interfaces.AboutBase;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using EPlast.WebApi.Models.AboutBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLog.Extensions.Logging;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AboutBaseController : ControllerBase
    {
        private readonly IAboutBaseSectionService _aboutBaseSectionService;
        private readonly IAboutBaseSubsectionService _aboutBaseSubsectionService;
        private readonly UserManager<User> _userManager;
        private readonly IPicturesManager _picturesManager;

        public AboutBaseController(
            IAboutBaseSectionService aboutBaseSectionService,
            IAboutBaseSubsectionService aboutBaseSubsectionService,
            UserManager<User> userManager,
            IPicturesManager picturesManager)
        {
            _aboutBaseSectionService = aboutBaseSectionService;
            _aboutBaseSubsectionService = aboutBaseSubsectionService;
            _userManager = userManager;
            _picturesManager = picturesManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GET()
        {
            return Ok(ConfigSettingLayoutRenderer.DefaultConfiguration.GetSection("Jwt")["Audience"]);
        }

        [HttpGet("AboutBaseSection/{id}")]
        public async Task<IActionResult> GetAboutBaseSection(int id)
        {
            SectionDto sectionDTO = await _aboutBaseSectionService.GetSection(id);
            if (sectionDTO == null)
                return NotFound();
            return Ok(sectionDTO);
        }

        [HttpGet("AboutBaseSections")]
        public async Task<IActionResult> GetAboutBaseSections()
        {
            IEnumerable<SectionDto> sectionDTOs = await _aboutBaseSectionService.GetAllSectionAsync();
            return Ok(sectionDTOs);
        }

        [HttpGet("AboutBaseSubsection/{id}")]
        public async Task<IActionResult> GetAboutBaseSubsection(int id)
        {
            SubsectionDto subsectionDTO = await _aboutBaseSubsectionService.GetSubsection(id);
            if (subsectionDTO == null)
                return NotFound();
            return Ok(subsectionDTO);
        }

        [HttpGet("AboutBaseSubsections")]
        public async Task<IActionResult> GetAboutBaseSubsections()
        {
            IEnumerable<SubsectionDto> subsectionDTOs = await _aboutBaseSubsectionService.GetAllSubsectionAsync();
            return Ok(subsectionDTOs);
        }

        /// <summary>
        /// Get pictures in Base64 format by subsection Id.
        /// </summary>
        /// <returns>List of pictures in Base64 format.</returns>
        /// <param name="subsectionId">The Id of subsection</param>
        /// <response code="200">List of pictures</response>
        /// <response code="400">Server could not understand the request due to invalid syntax</response> 
        [HttpGet("{subsectionId:int}/pictures")]
        public async Task<IActionResult> GetPictures(int subsectionId)
        {
            var pictures = await _picturesManager.GetPicturesAsync(subsectionId);

            return Ok(pictures);
        }

        [HttpDelete("DeleteSection/{id}")]
        public async Task<IActionResult> DeleteAboutBaseSection(int id)
        {
            try
            {
                await _aboutBaseSectionService.DeleteSection(id, await _userManager.GetUserAsync(User));
                return NoContent();
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        [HttpDelete("DeleteSubsection/{id}")]
        public async Task<IActionResult> DeleteAboutBaseSubsection(int id)
        {
            try
            {
                await _aboutBaseSubsectionService.DeleteSubsection(id, await _userManager.GetUserAsync(User));
                return NoContent();
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Delete picture by Id.
        /// </summary>
        /// <returns>Status code of the picture deleting operation.</returns>
        /// <param name="pictureId">The Id of picture</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response> 
        /// <response code="404">Not Found</response> 
        [HttpDelete("pictures/{pictureId:int}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeletePicture(int pictureId)
        {
            return StatusCode(await _picturesManager.DeletePictureAsync(pictureId));
        }

        [HttpPost("AboutBaseSection/Create")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> AddAboutBaseSection(SectionDto sectionDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _aboutBaseSectionService.AddSection(sectionDTO, await _userManager.GetUserAsync(User));
                    return NoContent();
                }
                catch
                {
                    return NotFound();
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("AboutBaseSubsection/Create")]
        public async Task<IActionResult> AddAboutBaseSubsection(SubsectionDto subsectionDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _aboutBaseSubsectionService.AddSubsection(subsectionDTO, await _userManager.GetUserAsync(User));
                    return NoContent();
                }
                catch
                {
                    return NotFound();
                }
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Add pictures to gallery of specific subsection by subsection Id.
        /// </summary>
        /// <returns>List of added pictures.</returns>
        /// <param name="subsectionId">The Id of subsection</param>
        /// <param name="files">List of uploaded pictures</param>
        /// <response code="200">List of added pictures</response>
        /// <response code="400">Server could not understand the request due to invalid syntax</response> 
        [HttpPost("{subsectionId:int}/subsectionPictures")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> FillSubsectionPictures(int subsectionId, [FromForm] IList<IFormFile> files)
        {
            return Ok(await _picturesManager.FillSubsectionPicturesAsync(subsectionId, files));
        }

        [HttpPut("EditSection/{id}")]
        public async Task<IActionResult> EditAboutBaseSection(SectionDto sectionDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _aboutBaseSectionService.ChangeSection(sectionDTO, await _userManager.GetUserAsync(User));
                    return NoContent();
                }
                catch (NullReferenceException)
                {
                    return NotFound();
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPut("EditSubsection/{id}")]
        public async Task<IActionResult> EditAboutBaseSubsection(SubsectionDto subsectionDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _aboutBaseSubsectionService.ChangeSubsection(subsectionDTO, await _userManager.GetUserAsync(User));
                    return NoContent();
                }
                catch (NullReferenceException)
                {
                    return NotFound();
                }
            }
            return BadRequest(ModelState);
        }
    }
}
