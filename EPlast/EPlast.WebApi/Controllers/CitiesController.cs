using AutoMapper;
using EPlast.BussinessLayer.DTO.City;
using EPlast.BussinessLayer.Interfaces.City;
using EPlast.BussinessLayer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ILoggerService<CitiesController> _logger;
        private readonly ICityService _cityService;
       
        public CitiesController(ILoggerService<CitiesController> logger, ICityService cityService)
        {
            _logger = logger;
            _cityService = cityService;
        }

        [HttpGet("Profile/{cityId}")]
        public async Task<IActionResult> GetProfile(int cityId)
        {
            try
            {
                CityProfileDTO cityProfileDto = await _cityService.CityProfileAsync(cityId);
                if (cityProfileDto == null)
                {
                    return NotFound();
                }
                return Ok(cityProfileDto);

            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }
        [HttpGet("Members/{cityId}")]
        public async Task<IActionResult> GetMembers(int cityId)
        {
            try
            {
                CityProfileDTO cityProfileDto = await _cityService.CityMembersAsync(cityId);
                if (cityProfileDto == null)
                {
                    return NotFound();
                }

                return Ok(cityProfileDto);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        [HttpGet("Followers/{cityId}")]
        public async Task<IActionResult> GetFollowers(int cityId)
        {
            try
            {
                CityProfileDTO cityProfile = await _cityService.CityFollowersAsync(cityId);
                if (cityProfile == null)
                {
                    return NotFound();
                }

                return Ok(cityProfile);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        [HttpGet("Admins/{cityId}")]
        public async Task<IActionResult> GetAdmins(int cityId)
        {
            try
            {
                CityProfileDTO cityProfileDto = await _cityService.CityAdminsAsync(cityId);
                if (cityProfileDto == null)
                {
                    return NotFound();
                }

                return Ok(cityProfileDto);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        [HttpGet("{cityId}")]
        public async Task<IActionResult> Get(int cityId)
        {
            try
            {
                CityProfileDTO cityProfileDto = await _cityService.EditAsync(cityId);
                if (cityProfileDto == null)
                {
                    return NotFound();
                }

                return Ok(cityProfileDto);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> Edit(CityProfileDTO cityProfileDTO, IFormFile file)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                await _cityService.EditAsync(cityProfileDTO, file);
                _logger.LogInformation($"City {cityProfileDTO.City.Name} was edited profile and saved in the database");

                return CreatedAtAction(nameof(Get), cityProfileDTO);

            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }

        }

        [HttpGet("NewCity")]
        public IActionResult Create()
        {
            try
            {
                return Ok(new CityProfileDTO());
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CityProfileDTO cityProfileDto, IFormFile file)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(cityProfileDto);
                }
                int cityId = await _cityService.CreateAsync(cityProfileDto, file);

                return CreatedAtAction(nameof(Create), cityProfileDto);

            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        [HttpGet("Details/{cityId}")]
        public async Task<IActionResult> Details(int cityId)
        {
            try
            {
                CityDTO cityDto = await _cityService.GetByIdAsync(cityId);
                if (cityDto == null)
                {
                    return NotFound();
                }

                return Ok(cityDto);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        [HttpGet("Documents/{cityId}")]
        public async Task<IActionResult> GetDocuments(int cityId)
        {
            try
            {
                CityProfileDTO cityProfileDto = await _cityService.CityDocumentsAsync(cityId);
                if (cityProfileDto == null)
                {
                    return NotFound();
                }

                return Ok(cityProfileDto);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }
    }
}