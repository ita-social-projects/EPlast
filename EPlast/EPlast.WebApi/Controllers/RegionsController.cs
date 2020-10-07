﻿using AutoMapper;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Interfaces.Region;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly ILoggerService<CitiesController> _logger;
        private readonly IRegionService _regionService;
        private readonly IRegionAdministrationService _regionAdministrationService;



        public RegionsController(ILoggerService<CitiesController> logger,
            IRegionService regionService,
            IRegionAdministrationService regionAdministrationService)
        {
            _logger = logger;
            _regionService = regionService;
            _regionAdministrationService = regionAdministrationService;
        }

        [HttpGet("Profiles")]
        public async Task<IActionResult> Index()
        {
            var regions = await _regionService.GetAllRegionsAsync();

            return Ok(regions);
        }

        [HttpPost("AddRegion")]
        public async Task<IActionResult> CreateRegion(RegionDTO region)
        {

           await  _regionService.AddRegionAsync(region);

            return Ok();
        }

        [HttpPut("EditRegion/{regId}")]
        public async Task<IActionResult> EditRegion(int regId, RegionDTO region)
        {
            await _regionService.EditRegionAsync(regId, region);

            return Ok();
        }




        [HttpGet("GetAdministration/{regionId}")]
        public async Task<IActionResult> GetRegionAdmins(int regionId)
        {
            var Admins = await _regionService.GetAdministrationAsync(regionId);
            return Ok(Admins);
        }




        [HttpGet("Profile/{regionId}")]
        public async Task<IActionResult> GetProfile(int regionId)
        {
            try
            {
                var region = await _regionService.GetRegionProfileByIdAsync(regionId);
                if (region == null)
                {
                    return NotFound();
                }

                return Ok(region);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        [HttpPost("AddAdministrator")]
        public async Task<IActionResult> AddAdministrator(RegionAdministrationDTO admin)
        {
                await _regionService.AddRegionAdministrator(admin);

                return Ok();
            
        }


        [HttpDelete("RemoveAdministration/{Id}")]
        public async Task<IActionResult> Remove(int Id)
        {
            await _regionService.DeleteAdminByIdAsync(Id);
            return Ok();
        }


        [HttpPost("AddDocument")]
        public async Task<IActionResult> AddDocument(RegionDocumentDTO document)
        {
            await _regionService.AddDocumentAsync(document);
            _logger.LogInformation($"Document with id {{{document.ID}}} was added.");

            return Ok(document);
        }


        [HttpDelete("RemoveRegion/{Id}")]
        public async Task<IActionResult> RemoveAdmin(int Id)
        {
            await _regionService.DeleteRegionByIdAsync(Id);
            return Ok();
        }

        [HttpGet("GetUserAdministrations/{userId}")]
        public async Task<IActionResult> GetUserAdministrations(string userId)
        {
           var secretaries=await _regionService.GetUsersAdministrations(userId);
            return Ok(secretaries);

        }


        [HttpDelete("RemoveDocument/{documentId}")]
        public async Task<IActionResult> RemoveDocument(int documentId)
        {
            await _regionService.DeleteFileAsync(documentId);
            _logger.LogInformation($"Document with id {{{documentId}}} was deleted.");

            return Ok();
        }




        [HttpGet("FileBase64/{fileName}")]
        public async Task<IActionResult> GetFileBase64(string fileName)
        {
            var fileBase64 = await _regionService.DownloadFileAsync(fileName);

            return Ok(fileBase64);
        }



        [HttpGet("getDocs/{regionId}")]
        public async Task<IActionResult> GetRegionDocs(int regionId)
        {
            var secretaries = await _regionService.GetRegionDocsAsync(regionId);
            return Ok(secretaries);

        }


        [HttpPost("AddFollower/{regionId}/{cityId}")]
        public async Task<IActionResult> AddFollower(int regionId, int cityId)
        {
            await _regionService.AddFollowerAsync(regionId, cityId);
            return Ok();
        }


        [HttpGet("GetMembers/{regionId}")]
        public async Task<IActionResult> GetMembers(int regionId)
        {
          var members =   await _regionService.GetMembersAsync(regionId);
            return Ok(members);
        }

        [HttpGet("GetAdminTypes")]
        public async Task<IActionResult> GetAdminTypes()
        {
            var types = await _regionService.GetAdminTypes();
            return Ok(types);
        }

    }
}
