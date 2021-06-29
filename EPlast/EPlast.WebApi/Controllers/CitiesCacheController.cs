using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using EPlast.WebApi.Extensions;
using EPlast.BLL.Interfaces.City;
using Microsoft.AspNetCore.Authorization;
using EPlast.Resources;
using EPlast.BLL.DTO.City;
using EPlast.WebApi.Models.City;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesCacheController : Controller
    {
        private readonly IDistributedCache _cache;
        private readonly ICityService _cityService;

        public CitiesCacheController(ICityService cityService,IDistributedCache cache)
        {
            _cache = cache;
            _cityService = cityService;
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
            string recordKey = "Cities_" + DateTime.Now.ToString("yyyyMMdd_hhmm");
            IEnumerable<CityDTO> cities = await _cache.GetRecordAsync<IEnumerable<CityDTO>>(recordKey);

            if (cities is null)
            {
                cities = await _cityService.GetAllDTOAsync();
                await _cache.SetRecordAsync(recordKey, cities);
            }
            var citiesViewModel = new CitiesViewModel(page, pageSize, cities, cityName, User.IsInRole(Roles.Admin));

            return Ok(citiesViewModel);
        }
    }
}
