using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.DTO;
using EPlast.WebApi.Models.AboutBase;
using Microsoft.AspNetCore.Authorization;
using EPlast.Resources;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces.AboutBase;
using Microsoft.AspNetCore.Identity;
using EPlast.DataAccess.Entities;
using EPlast.BLL.DTO.AboutBase;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AboutBaseController : ControllerBase
    {
        private readonly IAboutBaseSectionService _aboutBaseSectionService;
        private readonly IAboutBaseSubsectionService _aboutBaseSubsectionService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public AboutBaseController(
            IAboutBaseSectionService aboutBaseSectionService, 
            IAboutBaseSubsectionService aboutBaseSubsectionService, 
            IMapper mapper,
            UserManager<User> userManager)
        {
            _aboutBaseSectionService = aboutBaseSectionService;
            _aboutBaseSubsectionService = aboutBaseSubsectionService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet("AboutBaseSection/{id}")]
        public async Task<IActionResult> GetAboutBaseSection(int id)
        {
            SectionDTO sectionDTO = await _aboutBaseSectionService.GetSection(id);
            if (sectionDTO == null)
                return NotFound();
            return Ok(sectionDTO);
        }

        [HttpGet("AboutBaseSections")]
        public async Task<IActionResult> GetAboutBaseSections()
        {
            IEnumerable<SectionDTO> sectionDTOs = await _aboutBaseSectionService.GetAllSectionAsync();
            return Ok(sectionDTOs);
        }

        [HttpGet("AboutBaseSubsection/{id}")]
        public async Task<IActionResult> GetAboutBaseSubsection(int id)
        {
            SubsectionDTO subsectionDTO = await _aboutBaseSubsectionService.GetSubsection(id);
            if (subsectionDTO == null)
                return NotFound();
            return Ok(subsectionDTO);
        }

        [HttpGet("AboutBaseSubsections")]
        public async Task<IActionResult> GetAboutBaseSubsections()
        {
            IEnumerable<SubsectionDTO> subsectionDTOs = await _aboutBaseSubsectionService.GetAllSubsectionAsync();
            return Ok(subsectionDTOs);
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

        [HttpDelete("DeleveSubsection/{id}")]
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

        [HttpPost("AboutBaseSection/Create/{id}")]
        public async Task<IActionResult> AddAboutBaseSection(SectionDTO sectionDTO)
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

        [HttpPost("AboutBaseSubsection/Create/{id}")]
        public async Task<IActionResult> AddAboutBaseSubsection(SubsectionDTO subsectionDTO)
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

        [HttpPut("EditSection/{id}")]
        public async Task<IActionResult> EditAboutBaseSection(SectionDTO sectionDTO)
        {
            if(ModelState.IsValid)
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
        public async Task<IActionResult> EditAboutBaseSubsection(SubsectionDTO subsectionDTO)
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
