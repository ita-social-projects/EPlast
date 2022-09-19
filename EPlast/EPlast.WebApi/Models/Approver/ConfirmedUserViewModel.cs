using EPlast.Resources;
using EPlast.WebApi.Models.User;
using EPlast.WebApi.Models.UserModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.WebApi.Models.Approver
{
    public class ConfirmedUserViewModel
    {
        public int ID { get; set; }
        [Required]
        public UserInfoViewModel User { get; set; }
        public string UserID { get; set; }
        public int? ApproverID { get; set; }
        public ApproverViewModel Approver { get; set; }
        public DateTime ConfirmDate { get; set; }
        public ApproveType ApproveType { get; set; }
    }
}
