using EPlast.BLL.DTO.Blank;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Blank
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDTO>> GetAllAsync();
        Task<CourseDTO> AddCourseAsync(CourseDTO courseDto);

    }
}
