using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Logging;
using EPlast.WebApi.Models.City;
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
        private readonly IMapper _mapper;

        public CitiesController(ILoggerService<CitiesController> logger, ICityService cityService, IMapper mapper)
        {
            _logger = logger;
            _cityService = cityService;
            _mapper = mapper;
        }

        [HttpGet("Profiles")]
        public async Task<IActionResult> Index()
        {
            var cities = await _cityService.GetAllDTOAsync();
            
            return Ok(cities);
        }

        [HttpGet("Profile/{cityId}")]
        public async Task<IActionResult> GetProfile(int cityId)
        {
            try
            {
                var cityProfileDto = await _cityService.GetCityProfileAsync(cityId);
                if (cityProfileDto == null)
                {
                    return NotFound();
                }

                var cityProfile = _mapper.Map<CityProfileDTO, CityProfileViewModel>(cityProfileDto);
                cityProfile.SetMembersAndAdministration();

                return Ok(cityProfile.City);
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
                var cityProfileDto = await _cityService.GetCityMembersAsync(cityId);
                if (cityProfileDto == null)
                {
                    return NotFound();
                }

                var cityProfile = _mapper.Map<CityProfileDTO, CityProfileViewModel>(cityProfileDto);
                cityProfile.SetMembersAndAdministration();

                return Ok(cityProfile.Members);
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
                var cityProfileDto = await _cityService.GetCityFollowersAsync(cityId);
                if (cityProfileDto == null)
                {
                    return NotFound();
                }

                var cityProfile = _mapper.Map<CityProfileDTO, CityProfileViewModel>(cityProfileDto);
                cityProfile.SetMembersAndAdministration();

                return Ok(cityProfile.Followers);
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
                CityProfileDTO cityProfileDto = await _cityService.GetCityAdminsAsync(cityId);
                if (cityProfileDto == null)
                {
                    return NotFound();
                }

                var cityProfile = _mapper.Map<CityProfileDTO, CityProfileViewModel>(cityProfileDto);
                cityProfile.SetMembersAndAdministration();

                return Ok(cityProfile.CityAdmins);
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
                CityProfileDTO cityProfileDto = await _cityService.GetCityDocumentsAsync(cityId);
                if (cityProfileDto == null)
                {
                    return NotFound();
                }

                var cityProfile = _mapper.Map<CityProfileDTO, CityProfileViewModel>(cityProfileDto);
                cityProfile.SetMembersAndAdministration();

                return Ok(cityProfile.CityDoc);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        [HttpPut("EditCity/{cityId}")]
        public async Task<IActionResult> Edit(CityProfileDTO cityProfileDTO, IFormFile file)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _cityService.EditAsync(cityProfileDTO, file);
                _logger.LogInformation($"City {cityProfileDTO.City.Name} was edited profile and saved in the database");

                return NoContent();

            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        [HttpPost("CreateCity")]
        public async Task<IActionResult> Create(CityProfileDTO cityProfileDto, IFormFile file)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
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
    }
}