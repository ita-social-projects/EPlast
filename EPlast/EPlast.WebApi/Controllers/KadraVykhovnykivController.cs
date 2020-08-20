using AutoMapper;
using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.EducatorsStaff;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Services.EducatorsStaff;
using EPlast.DataAccess.Entities.EducatorsStaff;
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
        private readonly IMapper _mapper;
        private readonly IKVService _kvService;
        private readonly IKVsTypeService _kvTypeService;
     
        public KadraVykhovnykivController(ILoggerService<KadraVykhovnykivController> logger, IMapper mapper, IKVService kvService, IKVsTypeService kvTypeService)
        {
            _logger = logger;
            _mapper = mapper;
            _kvService = kvService;
            _kvTypeService = kvTypeService;
        }

        [HttpPost("CreateKadra")]
        public async Task<IActionResult> CreateKadra(KadrasDTO kvDTO)
        {
                if (User.IsInRole("Admin"))
                {
                    var newKadra=await _kvService.CreateKadra(kvDTO);
                    _logger.LogInformation($"User {{{kvDTO.UserId}}} gained Kadra Vykhovnykiv of type: {{{kvDTO.KVTypesID}}}");
                     return StatusCode(StatusCodes.Status200OK);
                }
                else
                {
                _logger.LogError("Current user is not an admin");
                return StatusCode(StatusCodes.Status403Forbidden);
                }
              
        }


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

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, KadrasDTO kadrasDTO)
        {
           
            if (User.IsInRole("Admin"))
            {
                if (id != kadrasDTO.ID)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                await _kvService.UpdateKadra(kadrasDTO);
                return StatusCode(StatusCodes.Status200OK);
            }
            else
            {
                _logger.LogError("Current user is not an admin");
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            
        }


    }
}