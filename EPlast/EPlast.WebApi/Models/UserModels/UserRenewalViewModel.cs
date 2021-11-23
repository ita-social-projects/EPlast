using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.WebApi.Models.UserModels
{
    public class UserRenewalViewModel
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int CityId { get; set; }
        [Required]
        public DateTime RequestDate { get; set; }
        public bool Approved { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string CityName { get; set; }
        public string Email { get; set; }
    }
}
