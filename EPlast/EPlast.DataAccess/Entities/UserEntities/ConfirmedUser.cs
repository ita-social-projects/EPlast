using EPlast.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace EPlast.DataAccess.Entities
{
    public class ConfirmedUser
    {
        public int ID { get; set; }
        [Required]
        public User User { get; set; }
        public string UserID { get; set; }
        public int? ApproverID { get; set; }
        public Approver Approver { get; set; }
        public DateTime ConfirmDate { get; set; }
        public ApproveType ApproveType { get; set; }

    }
}
