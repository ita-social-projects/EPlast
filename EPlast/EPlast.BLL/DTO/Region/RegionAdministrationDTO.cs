using System;
using System.ComponentModel.DataAnnotations;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.City;

namespace EPlast.BLL.DTO.Region
{
    public class RegionAdministrationDto
    {
        public int ID { get; set; }
        [Required]
        public string UserId { get; set; }
        public CityUserDto User { get; set; }
        public int CityId { get; set; }
        [Required]
        public int AdminTypeId { get; set; }
        public AdminTypeDto AdminType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Required]
        public int RegionId { get; set; }
        public RegionDto Region { get; set; }
        public bool Status { get; set; }
    }
}