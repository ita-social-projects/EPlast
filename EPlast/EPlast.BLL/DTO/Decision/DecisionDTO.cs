﻿using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.BLL.DTO
{
    public class DecisionDTO
    {
        public int ID { get; set; }
        [Required, MaxLength(60, ErrorMessage = "Назва рішення не має перевищувати 60 символів")]
        public string Name { get; set; }
        [Required]
        public DecisionStatusTypeDTO DecisionStatusType { get; set; }
        [Required]
        public GoverningBodyDTO GoverningBody { get; set; }
        [Required]
        public DecisionTargetDTO DecisionTarget { get; set; }
        [Required, MaxLength(1000, ErrorMessage = "Текст рішення не має перевищувати 1000 символів")]
        public string Description { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string UserId { get; set; }

        public string FileName { get; set; }
    }
}