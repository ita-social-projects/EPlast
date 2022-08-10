using EPlast.DataAccess.Entities.Blank;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.DTO.Course
{
    public class CourseDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public bool IsFinishedByUser { get; set; }
    }
}
