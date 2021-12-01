using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class UserRenewal
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
        [Required]
        public int CityId { get; set; }
        public City City { get; set; }
        [Required]
        public DateTime RequestDate { get; set; }
        public bool Approved { get; set; }
    }
}
