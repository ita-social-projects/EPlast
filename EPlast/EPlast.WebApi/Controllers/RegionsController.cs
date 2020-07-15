﻿using AutoMapper;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Interfaces.Region;
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
        private readonly IMapper _mapper;
        private readonly IRegionService _regionService;

        public RegionsController(ILoggerService<CitiesController> logger,
            IMapper mapper,
            IRegionService regionService)
        {
            _logger = logger;
            _mapper = mapper;
            _regionService = regionService;
        }

        [HttpGet("Profiles")]
        public async Task<IActionResult> Index()
        {
            var regions = await _regionService.GetAllRegionsAsync();

            return Ok(regions);
        }

        [HttpGet("Profile/{regionId}")]
        public async Task<IActionResult> GetProfile(int regionId)  // ПЕРЕВІРИТИ ВСЕ, добавляння адміна\видалення і регіони
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
    }
}
