﻿using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.BLL.DTO
{
    public class MethodicDocumentDTO
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public MethodicDocumentTypeDTO Type { get; set; }
        [Required]
        public OrganizationDTO Organization { get; set; }

        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime Date { get; set; }

        public string FileName { get; set; }
    }
}
