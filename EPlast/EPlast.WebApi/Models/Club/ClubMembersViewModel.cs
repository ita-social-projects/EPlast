using EPlast.WebApi.Models.User;
using System.ComponentModel.DataAnnotations;

namespace EPlast.WebApi.Models.Club
{
    public class ClubMembersViewModel
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public UserInfoViewModel User { get; set; }
        public int ClubId { get; set; }
        [Required]
        public bool IsApproved { get; set; }
    }
}
