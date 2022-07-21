using EPlast.BLL.DTO.Blank;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Blank
{
    public interface IUserCourseService
    {
        Task<IEnumerable<CourseDTO>> GetCourseByIdAsync(string userid);
        public  Task ChangeCourseStatus(string userid,int courseid);
      
    }
}
