using EPlast.ViewModels.UserInformation.UserProfile;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.ViewModels.City
{
    public class CityMembersViewModel
    {
        public int ID { get; set; }

        public int CityId { get; set; }
        public CityViewModel City { get; set; }

        [Required]
        public string UserId { get; set; }
        public UserViewModel User { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
