using System;
using System.ComponentModel.DataAnnotations;
using EPlast.BLL.DTO.City;

namespace EPlast.BLL.DTO.UserProfiles
{
    public class UserRenewalDto
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public UserDto User { get; set; }
        [Required]
        public int CityId { get; set; }
        public CityDto City { get; set; }
        [Required]
        public DateTime RequestDate { get; set; }
        public bool Approved { get; set; }
    }
}
