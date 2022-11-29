using EPlast.BLL.DTO.UserProfiles;
using System;

namespace EPlast.WebApi.Models.UserModels
{
    public class PersonalDataViewModel
    {
        public UserViewModel User { get; set; }
        public UserShortViewModel ShortUser { get; set; }
        public int TimeToJoinPlast { get; set; }
        public bool IsUserPlastun { get; set; }
        public bool IsUserAdmin { get; set; }
    }
}
