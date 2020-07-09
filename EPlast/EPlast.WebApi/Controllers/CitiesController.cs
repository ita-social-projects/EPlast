using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Logging;
using EPlast.WebApi.Models.City;
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

                return Ok(_mapper.Map<CityDTO, CityViewModel>(cityDto));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        [HttpGet("LogoBase64")]
        public async Task<IActionResult> GetPhotoBase64(string logoName)
        {
            var logoBase64 = await _cityService.GetLogoBase64(logoName);

            return Ok(logoBase64);
        }

        [HttpPut("EditCity/{cityId}")]
        public async Task<IActionResult> Edit(CityViewModel city)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var cityProfileDTO = new CityProfileDTO
                {
                    City = _mapper.Map<CityViewModel, CityDTO>(city)
                };

                await _cityService.EditAsync(cityProfileDTO);
                _logger.LogInformation($"City {cityProfileDTO.City.Name} was edited profile and saved in the database");

                return Ok(_mapper.Map<CityDTO, CityViewModel>(cityProfileDTO.City));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        [HttpPost("CreateCity")]
        public async Task<IActionResult> Create(CityViewModel city)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var cityProfileDTO = new CityProfileDTO
                {
                    City = _mapper.Map<CityViewModel, CityDTO>(city)
                };

                await _cityService.CreateAsync(cityProfileDTO);
                _logger.LogInformation($"City {cityProfileDTO.City.Name} was created profile and saved in the database");

                return Ok(_mapper.Map<CityDTO, CityViewModel>(cityProfileDTO.City));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }
    }
}