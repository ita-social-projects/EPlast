using EPlast.BLL.DTO.Blank;
using EPlast.BLL.DTO.Course;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Blank
{
    public interface IUserCourseService
    {
        Task<IEnumerable<CourseDto>> GetCourseByUserIdAsync(string userid);
        
      
    }
}
