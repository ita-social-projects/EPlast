﻿using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class GoverningBodyAdministration
    {
        public int ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int GoverningBodyId { get; set; }
        public Organization GoverningBody { get; set; }

        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
        public bool Status { get; set; }
        public int AdminTypeId { get; set; }
        public AdminType AdminType { get; set; }
    }
}
