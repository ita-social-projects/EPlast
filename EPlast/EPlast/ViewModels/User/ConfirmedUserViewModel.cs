using System;
using System.ComponentModel.DataAnnotations;
using EPlast.ViewModels.UserInformation.UserProfile;

namespace EPlast.ViewModels
{
    public class ConfirmedUserViewModel
    {
        public int ID { get; set; }
        [Required]
        public UserViewModel User { get; set; }
        public string UserID { get; set; }
        public int? ApproverID { get; set; }
        public ApproverViewModel Approver { get; set; }
        public DateTime ConfirmDate { get; set; }
        public bool isClubAdmin { get; set; }
        public bool isCityAdmin { get; set; }
    }
}
