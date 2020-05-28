using EPlast.DataAccess.Entities;
using EPlast.ViewModels.UserInformation.UserProfile;
using System;

namespace EPlast.ViewModels.City
{
    public class CityAdministrationViewModel
    {
        public int ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int CityId { get; set; }
        public CityViewModel City { get; set; }
        public string UserId { get; set; }
        public UserViewModel User { get; set; }
        public int AdminTypeId { get; set; }
        public AdminType AdminType { get; set; }
        public CityManagement CityManagement { get; set; }
    }
}
