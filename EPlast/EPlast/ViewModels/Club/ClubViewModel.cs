﻿using System.ComponentModel.DataAnnotations;

namespace EPlast.ViewModels
{
    public class ClubViewModel
    {
        public int ID { get; set; }
        [Required, MaxLength(50, ErrorMessage = "Club name cannot exceed 50 characters")]
        public string ClubName { get; set; }
        public string ClubURL { get; set; }
        [MaxLength(1024, ErrorMessage = "Club description cannot exceed 1024 characters")]
        public string Description { get; set; }
        public string Logo { get; set; }
    }
}
