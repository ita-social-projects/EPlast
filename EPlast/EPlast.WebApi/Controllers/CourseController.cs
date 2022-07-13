using EPlast.BLL.Interfaces.Blank;
using EPlast.BLL.Interfaces.Logging;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IUserCourseService _usercourseService;
        private readonly ILoggerService<CoursesController> _loggerService;
        private readonly UserManager<User> _userManager;

        public CoursesController(ICourseService courseService, IUserCourseService usercourseService , UserManager<User> userManager, ILoggerService<CoursesController> loggerService)
        {
            _courseService = courseService;
            _usercourseService = usercourseService;
            _userManager = userManager;
            _loggerService = loggerService;
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
        public async Task<IActionResult> GetAllCourseByUserId(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                _loggerService.LogError($"User not found. UserId:{userId}");
                return NotFound();
            }

            var courses = await _usercourseService.GetCourseByIdAsync(userId);
            
            if (!courses.Any())
            {
                return NotFound();
            }

            return Ok(courses);
        }
        [HttpPut("{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public  async Task<IActionResult> ChangeStatusCourseByUseerId(string userid)
        {
            await _usercourseService.ChangeCourseStatus(userid);
            return Ok();
        }
    }
}
