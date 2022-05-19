using EPlast.BLL;
using EPlast.Resources;
using EPlast.WebApi.Models.City;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.WebApi.Models.Precaution
{
    public class UserPrecautionViewModel
    {
        public int Id { get; set; }
        public int PrecautionId { get; set; }
        public PrecautionDTO Precaution { get; set; }
        public string Reporter { get; set; }
        public string Reason { get; set; }
        [Required]
        public UserPrecautionStatus? Status { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public CityUserViewModel User { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
