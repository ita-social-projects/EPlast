using EPlast.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace EPlast.DataAccess.Entities.UserEntities
{
    public class UserPrecaution
    {
        public int Id { get; set; }
        public int PrecautionId { get; set; }
        public Precaution Precaution { get; set; }
        public string Reporter { get; set; }
        public string Reason { get; set; }
        public UserPrecautionStatus Status { get; set; } = UserPrecautionStatus.Accepted;
        public int Number { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }

        [NotMapped]
        public DateTime EndDate => Date.AddMonths(Precaution.MonthsPeriod);
        
        [NotMapped]
        public bool IsActive => Date < DateTime.Now && DateTime.Now < EndDate && Status != UserPrecautionStatus.Cancelled;
    }
}
