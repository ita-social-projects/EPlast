using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.BLL.Interfaces.EducatorsStaff;
using EPlast.BLL.Interfaces.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KadraVykhovnykivController : ControllerBase
    {
        private readonly ILoggerService<KadraVykhovnykivController> _logger;
        private readonly IKadraService _kvService;
        private readonly IKadrasTypeService _kvTypeService;
     
        public KadraVykhovnykivController(ILoggerService<KadraVykhovnykivController> logger,  IKadraService kvService, IKadrasTypeService kvTypeService)
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
        /// <response code="403">User is not admin</response>
        [HttpPost("CreateKadra")]
        public async Task<IActionResult> CreateKadra(KadraVykhovnykivDTO kvDTO)
        {
                if (User.IsInRole("Admin"))
                {
                    var newKadra=await _kvService.CreateKadra(kvDTO);
                    _logger.LogInformation($"User {{{kvDTO.UserId}}} gained Kadra Vykhovnykiv of type: {{{kvDTO.KVTypesID}}}");
                return Ok(newKadra);
                }
                else
                {
                _logger.LogError("Current user is not an admin");
                return StatusCode(StatusCodes.Status403Forbidden);
                }
              
        }

        /// <summary>
        /// Deletes Kadra by id
        /// </summary>
        /// <param name="kadraId">The id of kadra that will be deleted</param>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User is not admin</response>
        ///  <response code="404">kadra with this id doesn't exist</response>
        [HttpDelete("RemoveKadra/{kadra_id}")]
        public async Task<IActionResult> Remove(int kadraId)
        {
            try
            {
                if (User.IsInRole("Admin"))
                {
                    await _kvService.DeleteKadra(kadraId);
                    return StatusCode(StatusCodes.Status200OK);
                }
                else
                {
                    _logger.LogError("Current user is not an admin");
                    return StatusCode(StatusCodes.Status403Forbidden);
                }
            }
            catch( InvalidOperationException e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status404NotFound);
            }
           
        }


        /// <summary>
        /// Deletes Updates by id
        /// </summary>
        /// <param name="kadrasDTO">The dto of kadra that will be updated</param>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User is not admin</response>
        [HttpPut()]
        public async Task<IActionResult> Update( KadraVykhovnykivDTO kadrasDTO)
        {
           
            if (User.IsInRole("Admin"))
            {
                await _kvService.UpdateKadra(kadrasDTO);
                return StatusCode(StatusCodes.Status200OK);
            }
            else
            {
                _logger.LogError("Current user is not an admin");
                return StatusCode(StatusCodes.Status403Forbidden);
            } 
        }


        /// <summary>
        /// Returns Kadras of given user
        /// </summary>
        /// <param name="userId">The id of user whose kadra we try to get</param>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User is not admin</response>
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
           if(Kadras == null)
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
        /// <response code="403">User is not admin</response>
        ///  <response code="404"> param is not valid</response>
        [HttpGet("{kvtype_id}")]
        public async Task<IActionResult> GetKVsWithType(int kvTypeId)
        {
            if (User.IsInRole("Admin"))
            {
                try
                {
                    var Kadras = await _kvService.GetKVsWithKVType(kvTypeId);
                    return Ok(Kadras);
                }
                catch (InvalidOperationException e)
                {
                    _logger.LogError(e.Message);
                    return StatusCode(StatusCodes.Status404NotFound);
                } 
            }
            else
            {
                _logger.LogError("Current user is not an admin");
                return StatusCode(StatusCodes.Status403Forbidden);
            }
        }


        /// <summary>
        /// Returns all kadra types
        /// </summary>
        /// <response code="200">Successful operation</response>
        ///  <response code="404"> no types yet in database</response>
        [HttpGet("kvTypes")]
        public async Task<IActionResult> GetKVTypes()
        {
            var Types = await _kvTypeService.GetAllKVTypesAsync();
            if(Types == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(Types);    
        }

        /// <summary>
        /// Returns all kadras 
        /// </summary>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User is not admin</response>
        ///  <response code="404"> no kadras yet in database</response>
        [HttpGet("kadras")]
        public async Task<IActionResult> GetAllKVs()
        {
            if (User.IsInRole("Admin"))
            {
                var KVs = await _kvService.GetAllKVsAsync();
                if (KVs == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                return Ok(KVs);
            }
            else
            {
                _logger.LogError("Current user is not an admin");
                return StatusCode(StatusCodes.Status403Forbidden);
            }
        }




    }
}