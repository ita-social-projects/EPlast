using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EPlast.DataAccess.Entities.Blank
{
    public class UserCourse
    {
        public int ID { get; set; }
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
        [Required]
        public int CourseId { get; set; }
        public Course Сourse { get; set; }
        public bool StatusPassedCourse { get; set; }

    }
}
