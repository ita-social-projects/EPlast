using EPlast.BLL.Interfaces.Blank;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IUserCourseService _usercourseService;

        public CoursesController(ICourseService courseService, IUserCourseService usercourseService)
        {
            _courseService = courseService;
            _usercourseService = usercourseService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _courseService.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAllCourseByUseerId(string userid)
        {
            var result = await _usercourseService.GetCourseByIdAsync(userid);
            return Ok(result);
        }
    }
}
