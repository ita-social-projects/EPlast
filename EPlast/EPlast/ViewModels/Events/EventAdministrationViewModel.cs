using EPlast.BussinessLayer.DTO.AnnualReport;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels.UserInformation.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.ViewModels.Events
{
    public class EventAdministrationViewModel
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }

}
