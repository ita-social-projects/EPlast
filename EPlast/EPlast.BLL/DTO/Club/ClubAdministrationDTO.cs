using System;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Admin;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.DTO.Club
{
    public class ClubAdministrationDTO
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public ClubUserDTO User { get; set; }
        public int ClubId { get; set; }
        public int AdminTypeId { get; set; }
        public AdminTypeDTO AdminType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Status { get; set; }

        public static implicit operator ClubAdministrationDTO(Task<ClubAdministration> v)
        {
            throw new NotImplementedException();
        }
    }
}