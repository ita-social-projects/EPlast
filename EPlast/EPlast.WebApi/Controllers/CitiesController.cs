using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.ExtensionMethods;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Logging;
using EPlast.WebApi.Models.City;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnnualReportDTOs = EPlast.BLL.DTO.AnnualReport;

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

        [HttpDelete("RemoveFollower/{userId}")]
        public async Task<IActionResult> RemoveFollower(string userId)
        {
            try
            {
                await _cityMembersService.RemoveMemberAsync(userId);
                _logger.LogInformation($"Follower with User-ID {{{userId}}} was removed.");

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        [HttpPut("ChangeApproveStatus/{userId}")]
        public async Task<IActionResult> ChangeApproveStatus(string userId)
        {
            try
            {
                await _cityMembersService.ToggleApproveStatusAsync(userId);
                _logger.LogInformation($"Status of user {{{userId}}} was changed.");

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

        [HttpDelete("RemoveAdmin/{userId}")]
        public async Task<IActionResult> RemoveAdmin(string userId)
        {
            try
            {
                await _cityAdministrationService.RemoveAdministratorAsync(userId);
                _logger.LogInformation($"Admin with User-ID {{{userId}}} was removed.");

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        [HttpGet("getLegalStatuses")]
        public IActionResult GetLegalStatuses()
        {
            var legalStatuses = new List<string>();
            foreach (var enumValue in Enum.GetValues(typeof(AnnualReportDTOs.CityLegalStatusTypeDTO)).Cast<AnnualReportDTOs.CityLegalStatusTypeDTO>())
            {
                legalStatuses.Add(enumValue.GetDescription());
            }
            return Ok(new { legalStatuses });
        }
    }
}