using EPlast.ViewModels.Admin;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.ViewModels
{
    public class ClubAdministrationViewModel
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public ClubUserViewModel User { get; set; }
        public int AdminTypeId { get; set; }
        public AdminTypeViewModel AdminType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
