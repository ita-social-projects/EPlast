using EPlast.BusinessLogicLayer.DTO.UserProfiles;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.BusinessLogicLayer.DTO
{
    public class ConfirmedUserDTO
    {
        public int ID { get; set; }
        [Required]
        public UserDTO User { get; set; }
        public string UserID { get; set; }
        public int? ApproverID { get; set; }
        public ApproverDTO Approver { get; set; }
        public DateTime ConfirmDate { get; set; }
        public bool isClubAdmin { get; set; }
        public bool isCityAdmin { get; set; }
    }
}
