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

        [HttpGet("Profiles/{page}")]
        public async Task<IActionResult> Index(int page, int pageSize)
        {
            var cities = await _cityService.GetAllDTOAsync();
            var citiesViewModel = new CitiesViewModel()
            {
                Cities = cities.Skip((page - 1) * pageSize).Take(pageSize),
                Total = cities.Count(),
                Page = new PageViewModel(cities.Count(), page, pageSize),
                CanCreate = true
            };

            return Ok(citiesViewModel);
        }

        [HttpGet("Profile/{cityId}")]
        public async Task<IActionResult> GetProfile(int cityId)
        {
            var cityProfileDto = await _cityService.GetCityProfileAsync(cityId);
            if (cityProfileDto == null)
            {
                return NotFound();
            }

            var cityProfile = _mapper.Map<CityProfileDTO, CityViewModel>(cityProfileDto);

            return Ok(cityProfile);
        }

        [HttpGet("Members/{cityId}")]
        public async Task<IActionResult> GetMembers(int cityId)
        {
            var cityProfileDto = await _cityService.GetCityMembersAsync(cityId);
            if (cityProfileDto == null)
            {
                return NotFound();
            }

            var cityProfile = _mapper.Map<CityProfileDTO, CityViewModel>(cityProfileDto);

            return Ok(cityProfile.Members);

        }

        [HttpGet("Followers/{cityId}")]
        public async Task<IActionResult> GetFollowers(int cityId)
        {
            var cityProfileDto = await _cityService.GetCityFollowersAsync(cityId);
            if (cityProfileDto == null)
            {
                return NotFound();
            }

            var cityProfile = _mapper.Map<CityProfileDTO, CityViewModel>(cityProfileDto);

            return Ok(cityProfile.Followers);

        }

        [HttpGet("Admins/{cityId}")]
        public async Task<IActionResult> GetAdmins(int cityId)
        {
            var cityProfileDto = await _cityService.GetCityAdminsAsync(cityId);
            if (cityProfileDto == null)
            {
                return NotFound();
            }

            var cityProfile = _mapper.Map<CityProfileDTO, CityViewModel>(cityProfileDto);

            return Ok(cityProfile.Administration);
        }

        [HttpGet("Documents/{cityId}")]
        public async Task<IActionResult> GetDocuments(int cityId)
        {
            var cityProfileDto = await _cityService.GetCityDocumentsAsync(cityId);
            if (cityProfileDto == null)
            {
                return NotFound();
            }

            var cityProfile = _mapper.Map<CityProfileDTO, CityViewModel>(cityProfileDto);

            return Ok(cityProfile.Documents);

        }

        [HttpGet("Details/{cityId}")]
        public async Task<IActionResult> Details(int cityId)
        {
            var cityDto = await _cityService.GetByIdAsync(cityId);
            if (cityDto == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CityDTO, CityViewModel>(cityDto));
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cityDTO = _mapper.Map<CityViewModel, CityDTO>(city);

            cityDTO.ID = await _cityService.CreateAsync(cityDTO);
            _logger.LogInformation($"City {{{cityDTO.Name}}} was created.");

            return Ok(city.ID);
        }

        [HttpPut("EditCity/{cityId}")]
        public async Task<IActionResult> Edit(CityViewModel city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cityDTO = _mapper.Map<CityViewModel, CityDTO>(city);

            await _cityService.EditAsync(cityDTO);
            _logger.LogInformation($"City {{{cityDTO.Name}}} was edited.");

            return Ok();
        }

        [HttpPost("AddFollower/{cityId}")]
        public async Task<IActionResult> AddFollower(int cityId)
        {
            var userId = User.Identity.Name;
            if (userId is null)
            {
                return NotFound();
            }

            await _cityMembersService.AddFollowerAsync(cityId, userId);
            _logger.LogInformation($"User {{{userId}}} became a follower of city {{{cityId}}}.");

            return Ok();
        }

        [HttpDelete("RemoveFollower/{followerId}")]
        public async Task<IActionResult> RemoveFollower(int followerId)
        {
            await _cityMembersService.RemoveFollowerAsync(followerId);
            _logger.LogInformation($"Follower with ID {{{followerId}}} was removed.");

            return Ok();
        }

        [HttpPut("ChangeApproveStatus/{memberId}")]
        public async Task<IActionResult> ChangeApproveStatus(int memberId)
        {
            await _cityMembersService.ToggleApproveStatusAsync(memberId);
            _logger.LogInformation($"Status of member with ID {{{memberId}}} was changed.");

            return Ok();
        }

        [HttpPost("AddAdmin/{cityId}")]
        public async Task<IActionResult> AddAdmin(CityAdministrationViewModel admin)
        {
            var adminDTO = _mapper.Map<CityAdministrationViewModel, CityAdministrationDTO>(admin);

            await _cityAdministrationService.AddAdministratorAsync(adminDTO);
            _logger.LogInformation($"User {{{admin.UserId}}} became admin for city {{{admin.CityId}}}" +
                $" with role {{{admin.AdminType.AdminTypeName}}}.");

            return Ok();
        }

        [HttpPut("RemoveAdmin/{adminId}")]
        public async Task<IActionResult> RemoveAdmin(int adminId)
        {
            await _cityAdministrationService.RemoveAdministratorAsync(adminId);
            _logger.LogInformation($"Admin with ID {{{adminId}}} was removed.");

            return Ok();
        }

        [HttpPut("EditAdmin/{adminId}")]
        public async Task<IActionResult> EditAdmin(CityAdministrationViewModel admin)
        {
            var adminDTO = _mapper.Map<CityAdministrationViewModel, CityAdministrationDTO>(admin);

            await _cityAdministrationService.EditAdministratorAsync(adminDTO);
            _logger.LogInformation($"Admin with User-ID {{{admin.UserId}}} was edited.");

            return Ok();
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