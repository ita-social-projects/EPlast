using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.ExtensionMethods;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Logging;
using EPlast.WebApi.Models.City;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Get a specific number of cities 
        /// </summary>
        /// <param name="page">A number of the page</param>
        /// <param name="pageSize">A count of cities to display</param>
        /// <returns>A specific number of cities</returns>
        [HttpGet("Profiles/{page}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetCities(int page, int pageSize)
        {
            var cities = await _cityService.GetAllDTOAsync();
            var citiesViewModel = new CitiesViewModel(page, pageSize, cities, User.IsInRole("Admin"));

            return Ok(citiesViewModel);
        }

        /// <summary>
        /// Get a specific city
        /// </summary>
        /// <param name="cityId">The id of the city</param>
        /// <returns>A specific city</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">City not found</response>
        [HttpGet("Profile/{cityId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetProfile(int cityId)
        {
            var cityProfileDto = await _cityService.GetCityProfileAsync(cityId, User);
            if (cityProfileDto == null)
            {
                return NotFound();
            }

            var cityProfile = _mapper.Map<CityProfileDTO, CityViewModel>(cityProfileDto);

            return Ok(cityProfile);
        }

        /// <summary>
        /// Get all members of a specific city
        /// </summary>
        /// <param name="cityId">The id of the city</param>
        /// <returns>All members of a specific city</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">City not found</response>
        [HttpGet("Members/{cityId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetMembers(int cityId)
        {
            var cityProfileDto = await _cityService.GetCityMembersAsync(cityId, User);
            if (cityProfileDto == null)
            {
                return NotFound();
            }

            var cityProfile = _mapper.Map<CityProfileDTO, CityViewModel>(cityProfileDto);

            return Ok(cityProfile.Members);

        }

        /// <summary>
        /// Get all followers of a specific city
        /// </summary>
        /// <param name="cityId">The id of the city</param>
        /// <returns>All followers of a specific city</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">City not found</response>
        [HttpGet("Followers/{cityId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetFollowers(int cityId)
        {
            var cityProfileDto = await _cityService.GetCityFollowersAsync(cityId, User);
            if (cityProfileDto == null)
            {
                return NotFound();
            }

            var cityProfile = _mapper.Map<CityProfileDTO, CityViewModel>(cityProfileDto);

            return Ok(cityProfile.Followers);

        }

        /// <summary>
        /// Get all administrators of a specific city
        /// </summary>
        /// <param name="cityId">The id of the city</param>
        /// <returns>All administrators of a specific city</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">City not found</response>
        [HttpGet("Admins/{cityId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAdmins(int cityId)
        {
            var cityProfileDto = await _cityService.GetCityAdminsAsync(cityId, User);
            if (cityProfileDto == null)
            {
                return NotFound();
            }

            var cityProfile = _mapper.Map<CityProfileDTO, CityViewModel>(cityProfileDto);

            return Ok(cityProfile.Administration);
        }

        /// <summary>
        /// Get all documents of a specific city
        /// </summary>
        /// <param name="cityId">The id of the city</param>
        /// <returns>All documents of a specific city</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">City not found</response>
        [HttpGet("Documents/{cityId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
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

        /// <summary>
        /// Get an information about a specific city
        /// </summary>
        /// <param name="cityId">The id of the city</param>
        /// <returns>An information about a specific city</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">City not found</response>
        [HttpGet("Details/{cityId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Details(int cityId)
        {
            var cityDto = await _cityService.GetByIdAsync(cityId);
            if (cityDto == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CityDTO, CityViewModel>(cityDto));
        }

        /// <summary>
        /// Get a photo in base64 format
        /// </summary>
        /// <param name="logoName">The name of a city logo</param>
        /// <returns>A base64 string of the city logo</returns>
        [HttpGet("LogoBase64")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetPhotoBase64(string logoName)
        {
            var logoBase64 = await _cityService.GetLogoBase64(logoName);

            return Ok(logoBase64);
        }

        /// <summary>
        /// Create a new city
        /// </summary>
        /// <param name="city">An information about a new city</param>
        /// <returns>An id of a new city</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="400">Wrong input</response>
        [HttpPost("CreateCity")]
        [Authorize(AuthenticationSchemes = "Bearer")]
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

        /// <summary>
        /// Edit a specific city
        /// </summary>
        /// <param name="city">An information about an edited city</param>
        /// <response code="200">Successful operation</response>
        /// <response code="400">Wrong input</response>
        [HttpPut("EditCity/{cityId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
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

        /// <summary>
        /// Add a current user to followers
        /// </summary>
        /// <param name="cityId">An id of the city</param>
        /// <returns>An information about a new follower</returns>
        [HttpPost("AddFollower/{cityId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddFollower(int cityId)
        {
            var follower = await _cityMembersService.AddFollowerAsync(cityId, User);
            _logger.LogInformation($"User {{{follower.UserId}}} became a follower of city {{{cityId}}}.");

            return Ok(follower);
        }

        /// <summary>
        /// Remove a specific follower from the city
        /// </summary>
        /// <param name="followerId">The id of the follower</param>
        [HttpDelete("RemoveFollower/{followerId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> RemoveFollower(int followerId)
        {
            await _cityMembersService.RemoveFollowerAsync(followerId);
            _logger.LogInformation($"Follower with ID {{{followerId}}} was removed.");

            return Ok();
        }

        /// <summary>
        /// Toggle an approve status for member
        /// </summary>
        /// <param name="memberId">The id of the member</param>
        /// <returns>An information about a specific member</returns>
        [HttpPut("ChangeApproveStatus/{memberId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> ChangeApproveStatus(int memberId)
        {
            var member = await _cityMembersService.ToggleApproveStatusAsync(memberId);
            _logger.LogInformation($"Status of member with ID {{{memberId}}} was changed.");

            return Ok(member);
        }

        /// <summary>
        /// Add a new administrator to the city
        /// </summary>
        /// <param name="newAdmin">An information about a new administrator</param>
        /// <returns>An information about a new administrator</returns>
        [HttpPost("AddAdmin/{cityId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddAdmin(CityAdministrationViewModel newAdmin)
        {
            var admin = _mapper.Map<CityAdministrationViewModel, CityAdministrationDTO>(newAdmin);

            await _cityAdministrationService.AddAdministratorAsync(admin);
            _logger.LogInformation($"User {{{admin.UserId}}} became admin for city {{{admin.CityId}}}" +
                $" with role {{{admin.AdminType.AdminTypeName}}}.");

            return Ok(admin);
        }

        /// <summary>
        /// Remove a specific administrator from the city
        /// </summary>
        /// <param name="adminId">The id of the administrator</param>
        [HttpPut("RemoveAdmin/{adminId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> RemoveAdmin(int adminId)
        {
            await _cityAdministrationService.RemoveAdministratorAsync(adminId);
            _logger.LogInformation($"Admin with ID {{{adminId}}} was removed.");

            return Ok();
        }

        /// <summary>
        /// Edit an information about a specific admininstrator
        /// </summary>
        /// <param name="admin">An information about a new administrator</param>
        /// <returns>An information about a specific admininstrator</returns>
        [HttpPut("EditAdmin/{adminId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> EditAdmin(CityAdministrationViewModel admin)
        {
            var adminDTO = _mapper.Map<CityAdministrationViewModel, CityAdministrationDTO>(admin);

            await _cityAdministrationService.EditAdministratorAsync(adminDTO);
            _logger.LogInformation($"Admin with User-ID {{{admin.UserId}}} was edited.");

            return Ok(adminDTO);
        }

        /// <summary>
        /// Get all legal statuses
        /// </summary>
        /// <returns>List of legal statuses</returns>
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