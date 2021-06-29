using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using EPlast.WebApi.Extensions;
using EPlast.BLL.Interfaces.Club;
using Microsoft.AspNetCore.Authorization;
using EPlast.Resources;
using EPlast.BLL.DTO.Club;
using EPlast.WebApi.Models.Club;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubCacheController : Controller
    {
        private readonly IDistributedCache _cache;
        private readonly IClubService _clubService;

        public ClubCacheController(IClubService clubService,IDistributedCache cache)
        {
            _clubService = clubService;
            _cache = cache;
        }
        /// <summary>
        /// Get a specific number of Clubs 
        /// </summary>
        /// <param name="page">A number of the page</param>
        /// <param name="pageSize">A count of Clubs to display</param>
        /// <param name="clubName">Optional param to find Clubs by name</param>
        /// <returns>A specific number of Clubs</returns>
        [HttpGet("Profiles/{page}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetClubs(int page, int pageSize, string clubName = null)
        {
            string recordKey = "Clubs_" + DateTime.Now.ToString("yyyyMM_hhmm");
            IEnumerable<ClubDTO> clubs = await _cache.GetRecordAsync<IEnumerable<ClubDTO>>(recordKey);

            if (clubs is null)
            {
                clubs = await _clubService.GetAllDTOAsync();
                await _cache.SetRecordAsync(recordKey, clubs);
            }

            var ClubsViewModel = new ClubsViewModel(page, pageSize, clubs, clubName, User.IsInRole(Roles.Admin));
            return Ok(ClubsViewModel);
        }
    }
}
