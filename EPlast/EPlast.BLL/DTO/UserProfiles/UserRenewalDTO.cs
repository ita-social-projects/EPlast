using System;
using System.ComponentModel.DataAnnotations;
using EPlast.BLL.DTO.City;


namespace EPlast.BLL.DTO.UserProfiles
{
    public class UserRenewalDTO
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public UserDTO User { get; set; }
        [Required]
        public int CityId { get; set; }
        public CityDTO City { get; set; }
        [Required]
        public DateTime RequestDate { get; set; }
        public bool Approved { get; set; }
    }
}
