using EPlast.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly IAreaService _areaService;

        public AreaController(IAreaService areaService)
        {
            _areaService = areaService;
        }

        /// <summary>
        /// Get all areas
        /// </summary>
        /// <returns>Array of areas</returns>
        /// <response code="200">Successful operation</response>
        [HttpGet]
        public async Task<IActionResult> GetAreas()
        {
            var areasDTOs = await _areaService.GetAllAsync();
            return Ok(areasDTOs);
        }

        /// <summary>
        /// Get one area
        /// </summary>
        /// <param name="id">Id of area</param>
        /// <returns>Area with this id</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Area with this id not found</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAreaById(int id)
        {
            try
            {
                var areaDTO = await _areaService.GetByIdAsync(id);
                return Ok(areaDTO);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
          
        }
    }
}
