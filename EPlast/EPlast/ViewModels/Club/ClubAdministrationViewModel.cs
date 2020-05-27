using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.ViewModels.Club
{
    public class ClubAdministrationViewModel
    {
        public int ID { get; set; }
        public int AdminTypeId { get; set; }
        public string AdminType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ClubId { get; set; }
        public CLubViewModel Club { get; set; }
        public ClubMembersViewModel ClubMembers { get; set; }
        public int ClubMembersID { get; set; }
    }
}
