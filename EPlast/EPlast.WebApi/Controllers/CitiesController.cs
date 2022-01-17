using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.ExtensionMethods;
using EPlast.BLL.Interfaces.Cache;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Logging;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using EPlast.WebApi.Models.City;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BLL.Handlers.CityHandlers;
using EPlast.BLL.Queries.City;
using MediatR;
using AnnualReportDTOs = EPlast.BLL.DTO.AnnualReport;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ILoggerService<CitiesController> _logger;
        private readonly ICacheService _cache;
        private readonly IMapper _mapper;
        private readonly ICityService _cityService;
        private readonly ICityParticipantsService _cityParticipantsService;
        private readonly ICityDocumentsService _cityDocumentsService;
        private readonly ICityAccessService _cityAccessService;
        private readonly UserManager<User> _userManager;
        private readonly IMediator _mediator;

        private const string ActiveCitiesCacheKey = "ActiveCities";
        private const string ArchivedCitiesCacheKey = "ArchivedCities";

        public CitiesController(ILoggerService<CitiesController> logger,
            IMapper mapper,
            ICityService cityService,
            ICityDocumentsService cityDocumentsService,
            ICityAccessService cityAccessService, UserManager<User> userManager,
            ICityParticipantsService cityParticipantsService, ICacheService cache,
            IMediator mediator)
        {
            _cache = cache;
            _logger = logger;
            _mapper = mapper;
            _cityService = cityService;
            _cityDocumentsService = cityDocumentsService;
            _cityAccessService = cityAccessService;
            _userManager = userManager;
            _cityParticipantsService = cityParticipantsService;
            _mediator = mediator;
        }

        /// <summary>
        /// Get a specific number of cities 
        /// </summary>
        /// <param name="page">A number of the page</param>
        /// <param name="pageSize">A count of cities to display</param>
        /// <param name="cityName">Optional param to find cities by name</param>
        /// <returns>A specific number of cities</returns>
        [HttpGet("Profiles/{page}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetCities(int page, int pageSize, string cityName = null)
        {
            var query = new GetAllCitiesOrByNameQuery(cityName);
            var cities = await _mediator.Send(query);
            var citiesViewModel = new CitiesViewModel(page, pageSize, cities, User.IsInRole(Roles.Admin));

            return Ok(citiesViewModel);
        }

        /// <summary>
        /// Get all cities 
        /// </summary>
        /// <returns>List of cities</returns>
        [HttpGet("Cities")]
        public async Task<IActionResult> GetCities()
        {
            var cities = await _cityService.GetCities();
            return Ok(cities);
        }

        /// <summary>
        /// Get all active cities using redis cache
        /// </summary>
        /// <returns>List of active cities</returns>
        [HttpGet("Profiles/Active/{page}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetActiveCities(int page, int pageSize, string name)
        {
            string cityRecordKey = $"{ActiveCitiesCacheKey}_{page}_{pageSize}_{name}";
            var citiesTuple = await _cache.GetRecordByKeyAsync<Tuple<System.Collections.Generic.IEnumerable<CityObjectDTO>, int>>(cityRecordKey);

            if (citiesTuple is null)
            {
                bool isArchive = false;
                citiesTuple = await _cityService.GetAllCitiesByPageAndIsArchiveAsync(page, pageSize, name, isArchive);
                if (!String.IsNullOrEmpty(name))
                {
                    TimeSpan expireTime = TimeSpan.FromMinutes(5);
                    await _cache.SetCacheRecordAsync(cityRecordKey, citiesTuple, expireTime);
                }
                await _cache.SetCacheRecordAsync(cityRecordKey, citiesTuple);
            }
            return StatusCode(StatusCodes.Status200OK, new { page = page, pageSize = pageSize, cities = citiesTuple.Item1, total = citiesTuple.Item2 });
        }


        /// <summary>
        /// Get all not active cities using redis cache
        /// </summary>
        /// <returns>List of not active cities</returns>
        [HttpGet("Profiles/NotActive/{page}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetNotActiveCities(int page, int pageSize, string name)
        {
            string cityRecordKey = $"{ArchivedCitiesCacheKey}_{page}_{pageSize}_{name}";
            var citiesTuple = await _cache.GetRecordByKeyAsync<Tuple<System.Collections.Generic.IEnumerable<CityObjectDTO>, int>>(cityRecordKey);

            if (citiesTuple is null)
            {
                bool isArchive = true;
                citiesTuple = await _cityService.GetAllCitiesByPageAndIsArchiveAsync(page, pageSize, name, isArchive);
                if (!String.IsNullOrEmpty(name))
                {
                    TimeSpan expireTime = TimeSpan.FromMinutes(5);
                    await _cache.SetCacheRecordAsync(cityRecordKey, citiesTuple, expireTime);
                }
                await _cache.SetCacheRecordAsync(cityRecordKey, citiesTuple);
            }
            return StatusCode(StatusCodes.Status200OK, new { cities = citiesTuple.Item1, total = citiesTuple.Item2 });
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
            var query = new GetCityProfileQuery(cityId, await _userManager.GetUserAsync(User));
            var cityProfileDto = await _mediator.Send(query);
            if (cityProfileDto == null)
            {
                return NotFound();
            }

            var cityProfile = _mapper.Map<CityProfileDTO, CityViewModel>(cityProfileDto);

            return Ok(cityProfile);
        }

        /// <summary>
        /// Get all users of a specific city
        /// </summary>
        /// <param name="cityId">The id of the city</param>
        /// <returns>All users of a specific city</returns>
        /// <response code="200">Successful operation</response>
        [HttpGet("CityUsers/{cityId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetCityUsers(int cityId)
        {
            var query = new GetCityUsersQuery(cityId);
            var cityUsers = await _mediator.Send(query);

            return Ok(cityUsers);
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
            var query = new GetCityMembersQuery(cityId);
            var cityProfileDto = await _mediator.Send(query);
            if (cityProfileDto == null)
            {
                return NotFound();
            }

            var cityProfile = _mapper.Map<CityProfileDTO, CityViewModel>(cityProfileDto);
            cityProfile.CanEdit = await _cityAccessService.HasAccessAsync(await _userManager.GetUserAsync(User), cityId);

            return Ok(new { cityProfile.Members, cityProfile.CanEdit, cityProfile.Name });
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
            var query = new GetCityFollowersQuery(cityId);
            var cityProfileDto = await _mediator.Send(query);
            if (cityProfileDto == null)
            {
                return NotFound();
            }

            var cityProfile = _mapper.Map<CityProfileDTO, CityViewModel>(cityProfileDto);
            cityProfile.CanEdit = await _cityAccessService.HasAccessAsync(await _userManager.GetUserAsync(User), cityId);

            return Ok(new { cityProfile.Followers, cityProfile.CanEdit, cityProfile.Name });
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
            var query = new GetCityAdminsQuery(cityId);
            var cityProfileDto = await _mediator.Send(query);
            if (cityProfileDto == null)
            {
                return NotFound();
            }

            var cityProfile = _mapper.Map<CityProfileDTO, CityViewModel>(cityProfileDto);
            cityProfile.CanEdit = await _cityAccessService.HasAccessAsync(await _userManager.GetUserAsync(User), cityId);

            return Ok(new { cityProfile.Administration, cityProfile.Head, cityProfile.HeadDeputy, cityProfile.CanEdit, cityProfile.Name });
        }

        /// <summary>
        /// Get all administrators of a specific city
        /// </summary>
        /// <param name="cityId">The id of the city</param>
        /// <returns>All administrators of a specific city</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">City not found</response>
        [HttpGet("GetAdministrations/{cityId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAdministrations(int cityId)
        {
            var query = new GetAdministrationQuery(cityId);
            var admins = await _mediator.Send(query);
            return Ok(admins);
        }

        /// <summary>
        /// Get all documents of a specific city
        /// </summary>
        /// <param name="cityId">The id of the city</param>
        /// <returns>All documents of a specific city</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">City not found</response>
        [HttpGet("Documents/{cityId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminPlastMemberAndSupporter)]
        public async Task<IActionResult> GetDocuments(int cityId)
        {
            var query = new GetCityDocumentsQuery(cityId);
            var cityProfileDto = await _mediator.Send(query);
            if (cityProfileDto == null)
            {
                return NotFound();
            }

            var cityProfile = _mapper.Map<CityProfileDTO, CityViewModel>(cityProfileDto);
            cityProfile.CanEdit = await _cityAccessService.HasAccessAsync(await _userManager.GetUserAsync(User), cityId);

            return Ok(new { cityProfile.Documents, cityProfile.CanEdit });
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
            var query = new GetCityByIdWthFullInfoQuery(cityId);
            var cityDto = await _mediator.Send(query);
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
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.Admin)]
        public async Task<IActionResult> Create(CityViewModel city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cityDTO = _mapper.Map<CityViewModel, CityDTO>(city);

            cityDTO.ID = await _cityService.CreateAsync(cityDTO);
            _logger.LogInformation($"City {{{cityDTO.Name}}} was created.");
            await _cache.RemoveRecordsByPatternAsync(ActiveCitiesCacheKey);
            await _cache.RemoveRecordsByPatternAsync(ArchivedCitiesCacheKey);

            return Ok(cityDTO.ID);
        }

        /// <summary>
        /// Edit a specific city
        /// </summary>
        /// <param name="city">An information about an edited city</param>
        /// <response code="200">Successful operation</response>
        /// <response code="400">Wrong input</response>
        [HttpPut("EditCity/{cityId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.CanEditCity)]
        public async Task<IActionResult> Edit(CityViewModel city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cityDTO = _mapper.Map<CityViewModel, CityDTO>(city);

            await _cityService.EditAsync(cityDTO);
            _logger.LogInformation($"City {{{cityDTO.Name}}} was edited.");
            await _cache.RemoveRecordsByPatternAsync(ActiveCitiesCacheKey);
            await _cache.RemoveRecordsByPatternAsync(ArchivedCitiesCacheKey);

            return Ok();
        }

        /// <summary>
        /// Archive a specific city
        /// </summary>
        /// <param name="cityId">The id of the city</param>
        [HttpPut("ArchiveCity/{cityId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.Admin)]
        public async Task<IActionResult> Archive(int cityId)
        {
            await _cityService.ArchiveAsync(cityId);
            await _cache.RemoveRecordsByPatternAsync(ActiveCitiesCacheKey);
            await _cache.RemoveRecordsByPatternAsync(ArchivedCitiesCacheKey);
            return Ok();
        }

        /// <summary>
        /// Unarchive a specific city
        /// </summary>
        /// <param name="cityId">The id of the city</param>
        [HttpPut("UnArchiveCity/{cityId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.Admin)]
        public async Task<IActionResult> UnArchive(int cityId)
        {
            await _cityService.UnArchiveAsync(cityId);
            await _cache.RemoveRecordsByPatternAsync(ActiveCitiesCacheKey);
            await _cache.RemoveRecordsByPatternAsync(ArchivedCitiesCacheKey);
            return Ok();
        }

        /// <summary>
        /// Remove a specific city
        /// </summary>
        /// <param name="cityId">The id of the city</param>
        [HttpDelete("RemoveCity/{cityId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.Admin)]
        public async Task<IActionResult> Remove(int cityId)
        {
            await _cityService.RemoveAsync(cityId);
            _logger.LogInformation($"City with id {{{cityId}}} was deleted.");
            await _cache.RemoveRecordsByPatternAsync(ActiveCitiesCacheKey);
            await _cache.RemoveRecordsByPatternAsync(ArchivedCitiesCacheKey);

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
            var follower = await _cityParticipantsService.AddFollowerAsync(cityId, await _userManager.GetUserAsync(User));
            _logger.LogInformation($"User {{{follower.UserId}}} became a follower of city {{{cityId}}}.");

            return Ok(follower);
        }

        /// <summary>
        /// Add the user to followers
        /// </summary>
        /// <param name="cityId">An id of the city</param>
        /// <param name="userId">An id of the user</param>
        /// <returns>An information about a new follower</returns>
        [HttpPost("AddFollowerWithId/{cityId}/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddFollowerWithId(int cityId, string userId)
        {
            var follower = await _cityParticipantsService.AddFollowerAsync(cityId, userId);
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
            await _cityParticipantsService.RemoveFollowerAsync(followerId);
            _logger.LogInformation($"Follower with ID {{{followerId}}} was removed.");

            return Ok();
        }

        /// <summary>
        /// Toggle an approve status for member
        /// </summary>
        /// <param name="memberId">The id of the member</param>
        /// <returns>An information about a specific member</returns>
        [HttpPut("ChangeApproveStatus/{memberId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminCityHeadOkrugaHeadCityHeadDeputyOkrugaHeadDeputy)]
        public async Task<IActionResult> ChangeApproveStatus(int memberId)
        {
            var member = await _cityParticipantsService.ToggleApproveStatusAsync(memberId);
            _logger.LogInformation($"Status of member with ID {{{memberId}}} was changed.");

            return Ok(member);
        }

        /// <summary>
        /// Returns either given user is approved or not
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>True if given user is approved, otherwise false. BadRequest if user doesn't exist</returns>
        [HttpGet("IsUserApproved/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> IsUserApproved(int userId)
        {
            var isApproved = await _cityParticipantsService.CheckIsUserApproved(userId);
            if(isApproved==null)
            {
                return BadRequest();
            }
            return Ok(isApproved);
        }

        /// <summary>
        /// City name only for approved member
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns>city name string</returns>
        [HttpGet("CityNameOfApprovedMember/{memberId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CityNameOfApprovedMember(string memberId)
        {
            var member = await _cityParticipantsService.CityOfApprovedMember(memberId);

            return Ok(member);
        }

        /// <summary>
        /// Add a new administrator to the city
        /// </summary>
        /// <param name="newAdmin">An information about a new administrator</param>
        /// <returns>An information about a new administrator</returns>
        [HttpPost("AddAdmin/{cityId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminCityHeadOkrugaHeadCityHeadDeputyOkrugaHeadDeputy)]
        public async Task<IActionResult> AddAdmin(CityAdministrationViewModel newAdmin)
        {
            var admin = _mapper.Map<CityAdministrationViewModel, CityAdministrationDTO>(newAdmin);
            await _cityParticipantsService.AddAdministratorAsync(admin);

            _logger.LogInformation($"User {{{admin.UserId}}} became Admin for city {{{admin.CityId}}}" +
                $" with role {{{admin.AdminType.AdminTypeName}}}.");

            return Ok(admin);
        }

        /// <summary>
        /// Remove a specific administrator from the city
        /// </summary>
        /// <param name="adminId">The id of the administrator</param>
        [HttpPut("RemoveAdmin/{adminId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminCityHeadOkrugaHeadCityHeadDeputyOkrugaHeadDeputy)]
        public async Task<IActionResult> RemoveAdmin(int adminId)
        {
            await _cityParticipantsService.RemoveAdministratorAsync(adminId);
            _logger.LogInformation($"Admin with ID {{{adminId}}} was removed.");

            return Ok();
        }

        /// <summary>
        /// Edit an information about a specific administrator
        /// </summary>
        /// <param name="admin">An information about a new administrator</param>
        /// <returns>An information about a specific administrator</returns>
        [HttpPut("EditAdmin/{adminId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminCityHeadOkrugaHeadCityHeadDeputyOkrugaHeadDeputy)]
        public async Task<IActionResult> EditAdmin(CityAdministrationViewModel admin)
        {
            if (admin.EndDate != null && admin.EndDate < DateTime.Today)
            {
                return BadRequest();
            }

            var adminDTO = _mapper.Map<CityAdministrationViewModel, CityAdministrationDTO>(admin);

            await _cityParticipantsService.EditAdministratorAsync(adminDTO);
            _logger.LogInformation($"Admin with User-ID {{{admin.UserId}}} was edited.");

            return Ok(adminDTO);
        }

        /// <summary>
        /// Add a document to the city
        /// </summary>
        /// <param name="document">An information about a specific document</param>
        /// <returns>A newly created document</returns>
        [HttpPost("AddDocument/{cityId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminCityHeadOkrugaHeadCityHeadDeputyOkrugaHeadDeputy)]
        public async Task<IActionResult> AddDocument(CityDocumentsViewModel document)
        {
            var documentDTO = _mapper.Map<CityDocumentsViewModel, CityDocumentsDTO>(document);

            await _cityDocumentsService.AddDocumentAsync(documentDTO);
            _logger.LogInformation($"Document with id {{{documentDTO.ID}}} was added.");

            return Ok(documentDTO);
        }

        /// <summary>
        /// Get a file in base64 format
        /// </summary>
        /// <param name="fileName">The name of a city file</param>
        /// <returns>A base64 string of the file</returns>
        [HttpGet("FileBase64/{fileName}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetFileBase64(string fileName)
        {
            var fileBase64 = await _cityDocumentsService.DownloadFileAsync(fileName);

            return Ok(fileBase64);
        }

        /// <summary>
        /// Remove a specific document
        /// </summary>
        /// <param name="documentId">The id of a specific document</param>
        [HttpDelete("RemoveDocument/{documentId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminCityHeadOkrugaHeadCityHeadDeputyOkrugaHeadDeputy)]
        public async Task<IActionResult> RemoveDocument(int documentId)
        {
            await _cityDocumentsService.DeleteFileAsync(documentId);
            _logger.LogInformation($"Document with id {{{documentId}}} was deleted.");

            return Ok();
        }

        [HttpGet("GetDocumentTypes")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetDocumentTypesAsync()
        {
            var documentTypes = await _cityDocumentsService.GetAllCityDocumentTypesAsync();

            return Ok(documentTypes);
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

        /// <summary>
        /// Get all cities that the user has access to
        /// </summary>
        /// <returns>List of cities</returns>
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetCitiesThatUserHasAccessTo()
        {
            return Ok(new { cities = await _cityAccessService.GetCitiesAsync(await _userManager.GetUserAsync(User)) });
        }

        /// <summary>
        /// Get id and name from all cities that the user has access to
        /// </summary>
        /// <returns>Tuple (int, string)</returns>
        [HttpGet("CitiesOptions")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetCitiesNameThatUserHasAccessTo()
        {
            return Ok(new { cities = await _cityAccessService.GetAllCitiesIdAndName(await _userManager.GetUserAsync(User)) });
        }

        [HttpGet("GetUserAdmins/{UserId}")]

        public async Task<IActionResult> GetUserAdministrations(string UserId)
        {
            var userAdmins = await _cityParticipantsService.GetAdministrationsOfUserAsync(UserId);

            return Ok(userAdmins);
        }

        [HttpGet("GetCheckPlastMember/{userId}")]

        public async Task<IActionResult> GetCheckPlastMember(string userId)
        {
            var query = new PlastMemberCheckQuery(userId);
            var check = await _mediator.Send(query);

            return Ok(check);
        }

        [HttpGet("GetUserPreviousAdmins/{UserId}")]

        public async Task<IActionResult> GetUserPreviousAdministrations(string UserId)
        {
            var userAdmins = await _cityParticipantsService.GetPreviousAdministrationsOfUserAsync(UserId);

            return Ok(userAdmins);
        }

        [HttpGet("GetAllAdministrationStatuses/{UserId}")]
        public async Task<IActionResult> GetAllAdministrationStatuses(string UserId)
        {
            var userAdmins = await _cityParticipantsService.GetAdministrationStatuses(UserId);

            return Ok(userAdmins);
        }

        /// <summary>
        /// Get admin ids of a specific city
        /// </summary>
        /// <param name="cityId">An id of a city</param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Not found</response>
        /// <returns>Admin ids of a city</returns>
        [HttpGet("AdminsIds/{cityId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAdminsIds(int cityId)
        {
            var query = new GetCityAdminsIdsQuery(cityId);
            var cityAdminsIds = await _mediator.Send(query);
            if (cityAdminsIds == null)
            {
                return NotFound();
            }

            return Ok(cityAdminsIds);
        }
    }
}
