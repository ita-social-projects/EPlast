using EPlast.Resources;
using EPlast.WebApi.ValidationAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.WebApi.Models.Precaution
{
    public class UserPrecautionCreateViewModel
    {
        public int PrecautionId { get; set; }
        public string Reporter { get; set; }
        public string Reason { get; set; }
        public UserPrecautionStatus Status { get; set; }
        public int Number { get; set; }   
        [NotFutureDate(ErrorMessage = "We cannot select future day as the start date of precaution")]
        public DateTime Date { get; set; }
        public string UserId { get; set; }
    }
}
