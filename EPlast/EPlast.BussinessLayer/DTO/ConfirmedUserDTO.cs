using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.DTO
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
