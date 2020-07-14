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
        private readonly IMapper _mapper;
        private readonly ICityService _cityService;
        private readonly ICityMembersService _cityMembersService;
        private readonly ICityAdministrationService _cityAdministrationService;

        public CitiesController(ILoggerService<CitiesController> logger,
            IMapper mapper,
            ICityService cityService,
            ICityMembersService cityMembersService,
            ICityAdministrationService cityAdministrationService)
        {
            _logger = logger;
            _mapper = mapper;
            _cityService = cityService;
            _cityMembersService = cityMembersService;
            _cityAdministrationService = cityAdministrationService;
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
                var cityProfileDto = await _cityService.GetCityAdminsAsync(cityId);
                if (cityProfileDto == null)
                {
                    return NotFound();
                }

                var cityProfile = _mapper.Map<CityProfileDTO, CityProfileViewModel>(cityProfileDto);
                cityProfile.SetMembersAndAdministration();

                return Ok(cityProfile.Admins);
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
                var cityProfileDto = await _cityService.GetCityDocumentsAsync(cityId);
                if (cityProfileDto == null)
                {
                    return NotFound();
                }

                var cityProfile = _mapper.Map<CityProfileDTO, CityProfileViewModel>(cityProfileDto);
                cityProfile.SetMembersAndAdministration();

                return Ok(cityProfile.Documents);
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
                _logger.LogInformation($"City {{{cityProfileDTO.City.Name}}} was created.");

                return Ok(_mapper.Map<CityDTO, CityViewModel>(cityProfileDTO.City));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
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
                _logger.LogInformation($"City {{{cityProfileDTO.City.Name}}} was edited.");

                return Ok(_mapper.Map<CityDTO, CityViewModel>(cityProfileDTO.City));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        [HttpPost("AddFollower/{cityId}/{userId}")]
        public async Task<IActionResult> AddFollower(int cityId, string userId)
        {
            try
            {
                await _cityMembersService.AddFollowerAsync(cityId, userId);
                _logger.LogInformation($"User {{{userId}}} became a follower of city {{{cityId}}}.");

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        [HttpDelete("RemoveFollower/{followerId}")]
        public async Task<IActionResult> RemoveFollower(int followerId)
        {
            try
            {
                await _cityMembersService.RemoveFollowerAsync(followerId);
                _logger.LogInformation($"Follower with ID {{{followerId}}} was removed.");

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        [HttpPut("ChangeApproveStatus/{memberId}")]
        public async Task<IActionResult> ChangeApproveStatus(int memberId)
        {
            try
            {
                await _cityMembersService.ToggleApproveStatusAsync(memberId);
                _logger.LogInformation($"Status of member with ID {{{memberId}}} was changed.");

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        [HttpPost("AddAdmin/{clubId}")]
        public async Task<IActionResult> AddAdmin(CityAdministrationViewModel admin)
        {
            try
            {
                var adminDTO = _mapper.Map<CityAdministrationViewModel, CityAdministrationDTO>(admin);

                await _cityAdministrationService.AddAdministratorAsync(adminDTO);
                _logger.LogInformation($"User {{{admin.UserId}}} became admin for city {{{admin.CityId}}}" +
                    $" with role {{{admin.AdminType.AdminTypeName}}}.");

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        [HttpDelete("RemoveAdmin/{adminId}")]
        public async Task<IActionResult> RemoveAdmin(int adminId)
        {
            try
            {
                await _cityAdministrationService.RemoveAdministratorAsync(adminId);
                _logger.LogInformation($"Admin with ID {{{adminId}}} was removed.");

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        [HttpPut("EditAdmin/{adminId}")]
        public async Task<IActionResult> EditAdmin(CityAdministrationViewModel admin)
        {
            try
            {
                var adminDTO = _mapper.Map<CityAdministrationViewModel, CityAdministrationDTO>(admin);

                await _cityAdministrationService.EditAdministratorAsync(adminDTO);
                _logger.LogInformation($"Admin with User-ID {{{admin.UserId}}} was edited.");

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }
    }
}