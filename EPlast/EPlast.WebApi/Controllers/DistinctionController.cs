using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.Interfaces.Logging;
using EPlast.DataAccess.Entities.UserEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistinctionController : ControllerBase
    {
        private readonly IDistinctionService _distinctionService;
        private readonly IUserDistinctionService _userDistinctionService;
        private readonly IMapper _mapper;
        private readonly ILoggerService<DistinctionController> _loggerService;


        public DistinctionController(IDistinctionService distinctionService, 
            IUserDistinctionService userDistinctionService,
            IMapper mapper, 
            ILoggerService<DistinctionController> loggerService)
        {
            _distinctionService = distinctionService;
            _userDistinctionService = userDistinctionService;
            _mapper = mapper;
            _loggerService = loggerService;
        }

        /// <summary>
        /// Returns the tinction by id
        /// </summary>
        /// <param name="id">Distinction id</param>
        /// <returns> object</returns>
        /// <response code="200">An instance of distinction</response>
        /// <response code="404">The distinction does not exist</response>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserDistinction(int id)
        {
            UserDistinctionDTO userDistinction = await _userDistinctionService.GetUserDistinction(id);
            if (userDistinction == null)
                return NotFound();
            return Ok(userDistinction);
        }
        /// <summary>
        /// Returns all distinction
        /// </summary>
        /// <returns>All distinction</returns>
        /// <response code="200">Array of all distinction</response>
        [HttpGet]
        public async Task<IActionResult> GetUserDistinction()
        {
            IEnumerable<UserDistinctionDTO> userDistinctions = await _userDistinctionService.GetAllUsersDistinctionAsync();
            return Ok(userDistinctions);
        }
    }
}