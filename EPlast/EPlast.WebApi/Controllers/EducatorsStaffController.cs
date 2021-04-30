using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.BLL.Interfaces.EducatorsStaff;
using EPlast.BLL.Interfaces.Logging;
using EPlast.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.HeadsAdminPlastunSupporterAndRegisteredUser)]
    public class EducatorsStaffController : ControllerBase
    {
        private readonly ILoggerService<EducatorsStaffController> _logger;
        private readonly IEducatorsStaffService _kvService;
        private readonly IEducatorsStaffTypesService _kvTypeService;

        public EducatorsStaffController(ILoggerService<EducatorsStaffController> logger, IEducatorsStaffService kvService, IEducatorsStaffTypesService kvTypeService)
        {
            _logger = logger;
            _kvService = kvService;
            _kvTypeService = kvTypeService;
        }

        /// <summary>
        /// Creates new kadra vykhovnykiv
        /// </summary>
        /// <param name="kvDTO">The dto of the new kadra</param>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User is not Admin</response>
        [HttpPost("CreateKadra")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> CreateKadra(EducatorsStaffDTO kvDTO)
        {
            var newKadra = await _kvService.CreateKadra(kvDTO);

            return Ok(newKadra);
        }

        /// <summary>
        /// Deletes Kadra by id
        /// </summary>
        /// <param name="kadraId">The id of kadra that will be deleted</param>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User is not Admin</response>
        ///  <response code="404">kadra with this id doesn't exist</response>
        [HttpDelete("RemoveKadra/{kadraId}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Remove(int kadraId)
        {
            await _kvService.DeleteKadra(kadraId);
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Deletes Updates by id
        /// </summary>
        /// <param name="kadrasDTO">The dto of kadra that will be updated</param>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User is not Admin</response>
        [HttpPut("EditKadra")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Update(EducatorsStaffDTO kadrasDTO)
        {
            await _kvService.UpdateKadra(kadrasDTO);
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Returns Kadras of given user
        /// </summary>
        /// <param name="userId">The id of user whose kadra we try to get</param>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User is not Admin</response>
        /// /// <response code="404">Users id is not valid</response>
        [HttpGet("UserKV/{userId}")]
        public async Task<IActionResult> GetUsersKVs(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError("User id is null");
                return NotFound();
            }

            var Kadras = await _kvService.GetKVsOfGivenUser(userId);
            if (Kadras == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return Ok(Kadras);
        }

        /// <summary>
        /// Returns Kadras of given type
        /// </summary>
        /// <param name="kvTypeId">The id of kadra type </param>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User is not Admin</response>
        ///  <response code="404"> param is not valid</response>
        [HttpGet("{kvTypeId}")]
        public async Task<IActionResult> GetKVsWithType(int kvTypeId)
        {
            var Kadras = await _kvService.GetKVsWithKVType(kvTypeId);
            return Ok(Kadras);
        }

        /// <summary>
        /// Returns all kadra types
        /// </summary>
        /// <response code="200">Successful operation</response>
        ///  <response code="404"> no types yet in database</response>
        [HttpGet("kvTypes")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetKVTypes()
        {
            var Types = await _kvTypeService.GetAllKVTypesAsync();
            if (Types == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(Types);
        }

        /// <summary>
        /// Returns all kadras
        /// </summary>
        /// <response code="403">User is not Admin</response>
        ///  <response code="404"> no kadras yet in database</response>
        [HttpGet("kadras")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetAllKVs()
        {
            var KVs = await _kvService.GetAllKVsAsync();
            if (KVs == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(KVs);
        }

        /// <summary>
        /// Detects if user has such type of educators staff
        /// </summary>
        /// <response code="200">Successful operation</response>
        [HttpGet("{UserId}/{kadraId}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<bool> GetUserStaff(string UserId, int kadraId)
        {
            bool hasstaff = await _kvService.DoesUserHaveSuchStaff(UserId, kadraId);
            return hasstaff;
        }

        /// <summary>
        /// Detects if theres already a staff with such register number
        /// </summary>
        /// <response code="200">Successful operation</response>
        [HttpGet("registerexist/{numberInRegister}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<bool> GetStaffWithRegisternumber(int numberInRegister)
        {
            bool hasstaff = await _kvService.StaffWithRegisternumberExists(numberInRegister);
            return hasstaff;
        }

        /// <summary>
        /// Detects if user except this one has such type of educators staff
        /// </summary>
        /// <response code="200">Successful operation</response>
        [HttpGet("edit/{UserId}/{kadraId}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<bool> GetUserStaffEdit(string UserId, int kadraId)
        {
            bool hasstaff = await _kvService.UserHasSuchStaffEdit(UserId, kadraId);
            return hasstaff;
        }

        /// <summary>
        /// Detects if edu staff with such register number except this one is already in database
        /// </summary>
        /// <response code="200">Successful operation</response>
        [HttpGet("edit/registerexist/{kadraId}/{numberInRegister}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<bool> GetStaffWithRegisternumberEdit(int kadraId, int numberInRegister)
        {
            bool hasstaff = await _kvService.StaffWithRegisternumberExistsEdit(kadraId, numberInRegister);
            return hasstaff;
        }

        [HttpGet("GetEduStaffById/{KadraID}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetEduStaffById(int KadraID)
        {
            var staff = await _kvService.GetKadraById(KadraID);
            return Ok(staff);
        }

        [HttpGet("findUserForRedirect/{EduStaffId}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<string> GetUserByEduStaff(int EduStaffId)
        {
            string UserId = await _kvService.GetUserByEduStaff(EduStaffId);
            return UserId;
        }
    }
}
