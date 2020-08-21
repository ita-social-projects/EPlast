using EPlast.BLL.DTO.UserProfiles;
using System;

namespace EPlast.BLL
{
    public class UserDistinctionDTO
    {
        public int Id { get; set; }
        public int DistinctionId { get; set; }
        public DistinctionDTO Distinction { get; set; }
        public string Reporter { get; set; }
        public string Reason { get; set; }
        public DateTime Date { get; set; }
        public bool CanChange { get; set; }
        public string UserId { get; set; }
        public UserDTO User { get; set; }
    }
}
