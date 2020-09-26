using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities.UserEntities
{
    public class UserDistinction
    {
        public int Id { get; set; }
        public int DistinctionId { get; set; }
        public Distinction Distinction { get; set; }
        public string Reporter { get; set; }
        public string Reason { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }

    }
}
