using EPlast.BLL.Interfaces.GoverningBodies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetGoverningBodies()
        {
            var governingBodies = await _governingBodiesService.GetGoverningBodiesListAsync();
            return Ok(governingBodies);
        }

    }
}
