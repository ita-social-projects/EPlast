using EPlast.BLL.DTO.Statistics;
using EPlast.BLL.ExtensionMethods;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Interfaces.Statistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [ApiController, Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Голова Пласту, Голова Округу, Голова Станиці")]
    public class StatisticsController: ControllerBase
    {
        private readonly ICityStatisticsService cityStatisticsService;
        private readonly IRegionStatisticsService regionStatisticsService;
        private readonly ILoggerService<StatisticsController> loggerService;

        public StatisticsController(ICityStatisticsService cityStatisticsService,
                                    IRegionStatisticsService regionStatisticsService,
                                    ILoggerService<StatisticsController> loggerService)
        {
            this.cityStatisticsService = cityStatisticsService;
            this.regionStatisticsService = regionStatisticsService;
            this.loggerService = loggerService;
        }
              
        /// <summary>
        /// Method to get statistics for the cities for the years
        /// </summary>
        /// <param name="statisticsParameters">Statistics parameters: cities id's, years, indicators</param>
        /// <returns>Statistics for the cities on the specified indicators and years</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User does not have access to the statistics</response>
        /// <response code="404">The statistics does not exist</response>
        [HttpGet("cities")]
        public async Task<IActionResult> GetCitiesStatistics([FromQuery] CitiesStatisticsParameters statisticsParameters)
        {
                return StatusCode(StatusCodes.Status200OK, await cityStatisticsService.GetCitiesStatisticsAsync(statisticsParameters.CityIds,
                                                                                                              statisticsParameters.Years,
                                                                                                              statisticsParameters.Indicators));
        }

        /// <summary>
        /// Method to get statistics for the regions for the years
        /// </summary>
        /// <param name="statisticsParameters">Statistics parameters: region id's, years, indicators</param>
        /// <returns>Statistics for the regions on the specified indicators and years</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User does not have access to the statistics</response>
        /// <response code="404">The statistics does not exist</response>
        [HttpGet("regions")]
        public async Task<IActionResult> GetRegionsStatistics([FromQuery] RegionsStatisticsParameters statisticsParameters)
        {
                return StatusCode(StatusCodes.Status200OK, await regionStatisticsService.GetRegionsStatisticsAsync(statisticsParameters.RegionIds,
                                                                                                              statisticsParameters.Years,
                                                                                                              statisticsParameters.Indicators));            
        }
    }
}
