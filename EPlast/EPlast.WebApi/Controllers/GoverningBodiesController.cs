using EPlast.BLL.Interfaces.GoverningBodies;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoverningBodiesController : ControllerBase
    {
        private readonly IGoverningBodiesService _governingBodiesService;
        public GoverningBodiesController(IGoverningBodiesService service) 
        {
            _governingBodiesService = service;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> getOrganizations()
        {
            var governingBodies = await _governingBodiesService.GetOrganizationListAsync();
            return Ok(governingBodies);
        }
    }
}
