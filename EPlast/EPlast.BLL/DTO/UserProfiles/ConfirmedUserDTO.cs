using System;
using System.ComponentModel.DataAnnotations;
using EPlast.BLL.DTO.UserProfiles;
 using EPlast.Resources;

namespace EPlast.BLL.DTO
{
    public class ConfirmedUserDto
    {
        public int ID { get; set; }

        [Required]
        public UserDto User { get; set; }

        public string UserID { get; set; }
        public int? ApproverID { get; set; }
        public ApproverDto Approver { get; set; }
        public DateTime ConfirmDate { get; set; }
        public ApproveType ApproveType { get; set; }
    }
}
