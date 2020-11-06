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
    //[Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Голова Пласту, Голова Округу, Голова Станиці")]
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
        /// Method to get statistics for all cities for all years
        /// </summary>        
        /// <returns>Statistics for all cities for all years</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User does not have access to the statistics</response>
        /// <response code="404">The statistics does not exist</response>
        [HttpGet("cities/allstatistics")]
        public async Task<IActionResult> GetAllCitiesStatistics()
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, await cityStatisticsService.GetAllCitiesStatisticsAsync());
            }
            catch (NullReferenceException)
            {
                loggerService.LogError($"The statistics was not found");
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                loggerService.LogError($"User does not have access to the statistics");
                return StatusCode(StatusCodes.Status403Forbidden);
            }
        }

        /// <summary>
        /// Method to get statistics for the city for the year
        /// </summary>
        /// <param name="cityId">City identification number</param>
        /// <param name="year">Year of which statistics is needed</param>
        /// <param name="indicators">Statistics parameters</param>
        /// <returns>Statistics for the city on the specified indicators and year</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User does not have access to the statistics</response>
        /// <response code="404">The statistics does not exist</response>
        [HttpGet("city/{cityId}/{year}")]
        public async Task<IActionResult> GetStatisticsForCityForYear(int cityId, int year, [FromQuery] IEnumerable<StatisticsItemIndicator> indicators)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, await cityStatisticsService.GetCityStatisticsAsync(cityId, year, indicators));
            }
            catch (NullReferenceException)
            {
                loggerService.LogError($"The statistics was not found");
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (UnauthorizedAccessException)
            {
                loggerService.LogError($"User does not have access to the statistics");
                return StatusCode(StatusCodes.Status403Forbidden);
            }
        }

        /// <summary>
        /// Method to get statistics for the city for the period
        /// </summary>
        /// <param name="cityId">City identification number</param>
        /// <param name="minYear">Start of statistics period</param>
        /// /// <param name="maxYear">Finish of statistics period</param>
        /// <param name="indicators">Statistics parameters</param>
        /// <returns>Statistics for the city on the specified indicators and period</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User does not have access to the statistics</response>
        /// <response code="404">The statistics does not exist</response>
        [HttpGet("city/{cityId}/{minYear}/{maxYear}")]
        public async Task<IActionResult> GetStatisticsForCityForPeriod(int cityId, int minYear, int maxYear, [FromQuery] IEnumerable<StatisticsItemIndicator> indicators)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, await cityStatisticsService.GetCityStatisticsAsync(cityId, minYear, maxYear, indicators));
            }
            catch (NullReferenceException)
            {
                loggerService.LogError($"The statistics was not found");
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (UnauthorizedAccessException)
            {
                loggerService.LogError($"User does not have access to the statistics");
                return StatusCode(StatusCodes.Status403Forbidden);
            }
        }

        /// <summary>
        /// Method to get statistics for the cities for the year
        /// </summary>
        /// <param name="citiesId">Cities identification numbers</param>
        /// <param name="year">Year of which statistics is needed</param>
        /// <param name="indicators">Statistics parameters</param>
        /// <returns>Statistics for the cities on the specified indicators and year</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User does not have access to the statistics</response>
        /// <response code="404">The statistics does not exist</response>
        [HttpGet("cities/{year}")]
        public async Task<IActionResult> GetForCitiesForYear([FromQuery] IEnumerable<int> citiesId, int year, [FromQuery] IEnumerable<StatisticsItemIndicator> indicators)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, await cityStatisticsService.GetCityStatisticsAsync(citiesId, year, indicators));
            }
            catch (NullReferenceException)
            {
                loggerService.LogError($"The statistics was not found");
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (UnauthorizedAccessException)
            {
                loggerService.LogError($"User does not have access to the statistics");
                return StatusCode(StatusCodes.Status403Forbidden);
            }
        }

        /// <summary>
        /// Method to get statistics for the cities for the period
        /// </summary>
        /// <param name="citiesId">Cities identification numbers</param>
        /// <param name="minYear">Start of statistics period</param>
        /// <param name="maxYear">Finish of statistics period</param>
        /// <param name="indicators">Statistics parameters</param>
        /// <returns>Statistics for the cities on the specified indicators and period</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User does not have access to the statistics</response>
        /// <response code="404">The statistics does not exist</response>
        [HttpGet("cities/{minYear}/{maxYear}")]
        public async Task<IActionResult> GetStatisticsForCitiesForPeriod([FromQuery] IEnumerable<int> citiesId, int minYear, int maxYear,
                                                                         [FromQuery] IEnumerable<StatisticsItemIndicator> indicators)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, await cityStatisticsService.GetCityStatisticsAsync(citiesId, minYear, maxYear, indicators));
            }
            catch (NullReferenceException)
            {
                loggerService.LogError($"The statistics was not found");
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (UnauthorizedAccessException)
            {
                loggerService.LogError($"User does not have access to the statistics");
                return StatusCode(StatusCodes.Status403Forbidden);
            }
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
        public async Task<IActionResult> GetForCitiesForYears( [FromQuery] StatisticsParameters statisticsParameters)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, await cityStatisticsService.GetCityStatisticsAsync(statisticsParameters.CitiesId,
                                                                                                              statisticsParameters.Years,
                                                                                                              statisticsParameters.Indicators));
            }
            catch (NullReferenceException)
            {
                loggerService.LogError($"The statistics was not found");
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (UnauthorizedAccessException)
            {
                loggerService.LogError($"User does not have access to the statistics");
                return StatusCode(StatusCodes.Status403Forbidden);
            }
        }

        /// <summary>
        /// Method to get statistics for all regions for all years
        /// </summary>        
        /// <returns>Statistics for all regions for all years</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User does not have access to the statistics</response>
        /// <response code="404">The statistics does not exist</response>
        [HttpGet("regions/allstatistics")]
        public async Task<IActionResult> GetAllRegionsStatistics()
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, await regionStatisticsService.GetAllRegionsStatisticsAsync());
            }
            catch (NullReferenceException)
            {
                loggerService.LogError($"The statistics was not found");
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                loggerService.LogError($"User does not have access to the statistics");
                return StatusCode(StatusCodes.Status403Forbidden);
            }
        }

        /// <summary>
        /// Method to get statistics for the region for the year
        /// </summary>
        /// <param name="regionId">Region identification number</param>
        /// <param name="year">Year of which statistics is needed</param>
        /// <param name="indicators">Statistics parameters</param>
        /// <returns>Statistics for the region on the specified indicators and year</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User does not have access to the statistics</response>
        /// <response code="404">The statistics does not exist</response>
        [HttpGet("region/{regionId}/{year}")]
        public async Task<IActionResult> GetStatisticsForRegionForYear(int regionId, int year, [FromQuery] IEnumerable<StatisticsItemIndicator> indicators)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, await regionStatisticsService.GetRegionStatisticsAsync(regionId, year, indicators));
            }
            catch (NullReferenceException)
            {
                loggerService.LogError($"The statistics was not found");
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (UnauthorizedAccessException)
            {
                loggerService.LogError($"User does not have access to the statistics");
                return StatusCode(StatusCodes.Status403Forbidden);
            }
        }

        /// <summary>
        /// Method to get statistics for the region for the period
        /// </summary>
        /// <param name="regionId">Region identification number</param>
        /// <param name="minYear">Start of statistics period</param>
        /// /// <param name="maxYear">Finish of statistics period</param>
        /// <param name="indicators">Statistics parameters</param>
        /// <returns>Statistics for the region on the specified indicators and period</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User does not have access to the statistics</response>
        /// <response code="404">The statistics does not exist</response>
        [HttpGet("region/{regionId}/{minYear}/{maxYear}")]
        public async Task<IActionResult> GetStatisticsForRegionForPeriod(int regionId, int minYear, int maxYear, [FromQuery] IEnumerable<StatisticsItemIndicator> indicators)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, await regionStatisticsService.GetRegionStatisticsAsync(regionId, minYear, maxYear, indicators));
            }
            catch (NullReferenceException)
            {
                loggerService.LogError($"The statistics was not found");
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (UnauthorizedAccessException)
            {
                loggerService.LogError($"User does not have access to the statistics");
                return StatusCode(StatusCodes.Status403Forbidden);
            }
        }

        /// <summary>
        /// Method to get statistics for the regions for the year
        /// </summary>
        /// <param name="regionsId">Regions identification numbers</param>
        /// <param name="year">Year of which statistics is needed</param>
        /// <param name="indicators">Statistics parameters</param>
        /// <returns>Statistics for the regions on the specified indicators and year</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User does not have access to the statistics</response>
        /// <response code="404">The statistics does not exist</response>
        [HttpGet("regions/{year}")]
        public async Task<IActionResult> GetStatisticsForRegionsForYear([FromQuery] IEnumerable<int> regionsId, int year,
                                                                        [FromQuery] IEnumerable<StatisticsItemIndicator> indicators)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, await regionStatisticsService.GetRegionStatisticsAsync(regionsId, year, indicators));
            }
            catch (NullReferenceException)
            {
                loggerService.LogError($"The statistics was not found");
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (UnauthorizedAccessException)
            {
                loggerService.LogError($"User does not have access to the statistics");
                return StatusCode(StatusCodes.Status403Forbidden);
            }
        }

        /// <summary>
        /// Method to get statistics for the regions for the period
        /// </summary>
        /// <param name="regionsId">Regions identification numbers</param>
        /// <param name="minYear">Start of statistics period</param>
        /// /// <param name="maxYear">Finish of statistics period</param>
        /// <param name="indicators">Statistics parameters</param>
        /// <returns>Statistics for the regions on the specified indicators and period</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User does not have access to the statistics</response>
        /// <response code="404">The statistics does not exist</response>
        [HttpGet("regions/{minYear}/{maxYear}")]
        public async Task<IActionResult> GetStatisticsForRegionsForPeriod([FromQuery] IEnumerable<int> regionsId, int minYear, int maxYear,
                                                                          [FromQuery] IEnumerable<StatisticsItemIndicator> indicators)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, await regionStatisticsService.GetRegionStatisticsAsync(regionsId, minYear, maxYear, indicators));
            }
            catch (NullReferenceException)
            {
                loggerService.LogError($"The statistics was not found");
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (UnauthorizedAccessException)
            {
                loggerService.LogError($"User does not have access to the statistics");
                return StatusCode(StatusCodes.Status403Forbidden);
            }
        }

        /// <summary>
        /// Method to get statistics for the regions for the years
        /// </summary>
        /// <param name="regionsId">Regions identification numbers</param>
        /// <param name="years">Years of which statistics is needed</param>
        /// <param name="indicators">Statistics parameters</param>
        /// <returns>Statistics for the regions on the specified indicators and years</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User does not have access to the statistics</response>
        /// <response code="404">The statistics does not exist</response>
        [HttpGet("regions")]
        public async Task<IActionResult> GetStatisticsForRegionsForYears([FromQuery] IEnumerable<int> regionsId,
                                                                         [FromQuery] IEnumerable<int> years,
                                                                         [FromQuery] IEnumerable<StatisticsItemIndicator> indicators)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, await regionStatisticsService.GetRegionStatisticsAsync(regionsId, years, indicators));
            }
            catch (NullReferenceException)
            {
                loggerService.LogError($"The statistics was not found");
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (UnauthorizedAccessException)
            {
                loggerService.LogError($"User does not have access to the statistics");
                return StatusCode(StatusCodes.Status403Forbidden);
            }
        }

        /// <summary>
        /// Method to get statistics item indicators
        /// </summary>
        /// <returns>List of enum values</returns>
        /// <response code="200">Successful operation</response>
        [HttpGet("getIndicators")]
        public IActionResult GetIndicators()
        {
            var indicators = new List<string>();
            foreach (var enumValue in Enum.GetValues(typeof(StatisticsItemIndicator)).Cast<StatisticsItemIndicator>())
            {
                indicators.Add(enumValue.GetDescription());
            }
            return StatusCode(StatusCodes.Status200OK, new { indicators });
        }

    }
}
