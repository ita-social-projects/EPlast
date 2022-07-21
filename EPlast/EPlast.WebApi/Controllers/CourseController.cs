using EPlast.BLL.DTO.Blank;
using EPlast.BLL.Interfaces.Blank;
using EPlast.BLL.Interfaces.Logging;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
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
            return Ok(courses);
        }
        [HttpPut("{userId}/{courseid}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public  async Task<IActionResult> ChangeStatusCourseByUseerId(string userid, int courseid)
        {
            await _usercourseService.ChangeCourseStatus(userid,courseid);
            return Ok();
        }

        [HttpPost("CreateCourse")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.Admin)]
        public async Task<IActionResult> AddCourse(CourseDTO courseDTO)
        {
            await _courseService.AddCourseAsync(courseDTO);

            return Created("Course", courseDTO);
        }

    }
}
