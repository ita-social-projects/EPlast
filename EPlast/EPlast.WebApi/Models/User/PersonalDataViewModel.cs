using EPlast.BusinessLogicLayer.DTO.UserProfiles;
using System;

namespace EPlast.WebApi.Models.User
{
    public class PersonalDataViewModel
    {
        public UserDTO User { get; set; }
        public TimeSpan TimeToJoinPlast { get; set; }
        public bool IsUserPlastun { get; set; }
    }
}
