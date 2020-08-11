﻿using EPlast.BLL.DTO.ActiveMembership;
using EPlast.BLL.Interfaces.ActiveMembership;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActiveMembershipController : ControllerBase
    {
        private IPlastDegreeService _plastDegreeService;
        private IAccessLevelService _accessLevelService;
        public ActiveMembershipController(IPlastDegreeService plastDegreeService, IAccessLevelService accessLevelService)
        {
            _plastDegreeService = plastDegreeService;
            _accessLevelService = accessLevelService;
        }
        [HttpGet("dergees")]
        public async Task<IActionResult> GetAllDergees()
        {
            return Ok(await _plastDegreeService.GetDergeesAsync());
        }

        [HttpGet("accessLevel/{userId}")]
        public async Task<IActionResult> GetAccessLevel(string userId)
        {
            return Ok(await _accessLevelService.GetUserAccessLevelsAsync(userId));
        }
    }
}