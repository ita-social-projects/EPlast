﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EPlast.DataAccess.Entities.Blank
{
    public class Course
    {
        public int ID { get; set; }
        [Required, MaxLength(50, ErrorMessage = "Назва курсу немає перевищувати 50 символів")]
        public string Name { get; set; }
        public string Link { get; set; }
        public ICollection<UserCourse> UserCourses { get; set; }

    }
}
